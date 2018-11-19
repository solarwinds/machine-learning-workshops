/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Collections.Generic;
using System.IO;

namespace SolarWinds.Workshops.MachineLearning.NeuralRegression
{
    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "InterfaceTrafficHistoryByHour.csv"); // Input Size = 24
        static readonly string _dataPath2 = Path.Combine(Environment.CurrentDirectory, "Data", "waittime.csv"); // Input Size = 6

        static int InputSize = 24;
        static int FrameSize = InputSize + 1;
        static void Main(string[] args)
        {
            try
            {
                // Get the data for initial 
                var datasetColumns = new DataSetColumns();
                PopulateDataSet(datasetColumns);

                // Learn about your data
                var stats = UnderstandData(datasetColumns);

                //Do Neural Networking
                //ModelAndTrain(datasetColumns, stats.MedianAbsoluteDeviation);
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
            var timeSeries = new List<double>();
            using (var reader = new StreamReader(_dataPath2))
            {
                reader.ReadLine(); // headers
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    timeSeries.Add(double.Parse(values[1]));
                }
            }
            dataSetColumns.TimeSeries = timeSeries.ToArray();
        }

        /// <summary>
        /// This method reads the training data and calculates a variety of statistical data about each column.
        /// </summary>
        public static MetricStatistics UnderstandData(DataSetColumns dataSetColumns)
        {
            //Calculate and output the statistics for your pleasure
            var stats = MetricStatistics.CalculateMetricStatisticsAsync(dataSetColumns.TimeSeries, DateTime.Now, TimeSpan.FromSeconds(1)).Result;
            Console.WriteLine("Time Series Statistics:");
            Console.WriteLine(stats.ToString());
            return stats;
        }

        public static void ModelAndTrain(DataSetColumns dataSetColumns, double accuracy)
        {

            var trainData = GetTrainingData(dataSetColumns);

            int numInput = InputSize; // number predictors
            int numHidden = InputSize * 2;
            int numOutput = 1; // regression

            NeuralNetwork bestNeuralNetwork = null;
            double bestTrainingAccuracy = 0.0;

            for (int i = 0; i < 6; i++)
            {
                numHidden += numInput;
                Console.WriteLine("Creating a " + numInput + "-" + numHidden +
                  "-" + numOutput + " neural network");
                NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput, true);

                int maxEpochs = 10000;
                double learnRate = 0.01;
                Console.WriteLine("\nSetting maxEpochs = " + maxEpochs);
                Console.WriteLine("Setting learnRate = " + learnRate.ToString("F2"));

                Console.WriteLine("\nStarting training");
                double[] weights = nn.Train(trainData, maxEpochs, learnRate);
                Console.WriteLine("Done");
                Console.WriteLine("\nFinal neural network model weights and biases:\n");
                ShowVector(weights, 2, 10, true);

                double trainAcc = nn.Accuracy(trainData, accuracy);  // within standard deviation
                if (bestNeuralNetwork == null || bestTrainingAccuracy < trainAcc)
                {
                    bestTrainingAccuracy = trainAcc;
                    bestNeuralNetwork = nn;
                }
                Console.WriteLine("\nModel accuracy (+/- input accuracy) on training data = " +
                  trainAcc.ToString("F4"));
            }

            Console.WriteLine("\n========================= Best Model accuracy (+/- input accuracy) on training data = " +
                  bestTrainingAccuracy);
        }

        private static double[][] GetTrainingData(DataSetColumns dataSetColumns)
        {
            var length = dataSetColumns.TimeSeries.Length - FrameSize;
            var retval = new double[length][];

            // build array where first 24 values are input and the last value is the expected output
            for (int i = 0; i < length; i++)
            {
                retval[i] = new double[FrameSize];
                for (int j = 0; j < FrameSize; j++)
                {
                    retval[i][j] = dataSetColumns.TimeSeries[i + j];
                }
            }

            return retval;
        }

        static void ShowVector(double[] vector, int decimals, int lineLen, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i > 0 && i % lineLen == 0) Console.WriteLine("");
                if (vector[i] >= 0) Console.Write(" ");
                Console.Write(vector[i].ToString("F" + decimals) + " ");
            }
            if (newLine == true)
                Console.WriteLine("");
        }
    }
}
