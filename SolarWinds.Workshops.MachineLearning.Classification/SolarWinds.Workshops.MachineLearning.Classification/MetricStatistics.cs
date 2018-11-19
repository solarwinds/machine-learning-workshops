/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SolarWinds.Workshops.MachineLearning.Classification
{
    /// <summary>
    /// Collection of statistical values about a data serie (metric)
    /// </summary>
    public class MetricStatistics
    {
        /// <summary>
        /// Minimum amount of values required for statistics computation
        /// </summary>
        public static readonly int RequiredValuesCountForComputation = 3;

        /// <summary>
        /// Begining of the time period the statistics are computed for
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Length of the time period the statistics are computed for
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Minimum value
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Maximum value
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Arithmetical mean (average)
        /// </summary>
        public double Mean { get; set; }

        /// <summary>
        /// Standard deviation of the mean
        /// </summary>
        public double StandardDeviation { get; set; }

        /// <summary>
        /// Generalized Extreme Studentized Deviate Test value
        /// </summary>
        public double GesdValue { get; set; }

        /// <summary>
        /// Variance
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// Variance sum TODO: compute
        /// </summary>
        public double VarianceSum { get; set; }

        /// <summary>
        /// Median (50% percentile)
        /// </summary>
        public double Median { get; set; }

        /// <summary>
        /// Median of absolute deviation (distance of values from mean)
        /// </summary>
        public double MedianAbsoluteDeviation { get; set; }

        /// <summary>
        /// First quartile (25% percentile)
        /// </summary>
        public double FirstQuartile { get; set; }

        /// <summary>
        /// Third quartile (75% percentile)
        /// </summary>
        public double ThirdQuartile { get; set; }

        /// <summary>
        /// Sum
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Slope of a linear regression trend
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Y-intercept of a linear regression trend slope
        /// </summary>
        public double SlopeYIntercept { get; set; }

        /// <summary>
        /// Mean squared error of a linear regression trend
        /// </summary>
        public double MeanSquaredError { get; set; }

        /// <summary>
        /// Skewness
        /// </summary>
        public double Skewness { get; set; }

        /// <summary>
        /// Excess kurtosis
        /// </summary>
        public double Kurtosis { get; set; }

        /// <summary>
        /// Count of elements
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Cardianality (amount of distinct elements in a serie)
        /// </summary>
        public int Cardinality { get; set; }

        /// <summary>
        /// Cardinality to count ratio
        /// </summary>
        public double CardinalityRatio { get; set; }

        /// <summary>
        /// Checks if there is enough of data to evaluate a value.
        /// </summary>
        /// <returns>True if the value can be evaluated; false otherwise.</returns>
        public bool CanEvaluateValue()
        {
            return Count > 10 && Cardinality > 10;
        }

        /// <summary>
        /// Evaluates level of anomality for a value based on statistics values.
        /// </summary>
        /// <param name="value">Value to compute level of anomality for</param>
        /// <returns>Level of anomality</returns>
        public double EvalulateValue(double value)
        {
            var retval = 0.0;
            retval += EvalulateValueByMedianAbsoluteDeviation(value);
            EvalulateValueByStandardDeviation(value, ref retval);
            return retval;
        }

        /// <summary>
        /// Evaluates level of anomality for a value based on Median Absolute Deviation.
        /// </summary>
        /// <param name="value">Value to compute level of anomality for</param>
        /// <param name="retval">Level of anomality to be updated</param>
        private void EvalulateValueByStandardDeviation(double value, ref double retval)
        {
            if (StandardDeviation > 1.0)
            {
                if (value > Mean + 3.0 * StandardDeviation || value < Mean - 3.0 * StandardDeviation)
                {
                    retval += 0.1;
                }
                if (value > Mean + 2.0 * StandardDeviation || value < Mean - 2.0 * StandardDeviation)
                {
                    retval += 0.05;
                }
                var gesdValue = Math.Abs(value - Mean) / StandardDeviation;
                if (gesdValue > 3.0)
                {
                    retval = 0.9;
                }
            }
        }

        /// <summary>
        /// Evaluates level of anomality for a value based on Standard Deviation.
        /// </summary>
        /// <param name="value">Value to compute level of anomality for</param>
        /// <returns>Level of anomality</returns>
        private double EvalulateValueByMedianAbsoluteDeviation(double value)
        {
            var retval = 0.0;
            if (MedianAbsoluteDeviation > 1.0)
            {
                if (value > Median + 3.0 * MedianAbsoluteDeviation || value < Median - 3.0 * MedianAbsoluteDeviation)
                {
                    retval += 0.1;
                }
                if (Math.Abs(FirstQuartile - Median) > MedianAbsoluteDeviation && value < FirstQuartile - MedianAbsoluteDeviation)
                {
                    retval += 0.1;
                }
                if (Math.Abs(ThirdQuartile - Median) > MedianAbsoluteDeviation && value > ThirdQuartile + MedianAbsoluteDeviation)
                {
                    retval += 0.1;
                }
            }
            return retval;
        }

        /// <summary>
        /// Computes statistics for a data serie (metric)
        /// </summary>
        /// <param name="values">Serie of data</param>
        /// <param name="startTime">Begining of the time period the statistics are computed for</param>
        /// <param name="duration">Length of the time period the statistics are computed for</param>
        /// <returns>Computed statistics</returns>
        public static Task<MetricStatistics> CalculateMetricStatisticsAsync(double[] values, DateTime startTime, TimeSpan duration)
        {
            if (values == null || values.Length < RequiredValuesCountForComputation)
                throw new ArgumentException($"{nameof(values)} is expected to be not null and have a length >= {RequiredValuesCountForComputation}.");

            return Task.Run(() =>
            {
                var retval = new MetricStatistics
                {
                    StartTime = startTime,
                    Duration = duration
                };

                var sortedArray = new double[values.Length];
                Array.Copy(values, sortedArray, values.Length);
                Array.Sort(sortedArray);

                var slopeIntercept = CalculateSlope(values);

                retval.Slope = slopeIntercept.Item1;
                retval.SlopeYIntercept = slopeIntercept.Item2;
                retval.MeanSquaredError = slopeIntercept.Item3;

                retval.Maximum = sortedArray[sortedArray.Length - 1];
                retval.Minimum = sortedArray[0];
                retval.Count = sortedArray.Length;
                retval.Sum = sortedArray.Sum();
                retval.Mean = retval.Sum / retval.Count;

                var quartiles = GetQuartiles(sortedArray);
                retval.Median = quartiles.Item2;
                retval.FirstQuartile = quartiles.Item1;
                retval.ThirdQuartile = quartiles.Item3;

                var stdDevSum = sortedArray.Sum(i => Math.Pow(i - retval.Mean, 2));
                retval.VarianceSum = stdDevSum;
                retval.Variance = stdDevSum / (retval.Count - 1);
                retval.StandardDeviation = Math.Sqrt(retval.Variance);
                retval.GesdValue = retval.StandardDeviation < double.Epsilon
                    ? 0.0
                    : Math.Max(retval.Mean - retval.Minimum, retval.Maximum - retval.Mean) / retval.StandardDeviation;

                var skewnessValue = values.Sum(d => Math.Pow(d - retval.Mean, 3));
                var kurtosisValue = values.Sum(d => Math.Pow(d - retval.Mean, 4));
                retval.Kurtosis = kurtosisValue / retval.Count / Math.Pow(retval.Variance, 2) - 3;
                retval.Skewness = Math.Sqrt((long)retval.Count * ((long)retval.Count - 1)) / (retval.Count - 2) *
                                  (skewnessValue / retval.Count) / Math.Pow(retval.StandardDeviation, 3);

                //sorted array will now contain sorted values of absolute deviation
                for (int i = 0; i < sortedArray.Length; i++)
                {
                    sortedArray[i] = Math.Abs(sortedArray[i] - retval.Median);
                }

                Array.Sort(sortedArray);

                retval.MedianAbsoluteDeviation = MedianOfSortedArrayRange(sortedArray, 0, sortedArray.Length - 1);

                retval.Cardinality = values.Distinct().Count();
                retval.CardinalityRatio = (double)retval.Cardinality / retval.Count;

                return retval;
            });
        }

        /// <summary>
        /// Computes statistics for a data serie (metric)
        /// </summary>
        /// <param name="values">Serie of data</param>
        /// <param name="startTime">Begining of the time period the statistics are computed for</param>
        /// <param name="duration">Length of the time period the statistics are computed for</param>
        /// <returns>Computed statistics</returns>
        public static Task<MetricStatistics> CalculateMetricStatisticsAsync(int[] values, DateTime startTime, TimeSpan duration)
        {
            return CalculateMetricStatisticsAsync(values.Select(i => (double)i).ToArray(), startTime, duration);
        }

        /// <summary>
        /// Computes statistics for a data serie (metric)
        /// </summary>
        /// <param name="values">Serie of data</param>
        /// <param name="startTime">Beginning of the time period the statistics are computed for</param>
        /// <param name="duration">Length of the time period the statistics are computed for</param>
        /// <returns>Computed statistics</returns>
        public static Task<MetricStatistics> CalculateMetricStatisticsAsync(long[] values, DateTime startTime, TimeSpan duration)
        {
            return CalculateMetricStatisticsAsync(values.Select(i => (double)i).ToArray(), startTime, duration);
        }

        /// <summary>
        /// Computes quartiles
        /// </summary>
        /// <param name="sortedArray">Sorted (asc) data serie</param>
        /// <returns>Tuple containing first quartile, median and third quartile.</returns>
        private static Tuple<double, double, double> GetQuartiles(double[] sortedArray)
        {
            var midPoint = sortedArray.Length / 2;
            var isOdd = sortedArray.Length % 2 != 0;
            var first = MedianOfSortedArrayRange(sortedArray, 0, isOdd ? midPoint : midPoint - 1);
            var median = MedianOfSortedArrayRange(sortedArray, 0, sortedArray.Length - 1);
            var third = MedianOfSortedArrayRange(sortedArray, midPoint, sortedArray.Length - 1);
            return new Tuple<double, double, double>(first, median, third);
        }

        private static double MedianOfSortedArrayRange(double[] sortedArray, int startIndex, int endIndex)
        {
            var count = endIndex - startIndex + 1;
            var midPoint = count / 2;
            double retval = count % 2 == 0
                ? (sortedArray[midPoint + startIndex] + sortedArray[midPoint - 1 + startIndex]) / 2.0
                : sortedArray[midPoint + startIndex];
            return retval;
        }

        /// <summary>
        /// Calculates parameters of linear regression trend 
        /// </summary>
        /// <param name="array">Data serie</param>
        /// <returns>Tuple containing slope, y-intercept and standard error of the slope</returns>
        private static Tuple<double, double, double> CalculateSlope(double[] array)
        {
            var n = array.Length;
            var ySum = array.Sum();
            var xSum = n * (n + 1) / 2;
            var xySum = 0.0;
            var xxSum = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                xySum += array[i] * (i + 1);
                xxSum += Math.Pow(i + 1, 2);
            }
            var denominator = n * xxSum - Math.Pow(xSum, 2);
            var slope = (n * xySum - xSum * ySum) / denominator;
            var yIntercept = (ySum * xxSum - xSum * xySum) / denominator;
            var standardError = CalculateSlopeStandardError(array, slope, yIntercept);
            return Tuple.Create(slope, yIntercept, standardError);
        }

        /// <summary>
        /// Calculates standard error of linear regression trend
        /// </summary>
        /// <param name="array">Serie of data the trend was computed from</param>
        /// <param name="slope">Slope of a trend</param>
        /// <param name="yIntercept">Y-intercept of a trend</param>
        /// <returns>Standard error</returns>
        private static double CalculateSlopeStandardError(double[] array, double slope, double yIntercept)
        {
            var sum = 0.0;
            var n = array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                var predictedValue = slope * (i + 1) + yIntercept;
                var value = array[i] - predictedValue;
                sum += value * value;
            }
            return Math.Sqrt(sum / n);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
