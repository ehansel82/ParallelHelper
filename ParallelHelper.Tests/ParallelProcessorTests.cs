using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParallelHelper.Tests
{
    [TestClass]
    public class ParallelProcessorTests
    {
        private TestHelper _testHelper;
        private ParallelProcessor<DataObject> _processor;

        [TestInitialize]
        public void Init()
        {
            _testHelper = new TestHelper(50);
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

            Assert.AreEqual(50, _processor.ParallelNumObjectsLastProcessed);
        }
    }
}