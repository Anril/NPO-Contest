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
            int[][] matrix = ReadMatrix("input.txt");

            if (matrix.Length > 0)
            {
                int[] maxLengths = FindMaxLengths(matrix);
                SortRows(matrix, 0, matrix.Length - 1, maxLengths);
                TransposeMatrix(ref matrix);

                maxLengths = FindMaxLengths(matrix);
                SortRows(matrix, 0, matrix.Length - 1, maxLengths);
                TransposeMatrix(ref matrix);
            }

            WriteMatrix("output.txt", matrix);
        }

        /// <summary>
        /// Read matrix from text file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static int[][] ReadMatrix(string fileName)
        {
            string text = System.IO.File.ReadAllText(fileName).Replace("\r\n", " ");
            int[] values = text.Split(' ').Select(n => int.Parse(n)).ToArray();

            int[][] tmpMatrix = CreateMatrix(values[0], values[1]);

            int curValue = 2;
            for (int i = 0; i < tmpMatrix.Length; i++)
            {
                for (int j = 0; j < tmpMatrix[0].Length; j++)
                {
                    tmpMatrix[i][j] = values[curValue];
                    curValue++;
                }
            }
            return tmpMatrix;
        }

        /// <summary>
        /// Matrix initialization
        /// </summary>
        /// <param name="heigth"></param>
        /// <param name="width"></param>
        private static int[][] CreateMatrix(int heigth, int width)
        {
            int[][] tmpMatrix = new int[heigth][];
            for (int i = 0; i < tmpMatrix.Length; i++)
            {
                tmpMatrix[i] = new int[width];
            }
            return tmpMatrix;
        }

        /// <summary>
        /// Sort rows using maximum lengths of sequences. Quick sort
        /// </summary>
        /// <param name="matrix"></param>
        private static void SortRows(int[][] matrix, int left, int right, int[] maxLengths)
        {
            int center = maxLengths[left + (right - left) / 2];

            int i = left;
            int j = right;

            while (i <= j)
            {
                while (maxLengths[i] > center) i++;
                while (maxLengths[j] < center) j--;
                if (i <= j)
                {
                    if (maxLengths[i] != maxLengths[j])
                    {
                        Swap<int>(ref maxLengths[i], ref maxLengths[j]);
                        Swap<int[]>(ref matrix[i], ref matrix[j]);
                    }
                    i++;
                    j--;
                }
                
            }

            if (i < right) SortRows(matrix, i, right, maxLengths);
            if (left < j) SortRows(matrix, left, j, maxLengths);
        }

        /// <summary>
        /// Exchange values of the variables
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        /// <summary>
        /// Returns array of maximum lengths of sequence from each row
        /// </summary>
        /// <param name="array"></param>
        private static int[] FindMaxLengths(int[][] matrix)
        {
            int[] result = new int[matrix.GetLength(0)];

            for (int i = 0; i < matrix.Length; i++)
            {
                int maxLength = 2;
                SequanceState curState, prevState;

                if (matrix[i].Length > 2)
                {
                    if (matrix[i][0] < matrix[i][1])
                    {
                        curState = prevState = SequanceState.Rising;
                    }
                    else
                    {
                        curState = prevState = SequanceState.Falling;
                    }

                    for (int j = 1; j < matrix[i].Length - 1; j++)
                    {
                        if (matrix[i][j] < matrix[i][j + 1])
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
                result[i] = maxLength;
            }
            return result;
        }

        /// <summary>
        /// Transpose of the matrix
        /// </summary>
        /// <param name="matrix"></param>
        private static void TransposeMatrix(ref int[][] matrix)
        {
            int[][] newMatrix = CreateMatrix(matrix[0].Length, matrix.Length);

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    newMatrix[j][i] = matrix[i][j];
                }
            }

            matrix = newMatrix;
        }

        /// <summary>
        /// Write matrix to a text file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="matrix"></param>
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
