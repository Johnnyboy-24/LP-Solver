using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Solver
{
    public class Parser
    {
        public bool maximierung { get; set; }
        public double[,] problem_Tableau { get; set; }
        public List<string> text { get; set; }
        public int variables_Count { get; set; }

        private string _path; 
        public string path
        { 
            get {return _path; } 
            set 
            {
                if (value == null) { throw new ArgumentNullException(); }
                else { _path = value; read_textfile_from_path(); }
            } 
        }
        public void read_textfile_from_path()
        {
            if (!_path.EndsWith(".txt")) { throw new ArgumentException();}
                
            else 
                text= File.ReadAllLines(_path).ToList();
            
        }
        
        
 
        //Creates the final table by:
        //      -converting numerals from char to double 
        //      -transposing if the LP is of Minimization-nature
        //      -adding slack variables
        public void to_Simplex_Tableau()
        {
            List<string[]> filtered_Text = filter_text();
            variables_Count = filtered_Text[0].Length - 1;
            
            double[,] result = new double[filtered_Text.Count(), filtered_Text[1].Length];            
            
            for (int i = 0; i < filtered_Text.Count(); i++)
            {
                for (int u = 0; u < filtered_Text[i].Length; u++)
                {                    
                    result[i, u] = double.Parse(filtered_Text[i][u]);
                }
            }
            if (!maximierung)
                result=transpose(result,result.GetLength(1),result.GetLength(0));

            problem_Tableau = add_slack_variables(result);
                
        }

        private double[,] transpose(double[,] original, int width , int height)
        {
            double[,] result = new double[width, height];
            for (int i =0; i < height; i++)
            {
                for(int u =0; u < width; u++)
                {
                    result[u, i] = original[i, u];
                }
            }
            return result;
        }

        //Copies variable-coefficients and constraints into a new matrix and adds a slack variable for each constraint
        public double[,] add_slack_variables(double[,] matrix)
        {
            int height = matrix.GetLength(0);
            int width_OLD = matrix.GetLength(1);
            int width = width_OLD + height-1;

            double[,] result = new double[height, width];
            
            for (int i = 0; i < width; i++)
            {
                for(int u = 0; u< height; u++)
                {
                    if(i < width_OLD-1) 
                        result[u, i] = matrix[u, i];
                    else if ((u == i- (width_OLD-1)) && (u<height-1))
                        result[u, i ]=1;
                    else if (i == width - 1 )
                        result[u, i] = matrix[u, width_OLD-1];
                    else
                        result[u, i ] = 0;                                     

                }
            }
            return result;
        }
        public List<string[]> filter_text()
        {
            List<string[]> result = new List<string[]>();

            //Clear text of all special characters and text. As coverage might be insufficient for some input, only use problem-sytax according to lp-solve. 
            for (int i = 0; i < text.Count; i++)
            {
                if (string.IsNullOrEmpty(text[i])) { text.RemoveAt(i); }
                if (text[i].StartsWith("//")) { text.RemoveAt(i); }                
                if (text[i].StartsWith("min")) { text[i] = text[i].Remove(0, 4); maximierung = false; }
                if (text[i].StartsWith("max")) { text[i] = text[i].Remove(0, 4); maximierung = true; }
                if (text[i].StartsWith(" +")) { text[i] = text[i].Remove(0, 2); }
                


                text[i] = text[i].Replace(">=", "+");
                text[i] = text[i].Replace("<=", "+");

                // Split into substrings and add to result
                result.Add(text[i].Split('+'));

                //remove rest
                for (int u = 0; u < result[i].Length; u++)
                    if (result[i][u].IndexOf('*') > 0)
                        result[i][u] = result[i][u].Remove(result[i][u].IndexOf('*'));

                    else if (result[i][u].IndexOf(';') > 0)
                        result[i][u] = result[i][u].Remove(result[i][u].IndexOf(';'));
            }
            
            return Swap(result, 0, result.Count-1);
        }
        static List<string[]> Swap(List<string[]> list, int first_Item, int second_Item)
        {
            string[] helper = list[first_Item];
            list[first_Item] = list[second_Item];
            list[second_Item] = helper;
            return list;
        }
    }
}
