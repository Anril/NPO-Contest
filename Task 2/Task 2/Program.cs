using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    public class Data
    {
        public int price;
        public Node prevNode;
    }

    public class Node
    {
        public int name;
        public List<Edge> edges;
    }

    public class Edge
    {
        public Node first;
        public Node second;
    }

    public class Program
    {
        
        private static void Main(string[] args)
        {
            Node startNode, endNode;
            List<Node> listNodes = new List<Node>();
            List<Edge> listEdges = new List<Edge>();

            ReadFile("input.txt", listNodes, listEdges, out startNode, out endNode);

            List<Node> minPath = FindShortPath(listNodes , listEdges, startNode, endNode);

            WriteFile("output.txt", minPath);
        }

        private static void ReadFile(string fileName, List<Node> listNodes, 
            List<Edge> listEdges,
            out Node startNode, out Node endNode)
        {
            string text = System.IO.File.ReadAllText(fileName).Replace("\r\n", " ");
            int[] values = text.Split(' ').Select(n => int.Parse(n)).ToArray();

            int counNodes = values[0];

            for (int i = 0; i < counNodes; i++)
            {
                listNodes.Add(new Node { name = i + 1 });
            }

            startNode = listNodes[values[1] - 1];
            endNode = listNodes[values[2] - 1];

            int curValue = 3;
            for (int i = curValue; i <= values.Length-1; i++)
            {
                Edge tmp = new Edge();
                tmp.first = listNodes[values[i] - 1];
                i++;
                tmp.second = listNodes[values[i] - 1]; ;
                listEdges.Add(tmp);
            }

            for (int i = 0; i < listNodes.Count; i++)
            {
                listNodes[i].edges = new List<Edge>();
                for (int j = 0; j < listEdges.Count; j++)
                {
                    if (listNodes[i] == listEdges[j].first ||
                        listNodes[i] == listEdges[j].second)
                    {
                        listNodes[i].edges.Add(listEdges[j]);
                    }
                }
            }

        }

        private static List<Node> FindShortPath(List<Node> listNodes, 
            List<Edge> listEdges, Node startNode, Node endNode)
        {
            List<Node> notVisitedNodes = listNodes;
            var track = new Dictionary<Node, Data>();
            track.Add(startNode, new Data { prevNode = null, price = 0 });

            while (true)
            {
                Node curNode = null;
                int bestPrice = Int32.MaxValue;
                foreach (Node node in notVisitedNodes)
                {
                    if (track.ContainsKey(node) && track[node].price <= bestPrice)
                    {
                        curNode = node;
                        bestPrice = track[node].price;
                    }
                }

                if (curNode == endNode) break;
                if (curNode == null) return null;

                foreach (var e in curNode.edges.Where(p => p.first == curNode))
                {
                    int curPrice = track[curNode].price + 1;
                    var nextNode = e.second;
                    if (!track.ContainsKey(nextNode) || track[nextNode].price > curPrice)
                    {
                        track[nextNode] = new Data { price = curPrice, prevNode = curNode };
                    }
                }
                notVisitedNodes.Remove(curNode);
            }

            var result = new List<Node>();
            while (endNode != null)
            {
                result.Add(endNode);
                endNode = track[endNode].prevNode;
            }
            result.Reverse();

            return result;
        }

        private static void WriteFile(string fileName, List<Node> minPath)
        {
            string line = String.Empty;
            if (minPath != null)
            {
                foreach (var value in minPath)
                {
                    line += value.name.ToString() + " ";
                }
                System.IO.File.WriteAllText(fileName, line);
            }
        }
    }
}
