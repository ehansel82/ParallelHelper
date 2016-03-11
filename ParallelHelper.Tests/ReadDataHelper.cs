using System;
using System.Collections.Generic;
using System.Linq;

namespace ParallelHelper.Tests
{
    public class ReadDataHelper
    {
        private List<string> _strings = new List<string>()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"
        };

        public List<string> GetStrings()
        {
            return new List<string>() { "This is s string", "This is s string", "This is s string", "This is s string" };
        }

        public List<string> GetStringsSkipTake()
        {
            throw new NotImplementedException();
        }
    }
}