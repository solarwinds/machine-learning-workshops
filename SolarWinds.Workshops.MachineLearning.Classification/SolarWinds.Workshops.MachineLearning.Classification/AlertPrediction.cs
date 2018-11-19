/// SolarWinds Machine Learning Workshop
/// Author: Karlo Zatylny - github: kzatylny
/// Date: November 2018
/// License: MIT

using System;

namespace SolarWinds.Workshops.MachineLearning.Classification
{
    class AlertPrediction
    {
        public bool NextHourAlert;
        public bool PredictedLabel;
        public float Score;
        public float Probability;

        public void PrintToConsole()
        {
            Console.WriteLine($"Predicted Label: {PredictedLabel}");
            Console.WriteLine($"Probability: {Probability}  ({Score})");
        }
    }
}
