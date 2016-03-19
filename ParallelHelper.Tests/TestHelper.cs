using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParallelHelper.Tests
{
    public class TestHelper
    {
        private List<DataObject> _data;
        private int _skip;
        private int _take;
        private int _totalRecs;

        public TestHelper()
        {
            _take = 3;
            _skip = 0;
            _totalRecs = 12;
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

        public List<DataObject> GetObjectsSkipTake()
        {
            var data = _data.Skip(_skip).Take(_take).ToList();
            Thread.Sleep(10);
            _skip += _take;
            return data;
        }

        public void ProcessData(List<DataObject> objects)
        {
        }
    }
}