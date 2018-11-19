# Machine Learning in C#

This repo contains a collection of resources for understanding some basic usage of Machine Learning in C#.

The lessons are in a single solution and take the learner through the usage of different machine learning algorithms.

This code is based on the ML.NET Taxi Fare Prediciton sample. https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/taxi-fare

The solution uses Microsoft Visual Studio 2017.
The solution uses nuget packages:
Newtonsoft.Json
Microsoft.ML
.NET Core 2.X

Data Science Question: Given the distance, passenger count and other inputs can I accurately predict the fare for the taxi ride?

1. Build the solution. If it does not build, ensure you have the right nuget packages and .net versions to execute the code.
2. The input data for this test is taxi fare data from New York City. Open the CSV file taxi-fare-train.csv to get a good idea of what the data looks like.
3. Build and run the solution. The first outputs are the different metric statistics about the values in the training dataset.
 - A GESD value greater than 3 will indicate a potential anomaly. Which columns have potential anomalies in them? Would we want to eliminate these rows of data from the training set?
 - What count of riders is a potential anomaly?
4. Uncomment line 36 of the code to see the Pearson Correlation Coefficients of some of the data. Build and run the solution. Are there high correlations?
  - The distance and time are highly correlated. Part of the value of this algorithm is to predict the fare BEFORE we know how much time you will spend in the taxi, so we will not use duration as part of the inputs. However, even without this caveat, we would likely not need both columns of data for an accurate prediction.
5. Uncomment line 39 of Program.cs. Build and run the solution.
  - Note in the output of the training, the data gets normalized. Why is this significant? Look back at the metric statistics to guess which columns might dominate if normalization was not done.
6. Uncomment line 42 of Program.cs. Build and run the solution.
 - The training of the model outputs two values RMS and R-squared. https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.legacy.models.regressionmetrics?view=ml-dotnet http://www.chioka.in/differences-between-l1-and-l2-as-loss-function-and-regularization/  https://en.wikipedia.org/wiki/Coefficient_of_determination
7. Uncomment lines 45 and 46. Build and run the solution.
 - You have just received one prediction. How might you gain more confidence in your predictions?

What data in your environment do you think is predictable? What are your inputs and what would be your predicted output?