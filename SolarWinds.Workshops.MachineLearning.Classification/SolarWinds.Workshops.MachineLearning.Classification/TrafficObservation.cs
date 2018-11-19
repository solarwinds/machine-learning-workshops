/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

namespace SolarWinds.Workshops.MachineLearning.Classification
{
    class TrafficObservation
    {
        public float AvgTotalBytes { get; set; }
        public float AvgTotalPackets { get; set; }
        public float AvgAveragebps { get; set; }
        public float AvgOutPercentUtil { get; set; }
        public float AvgInPercentUtil { get; set; }
        public float AvgPercentUtil { get; set; }
        public float MinTotalBytes { get; set; }
        public float MinTotalPackets { get; set; }
        public float MinAveragebps { get; set; }
        public float MinOutPercentUtil { get; set; }
        public float MinInPercentUtil { get; set; }
        public float MinPercentUtil { get; set; }
        public float MaxTotalBytes { get; set; }
        public float MaxTotalPackets { get; set; }
        public float MaxAveragebps { get; set; }
        public float MaxOutPercentUtil { get; set; }
        public float MaxInPercentUtil { get; set; }
        public float MaxPercentUtil { get; set; }
        public bool NextHourAlert { get; set; }
    }
}
