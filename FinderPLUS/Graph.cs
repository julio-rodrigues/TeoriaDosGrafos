using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FinderPLUS
{
    public class Graph
    {
        private int _nextId = 0;
        public Dictionary<int, Node> Nodes { get; set; }

        public Graph(){
            this.Nodes = new Dictionary<int, Node>();
        }

        public Node AddNode(string name, string latitude, string longitude, string profissao, bool disponibilidade)
        {
            var node = new Node(_nextId,name,latitude,longitude,profissao, disponibilidade);
            this.Nodes.Add(_nextId, node);
            _nextId++;
            return node;
        }

        public void AddAdjacency(Node node, Node dest, double cost)
        {
            var edge = new Edge(cost);
            if (!node.AdjacencyList.ContainsKey(dest))
                node.AdjacencyList.Add(dest, edge);
        }
        public string GetColorName(Color color)
        {
            if (color == Color.Red)
                return "red";
            if (color == Color.Yellow)
                return "yellow";
            if (color == Color.Green)
                return "green";
            if (color == Color.Blue)
                return "blue";
            if (color == Color.White)
                return "white";
            if (color == Color.Orange)
                return "orange";
            if (color == Color.Black)
                return "black";

            return "white";
        }

        public Node GetNodeById(int id)
        {
            if (!Nodes.ContainsKey(id))
                return null;
            return Nodes[id];
        }

        public Graph Copy()
        {
            var graph = (Graph)this.MemberwiseClone();

            graph.Nodes = new Dictionary<int, Node>();

            foreach (var node in Nodes)
            {
                var copyNode = node.Value.Copy();
                graph.Nodes.Add(node.Key, copyNode);
            }

            var edgeCache = new Dictionary<Edge, Edge>();

            foreach (var node in Nodes)
            {
                var copyNode = graph.GetNodeById(node.Key);

                foreach (var edge in node.Value.AdjacencyList)
                {
                    var destNode = graph.GetNodeById(edge.Key.GetId());

                    if (!edgeCache.ContainsKey(edge.Value))
                        edgeCache.Add(edge.Value, edge.Value.Copy());
                    copyNode.AdjacencyList.Add(destNode, edgeCache[edge.Value]);
                }
            }
            return graph;
        }
        private Node GetSmallestNode(IEnumerable<Node> nodes, Dictionary<Node, double> distances)
        {
            var min = double.MaxValue;
            Node selected = null;

            foreach (var node in nodes)
            {
                if (distances[node] >= min)
                    continue;

                min = distances[node];
                selected = node;
            }

            return selected;
        }
        public List<Graph> Dijkstra(Node startNode, Node endNode)
        {
            var steps = new List<Graph>();
            steps.Add(this.Copy());

            if (this.Nodes.Any(e => e.Value.AdjacencyList.Any(f => f.Value.Cost < 0)))
                return new List<Graph>();

            if (endNode == startNode)
            {
                startNode.Color = Color.Orange;
                steps.Add(this.Copy());
                return steps;
            }

            var nodes = new List<Node>(Nodes.Values);
            var distance = new Dictionary<Node, double>();
            var shortest = new Dictionary<Node, Node>();

            foreach (var node in Nodes)
            {
                distance.Add(node.Value, double.MaxValue);
                shortest.Add(node.Value, null);
                //node.Value.Color = Color.White;
            }

            distance[startNode] = 0;

            while (nodes.Count > 0)
            {
                var u = GetSmallestNode(nodes, distance);

                if (u == null)
                    break;

                nodes.Remove(u);

                //u.Color = Color.Blue;

                foreach (var v in u.AdjacencyList)
                {
                    if (distance[v.Key] > distance[u] + v.Value.Cost)
                    {
                        distance[v.Key] = distance[u] + v.Value.Cost;

                        if (shortest[v.Key] != null)
                            v.Key.AdjacencyList[shortest[v.Key]].Color = Color.Red;

                        shortest[v.Key] = u;

                        v.Value.Color = Color.Yellow;

                        steps.Add(this.Copy());

                        nodes.Add(v.Key);
                    }

                }

                //u.Color = Color.Orange;
            }

            Node aux = endNode;

            if (shortest[aux] == null)
                return new List<Graph>();

            while (aux != startNode)
            {
                var edge = startNode.AdjacencyList[aux];
                //aux.Color = Color.Blue;
                edge.Color = Color.Green;
                steps.Add(this.Copy());
                startNode.Color = Color.Blue;
                aux = shortest[aux];
            }

            steps.Add(this.Copy());

            return steps;
        }
        public string ToDotFormat()
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            Node[] nodecoy = new Node[Nodes.Count];
            string b = "digraph {";
            var addedEdges = new List<Edge>();
            foreach (var node in Nodes.Values){
                for (int i = 0; i < comAcentos.Length; i++){
                    node.Name = node.Name.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
                }
            }
            b += "{" + string.Join(";", Nodes.Select(e => e.Key + " [fillcolor=" + GetColorName(e.Value.Color) + ",style=filled,label=\"" + e.Value.Name + "\"]")) + "}";
            foreach (var node in Nodes.Values)
            {
                if (!string.IsNullOrEmpty(b))
                    b += ";";

                foreach (var edge in node.AdjacencyList)
                {
                    if (!string.IsNullOrEmpty(b))
                        b += ";";
                    if (!addedEdges.Contains(edge.Value))
                    {
                        b += node.GetId() + " -> " + edge.Key.GetId() +
                             "[label=\"" + edge.Value.Cost.ToString("F")+" KM" +
                             "\",Cost=\"" + edge.Value.Cost + "\",color=\"" +
                             GetColorName(edge.Value.Color) + "\"]";
                        addedEdges.Add(edge.Value);
                    }
                }
            }
            b += "}";
            return b;
        }
    }
    [Serializable]
    public class Node
    {
        public string Name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string profissao { get; set; }
        public bool disponibilidade { get;set; }
        private int Id { get; set;}
        public Dictionary<Node, Edge> AdjacencyList { get; set; }
        public Color Color { get; set; }


        public Node(int id, string name, string latitude, string longitude, string profissao, bool disponibilidade)
        {
            this.Name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.profissao = profissao;
            this.disponibilidade = disponibilidade;
            this.AdjacencyList = new Dictionary<Node, Edge>();
            this.Color = Color.White;
            this.Id = id;
        }

        public Node Copy()
        {
            var node = new Node(this.Id, this.Name, this.latitude, this.longitude, this.profissao, this.disponibilidade);
            node.Color = this.Color;
            return node;
        }

        public int GetId()
        {
            return Id;
        }
    }
    [Serializable]
    public class Edge
    {
        public double Cost { get; set; }
        public Color Color { get; set; }

        public Edge(double cost)
        {
            this.Cost = cost;
            this.Color = Color.Black;
        }

        public Edge Copy()
        {
            var edge = new Edge(this.Cost);
            edge.Color = this.Color;
            return edge;
        }
    }
}
