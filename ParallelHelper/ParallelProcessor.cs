using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ParallelHelper
{
    public class ParallelProcessor<T> where T : class, new()
    {
        private Stopwatch _watch;

        public ParallelProcessor()
        {
            List1 = new List<T>();
            List2 = new List<T>();
            _watch = new Stopwatch();
        }
        public List<T> List1 { get; private set; }
        public List<T> List2 { get; private set; }
        public long SequentialLastMilliseconds { get; private set; }
        public long ParallelLastMilliseconds { get; private set; }

        public Func<List<T>> LoadData;
        public Action<List<T>> ProcessData;

        public void Start()
        {
            _watch.Restart();
            ParallelLastMilliseconds = 0;
            var readList = 1;
            var done = true;           
            while (!done)
            {
                Task loadTask = Task.Factory.StartNew(() => {
                    if (readList == 1)
                    {
                        List1 = LoadData.Invoke();
                        if (List1.Count == 0) done = true;
                    }
                    else
                    {
                        List2 = LoadData.Invoke();
                        if (List1.Count == 0) done = true;
                    }
                });

                Task processTask = Task.Factory.StartNew(() => {
                    if (readList == 1)
                    {
                        ProcessData.Invoke(List2);
                    }
                    else
                    {
                        ProcessData.Invoke(List1);
                    }
                });

                Task.WaitAll(loadTask, processTask);
                readList = (readList == 1) ? 2 : 1;
            }
            _watch.Stop();
            ParallelLastMilliseconds = _watch.ElapsedMilliseconds;
            Debug.WriteLine("Parallel processing took:{0} ms", ParallelLastMilliseconds);
        }

        public void StartSequential()
        {
            _watch.Restart();
            SequentialLastMilliseconds = 0;
            var done = false;
            while (!done)
            {
                var objectsToProcess = LoadData.Invoke();
                if(objectsToProcess.Count() == 0)
                {
                    done = true;
                }
                else
                {
                    ProcessData(objectsToProcess);
                }
            }
            _watch.Stop();
            SequentialLastMilliseconds = _watch.ElapsedMilliseconds;
            Debug.WriteLine("Sequential processing took:{0} ms", SequentialLastMilliseconds);
        }
    }
}
