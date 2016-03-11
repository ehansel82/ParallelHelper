using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelHelper;

namespace ParallelHelper.Tests
{
    [TestClass]
    public class LoadDataTests
    {
        [TestMethod]
        public void LoadData_Calls_Data()
        {
            var testHelper = new ReadDataHelper();
            var processor = new ParallelProcessor<string,string>();

            processor.LoadData = testHelper.GetStrings;

            processor.Start();

            Assert.AreEqual(4, processor.List1.Count);            
        }
    }
}
