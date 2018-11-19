# Machine Learning in C#

This repo contains a collection of resources for understanding some basic usage of Machine Learning in C#.

The lessons are in a single solution and take the learner through the usage of different machine learning algorithms.

This workshop is based on the MSDN article: https://msdn.microsoft.com/en-us/magazine/mt826350.aspx

1. Compile the solution to make sure it can compile for you.
2. The solution has two data sets. 
  - One is network utilization traffic from an interface
  - One is query wait time on a database
3. You can select one or the other dataset on line 44 of Program.cs to try out the neural network on each one.
4. Run the program and see the output. What conclusions can you draw from the statistics? If you selected the database wait time dataset, look at the Mean and Standard Deviation. What do you think it means that the Standard Deviation is larger than the Mean?
5. Note that depending which dataset you select, you must change the InputSize parameter. The neural network tries to use the previous values to predict the next value.
  - The network traffic is attempting to predict the next hours traffic based on the previous 24 hours
  - The wait time is attempting to predict the wait time of the next 10 minutes based on the last 6 10 minutes (1 hour).
6. Uncomment line 31 of the program and run it.
7. Note the error output for each of the epochs. What pattern do you see? Is this what you expected from the training of the neural network?
8. The neural network is a single hidden layer network. The program iteratively changes the number of hidden nodes to see which count of hidden neurons works best based on prediction accuracy.
9. How is the accuracy of the different counts of hidden neurons? What would you expect to happen with a second layer of neurons added?
10. This specific implementation does not have a feedback loop built in so changes are not evaluated for being an improvement or not. How could we add this?

Nerual networks come in a variety of flavors. Future workshops will include the Microsoft CNTK and LSTM neural network examples.