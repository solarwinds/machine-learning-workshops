/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using Microsoft.ML.Runtime.Api;

namespace SolarWinds.Workshops.MachineLearning.Clustering
{
    // Column Options 
    /*
     * 0 NodeID,
     * 1 Vendor,
     * 2 Location,
     * 3 IOSVersion,
     * 4 SystemUpTime,
     * 5 ResponseTime,
     * 6 PercentLoss,
     * 7 AvgResponseTime,
     * 8 MinResponseTime,
     * 9 MaxResponseTime,
     * 10 CPUCount,
     * 11 CPULoad,
     * 12 MemoryUsed,
     * 13 MemoryAvailable,
     * 14 PercentMemoryUsed,
     * 15 PercentMemoryAvailable,
     * 16 MachineType,
     * 17 IsServer,
     * 18 TotalMemory,
     * 19 SumTriggers
     */
    public class NodeData
    {
        [Column("1")]
        public string Vendor;

        [Column("4")]
        public float SystemUpTime; // important these are all floats

        [Column("11")]
        public float CPUCount;

        [Column("18")]
        public float TotalMemory;

        [Column("19")]
        public float SumTriggers;
    }

    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}
