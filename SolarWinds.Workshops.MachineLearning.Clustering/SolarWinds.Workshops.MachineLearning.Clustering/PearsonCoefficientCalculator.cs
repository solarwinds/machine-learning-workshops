/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;
using System.Linq;

namespace SolarWinds.Workshops.MachineLearning.Clustering
{
    /// <summary>
    /// Class to help compute the Pearson Correlation Coefficient
    /// This method uses the Pearson calculation for samples, not population.
    /// </summary>
    public class PearsonCoefficientCalculator
    {
        public static double ComputeCoefficient(double[] series1, double[] series2)
        {
            var retval = 0.0;
            if (series1 == null || series2 == null || series1.Length != series2.Length || series1.Length < 2)
            {
                throw new ArgumentException("Series must not be null and of same length and of length > 1.");
            }
            var mean1 = series1.Average();
            var mean2 = series2.Average();
            var stdDev1 = Math.Sqrt(series1.Sum(i => (i - mean1) * (i - mean1)));
            var stdDev2 = Math.Sqrt(series2.Sum(i => (i - mean2) * (i - mean2)));
            var denominator = stdDev1 * stdDev2;
            var sum = 0.0;
            for (int i = 0; i < series1.Length; i++)
            {
                sum += (series1[i] - mean1) * (series2[i] - mean2);
            }
            retval = sum / denominator;
            return retval;
        }
    }
}
