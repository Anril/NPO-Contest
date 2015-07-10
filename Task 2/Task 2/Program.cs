using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    public class Program
    {
        
        private static void Main(string[] args)
        {
            int startNode, endNode;
            int[,] edges;
            int[] shortPath;

            edges = ReadFile("input.txt", out startNode, out endNode);
            shortPath = FindShortPath(edges, startNode, endNode);

            WriteFile("output.txt", shortPath);
        }

        /// <summary>
        /// Read data from text file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="edges"></param>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private static int[,] ReadFile(string fileName, out int startNode, out int endNode)
        {
            string text = System.IO.File.ReadAllText(fileName).Replace("\r\n", " ");
            int[] values = text.Split(' ').Select(n => int.Parse(n)).ToArray();

            int[,] edges = new int[values[0], values[0]];
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                for (int j = 0; j < edges.GetLength(1); j++)
                {
                    edges[i, j] = 0;
                }
            }

            startNode = values[1] - 1;
            endNode = values[2] - 1;

            for (int i = 3; i < values.Length - 1; i++)
            {
                edges[values[i] - 1, values[++i] - 1] = 1;
            }
            return edges;
        }

        /// <summary>
        /// BFS algorithm
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private static int[] FindShortPath(int[,] edges, int startNode, int endNode)
        {
            Queue<int> queue = new Queue<int>();
            bool[] used = new bool[edges.GetLength(0)];
            int[] parrents = new int[edges.GetLength(0)];

            queue.Enqueue(startNode);
            used[startNode] = true;
            parrents[startNode] = startNode;

            while (queue.Count > 0)
            {
                int curNode = queue.Dequeue();
                for (int i = 0; i < edges.GetLength(1); i++)
                {
                    if (edges[curNode, i] == 1 && !used[i])
                    {
                        used[i] = true;
                        parrents[i] = curNode;
                        queue.Enqueue(i);
                    }
                }
            }

            List<int> shortPath = new List<int>();
            while (parrents[endNode] != endNode)
            {
                shortPath.Add(endNode);
                endNode = parrents[endNode];
                
            }
            shortPath.Add(parrents[endNode]);
            shortPath.Reverse();

            return shortPath.ToArray();
        }

        /// <summary>
        /// Write short path to text file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="shortPath"></param>
        private static void WriteFile(string fileName, int[] shortPath)
        {
            string line = String.Empty;
            if (shortPath != null)
            {
                foreach (var value in shortPath)
                {
                    line += (value + 1).ToString() + " ";
                }
                System.IO.File.WriteAllText(fileName, line);
            }
        }
    }
}
