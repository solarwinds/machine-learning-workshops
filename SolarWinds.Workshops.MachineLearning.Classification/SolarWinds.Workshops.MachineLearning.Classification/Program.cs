/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.ML;
using Microsoft.ML.Runtime;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Data.IO;
using static Microsoft.ML.Transforms.Normalizers.NormalizingEstimator;
using static Microsoft.ML.Runtime.Api.GenerateCodeCommand;

namespace SolarWinds.Workshops.MachineLearning.Classification
{
    class Program
    {
        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "BinaryClassification.csv");
        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "BinaryClassification.zip");

        static void Main(string[] args)
        {
            try
            {
                // Get the data for initial 
                var datasetColumns = new DataSetColumns();
                PopulateDataSet(datasetColumns);

                // Learn about your data
                UnderstandData(datasetColumns);

                // Find Correlations
                //FindCorrelations(datasetColumns);

                //Do Binary Classification
                //var predictor = ModelAndTrain();

                // Evaluate Results
                //EvaluateModel(predictor);
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
            var AvgTotalBytes = new List<double>();
            var AvgTotalPackets = new List<double>();
            var AvgAveragebps = new List<double>();
            var AvgOutPercentUtil = new List<double>();
            var AvgInPercentUtil = new List<double>();
            var AvgPercentUtil = new List<double>();
            var MinTotalBytes = new List<double>();
            var MinTotalPackets = new List<double>();
            var MinAveragebps = new List<double>();
            var MinOutPercentUtil = new List<double>();
            var MinInPercentUtil = new List<double>();
            var MinPercentUtil = new List<double>();
            var MaxTotalBytes = new List<double>();
            var MaxTotalPackets = new List<double>();
            var MaxAveragebps = new List<double>();
            var MaxOutPercentUtil = new List<double>();
            var MaxInPercentUtil = new List<double>();
            var MaxPercentUtil = new List<double>();
            var NextHourAlert = new List<int>();
            using (var reader = new StreamReader(_datapath))
            {
                reader.ReadLine(); // headers
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    AvgTotalBytes.Add(double.Parse(values[2]));
                    AvgTotalPackets.Add(double.Parse(values[3]));
                    AvgAveragebps.Add(double.Parse(values[4]));
                    AvgOutPercentUtil.Add(double.Parse(values[5]));
                    AvgInPercentUtil.Add(double.Parse(values[6]));
                    AvgPercentUtil.Add(double.Parse(values[7]));
                    MinTotalBytes.Add(double.Parse(values[8]));
                    MinTotalPackets.Add(double.Parse(values[9]));
                    MinAveragebps.Add(double.Parse(values[10]));
                    MinOutPercentUtil.Add(double.Parse(values[11]));
                    MinInPercentUtil.Add(double.Parse(values[12]));
                    MinPercentUtil.Add(double.Parse(values[13]));
                    MaxTotalBytes.Add(double.Parse(values[14]));
                    MaxTotalPackets.Add(double.Parse(values[15]));
                    MaxAveragebps.Add(double.Parse(values[16]));
                    MaxOutPercentUtil.Add(double.Parse(values[17]));
                    MaxInPercentUtil.Add(double.Parse(values[18]));
                    MaxPercentUtil.Add(double.Parse(values[19]));
                    NextHourAlert.Add(int.Parse(values[20]));
                }
            }
            dataSetColumns.AvgTotalBytes = AvgTotalBytes.ToArray();
            dataSetColumns.AvgTotalPackets = AvgTotalPackets.ToArray();
            dataSetColumns.AvgAveragebps = AvgAveragebps.ToArray();
            dataSetColumns.AvgOutPercentUtil = AvgOutPercentUtil.ToArray();
            dataSetColumns.AvgInPercentUtil = AvgInPercentUtil.ToArray();
            dataSetColumns.AvgPercentUtil = AvgPercentUtil.ToArray();
            dataSetColumns.MinTotalBytes = MinTotalBytes.ToArray();
            dataSetColumns.MinTotalPackets = MinTotalPackets.ToArray();
            dataSetColumns.MinAveragebps = MinAveragebps.ToArray();
            dataSetColumns.MinOutPercentUtil = MinOutPercentUtil.ToArray();
            dataSetColumns.MinInPercentUtil = MinInPercentUtil.ToArray();
            dataSetColumns.MinPercentUtil = MinPercentUtil.ToArray();
            dataSetColumns.MaxTotalBytes = MaxTotalBytes.ToArray();
            dataSetColumns.MaxTotalPackets = MaxTotalPackets.ToArray();
            dataSetColumns.MaxAveragebps = MaxAveragebps.ToArray();
            dataSetColumns.MaxOutPercentUtil = MaxOutPercentUtil.ToArray();
            dataSetColumns.MaxInPercentUtil = MaxInPercentUtil.ToArray();
            dataSetColumns.MaxPercentUtil = MaxPercentUtil.ToArray();
            dataSetColumns.NextHourAlert = NextHourAlert.ToArray();
        }

        /// <summary>
        /// This method reads the training data and calculates a variety of statistical data about each column.
        /// </summary>
        public static void UnderstandData(DataSetColumns dataSetColumns)
        {
            //Calculate and output the statistics for your pleasure
            var stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgTotalBytes, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgTotalBytes Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgTotalPackets, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgTotalPackets Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgAveragebps, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgAveragebps Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgOutPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgOutPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgInPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgInPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.AvgPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("AvgPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinTotalBytes, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinTotalBytes Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinTotalPackets, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinTotalPackets Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinAveragebps, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinAveragebps Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinOutPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinOutPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinInPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinInPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MinPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MinPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxTotalBytes, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxTotalBytes Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxTotalPackets, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxTotalPackets Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxAveragebps, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxAveragebps Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxOutPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxOutPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxInPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxInPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.MaxPercentUtil, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("MaxPercentUtil Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.NextHourAlert, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("NextHourAlert Statistics:");
            Console.WriteLine(stats.ToString());
        }

        public static void FindCorrelations(DataSetColumns dataSetColumns)
        {
            var pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.AvgTotalBytes, dataSetColumns.AvgTotalPackets);
            Console.WriteLine("Pearson coefficient between AvgTotalBytes and AvgTotalPackets");
            Console.WriteLine(pcc);

            pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.AvgOutPercentUtil, dataSetColumns.AvgInPercentUtil);
            Console.WriteLine("Pearson coefficient between AvgOutPercentUtil and AvgInPercentUtil");
            Console.WriteLine(pcc);
        }

        public static PredictionFunction<TrafficObservation, AlertPrediction> ModelAndTrain()
        {
            Console.WriteLine("Starting Machine Learning Binary Classification");
            MLContext mlContext = new MLContext(seed: 1);

            IDataView data = null;
            IDataView trainData = null;
            IDataView testData = null;

            // Step one: read the data as an IDataView.
            // Create the reader: define the data columns 
            // and where to find them in the text file.
            var reader = new TextLoader(mlContext, new TextLoader.Arguments
            {
                Column = new[] {
                    // A boolean column depicting the 'label'.
                    new TextLoader.Column("NextHourAlert", DataKind.BL, 20),
                    // 18 Features 
                    new TextLoader.Column("AvgTotalBytes", DataKind.R4, 2),
                    new TextLoader.Column("AvgTotalPackets", DataKind.R4, 3),
                    new TextLoader.Column("AvgAveragebps", DataKind.R4, 4),
                    new TextLoader.Column("AvgOutPercentUtil", DataKind.R4, 5),
                    new TextLoader.Column("AvgInPercentUtil", DataKind.R4, 6),
                    new TextLoader.Column("AvgPercentUtil", DataKind.R4, 7),
                    new TextLoader.Column("MinTotalBytes", DataKind.R4, 8),
                    new TextLoader.Column("MinTotalPackets", DataKind.R4, 9),
                    new TextLoader.Column("MinAveragebps", DataKind.R4, 10),
                    new TextLoader.Column("MinOutPercentUtil", DataKind.R4, 11),
                    new TextLoader.Column("MinInPercentUtil", DataKind.R4, 12),
                    new TextLoader.Column("MinPercentUtil", DataKind.R4, 13),
                    new TextLoader.Column("MaxTotalBytes", DataKind.R4, 14),
                    new TextLoader.Column("MaxTotalPackets", DataKind.R4, 15),
                    new TextLoader.Column("MaxAveragebps", DataKind.R4, 16),
                    new TextLoader.Column("MaxOutPercentUtil", DataKind.R4, 17),
                    new TextLoader.Column("MaxInPercentUtil", DataKind.R4, 18),
                    new TextLoader.Column("MaxPercentUtil", DataKind.R4, 19)
                },
                // First line of the file is a header, not a data row.
                HasHeader = true,
                Separator = ","
            });

            // We know that this is a Binary Classification task,
            // so we create a Binary Classification context:
            // it will give us the algorithms we need,
            // as well as the evaluation procedure.
            var classification = new BinaryClassificationContext(mlContext);

            data = reader.Read(new MultiFileSource(_datapath));

            (trainData, testData) = classification.TrainTestSplit(data, testFraction: 0.2);

            //Create a flexible pipeline (composed by a chain of estimators) for building/traing the model.

            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "AvgTotalBytes", "AvgTotalPackets", "AvgAveragebps", "AvgOutPercentUtil", "AvgInPercentUtil", "AvgPercentUtil",
"MinTotalBytes", "MinTotalPackets", "MinAveragebps", "MinOutPercentUtil", "MinInPercentUtil", "MinPercentUtil",
"MaxTotalBytes", "MaxTotalPackets", "MaxAveragebps", "MaxOutPercentUtil", "MaxInPercentUtil", "MaxPercentUtil" })
                            .Append(mlContext.Transforms.Normalize(inputName: "Features", outputName: "FeaturesNormalizedByMeanVar", mode: NormalizerMode.MeanVariance))
                            .Append(mlContext.BinaryClassification.Trainers.FastTree(label: "NextHourAlert",
                                                                                      features: "Features",
                                                                                      numLeaves: 20,
                                                                                      numTrees: 100,
                                                                                      minDatapointsInLeafs: 10,
                                                                                      learningRate: 0.2));
            var model = pipeline.Fit(trainData);

            var metrics = classification.Evaluate(model.Transform(testData), "NextHourAlert");
            Console.WriteLine("Acuracy: " + metrics.Accuracy);
            Console.WriteLine($"Area under ROC curve: {metrics.Auc}");
            Console.WriteLine($"Area under the precision/recall curve: {metrics.Auprc}");
            Console.WriteLine($"Entropy: {metrics.Entropy}");
            Console.WriteLine($"F1 Score: {metrics.F1Score}");
            Console.WriteLine($"Log loss: {metrics.LogLoss}");
            Console.WriteLine($"Log loss reduction: {metrics.LogLossReduction}");
            Console.WriteLine($"Negative precision: {metrics.NegativePrecision}");
            Console.WriteLine($"Positive precision: {metrics.PositivePrecision}");
            Console.WriteLine($"Positive recall: {metrics.PositiveRecall}");

            var predictor = model.MakePredictionFunction<TrafficObservation, AlertPrediction>(mlContext);
            return predictor;
        }

        private static void EvaluateModel(PredictionFunction<TrafficObservation, AlertPrediction> predictor)
        {
            Console.WriteLine("Predicting the Future!");
            // Can you find a data set that would trigger the prediction to be true?
            var predictionResult = predictor.Predict(new TrafficObservation()
            {
                AvgTotalBytes = 0,
                AvgTotalPackets = 0,
                AvgAveragebps = 0,
                AvgOutPercentUtil = 0,
                AvgInPercentUtil = 0,
                AvgPercentUtil = 0,
                MinTotalBytes = 0,
                MinTotalPackets = 0,
                MinAveragebps = 0,
                MinOutPercentUtil = 0,
                MinInPercentUtil = 0,
                MinPercentUtil = 0,
                MaxTotalBytes = 0,
                MaxTotalPackets = 0,
                MaxAveragebps = 0,
                MaxOutPercentUtil = 0,
                MaxInPercentUtil = 0,
                MaxPercentUtil = 0
            });

            predictionResult.PrintToConsole();
        }
    }
}
