using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelHelper
{
    public class ParallelProcessor<T>
    {
        public ParallelProcessor()
        {
            List1 = new List<T>();
            List2 = new List<T>();
        }
        public List<T> List1 { get; private set; }
        public List<T> List2 { get; private set; }

        public Func<List<T>> LoadData;
        public Action<List<T>> ProcessData;

        public void Start()
        {
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
        }
    }

    public class SequentialProcessor<TObjectToPocess, TProcessedObject>
    {
    }
}
