using System;

namespace Aula_14_08_18
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjacencyList adjacencyList = new AdjacencyList();
            adjacencyList.GrapHinit(6);
            adjacencyList.GrapHinsertE(0, 2, 1);
            adjacencyList.GrapHinsertE(0, 3, 1);
            adjacencyList.GrapHinsertE(0, 4, 1);
            adjacencyList.GrapHinsertE(1, 2, 1);
            adjacencyList.GrapHinsertE(1, 4, 1);
            adjacencyList.GrapHinsertE(2, 4, 1);
            adjacencyList.GrapHinsertE(3, 4, 1);
            adjacencyList.GrapHinsertE(3, 5, 1);
            adjacencyList.GrapHinsertE(4, 5, 1);
            adjacencyList.GrapHinsertE(5, 1, 1);
            adjacencyList.GraphShow();

            Console.WriteLine("Enfileirando");
            adjacencyList.Enfileirar();
            Console.WriteLine("Desenfileirando: ");
            adjacencyList.Desenfileirar();




            Console.ReadKey();

        }
    }
}
