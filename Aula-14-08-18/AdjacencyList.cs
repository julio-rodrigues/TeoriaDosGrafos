using System;
using System.Collections.Generic;

namespace Aula_14_08_18
{
    public class AdjacencyList
    {
        private LinkedList<Tuple<int, int>>[] adjacencyList;

        public void GRAPHinit(int V){
            adjacencyList = new LinkedList<Tuple<int, int>>[V];
            for (int i = 0 ; i < adjacencyList.Length; i++){
                adjacencyList[i] = new LinkedList<Tuple<int, int>>();
            }
        }

        public void GRAPHinsertE(int inicio, int final, int peso){
            adjacencyList[inicio].AddLast(new Tuple<int, int>(final, peso));
            adjacencyList[final].AddLast(new Tuple<int, int>(inicio, peso));
        }

        public void GRAPHShow(){
            int i = 0;
            foreach (LinkedList<Tuple<int,int>> List in adjacencyList){
                if(i!=0)
                    Console.Write("[" + i + "] : ");
                foreach (Tuple<int,int> edges in List){
                    if(i!=0)
                    Console.Write(edges.Item1+" ");
                }
                i++;
                if(i!=0)
                Console.WriteLine();
            }
        }



    }
}
