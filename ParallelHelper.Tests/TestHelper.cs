using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParallelHelper.Tests
{
    /// <summary>
    /// Quick implementation of class that can be used to test ParallelProcessor
    /// Simply using Thread.Sleep() to simulate an I/O delay.
    /// </summary>
    public class TestHelper
    {
        private List<DataObject> _data;
        private int _skip;
        private int _take;
        private int _totalRecs;

        public List<DataObject> data
        {
            get { return _data; }
            set { _data = value; }
        }

        public TestHelper(int totalRecs)
        {
            _take = 3;
            _skip = 0;
            _totalRecs = totalRecs;
            InitializeData();
        }

        public void InitializeData()
        {
            _data = new List<DataObject>();
            for (var i = 0; i < _totalRecs; i++)
            {
                _data.Add(new DataObject());
            }
        }

        /// <summary>
        /// An example of a function that can be used as the delegate to
        /// ParalleProcessor.LoadData.  
        /// </summary>
        public List<DataObject> GetObjectsSkipTake()
        {
            var data = _data.Skip(_skip).Take(_take).ToList();
            Thread.Sleep(10);
            _skip += _take;
            return data;
        }

        /// <summary>
        /// An example of a routine that can be used as the delegate
        /// to ParaleleProcessor.ProcessData.
        /// </summary>
        /// <param name="obj"></param>
        public void ProcessData(DataObject obj)
        {
            Thread.Sleep(20);
            obj.Data += string.Format(": {0}", DateTime.Now);
        }
    }
}