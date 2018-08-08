//julio cesar rodrigues da costa
//1512082111
namespace Aula_07_08_18{
    class Program{
        static void Main(string[] args){
            Digraph digraph = new Digraph();
            digraph.DigraphInit(7);
            digraph.DigraphInsertA(1, 2);
            digraph.DigraphInsertA(1, 3);
            digraph.DigraphInsertA(2, 4);
            digraph.DigraphInsertA(3, 4);
            digraph.DigraphInsertA(4, 5);
            digraph.DigraphInsertA(5, 6);
            digraph.DigraphShow();
        }
    }
}