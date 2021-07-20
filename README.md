# LP-Solver
This is a Solver for linear Optimization Problems. It takes the path of a defined problem, reads it and spills a table with the optimum Result and the values for each variable.

## How to use
Run the application from the console (Powershell/Bash)
In order to solve a problem its path needs to be passed:
* -p : pass the path to the problem.txt 

## Syntax 
The program can read problems defined according to the LP-Solve Syntax. 
For further information please refer to [LP-Solve guide](http://lpsolve.sourceforge.net/5.5/lp_solve.htm).

Following parameters can be used: 
* -h : get's the USER GUIDE                   
* -d : set the Number of decimals in the result (default= 2)                                 
* "exit" : this will end the programm

![Example.png](https://github.com/Johnnyboy-24/LP-Solver/blob/main/Images/Example.png)

## Compatability
The Solver is build on .Net Core 3.1.10. In order to run the program make sure that .Net Core 3.1. or higher is installed on your machine. 
If not get .NET here (If you only plan on running this programm pick .NET Runtime 3.1.): 
* [Get .NET](https://dotnet.microsoft.com/download/dotnet/3.1)
  
 