using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelHelper
{
    public class ParallelProcessor<TObjectToProcess, TProcessedObject>
    {
        public List<TObjectToProcess> List1 { get; private set; }

        public List<TObjectToProcess> List2 { get; private set; }

        public Func<List<TObjectToProcess>> LoadData;

        public void Start()
        {
            List1 = LoadData.Invoke();
        }
    }
}
