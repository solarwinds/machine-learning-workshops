/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

namespace SolarWinds.Workshops.MachineLearning.Clustering
{
    class Program
    {
        static int NumClusters = 6;
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "NodesWithAlerts.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "NodesWithAlertsModel.zip");
        static void Main(string[] args)
        {
            try
            {
                var datasetColumns = new DataSetColumns();
                PopulateDataSet(datasetColumns);

                // Learn about your data
                UnderstandData(datasetColumns);

                // Find Correlations
                //FindCorrelations(datasetColumns);

                // Train the clustering model
                //var model = TrainTheModel();

                // Evaluate the model
                //EvaluateModel(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Execution Complete -- Press Enter");
            Console.ReadLine();
        }

        public static void PopulateDataSet(DataSetColumns dataSetColumns)
        {
            var cpuCount = new List<double>();
            var totalMemory = new List<double>();
            var systemUpTime = new List<double>();
            var sumTriggers = new List<double>();
            using (var reader = new StreamReader(_dataPath))
            {
                reader.ReadLine(); // headers
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    cpuCount.Add(double.Parse(values[11]));
                    totalMemory.Add(float.Parse(values[18], System.Globalization.NumberStyles.Float));
                    systemUpTime.Add(double.Parse(values[4]));
                    sumTriggers.Add(double.Parse(values[19]));
                }
            }
            dataSetColumns.CPUCount = cpuCount.ToArray();
            dataSetColumns.TotalMemory = totalMemory.ToArray();
            dataSetColumns.SystemUpTime = systemUpTime.ToArray();
            dataSetColumns.SumTriggers = sumTriggers.ToArray();
        }

        /// <summary>
        /// This method reads the training data and calculates a variety of statistical data about each column.
        /// </summary>
        public static void UnderstandData(DataSetColumns dataSetColumns)
        {
            //Calculate and output the statistics for your pleasure
            var stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.CPUCount, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("CPUCount Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.TotalMemory, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("TotalMemory Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.SystemUpTime, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("SystemUpTime Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.SumTriggers, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("SumTriggers Statistics:");
            Console.WriteLine(stats.ToString());
        }

        public static void FindCorrelations(DataSetColumns dataSetColumns)
        {
            var pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.SystemUpTime, dataSetColumns.SumTriggers);
            Console.WriteLine("Pearson coefficient between SystemUpTime and SumTriggers");
            Console.WriteLine(pcc);

            pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.CPUCount, dataSetColumns.TotalMemory);
            Console.WriteLine("Pearson coefficient between CPUCount and TotalMemory");
            Console.WriteLine(pcc);
        }

        private static PredictionModel<NodeData, ClusterPrediction> TrainTheModel()
        {
            Console.WriteLine();
            Console.WriteLine("Starting Machine Learning...");
            PredictionModel<NodeData, ClusterPrediction> model = Train();
            model.WriteAsync(_modelPath).Wait();
            var prediction = model.Predict(TestNodeData.RandomNode);
            Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
            Console.WriteLine($"Distances: {string.Join(" ", prediction.Distances)}");
            Console.WriteLine("<<<<< Machine Learning Complete! >>>>>");
            return model;
        }

        private static PredictionModel<NodeData, ClusterPrediction> Train()
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_dataPath).CreateFrom<NodeData>(useHeader: true, separator: ','));
            //pipeline.Add(new ColumnCopier(("Vendor", "Label")));
            pipeline.Add(new ColumnConcatenator(
                            "Features",
                            "SystemUpTime",
                            "CPUCount",
                            "TotalMemory",
                            "SumTriggers"));
            //pipeline.Add(new LpNormalizer("SystemUpTime",
            //                "CPUCount",
            //                "TotalMemory",
            //                "SumTriggers"));
            pipeline.Add(new KMeansPlusPlusClusterer() { K = NumClusters });
            var model = pipeline.Train<NodeData, ClusterPrediction>();
            return model;
        }

        private static NodeData[] GetTestingData()
        {
            var retval = new List<NodeData>();
            using (var reader = new StreamReader(_dataPath))
            {
                reader.ReadLine(); // headers
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    retval.Add(new NodeData
                    {
                        Vendor = values[1],
                        SystemUpTime = float.Parse(values[4]),
                        CPUCount = float.Parse(values[11]),
                        TotalMemory = float.Parse(values[18]),
                        SumTriggers = float.Parse(values[19])
                    });
                }
            }
            return retval.ToArray();
        }

        private static void EvaluateModel(PredictionModel<NodeData, ClusterPrediction> model)
        {
            Console.WriteLine("Let's evaluate the model!");
            // How do our clusters look
            var testResults = GetTestingData();
            var clusterDataSets = new Dictionary<int, DataSetLists>();
            var clusterDistances = new Dictionary<uint, Dictionary<int, List<double>>>();
            for (int i = 0; i < testResults.Length; i++)
            {
                // View the cluster for each value in the data
                var prediction = model.Predict(testResults[i]);
                Console.WriteLine($"{testResults[i].Vendor} = Cluster: {prediction.PredictedClusterId} Distances: {string.Join(" ", prediction.Distances)}");
                // How would we evaluate our clusters to draw conclusions?
                if (!clusterDataSets.ContainsKey((int)prediction.PredictedClusterId))
                {
                    clusterDataSets[(int)prediction.PredictedClusterId] = new DataSetLists();
                }
                clusterDataSets[(int)prediction.PredictedClusterId].CPUCount.Add(testResults[i].CPUCount);
                clusterDataSets[(int)prediction.PredictedClusterId].SumTriggers.Add(testResults[i].SumTriggers);
                clusterDataSets[(int)prediction.PredictedClusterId].SystemUpTime.Add(testResults[i].SystemUpTime);
                clusterDataSets[(int)prediction.PredictedClusterId].TotalMemory.Add(testResults[i].TotalMemory);

                // Analyze the distances within clusters and between clusters
                if (!clusterDistances.ContainsKey(prediction.PredictedClusterId))
                {
                    clusterDistances[prediction.PredictedClusterId] = new Dictionary<int, List<double>>();
                }
                for (int j = 0; j < prediction.Distances.Length; j++)
                {
                    if (!clusterDistances[prediction.PredictedClusterId].ContainsKey(j))
                    {
                        clusterDistances[prediction.PredictedClusterId][j] = new List<double>();
                    }
                    clusterDistances[prediction.PredictedClusterId][j].Add(prediction.Distances[j]);
                }
            }

            Console.WriteLine("------------- Statistical Analysis of Clusters ---------------");
            foreach (var item in clusterDistances)
            {
                Console.WriteLine($"Analysis of cluster {item.Key}");
                foreach (var cluster in item.Value)
                {
                    var stats = MetricStatistics.CalculateMetricStatisticsAsync(cluster.Value.ToArray(), DateTime.Now, TimeSpan.FromSeconds(1)).Result;
                    if ((item.Key - 1) == cluster.Key)
                    {
                        Console.WriteLine($"Statistics for items within the cluster {item.Key}:");
                    }
                    else
                    {
                        Console.WriteLine($"Statistics for items from {item.Key} to cluster {cluster.Key + 1}");
                    }
                    Console.WriteLine(stats.ToString());
                }
            }

            Console.WriteLine();
            Console.WriteLine("------------- Statistical Analysis of Model ---------------");
            foreach (var item in clusterDataSets)
            {
                Console.WriteLine($"#################### Cluster: {item.Key}");
                UnderstandData(item.Value.GetDataSetColumns());
            }
        }
    }
}
