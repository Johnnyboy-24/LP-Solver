using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solver
{ 
    /// <summary>
    /// The Programm holds to classes/modules: A parser and a solver
    /// The parser extracts the objective function & constraints and creates a problem-matrix/- tableu 
    /// The Solver takes the matrix created by the parser and solves it according to passed parameters
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int decimals = 2;
            Parser parser = new Parser();
            string[] parameters;
            bool endprogramm= false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Solver.Properties.Resources.welcome_Message);

            while (!endprogramm)
            {
                parameters = Console.ReadLine().Split(" ");
                parse_Params(parameters);
                if(parser.text != null)
                {
                    Console.WriteLine(Solver.Properties.Resources.return_string);
                    print_Text(parser.text);
                    parser.to_Simplex_Tableau();
                    Simplex simplex_Solver = new Simplex(parser.problem_Tableau, parser.variables_Count, parser.maximierung, decimals);
                    simplex_Solver.solve();
                    Console.WriteLine(Solver.Properties.Resources.return_string);
                    Console.WriteLine("Solve another another Problem by passing its path or type 'Exit'!");
                    parser = new Parser();
                }
                
            }
            
            void parse_Params(string[] parameters)
            {
               
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] != "")
                    {
                        switch (parameters[i])
                        {
                            case "-h":
                                Console.WriteLine(Solver.Properties.Resources.help_Message);
                                break;
                            case "-p":
                                try { parser.path = @parameters[i + 1]; }
                                catch (ArgumentNullException) { Console.WriteLine("No Path was passed! Use -p to pass a path or refer to -h for help. "); }
                                catch (ArgumentException) { Console.WriteLine("File needs to be of type: .txt!"); }
                                break;
                            case "exit":
                                endprogramm = true;
                                break;
                            case "-d":
                                if (decimals <= 10 && decimals >= 1)
                                    decimals = int.Parse(parameters[i + 1]);
                                else
                                    Console.WriteLine("Number of decimals can only be between 1 and 10");
                                break;
                            default:
                                break;
                        }
                    }
                    
                }
            }
            
            void print_Text(List<string> text)
            {
                foreach (var line in text)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}

