# TeoriaDosGrafos
Será postado os trabalhos e exercícios da disciplina

Para Usar o Mini Framework para criar um grafo.
Instancie,
Graph graph = new Graph();
Para Adicionar um vertice, Passe um valor em string, porque é o nome do no, pode dar qualquer nome.
graph.AddNode("0"); 
Para criar uma ligação entre dois vertices com uma aresta com PESO.
graph.AddAdjacency(graph.GetNodeById(0), graph.GetNodeById(1), 1);
graph.AddAdjacency(AQUI É O ID DO PRIMEIRO VERTICE, AQUI O ID DO SEGUNDO VERTICE, AQUI O PESO);
Para visualizar
graph.GraphShow();
Para Usar á Fila.
Instancie 
QueueGraph queueGraph = new QueueGraph();
Essa função para enfileirar
queueGraph.Enfileirar(graph.GetNodeById(0));
queueGraph.Enfileirar(AQUI O ID DO VERTICE PARA ENFILEIRAR);
E essa função para desenfileirar
queueGraph.Desenfileirar(); //ele já mostrar na posição correta os vertices desenfileirados.
