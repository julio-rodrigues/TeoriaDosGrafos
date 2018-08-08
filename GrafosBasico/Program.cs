using System;
namespace GrafosBasico {
    public class Program {
        public static void Main(string[] args) {
            Vertex[] vertex = new Vertex[6];
            Arc[] r = new Arc[7];
            vertex[0] = new Vertex { Name = "a" };
            vertex[1] = new Vertex { Name = "v" };
            vertex[2] = new Vertex { Name = "d" };
            vertex[3] = new Vertex { Name = "f" };
            vertex[4] = new Vertex { Name = "w" };
            vertex[5] = new Vertex { Name = "c" };
            r[0] = CriaArc(vertex[0], vertex[1]);
            r[1] = CriaArc(vertex[1], vertex[2]);
            r[2] = CriaArc(vertex[2], vertex[3]);
            r[3] = CriaArc(vertex[0], vertex[5]);
            r[4] = CriaArc(vertex[5], vertex[4]);
            r[5] = CriaArc(vertex[4], vertex[3]);
            r[6] = CriaArc(vertex[1], vertex[4]);
            for (int i=0; i <= 6; i++){
                Console.WriteLine(r[i].V.Name + "->" + r[i].W.Name);
            }
            Console.ReadKey();
        }
        public static Arc CriaArc(Vertex inicial, Vertex final){
            Arc retorno = new Arc{
                V = inicial,
                W = final
            };
            return retorno;
        }
    }
}
