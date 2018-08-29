using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFramework
{
    public class QueueGraph
    {
        private readonly Queue<Node> _queue = new Queue<Node>();

        public void Enfileirar(Node node){
            _queue.Enqueue(node);
        }

        public void Desenfileirar(){
            int qtd = _queue.Count;
            Console.WriteLine("Retirando da fila: ");
            for (int i = 0; i < qtd; i++){
                Console.Write(_queue.Dequeue().Id +" ");
            }
        }
    }
}
