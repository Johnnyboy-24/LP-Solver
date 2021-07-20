using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleTables;
namespace Solver
{
    class Simplex
    {
        public double[,] problem_Tableu { get; private set; }
        public int height { get; private set; }
        public int width { get; private set; }
        public int variables_Count { get; private set; }
        public bool maximization { get; private set; }
        public int decimals { get; private set; }

        public Simplex(double[,] problem_Tableu, int variables_Count, bool maximization, int decimals )
        {
            this.problem_Tableu = problem_Tableu;
            this.height = problem_Tableu.GetLength(0);
            this.width = problem_Tableu.GetLength(1);
            this.variables_Count = variables_Count;
            this.maximization = maximization;
            this.decimals = decimals;
        }
        public int get_Pivot_Row(int column_Index)
        {
            double max = 0;
            int index_Result= -1;
            for (int i = 0; i < height-1; i++)
            {
                if (problem_Tableu[i, column_Index] > 0)
                {
                    double constraint_Fraction = (problem_Tableu[i, width - 1]) / (problem_Tableu[i, column_Index]);
                    if (max==0 || max > constraint_Fraction) { max = constraint_Fraction; index_Result = i; }
                }
            }
            return index_Result;
        }
        public int get_Pivot_Column()
        {
            double max = 0;
            int index_Result = -1;
            for (int i = 0; i < width-1; i++)
            {
                if (max == 0 || problem_Tableu[height-1, i] > max) { max = problem_Tableu[height-1, i]; index_Result = i; }
            }
            return index_Result;
        }
        public void make_Unit_Vector(int row, int column)
        {
            double denominator= problem_Tableu[row,column];
            for (int i = 0; i < width; i++)
            {
                problem_Tableu[row, i] /= denominator;
            }
            for (int u = 0; u< height; u++)
            {
                if (u!= row)
                {
                    double factor = problem_Tableu[u, column];
                    for (int s = 0; s < width; s++)
                    {
                        problem_Tableu[u, s] -= problem_Tableu[row, s] * factor;
                    }
                }
                
            }
        }
        
        public bool is_optimized()
        {
            for (int i =0; i < width-1; i++)
            {
                if (problem_Tableu[height-1, i] > 0)
                    return false;
            }
            return true;
        }
        
        public void solve()
        {
            int row;
            int column;
            while (!is_optimized())
            {
                column = get_Pivot_Column();
                row = get_Pivot_Row(column);
                make_Unit_Vector(row, column);               

            }
            
            serve_Variables();
        }
        public void serve_Variables()
        {
            var table = new ConsoleTable("Variables", "result");
            table.AddRow(" ", Math.Round(problem_Tableu[height - 1, width - 1]* -1, decimals));
            for (int i= 0; i<variables_Count; i++)
            {
                if (maximization)
                {
                    table.AddRow(i, Math.Round(problem_Tableu[i, width - 1], decimals));
                }
                else
                {
                    if (problem_Tableu[height - 1, width - 1 -variables_Count + i] < 0)
                        problem_Tableu[height - 1, width - 1 -variables_Count + i] *= -1;
                    table.AddRow(i, Math.Round(problem_Tableu[height - 1, width - 1 -variables_Count + i], decimals));
                }
            }
            table.Write();
        }

    }
}
