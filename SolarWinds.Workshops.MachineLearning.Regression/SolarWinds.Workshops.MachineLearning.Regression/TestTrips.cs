/// SolarWinds Machine Learning Workshop
/// Copied from https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/taxi-fare

namespace SolarWinds.Workshops.MachineLearning.Regression
{
    static class TestTrips
    {
        internal static readonly TaxiTrip Trip1 = new TaxiTrip
        {
            VendorId = "VTS",
            RateCode = "1",
            PassengerCount = 1,
            TripDistance = 10.33f,
            PaymentType = "CSH",
            FareAmount = 0 // predict it. actual = 29.5
        };
    }
}
