using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParallelHelper.Tests
{
    /// <summary>
    /// NOTE: change the private field numObjectsToProcess to reduce or 
    /// lengthen the time to test. More objects reveal the performance
    /// increase over SequentialStart. 
    /// </summary>
    [TestClass]
    public class ParallelProcessorTests
    {
        private TestHelper _testHelper;
        private ParallelProcessor<DataObject> _processor;
        private int numObjectsToProcess = 500;

        [TestInitialize]
        public void Init()
        {
            _testHelper = new TestHelper(numObjectsToProcess);
            _processor = new ParallelProcessor<DataObject>();
        }

        [TestMethod]
        public void StartSequentialSetsElapsedMilliseconds()
        {
            _processor.LoadData = _testHelper.GetObjectsSkipTake;
            _processor.ProcessData = _testHelper.ProcessData;

            _processor.StartSequential();

            Assert.IsTrue(_processor.SequentialLastMilliseconds > 0);
        }

        [TestMethod]
        public void StartSetsElapsedMilliseconds()
        {
            _processor.LoadData = _testHelper.GetObjectsSkipTake;
            _processor.ProcessData = _testHelper.ProcessData;

            _processor.Start();

            Assert.IsTrue(_processor.ParallelLastMilliseconds > 0);
        }

        [TestMethod]
        public void StartProcessesAnticipatedNumberOfRecords()
        {
            _processor.LoadData = _testHelper.GetObjectsSkipTake;
            _processor.ProcessData = _testHelper.ProcessData;

            _processor.Start();

            Assert.AreEqual(numObjectsToProcess, _processor.ParallelNumObjectsLastProcessed);
        }

        [TestMethod]
        public void StartUpdatesDataCorrectly()
        {
            _testHelper = new TestHelper(numObjectsToProcess);
            foreach (var obj in _testHelper.data)
            {
                Assert.IsFalse(obj.Data.Contains(DateTime.Now.Date.ToString()));
            }
            _processor.LoadData = _testHelper.GetObjectsSkipTake;
            _processor.ProcessData = _testHelper.ProcessData;

            _processor.Start();

            foreach (var obj in _testHelper.data)
            {
                Assert.IsTrue(obj.Data.Contains(DateTime.Now.Date.ToShortDateString()));
            }
        }
    }
}