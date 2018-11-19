/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT
/// 
using System.Collections.Generic;

namespace SolarWinds.Workshops.MachineLearning.Clustering
{
    class DataSetLists
    {
        public List<double> CPUCount { get; } = new List<double>();
        public List<double> TotalMemory { get; } = new List<double>();
        public List<double> SystemUpTime { get; } = new List<double>();
        public List<double> SumTriggers { get; } = new List<double>();

        public DataSetColumns GetDataSetColumns()
        {
            return new DataSetColumns()
            {
                CPUCount = CPUCount.ToArray(),
                TotalMemory = TotalMemory.ToArray(),
                SystemUpTime = SystemUpTime.ToArray(),
                SumTriggers = SumTriggers.ToArray()
            };
        }
    }
}
