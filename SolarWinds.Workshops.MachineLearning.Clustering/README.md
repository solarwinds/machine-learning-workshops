# Machine Learning in C#

This repo contains a collection of resources for understanding some basic usage of Machine Learning in C#.

The lessons are in a single solution and take the learner through the usage of different machine learning algorithms.

The solution uses Microsoft Visual Studio 2017.
The solution uses nuget packages:
Newtonsoft.Json
Microsoft.ML
.NET Core 2.X

Steps to understanding this solution.

1. Open Data/NodesWithAlerts.csv - this file is a set of data from a SolarWinds Orion installation. The columns are the default columns with the Nodes table with the addition of the last column "SumTriggers".
This column represents the number of alert triggers that have happened on this specific node.
2. Open Program.cs and try to build the solution. If you can't build then you may need to update your environment to have the latest .net runtimes or other libraries.
3. The Main method in Program.cs has all the code necessary to all the execution already, but with some lines commented out.
4. Data Science Goal: Can we predict Vendor of the Node by the columns: CPUCount, SystemUpTime, SumTriggers, and TotalMemory? We will try to do this via the K-means clustering algorithm.
5. On the first execution, the program reads selected columns and computes some useful statistics about each column.
  - Take some time to understand some of the values that the MetricStatistics calculates for you.
  - What can you conclude when the mean is higher or lower than the median?
  - The GESD Value tells you if you have outliers. Is it greater than 3.0?
6. Uncomment the line of code in Main - FindCorrelations(datasetColumns);
  - Build and run the project again.
  - This line calculates the Pearson Correlation Coefficient between two columns. Closer to 1.0 means a strong positve correlation. Closer to -1.0 means a strong negative correlation. Closer to 0.0 means no correlation between the two values.
  - Often in data science you will not want to use values that are highly correlated as the additional column gives no additional data to the algorithm.
  - Are there any highly correlated columns? The code calculates two comparisons for you, but you can copy the code for additional comparisons.
7. Uncomment the section of code through the line "var model = TrainTheModel();" of try statement in Main.
  - Build and run the project now to engage the ML.NET clustering algorithm.
  - Note at the top of the Program class there is a value - static int NumClusters = 6; -- This value is the target number of clusters we want to get and is the number of unique Vendors in the data file.
  - Notice the ML.NET code outputs: "Automatically adding a MinMax normalization transform"
  - ML.NET noticed that the data needed to be normalized so that one column does not affect the clustering too much.
  - To help understand the results, we select one random node to see what cluster it belongs to.
  - We have 6 clusters of data. Now we need to understand if they have classified what we wanted. Do each of the clusters represent a Vendor? Guesses?
8. Uncomment the last lines of the Main method to evaluate our clusters.
  - Do you notice if we were successful?
  - The evaluation also calculates the MetricStatistics for each column of each cluster. Can you determine what the clustering algorithm used to cluster?
  - Are there any abnormalities in the data. Can you see any "-2" values? In Orion, a -2 means that the value has not been polled. To get rid of these missing values we should clean the data and replace the -2 with the mean of the rest of the values in the column.

The data in this exercise did not answer the target question. What questions could the data answer? Could we predict the number of SumTriggers from other columns? Try adding more columns from the input data to see if you can find clusters that find high alerting nodes vs low alerting nodes.

What if we were to change the number of target clusters? How would we evaluate which clusters are good representations of the data or not?

  - Clustering effectiveness is evaluated based on how tight a cluster is within itself and how far away the cluster is from the other clusters. What happens if you change the number of clusters in this program?

What are some questions that you think this data set could answer?
