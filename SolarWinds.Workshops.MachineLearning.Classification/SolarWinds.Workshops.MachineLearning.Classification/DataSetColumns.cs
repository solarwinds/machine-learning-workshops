/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

namespace SolarWinds.Workshops.MachineLearning.Classification
{
    class DataSetColumns
    {
        public double[] AvgTotalBytes { get; set; }
        public double[] AvgTotalPackets { get; set; }
        public double[] AvgAveragebps { get; set; }
        public double[] AvgOutPercentUtil { get; set; }
        public double[] AvgInPercentUtil { get; set; }
        public double[] AvgPercentUtil { get; set; }
        public double[] MinTotalBytes { get; set; }
        public double[] MinTotalPackets { get; set; }
        public double[] MinAveragebps { get; set; }
        public double[] MinOutPercentUtil { get; set; }
        public double[] MinInPercentUtil { get; set; }
        public double[] MinPercentUtil { get; set; }
        public double[] MaxTotalBytes { get; set; }
        public double[] MaxTotalPackets { get; set; }
        public double[] MaxAveragebps { get; set; }
        public double[] MaxOutPercentUtil { get; set; }
        public double[] MaxInPercentUtil { get; set; }
        public double[] MaxPercentUtil { get; set; }
        public int[] NextHourAlert { get; set; }
    }
}
