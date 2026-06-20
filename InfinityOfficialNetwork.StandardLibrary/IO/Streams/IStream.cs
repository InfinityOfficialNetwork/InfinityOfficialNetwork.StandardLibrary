using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.IO.Streams;

public enum IOOperationResultKind
{
	Success,
	Eof,
	Failure
}

public readonly struct IOOperationResult
{
	public bool Success { get; init; }
	public ulong ElementsRead { get; init; }
}

public interface IAsyncStream<TArg> : IDisposable
	where TArg : unmanaged
{
}

public interface IReadOnlyAsyncStreamDevice<TArg> : IDisposable, IAsyncDisposable
{
	//public ValueTask<ulong> ReadAsync<TSpan>(TSpan span, CancellationToken cancellationToken = default)
		//where TSpan : IWriteOnlySpan<TArg>;

	//public ValueTask<ulong> ReadAsync<TSpanIteratable, TSpan>(TSpanIteratable container, CancellationToken cancellationToken = default)
		//where TSpanIteratable : IReadOnlyContainerStreamingIteratable<TSpan>
		//where TSpan : IWriteOnlySpan<TArg>;
}

public interface IReadOnlyAsyncPullBasedStreamDevice<TArg>
{ }

public interface IReadOnlyAsyncPushBasedStreamDevice<TArg>
{ }

public interface IReadOnlyAsyncSeekableStream
{
	public ValueTask<ulong> SeekAsync(ulong pos);
	public ValueTask<ulong> GetPositionAsync(ulong pos);
}

///

//public interface IWriteOnlyAsyncStreamDevice<TArg> : IDisposable, IAsyncDisposable
//{
//	public ValueTask<ulong> WriteAsync<TSpan>(TSpan span, CancellationToken cancellationToken = default)
//		where TSpan : IWriteOnlySpan<TArg>;

//	public ValueTask<ulong> WriteAsync<TSpanIteratable, TSpan>(TSpanIteratable container, CancellationToken cancellationToken = default)
//		where TSpanIteratable : IReadOnlyContainerStreamingIteratable<TSpan>
//		where TSpan : IWriteOnlySpan<TArg>;
//}