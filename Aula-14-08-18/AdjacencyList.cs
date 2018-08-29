using System;
using System.Collections.Generic;

namespace Aula_14_08_18
{
    public class AdjacencyList
    {
        public Queue<string> queue = new Queue<string>();
        private LinkedList<Tuple<int, int>>[] _adjacencyList;

        public void GrapHinit(int v){
            _adjacencyList = new LinkedList<Tuple<int, int>>[v];
            for (int i = 0 ; i < _adjacencyList.Length; i++){
                _adjacencyList[i] = new LinkedList<Tuple<int, int>>();
            }
        }

        public void GrapHinsertE(int inicio, int final, int peso){
            _adjacencyList[inicio].AddLast(new Tuple<int, int>(final, peso));
            //_adjacencyList[final].AddLast(new Tuple<int, int>(inicio, peso));
        }

        public void GraphShow(){
            int i = 0;
            foreach (LinkedList<Tuple<int,int>> list in _adjacencyList){
                Console.Write("[" + i + "] : ");
                foreach (Tuple<int,int> edges in list){
                        Console.Write(edges.Item1 + "(" + edges.Item2 + ") ");
                }
                i++;
                Console.WriteLine();
            }
        }
        public void Enfileirar(){
            foreach (LinkedList<Tuple<int, int>> list in _adjacencyList){
                foreach (Tuple<int, int> edges in list){
                    queue.Enqueue(edges.Item1.ToString());
                }
            }
        }

        public void Desenfileirar(){
            int qtd = queue.Count;
            for (int i = 0; i < qtd; i++){
                Console.Write(queue.Dequeue()+" ");
            }
        }
    }
}
