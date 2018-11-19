/// SolarWinds Machine Learning Workshop
/// Copied from https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/taxi-fare

using Microsoft.ML.Runtime.Api;

namespace SolarWinds.Workshops.MachineLearning.Regression
{
    public class TaxiTrip
    {
        [Column("0")]
        public string VendorId;

        [Column("1")]
        public string RateCode;

        [Column("2")]
        public float PassengerCount;

        [Column("3")]
        public float TripTime;

        [Column("4")]
        public float TripDistance;

        [Column("5")]
        public string PaymentType;

        [Column("6")]
        public float FareAmount;
    }

    public class TaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}
