using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelHelper.Tests
{
    public class DataObject
    {
        public string Data { get; set; }
        public DataObject()
        {
            Data = new Random().Next().ToString();
        }
    }
}
