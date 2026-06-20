using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

// Static usage for cleaner SyntaxFactory code
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace InfinityOfficialNetwork.Generators
{
	[Generator]
	public class ContractSourceGenerator : IIncrementalGenerator
	{
		// -------------------------------------------------------------------------
		// Constants & Diagnostic Descriptors
		// -------------------------------------------------------------------------

		private const string NamespaceBase = "InfinityOfficialNetwork.StandardLibrary.Contracts";
		private const string AttrCompliant = NamespaceBase + ".ContractCompliantAttribute";
		private const string AttrGuaranteed = NamespaceBase + ".ContractGuaranteedAttribute";
		private const string AttrIgnored = NamespaceBase + ".ContractCompliantIgnoredMethodAttribute";

		private const string AttrInvariant = NamespaceBase + ".InvariantMethodAttribute";
		private const string AttrStaticInvariant = NamespaceBase + ".StaticInvariantMethodAttribute";
		private const string AttrPrecondition = NamespaceBase + ".PreconditionMethodAttribute";
		private const string AttrPostcondition = NamespaceBase + ".PostconditionMethodAttribute";

		private static readonly DiagnosticDescriptor ION001_MustBePartial = new(
			"ION001", "Class must be partial", "The class '{0}' uses contracts and must be declared partial", "Contracts", DiagnosticSeverity.Error, true);

		private static readonly DiagnosticDescriptor ION002_MissingPrecondition = new(
			"ION002", "Missing Precondition", "The method '{0}' requires a precondition method named '{1}'", "Contracts", DiagnosticSeverity.Error, true);

		private static readonly DiagnosticDescriptor ION003_MissingPostcondition = new(
			"ION003", "Missing Postcondition", "The method '{0}' requires a postcondition method named '{1}'", "Contracts", DiagnosticSeverity.Error, true);

		private static readonly DiagnosticDescriptor ION004_InvariantSignature = new(
			"ION004", "Invalid Invariant Signature", "Invariant method '{0}' must accept 'ContractToken' as its only parameter", "Contracts", DiagnosticSeverity.Error, true);

		private static readonly DiagnosticDescriptor ION005_SignatureMismatch = new(
			"ION005", "Condition Signature Mismatch", "The condition method '{0}' does not match the expected signature {1}", "Contracts", DiagnosticSeverity.Error, true);

		private static readonly DiagnosticDescriptor ION006_MissingInvariants = new(
			"ION006", "Missing Invariants", "Contract Guaranteed classes must have at least one Invariant and one Static Invariant", "Contracts", DiagnosticSeverity.Error, true);

		// -------------------------------------------------------------------------
		// Data Models
		// -------------------------------------------------------------------------

		private record struct ClassAnalysisResult
		{
			public INamedTypeSymbol Symbol;
			public bool IsValid;
			public ImmutableArray<Diagnostic> Diagnostics;
			public ImmutableArray<MethodGenerationModel> MethodsToGenerate;
			public ImmutableArray<IMethodSymbol> InstanceInvariants;
			public ImmutableArray<IMethodSymbol> StaticInvariants;
		}

		private record struct MethodGenerationModel
		{
			public IMethodSymbol OriginalMethod;
			public string PreCheckName;
			public string PostCheckName;
			public IMethodSymbol? PreconditionMethod;
			public IMethodSymbol? PostconditionMethod;
		}

		// -------------------------------------------------------------------------
		// Pipeline Initialization
		// -------------------------------------------------------------------------

		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			//Debugger.Launch();
			//Debugger.Break();

			var classesToAnalyze = context.SyntaxProvider
				.CreateSyntaxProvider(
					predicate: static (node, _) => node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0,
					transform: static (ctx, _) => AnalyzeClass(ctx))
				// Check !IsDefault before checking Length
				.Where(static x => x.IsValid || (!x.Diagnostics.IsDefault && x.Diagnostics.Length > 0));

			context.RegisterSourceOutput(classesToAnalyze, GenerateOutput);
		}

		// -------------------------------------------------------------------------
		// Analysis Phase
		// -------------------------------------------------------------------------

		private static ClassAnalysisResult AnalyzeClass(GeneratorSyntaxContext context)
		{
			var classDecl = (ClassDeclarationSyntax)context.Node;
			var symbol = context.SemanticModel.GetDeclaredSymbol(classDecl);

			// 1. Safe Fallback Helper
			// Ensures we never return a struct with uninitialized ImmutableArrays (which cause NREs)
			ClassAnalysisResult EmptyResult(bool isValid = false) => new ClassAnalysisResult
			{
				Symbol = symbol!, // Nullable forgiveness (filtered out if null anyway)
				IsValid = isValid,
				Diagnostics = ImmutableArray<Diagnostic>.Empty,
				MethodsToGenerate = ImmutableArray<MethodGenerationModel>.Empty,
				InstanceInvariants = ImmutableArray<IMethodSymbol>.Empty,
				StaticInvariants = ImmutableArray<IMethodSymbol>.Empty
			};

			if (symbol == null) return EmptyResult();

			// 2. Determine Mode
			bool isGuaranteed = HasAttribute(symbol, AttrGuaranteed);
			bool isCompliant = HasAttribute(symbol, AttrCompliant);

			if (!isGuaranteed && !isCompliant)
				return EmptyResult();

			var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
			var methodsToGen = ImmutableArray.CreateBuilder<MethodGenerationModel>();
			var instanceInvariants = ImmutableArray.CreateBuilder<IMethodSymbol>();
			var staticInvariants = ImmutableArray.CreateBuilder<IMethodSymbol>();

			// 3. Check Partial
			if (!classDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
			{
				diagnostics.Add(Diagnostic.Create(ION001_MustBePartial, classDecl.Identifier.GetLocation(), symbol.Name));

				// Return explicit failure state with properly initialized empty arrays
				var res = EmptyResult(isValid: false);
				res.Diagnostics = diagnostics.ToImmutable();
				return res;
			}

			// 4. Scan Members
			var allMethods = symbol.GetMembers().OfType<IMethodSymbol>().ToList();

			foreach (var m in allMethods)
			{
				if (HasAttribute(m, AttrInvariant))
				{
					if (ValidateInvariantSignature(m)) instanceInvariants.Add(m);
					else diagnostics.Add(Diagnostic.Create(ION004_InvariantSignature, m.Locations.FirstOrDefault(), m.Name));
				}
				if (HasAttribute(m, AttrStaticInvariant))
				{
					if (ValidateInvariantSignature(m)) staticInvariants.Add(m);
					else diagnostics.Add(Diagnostic.Create(ION004_InvariantSignature, m.Locations.FirstOrDefault(), m.Name));
				}
			}

			if (isGuaranteed)
			{
				if (instanceInvariants.Count == 0 || staticInvariants.Count == 0)
					diagnostics.Add(Diagnostic.Create(ION006_MissingInvariants, classDecl.Identifier.GetLocation()));
			}

			foreach (var m in allMethods)
			{
				if (m.MethodKind != MethodKind.Ordinary) continue;
				if (IsConditionMethod(m)) continue;
				if (HasAttribute(m, AttrIgnored)) continue;

				string preName = m.Name + "Precondition";
				string postName = m.Name + "Postcondition";

				var preMethod = allMethods.FirstOrDefault(x => x.Name == preName);
				var postMethod = allMethods.FirstOrDefault(x => x.Name == postName);

				bool hasPre = preMethod != null;
				bool hasPost = postMethod != null;

				if (isGuaranteed)
				{
					if (!hasPre) diagnostics.Add(Diagnostic.Create(ION002_MissingPrecondition, m.Locations.FirstOrDefault(), m.Name, preName));
					if (!hasPost) diagnostics.Add(Diagnostic.Create(ION003_MissingPostcondition, m.Locations.FirstOrDefault(), m.Name, postName));
				}
				else if (!hasPre && !hasPost)
				{
					continue;
				}

				if (hasPre && !ValidatePreconditionSignature(m, preMethod!, out string preErr))
					diagnostics.Add(Diagnostic.Create(ION005_SignatureMismatch, preMethod!.Locations.FirstOrDefault(), preMethod.Name, preErr));

				if (hasPost && !ValidatePostconditionSignature(m, postMethod!, out string postErr))
					diagnostics.Add(Diagnostic.Create(ION005_SignatureMismatch, postMethod!.Locations.FirstOrDefault(), postMethod.Name, postErr));

				methodsToGen.Add(new MethodGenerationModel
				{
					OriginalMethod = m,
					PreCheckName = m.Name + "PreCheck",
					PostCheckName = m.Name + "PostCheck",
					PreconditionMethod = preMethod,
					PostconditionMethod = postMethod
				});
			}

			// 5. Final Return
			// Even if validation failed (diagnostics > 0), we populate the result correctly
			// so GenerateOutput can handle the reporting without crashing.
			return new ClassAnalysisResult
			{
				Symbol = symbol,
				IsValid = true, // We mark true here to pass the pipeline filter; explicit errors in Diagnostics will stop generation later.
				Diagnostics = diagnostics.ToImmutable(),
				MethodsToGenerate = methodsToGen.ToImmutable(),
				InstanceInvariants = instanceInvariants.ToImmutable(),
				StaticInvariants = staticInvariants.ToImmutable()
			};
		}

		// -------------------------------------------------------------------------
		// Validation Helpers
		// -------------------------------------------------------------------------

		private static bool HasAttribute(ISymbol symbol, string attributeName)
		{
			return symbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == attributeName);
		}

		private static bool IsConditionMethod(IMethodSymbol m)
		{
			return HasAttribute(m, AttrInvariant) || HasAttribute(m, AttrStaticInvariant) ||
				   HasAttribute(m, AttrPrecondition) || HasAttribute(m, AttrPostcondition);
		}

		private static bool ValidateInvariantSignature(IMethodSymbol m)
		{
			return m.ReturnsVoid && m.Parameters.Length == 1 &&
				   m.Parameters[0].Type.ToDisplayString() == NamespaceBase + ".ContractToken";
		}

		private static bool ValidatePreconditionSignature(IMethodSymbol original, IMethodSymbol condition, out string error)
		{
			error = string.Empty;
			if (!condition.ReturnsVoid) { error = "Must return void."; return false; }

			// Param 0 must be ContractToken
			if (condition.Parameters.Length == 0 || condition.Parameters[0].Type.ToDisplayString() != NamespaceBase + ".ContractToken")
			{
				error = "First parameter must be ContractToken.";
				return false;
			}

			// Remaining params must match Original, excluding OUT parameters
			var expectedParams = original.Parameters.Where(p => p.RefKind != RefKind.Out).ToList();
			var actualParams = condition.Parameters.Skip(1).ToList();

			if (expectedParams.Count != actualParams.Count)
			{
				error = $"Parameter count mismatch. Expected {expectedParams.Count} parameters (skipping out).";
				return false;
			}

			for (int i = 0; i < expectedParams.Count; i++)
			{
				if (!SymbolEqualityComparer.Default.Equals(expectedParams[i].Type, actualParams[i].Type))
				{
					error = $"Parameter {i + 1} type mismatch.";
					return false;
				}
				if (expectedParams[i].RefKind != actualParams[i].RefKind)
				{
					error = $"Parameter {i + 1} ref/in mismatch.";
					return false;
				}
			}
			return true;
		}

		private static bool ValidatePostconditionSignature(IMethodSymbol original, IMethodSymbol condition, out string error)
		{
			error = string.Empty;
			if (!condition.ReturnsVoid) { error = "Must return void."; return false; }

			// Param 0 must be ContractToken
			if (condition.Parameters.Length == 0 || condition.Parameters[0].Type.ToDisplayString() != NamespaceBase + ".ContractToken")
			{
				error = "First parameter must be ContractToken.";
				return false;
			}

			// Expected: All Original Params (Out becomes Ref) + Return Value (as Ref/In Last)
			var originalParams = original.Parameters;
			var condParams = condition.Parameters.Skip(1).ToList();

			int expectedCount = originalParams.Length;
			if (!original.ReturnsVoid) expectedCount++; // Add return value

			if (condParams.Count != expectedCount)
			{
				error = $"Parameter count mismatch. Expected {expectedCount} parameters (including return value).";
				return false;
			}

			// Check original parameters
			for (int i = 0; i < originalParams.Length; i++)
			{
				var op = originalParams[i];
				var cp = condParams[i];

				if (!SymbolEqualityComparer.Default.Equals(op.Type, cp.Type))
				{
					error = $"Parameter '{op.Name}' type mismatch.";
					return false;
				}

				// Logic: Out -> Ref. Others must match.
				if (op.RefKind == RefKind.Out)
				{
					if (cp.RefKind != RefKind.Ref)
					{
						error = $"Parameter '{op.Name}' is 'out' in original, must be 'ref' in postcondition.";
						return false;
					}
				}
				else
				{
					if (op.RefKind != cp.RefKind)
					{
						error = $"Parameter '{op.Name}' modifier mismatch.";
						return false;
					}
				}
			}

			// Check Return Value (if exists)
			if (!original.ReturnsVoid)
			{
				var retParam = condParams.Last();
				// Must be ref or in? User didn't specify strict constraint, but ref is preferred for Task/LargeStruct
				// Special Async Handling: Task<T> -> ref Task<T>
				// Standard: T -> ref T

				if (!SymbolEqualityComparer.Default.Equals(original.ReturnType, retParam.Type))
				{
					error = $"Return value parameter type mismatch. Expected {original.ReturnType.ToDisplayString()}.";
					return false;
				}

				if (retParam.RefKind != RefKind.Ref && retParam.RefKind != RefKind.In)
				{
					error = "Return value parameter must be passed as 'ref' or 'in'.";
					return false;
				}
			}

			return true;
		}

		// -------------------------------------------------------------------------
		// Generation Phase
		// -------------------------------------------------------------------------

		private static void GenerateOutput(SourceProductionContext spc, ClassAnalysisResult result)
		{
			// Report Diagnostics
			foreach (var diag in result.Diagnostics)
			{
				spc.ReportDiagnostic(diag);
			}

			// Stop if strict errors exist
			if (result.Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
				return;

			if (result.MethodsToGenerate.IsEmpty)
				return;

			try
			{
				var compilationUnit = BuildCompilationUnit(result);
				spc.AddSource($"{result.Symbol.Name}_Contracts.g.cs", SourceText.From(compilationUnit.NormalizeWhitespace().ToFullString(), Encoding.UTF8));
			}
			catch (Exception ex)
			{
				// Fallback for generator bugs
				var d = Diagnostic.Create(new DiagnosticDescriptor("ION999", "Generator Failure", ex.Message, "Gen", DiagnosticSeverity.Error, true), Location.None);
				spc.ReportDiagnostic(d);
			}
		}

		private static CompilationUnitSyntax BuildCompilationUnit(ClassAnalysisResult result)
		{
			var members = new List<MemberDeclarationSyntax>();

			foreach (var method in result.MethodsToGenerate)
			{
				members.Add(BuildPreCheck(method, result));
				members.Add(BuildPostCheck(method, result));
			}

			// Reconstruct Class Hierarchy
			var classDecl = ReconstructClassHierarchy(result.Symbol, members);

			// Add Namespace
			MemberDeclarationSyntax rootMember = classDecl;
			if (!result.Symbol.ContainingNamespace.IsGlobalNamespace)
			{
				rootMember = FileScopedNamespaceDeclaration(ParseName(result.Symbol.ContainingNamespace.ToDisplayString()))
					.WithMembers(SingletonList(rootMember));
			}

			return CompilationUnit()
				.AddUsings(UsingDirective(ParseName("System")))
				.AddUsings(UsingDirective(ParseName("System.Diagnostics")))
				.AddMembers(rootMember);
		}

		private static MethodDeclarationSyntax BuildPreCheck(MethodGenerationModel method, ClassAnalysisResult context)
		{
			// 1. Signature: Copy parameters, Excluding OUT params
			var paramsList = new List<ParameterSyntax>();
			foreach (var p in method.OriginalMethod.Parameters)
			{
				if (p.RefKind == RefKind.Out) continue;
				paramsList.Add(CreateParameter(p, forceRef: false));
			}

			// 2. Body
			var statements = new List<StatementSyntax>();

			// var storage = new InfinityOfficialNetwork.StandardLibrary.Contracts.ContractStorage();
			statements.Add(LocalDeclarationStatement(
				VariableDeclaration(ParseTypeName(NamespaceBase + ".ContractStorage"))
				.WithVariables(SingletonSeparatedList(
					VariableDeclarator(Identifier("storage"))
					.WithInitializer(EqualsValueClause(
						ObjectCreationExpression(ParseTypeName(NamespaceBase + ".ContractStorage"))
						.WithArgumentList(ArgumentList())
					))
				))));

			// var token = new InfinityOfficialNetwork.StandardLibrary.Contracts.ContractToken(ref storage);
			statements.Add(LocalDeclarationStatement(
				VariableDeclaration(IdentifierName("var"))
				.WithVariables(SingletonSeparatedList(
					VariableDeclarator(Identifier("token"))
					.WithInitializer(EqualsValueClause(
						ObjectCreationExpression(ParseTypeName(NamespaceBase + ".ContractToken"))
						.WithArgumentList(ArgumentList(SingletonSeparatedList(
							Argument(IdentifierName("storage")).WithRefKindKeyword(Token(SyntaxKind.RefKeyword))
						)))
					))
				))));

			// Call User Precondition (if exists)
			// Precondition(token, args...);
			if (method.PreconditionMethod != null)
			{
				var args = new List<ArgumentSyntax> { Argument(IdentifierName("token")) };
				foreach (var p in method.OriginalMethod.Parameters)
				{
					if (p.RefKind == RefKind.Out) continue;
					args.Add(CreateArgument(p, forceRef: false));
				}

				statements.Add(ExpressionStatement(
					InvocationExpression(IdentifierName(method.PreconditionMethod.Name))
					.WithArgumentList(ArgumentList(SeparatedList(args)))));
			}

			// Call Invariants
			AddInvariantCalls(statements, context, "token");

			// token.ProcessAllConditions();
			statements.Add(ExpressionStatement(
				InvocationExpression(
					MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("token"), IdentifierName("ProcessAllConditions")))));

			return CreateMethodDeclaration(method.PreCheckName, method.OriginalMethod, paramsList, statements);
		}

		private static MethodDeclarationSyntax BuildPostCheck(MethodGenerationModel method, ClassAnalysisResult context)
		{
			// 1. Signature: Copy parameters. Out -> Ref. Return Value -> Ref Last
			var paramsList = new List<ParameterSyntax>();
			foreach (var p in method.OriginalMethod.Parameters)
			{
				// If Out, force Ref
				paramsList.Add(CreateParameter(p, forceRef: p.RefKind == RefKind.Out));
			}

			// Add Return Value param
			if (!method.OriginalMethod.ReturnsVoid)
			{
				var retType = method.OriginalMethod.ReturnType;
				paramsList.Add(Parameter(Identifier("retVal"))
					.WithModifiers(TokenList(Token(SyntaxKind.RefKeyword)))
					.WithType(ParseTypeName(retType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))));
			}

			// 2. Body
			var statements = new List<StatementSyntax>();

			// Setup Storage & Token
			statements.Add(LocalDeclarationStatement(
			   VariableDeclaration(ParseTypeName(NamespaceBase + ".ContractStorage"))
			   .WithVariables(SingletonSeparatedList(
				   VariableDeclarator(Identifier("storage"))
				   .WithInitializer(EqualsValueClause(ObjectCreationExpression(ParseTypeName(NamespaceBase + ".ContractStorage")).WithArgumentList(ArgumentList())))
			   ))));

			statements.Add(LocalDeclarationStatement(
				VariableDeclaration(IdentifierName("var"))
				.WithVariables(SingletonSeparatedList(
					VariableDeclarator(Identifier("token"))
					.WithInitializer(EqualsValueClause(
						ObjectCreationExpression(ParseTypeName(NamespaceBase + ".ContractToken"))
						.WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName("storage")).WithRefKindKeyword(Token(SyntaxKind.RefKeyword)))))
					))
				))));

			// Call Invariants (First in PostCheck)
			AddInvariantCalls(statements, context, "token");

			// Call User Postcondition
			if (method.PostconditionMethod != null)
			{
				var args = new List<ArgumentSyntax> { Argument(IdentifierName("token")) };
				foreach (var p in method.OriginalMethod.Parameters)
				{
					// Pass Out as Ref
					args.Add(CreateArgument(p, forceRef: p.RefKind == RefKind.Out));
				}

				if (!method.OriginalMethod.ReturnsVoid)
				{
					args.Add(Argument(IdentifierName("retVal")).WithRefKindKeyword(Token(SyntaxKind.RefKeyword)));
				}

				statements.Add(ExpressionStatement(
					InvocationExpression(IdentifierName(method.PostconditionMethod.Name))
					.WithArgumentList(ArgumentList(SeparatedList(args)))));
			}

			// Process
			statements.Add(ExpressionStatement(
				 InvocationExpression(
					 MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("token"), IdentifierName("ProcessAllConditions")))));

			return CreateMethodDeclaration(method.PostCheckName, method.OriginalMethod, paramsList, statements);
		}

		private static void AddInvariantCalls(List<StatementSyntax> statements, ClassAnalysisResult context, string tokenName)
		{
			// Instance
			foreach (var inv in context.InstanceInvariants)
			{
				statements.Add(ExpressionStatement(
					InvocationExpression(IdentifierName(inv.Name))
					.WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName(tokenName)))))));
			}
			// Static
			foreach (var inv in context.StaticInvariants)
			{
				statements.Add(ExpressionStatement(
					InvocationExpression(IdentifierName(inv.Name))
					.WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName(tokenName)))))));
			}
		}

		// -------------------------------------------------------------------------
		// Syntax Factory Utils
		// -------------------------------------------------------------------------

		private static MethodDeclarationSyntax CreateMethodDeclaration(string name, IMethodSymbol original, List<ParameterSyntax> parameters, List<StatementSyntax> body)
		{
			var method = MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier(name))
				.WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword)))
				.AddAttributeLists(AttributeList(SingletonSeparatedList(
					Attribute(ParseName("System.Diagnostics.Conditional"))
					.AddArgumentListArguments(AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("DEBUG")))))))
				.WithParameterList(ParameterList(SeparatedList(parameters)))
				.WithBody(Block(body));

			// Copy generics
			if (original.TypeParameters.Length > 0)
			{
				method = method.WithTypeParameterList(TypeParameterList(
					SeparatedList(original.TypeParameters.Select(tp => TypeParameter(tp.Name)))));
			}

			// Handle Static
			if (original.IsStatic)
			{
				method = method.AddModifiers(Token(SyntaxKind.StaticKeyword));
			}

			return method;
		}

		private static ParameterSyntax CreateParameter(IParameterSymbol p, bool forceRef)
		{
			var typeFormat = SymbolDisplayFormat.FullyQualifiedFormat;
			var paramSyntax = Parameter(Identifier(p.Name))
				.WithType(ParseTypeName(p.Type.ToDisplayString(typeFormat)));

			// Modifiers
			if (forceRef)
			{
				paramSyntax = paramSyntax.AddModifiers(Token(SyntaxKind.RefKeyword));
			}
			else
			{
				if (p.RefKind == RefKind.Ref) paramSyntax = paramSyntax.AddModifiers(Token(SyntaxKind.RefKeyword));
				else if (p.RefKind == RefKind.Out) paramSyntax = paramSyntax.AddModifiers(Token(SyntaxKind.OutKeyword));
				else if (p.RefKind == RefKind.In) paramSyntax = paramSyntax.AddModifiers(Token(SyntaxKind.InKeyword));
			}

			return paramSyntax;
		}

		private static ArgumentSyntax CreateArgument(IParameterSymbol p, bool forceRef)
		{
			var arg = Argument(IdentifierName(p.Name));

			if (forceRef)
			{
				arg = arg.WithRefKindKeyword(Token(SyntaxKind.RefKeyword));
			}
			else
			{
				if (p.RefKind == RefKind.Ref) arg = arg.WithRefKindKeyword(Token(SyntaxKind.RefKeyword));
				else if (p.RefKind == RefKind.Out) arg = arg.WithRefKindKeyword(Token(SyntaxKind.OutKeyword));
				else if (p.RefKind == RefKind.In) arg = arg.WithRefKindKeyword(Token(SyntaxKind.InKeyword));
			}

			return arg;
		}

		private static ClassDeclarationSyntax ReconstructClassHierarchy(INamedTypeSymbol symbol, List<MemberDeclarationSyntax> innerMembers)
		{
			ClassDeclarationSyntax? currentDecl = null;
			INamedTypeSymbol? currentSym = symbol;

			// Traverse up to find the top-level containing class
			while (currentSym != null && currentSym.ContainingType != null)
			{
				currentSym = currentSym.ContainingType;
			}

			// Stack them so we process outermost -> innermost
			var stack = new Stack<INamedTypeSymbol>();
			var temp = symbol;
			while (temp != null)
			{
				stack.Push(temp);
				temp = temp.ContainingType;
			}

			MemberDeclarationSyntax? currentContent = null;

			while (stack.Count > 0)
			{
				var sym = stack.Pop(); // Outermost first
				var isInnermost = stack.Count == 0;

				// --- Build Modifiers in Correct Order: Accessibility -> Static -> Partial ---
				var modifiers = new List<SyntaxToken>();

				// 1. Accessibility
				switch (sym.DeclaredAccessibility)
				{
					case Accessibility.Public:
						modifiers.Add(Token(SyntaxKind.PublicKeyword));
						break;
					case Accessibility.Internal:
						modifiers.Add(Token(SyntaxKind.InternalKeyword));
						break;
					case Accessibility.Protected:
						modifiers.Add(Token(SyntaxKind.ProtectedKeyword));
						break;
					case Accessibility.Private:
						modifiers.Add(Token(SyntaxKind.PrivateKeyword));
						break;
					case Accessibility.ProtectedOrInternal: // "protected internal"
						modifiers.Add(Token(SyntaxKind.ProtectedKeyword));
						modifiers.Add(Token(SyntaxKind.InternalKeyword));
						break;
					case Accessibility.ProtectedAndInternal: // "private protected"
						modifiers.Add(Token(SyntaxKind.PrivateKeyword));
						modifiers.Add(Token(SyntaxKind.ProtectedKeyword));
						break;
				}

				// 2. Static
				if (sym.IsStatic)
				{
					modifiers.Add(Token(SyntaxKind.StaticKeyword));
				}

				// 3. Partial (Always last, so it sits right before 'class')
				modifiers.Add(Token(SyntaxKind.PartialKeyword));

				// Create Class with ordered modifiers
				var cls = ClassDeclaration(sym.Name)
					.WithModifiers(TokenList(modifiers));

				// Copy Generics
				if (sym.TypeParameters.Length > 0)
				{
					cls = cls.WithTypeParameterList(TypeParameterList(
						SeparatedList(sym.TypeParameters.Select(tp => TypeParameter(tp.Name)))));
				}

				// Add Members (Innermost gets the generated code, Outer gets the inner class)
				if (isInnermost)
				{
					cls = cls.WithMembers(List(innerMembers));
				}
				else
				{
					if (currentContent != null)
						cls = cls.WithMembers(SingletonList(currentContent));
				}

				currentContent = cls;
			}

			return (ClassDeclarationSyntax)currentContent!;
		}
	}
}