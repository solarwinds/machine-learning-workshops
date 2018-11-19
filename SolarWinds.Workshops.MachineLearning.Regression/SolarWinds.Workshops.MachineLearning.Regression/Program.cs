/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

namespace SolarWinds.Workshops.MachineLearning.Regression
{
    class Program
    {
        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
        static readonly string _testdatapath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static async Task Main(string[] args)
        {
            try
            {
                var datasetColumns = new DataSetColumns();
                PopulateDataSet(datasetColumns);

                // Learn about your data
                UnderstandData(datasetColumns);

                // Find Correlations
                //FindCorrelations(datasetColumns);

                // Build and train your model
                //PredictionModel<TaxiTrip, TaxiTripFarePrediction> model = await Train();

                // Evaluate your model (sample test to see how accurate your model is)
                //Evaluate(model);

                // Use your model.
                //TaxiTripFarePrediction prediction = model.Predict(TestTrips.Trip1);
                //Console.WriteLine("Predicted fare: {0}, actual fare: 29.5", prediction.FareAmount);
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
            var riders = new List<int>();
            var tripTime = new List<int>();
            var tripDistance = new List<double>();
            var fareAmount = new List<double>();
            using (var reader = new StreamReader(_datapath))
            {
                reader.ReadLine(); // headers
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    riders.Add(int.Parse(values[2]));
                    tripTime.Add(int.Parse(values[3]));
                    tripDistance.Add(double.Parse(values[4]));
                    fareAmount.Add(double.Parse(values[6]));
                }
            }
            dataSetColumns.Riders = riders.ToArray();
            dataSetColumns.TripTime = tripTime.ToArray();
            dataSetColumns.TripDistance = tripDistance.ToArray();
            dataSetColumns.FareAmount = fareAmount.ToArray();
        }

        /// <summary>
        /// This method reads the training data and calculates a variety of statistical data about each column.
        /// </summary>
        public static void UnderstandData(DataSetColumns dataSetColumns)
        {
            //Calculate and output the statistics for your pleasure
            var stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.Riders, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("Rider Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.TripTime, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("Trip Time Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.TripDistance, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("Trip Distance Statistics:");
            Console.WriteLine(stats.ToString());
            stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.FareAmount, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("Fare Amount Statistics:");
            Console.WriteLine(stats.ToString());
        }

        public static void FindCorrelations(DataSetColumns dataSetColumns)
        {
            var pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.TripDistance, dataSetColumns.TripTime.Select(i => (double)i).ToArray());
            Console.WriteLine("Pearson coefficient between Time and Distance");
            Console.WriteLine(pcc);

            pcc = PearsonCoefficientCalculator.ComputeCoefficient(dataSetColumns.TripDistance, dataSetColumns.Riders.Select(i => (double)i).ToArray());
            Console.WriteLine("Pearson coefficient between Riders and Distance");
            Console.WriteLine(pcc);
        }

        public async static Task<PredictionModel<TaxiTrip, TaxiTripFarePrediction>> Train()
        {
            Console.WriteLine();
            Console.WriteLine("Starting Machine Learning...");
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_datapath).CreateFrom<TaxiTrip>(useHeader: true, separator: ','));
            pipeline.Add(new ColumnCopier(("FareAmount", "Label")));
            pipeline.Add(new CategoricalOneHotVectorizer("VendorId",
                                             "RateCode",
                                             "PaymentType"));
            pipeline.Add(new ColumnConcatenator("Features",
                                    "VendorId",
                                    "RateCode",
                                    "PassengerCount",
                                    "TripDistance",
                                    "PaymentType"));
            pipeline.Add(new FastTreeRegressor());

            PredictionModel<TaxiTrip, TaxiTripFarePrediction> model = pipeline.Train<TaxiTrip, TaxiTripFarePrediction>();
            await model.WriteAsync(_modelpath);
            return model;
        }

        private static void Evaluate(PredictionModel<TaxiTrip, TaxiTripFarePrediction> model)
        {
            var testData = new TextLoader(_testdatapath).CreateFrom<TaxiTrip>(useHeader: true, separator: ',');
            var evaluator = new RegressionEvaluator();
            RegressionMetrics metrics = evaluator.Evaluate(model, testData);
            Console.WriteLine($"Rms = {metrics.Rms}");
            Console.WriteLine($"RSquared = {metrics.RSquared}");
        }
    }
}
