using System;
namespace Aula_07_08_18{
    public class Digraph{

        private int v;
        private int a;
        private int[,] matrix;

        public int[,] MATRIXInt(int l, int c, int val){
            int i, j;
            int[,] grafo = new int[l, c];
            for (i = 1; i < l; i++){
                for (j = 1; j < c; j++){
                    grafo[i, j] = val;
                }
            }
            return grafo;
        }
        public void DigraphInit(int val){
            v = val;
            a = 0;
            matrix = MATRIXInt(v, v, 0);
        }
        public void DigraphInsertA(int v, int w){
            if (matrix[v, w] == 0){
                matrix[v, w] = 1;
                matrix[w, v] = 1;
                a++;
            }
        }
        public void DigraphShow(){
            for (int i = 1; i <= 6; i++){
                for (int j = 1; j <= 6; j++){
                    Console.Write(matrix[i, j] + "   ");
                }
                Console.WriteLine("\n");
            }
            Console.ReadKey();
        }
    }
}
