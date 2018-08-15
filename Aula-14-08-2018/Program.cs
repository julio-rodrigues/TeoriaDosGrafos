using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula_14_08_2018
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjacencyList adjacencyList = new AdjacencyList();

            //Console.WriteLine("Entre com á quantidade de vertices");
            //int vertices = Int32.Parse(Console.ReadLine());
            adjacencyList.GRAPHinit(7);

            //Console.WriteLine("Entre com a quantidade de arestas");
            //int arestas = Int32.Parse(Console.ReadLine());

            adjacencyList.GRAPHinsertE(1, 2, 0);
            adjacencyList.GRAPHinsertE(1, 3, 0);
            adjacencyList.GRAPHinsertE(2, 4, 0);
            adjacencyList.GRAPHinsertE(3, 4, 0);
            adjacencyList.GRAPHinsertE(4, 5, 0);
            adjacencyList.GRAPHinsertE(5, 6, 0);

            //for (int i = 1; i <= arestas; i++){
            //    Console.WriteLine("Digite o Inicio - ");
            //    int inicio = Int32.Parse(Console.ReadLine());
            //    Console.WriteLine("Digite o Final - ");
            //    int final = Int32.Parse(Console.ReadLine());
            //    adjacencyList.GRAPHinsertE(inicio,final,0);
            //}
            adjacencyList.GRAPHShow();
            Console.ReadKey();

        }
    }
}
