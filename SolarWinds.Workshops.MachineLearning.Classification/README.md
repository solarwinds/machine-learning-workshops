# Machine Learning in C#

This repo contains a collection of resources for understanding some basic usage of Machine Learning in C#.

The lessons are in a single solution and take the learner through the usage of different machine learning algorithms.

The solution uses Microsoft Visual Studio 2017.
The solution uses nuget packages:
Newtonsoft.Json
Microsoft.ML
.NET Core 2.X

This workshop is designed to teach you about classification algorithms. This workshop uses interface traffic data to classify if the next hour will contain an alert.

Question: Can we predict if an alert will happen in the next hour based on the current hours traffic?

1. Try and build the solution. If it does not build, make sure you have the correct Nuget packages and .net versions.
2. Examine the CSV file. Note that the data contains many of the interface traffic metrics and the last column identifies if an alert happened in the hour after these metrics were collected.
  - Is our data set big enough? This training set may or may not be big enough, so pay attention to the accuracy of the output in the end.
3. Build and run the solution and note the statistics output from each of the given metrics. Can you find any metrics that have strange behavior? Would you eliminate and of the columns based on this?
4. Should we normalize?  [WikiPedia Normalization](https://en.wikipedia.org/wiki/Normalization_%28statistics%29)  
5. Uncomment the line 36 to view the Pearson correlation coefficient. Build and run the program again.
6. What do you notice about the correlations? When values are highly correlated either positively or negatively, then they are redundant pieces of information. Which columns might you eliminate because of this?
7. Uncomment line 39 of Program.cs. Build and run the program again.
8. Note the output of the training. What do you conclude about the accuracy of being able to predict alerts? Would you want more or less data to be more accurate?
9. Uncomment line 42 of Program.cs. Build and run the program again.
10. The current code sends in a dataset that has all zeros as input. What is the prediction? Can you change the input to see if you can create a true prediction?

Change this program to use less columns to see if you can find the key indicators for alert prediction. Can you find the best inputs for predicting alerts? 

(Note that many machine learning algorithms use more than just Min, Max, and Avg as inputs. The MetricStatistics class contains a good representative set of values used for machine learning inputs to different algorithms.)