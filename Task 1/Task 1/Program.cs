
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public class Program
    {

        private enum SequanceState
        { 
            Rising,
            Falling
        }

        private static void Main(string[] args)
        {
            int[][] matrix;

            ReadMatrix("input.txt", out matrix);

            if (matrix.Length > 0)
            {
                SortStrings(ref matrix);
                TransposeMatrix(ref matrix);
                SortStrings(ref matrix);
                TransposeMatrix(ref matrix);
            }

            WriteMatrix("output.txt", matrix);
        }

        private static void ReadMatrix(string fileName, out int[][] matrix)
        {
            string text = System.IO.File.ReadAllText(fileName).Replace("\r\n", " ");
            int[] values = text.Split(' ').Select(n => int.Parse(n)).ToArray();

            CreateMatrix(out matrix, values[0], values[1]);

            int curValue = 2;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    matrix[i][j] = values[curValue];
                    curValue++;
                }
            }
        }

        private static void CreateMatrix(out int [][] matrix, int heigth, int width)
        {
            matrix = new int[heigth][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new int[width];
            }
        }

        private static void SortStrings(ref int[][] matrix)
        {
            int[] maxLengths = new int[matrix.GetLength(0)];

            for (int i = 0; i < matrix.Length; i++)
            {
                maxLengths[i] = FindMaxLength(matrix[i]);
            }

            for (int i = 0; i < maxLengths.Length; i++)
            {
                for (int j = i + 1; j < maxLengths.Length; j++)
                {
                    if (maxLengths[j] > maxLengths[i])
                    {
                        Swap<int>(ref maxLengths[i], ref maxLengths[j]);
                        Swap<int[]>(ref matrix[i], ref matrix[j]);
                    }
                }
            }
        }

        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        private static int FindMaxLength(int[] array)
        {
            int maxLength = 2;
            SequanceState curState, prevState;

            if (array.Length > 2)
            {
                if (array[0] < array[1])
                {
                    curState = prevState = SequanceState.Rising;
                }
                else
                {
                    curState = prevState = SequanceState.Falling;
                }

                for (int i = 1; i < array.Length - 1; i++)
                {
                    if (array[i] < array[i + 1])
                    {
                        curState = SequanceState.Rising;
                    }
                    else
                    {
                        curState = SequanceState.Falling;
                    }

                    if (prevState == curState) maxLength++;

                    prevState = curState;
                }
            }
            return maxLength;
        }

        private static void TransposeMatrix(ref int[][] matrix)
        {
            int[][] newMatrix;
            CreateMatrix(out newMatrix, matrix[0].Length, matrix.Length);

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    newMatrix[j][i] = matrix[i][j];
                }
            }

            matrix = newMatrix;
        }

        private static void WriteMatrix(string fileName, int[][] matrix)
        {
            string[] lines = new string[matrix.Length];

            for (int i = 0; i < lines.Length; i++)
            { 
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    lines[i] += matrix[i][j] + (j < matrix[0].Length - 1 ? " ":"");
                }
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }
    }
}
