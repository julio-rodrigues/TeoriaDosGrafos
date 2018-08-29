using System;
using System.Collections.Generic;

namespace GraphFramework{
    public class Graph{
        private int _nextId = 0;
        public Dictionary<int, Node> Nodes { get; set; }

        //Inicializar o grafo
        public Graph(){
            Nodes = new Dictionary<int, Node>();
        }

        //Adicionar um vertice
        public Node AddNode(string name){
            var node = new Node(_nextId, name);
            Nodes.Add(_nextId, node);
            _nextId++;
            return node;
        }

        //Criar uma aresta não direcionada entre dois vertices com pesos
        public void AddAdjacency(Node node, Node dest, int cost) {
            var edge = new Edge(cost);
            //Criar Nomal
            if (!node.AdjacencyList.ContainsKey(dest))
                node.AdjacencyList.Add(dest, edge); ;
            //Criar Espelhado
            if (!dest.AdjacencyList.ContainsKey(node))
                dest.AdjacencyList.Add(node, edge);
        }

        //Buscar um Vertice pelo ID
        public Node GetNodeById(int id) {
            if (!Nodes.ContainsKey(id))
                return null;
            return Nodes[id];
        }

        //Mostra O grafo
        public void GraphShow(){
            foreach (var node in Nodes.Values){
                foreach (var edge in node.AdjacencyList){
                    Console.WriteLine(node.Id+" => "+edge.Key.Id+" Custo: "+ edge.Value.Cost);
                }
            }
        }
    }
}
