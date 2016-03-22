using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelHelper
{
    /// <summary>
    /// Manages processing of data using the TPL to optimize performance.
    /// </summary>
    /// <typeparam name="T">The type of object to be processed.</typeparam>
    public class ParallelProcessor<T> where T : class, new()
    {
        private Stopwatch _watch;

        private List<T> _list1;
        private List<T> _list2;

        public ParallelProcessor()
        {
            _list1 = new List<T>();
            _list2 = new List<T>();
            _watch = new Stopwatch();
        }

        /// <summary>
        /// The time elapsed for the last run of the StartSequential control method.
        /// </summary>
        public long SequentialLastMilliseconds { get; private set; }
        /// <summary>
        /// The time elapsed for the last run of the Start() method.
        /// </summary>
        public long ParallelLastMilliseconds { get; private set; }
        /// <summary>
        /// Number of objects processed by the last run of the Start() method.
        /// </summary>
        public long ParallelNumObjectsLastProcessed { get; private set; }
        /// <summary>
        /// Number fo objects processed by the last run of the StartSequential() conrol method.
        /// </summary>
        public long SequentialNumObjectsProcessed { get; private set; }

        /// <summary>
        /// Delegate function which will return a list of data objects of type T to process.
        /// </summary>
        public Func<List<T>> LoadData;
        /// <summary>
        ///  Delegate routine that should will individually process each object of type T retunred in a list from the LoadData delegate.
        /// </summary>
        public Action<T> ProcessData;

        /// <summary>
        /// Processes data on two threads by calling LoadData until LoadData returns ether null or an empty list.
        /// Every time LoadData completes and returns a list of data, ProcessData is invoked on each object in the
        /// list in a Parallel loop, while LoadData is simulaneously called again to load more data.
        /// </summary>
        public void Start()
        {
            _watch.Restart();
            ParallelLastMilliseconds = 0;
            long numProcessed = 0;
            var readList = 1;
            var done = false;
            while (!done)
            {
                Task loadTask = Task.Factory.StartNew(() =>
                {
                    if (readList == 1)
                    {
                        _list1 = LoadData.Invoke();
                        if (_list1 == null || _list1.Count == 0) done = true;
                    }
                    else
                    {
                        _list2 = LoadData.Invoke();
                        if (_list2 == null || _list2.Count == 0) done = true;
                    }
                });

                Task processTask = Task.Factory.StartNew(() =>
                {
                    List<T> ListToProcess = null;
                    if (readList == 1)
                    {
                        ListToProcess = _list2;
                    }
                    else
                    {
                        ListToProcess = _list1;
                    }
                    Parallel.ForEach(ListToProcess, (obj) =>
                    {
                        ProcessData.Invoke(obj);
                        Interlocked.Increment(ref numProcessed);
                    });
                });

                Task.WaitAll(loadTask, processTask);
                readList = (readList == 1) ? 2 : 1;
            }
            _watch.Stop();
            ParallelLastMilliseconds = _watch.ElapsedMilliseconds;
            ParallelNumObjectsLastProcessed = numProcessed;
            Debug.WriteLine("Parallel processing took:{0} ms", ParallelLastMilliseconds);
        }

        /// <summary>
        /// This is a control method that processes data in a standard sequential fashion and
        /// can be used to benchmark against the Start() which ustilizes the TPL.
        /// </summary>
        public void StartSequential()
        {
            _watch.Restart();
            SequentialLastMilliseconds = 0;
            SequentialNumObjectsProcessed = 0;
            var done = false;
            while (!done)
            {
                var objectsToProcess = LoadData.Invoke();
                if (objectsToProcess == null || objectsToProcess.Count() == 0)
                {
                    done = true;
                }
                else
                {
                    foreach (var obj in objectsToProcess)
                    {
                        ProcessData.Invoke(obj);
                        SequentialNumObjectsProcessed++;
                    }
                }
            }
            _watch.Stop();
            SequentialLastMilliseconds = _watch.ElapsedMilliseconds;
            SequentialNumObjectsProcessed = SequentialNumObjectsProcessed;
            Debug.WriteLine("Sequential processing took:{0} ms", SequentialLastMilliseconds);
        }
    }
}