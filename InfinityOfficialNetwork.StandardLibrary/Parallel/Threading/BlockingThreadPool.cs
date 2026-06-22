using InfinityOfficialNetwork.StandardLibrary.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace InfinityOfficialNetwork.StandardLibrary.Parallel.Threading;

public readonly unsafe struct StaticCallable
{
	private readonly delegate* managed<void> ptr;

	public void Invoke() => ptr();

	public delegate void CallableType();

	public unsafe StaticCallable(delegate* managed<void> ptr)
	{
		this.ptr = ptr;
	}
}

public unsafe static class BlockingThreadPool
{
	private const int ThreadTimeoutMs = 10000;

	private enum ThreadWakeupReason : byte
	{
		Spurious,
		TaskReady,
		ThreadShutdownRequested,
	}

	private unsafe class ThreadData : IDisposable
	{
		public Thread Thread;
		public readonly object Mutex = new object();
		public readonly ManualResetEventSlim ManualResetEventSlim = new ManualResetEventSlim(true);
		public Action? CurrentTask;
		public ThreadWakeupReason WakeupReason = ThreadWakeupReason.Spurious;

		public ThreadData() { }

		public void Dispose()
		{
			ManualResetEventSlim.Dispose();
			GC.SuppressFinalize(this);
		}
	}

	private static readonly ConcurrentBag<ThreadData> threads = new ConcurrentBag<ThreadData>();
	private static readonly ConcurrentStack<ThreadData> idleThreads = new ConcurrentStack<ThreadData>();

	public static Task Run(Action action)
	{
		TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

		if (idleThreads.TryPop(out ThreadData? threadData))
		{
			lock (threadData.Mutex)
			{
				threadData.CurrentTask = () =>
				{
					try
					{
						action();
						taskCompletionSource.SetResult();
					}
					catch (Exception ex)
					{
						taskCompletionSource.SetException(ex);
					}
				};
				threadData.WakeupReason = ThreadWakeupReason.TaskReady;
				threadData.ManualResetEventSlim.Set();
			}
		}
		else
		{
			TaskCompletionSource<ThreadData> threadDataSource = new TaskCompletionSource<ThreadData>();
			Thread newThread = new Thread(() => Worker(threadDataSource.Task));
			threadData = new ThreadData() { Thread = newThread, CurrentTask = action, WakeupReason = ThreadWakeupReason.TaskReady };
			threadDataSource.SetResult(threadData);
			threads.Add(threadData);
		}
		return taskCompletionSource.Task;
	}

	private static void Worker(Task<ThreadData> thread)
	{
		ThreadData thisThread = thread.Result;

		while (true)
		{
			Stopwatch timer = Stopwatch.StartNew();
			do
				thisThread.ManualResetEventSlim.Wait(ThreadTimeoutMs);
			while (thisThread.WakeupReason == ThreadWakeupReason.Spurious && timer.ElapsedMilliseconds < ThreadTimeoutMs);

			if (timer.ElapsedMilliseconds > ThreadTimeoutMs)
			{
				if (idleThreads.TryPop(out ThreadData? threadToStop))
				{
					if (threadToStop.Thread.ManagedThreadId == thisThread.Thread.ManagedThreadId)
						break;

					lock (threadToStop.Mutex)
					{
						threadToStop.WakeupReason = ThreadWakeupReason.ThreadShutdownRequested;
						threadToStop.ManualResetEventSlim.Set();
					}
				}
			}
			else if (thisThread.WakeupReason == ThreadWakeupReason.TaskReady)
				lock (thisThread.Mutex)
				{
					thisThread.CurrentTask!.Invoke();
					thisThread.CurrentTask = null;

					thisThread.WakeupReason = ThreadWakeupReason.Spurious;
					idleThreads.Push(thisThread);
				}
			else if (thisThread.WakeupReason == ThreadWakeupReason.ThreadShutdownRequested)
			{
				break;
			}

		}

		if (threads.TryTake(out ThreadData? threadData))
		{
			threadData.Dispose();
		}
		else
		{
			Assert.Fail("this thread was not in threads");
		}

		return;
	}
}