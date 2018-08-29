using System.Collections.Generic;

namespace GraphFramework{
    //Classe para o vertice
    public class Node {
        public string Name { get; set; }
        public int Id { get; private set; }
        public Dictionary<Node, Edge> AdjacencyList { get; set; }

        public Node(int id, string name){
            Name = name;
            AdjacencyList = new Dictionary<Node, Edge>();
            Id = id;
        }
    }
}
