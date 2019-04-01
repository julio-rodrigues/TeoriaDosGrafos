using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.Device.Location;

namespace FinderPLUS
{
    public partial class Form1 : Form, IMessageFilter
    {
        public Form1()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            this.panel1.MouseWheel += pictureBox1_MouseWheel;
            this.Steps = new List<Graph>();
        }
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        public Graph CurrentGraph { get; set; }
        public Graph BaseGraph { get; set; }
        public GraphGeneration Generator { get; set; }
        public Image OriginalBitmap { get; set; }
        public Image CurrentBitmap { get; set; }
        private int CurrentStep = 0;
        private List<Graph> Steps { get; set; }

        private const int scrollSpeed = 3;
        int _picWidth, _picHeight, _zoomInt = 100;
        private double _picRatio;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.CurrentGraph = new Graph();
            BaseGraph = new Graph();
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            this.Generator = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
        }

        private bool _isPanning = false;
        private Point _startPt;
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x20a)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            return false;
        }

        public void GetRatio()
        {
            _picRatio = ((double)_picHeight / (double)_picWidth);// needed for aspect ratio

        }
        public void ZoomPictureBox()
        {
            pictureBox1.Width = _picWidth;
            pictureBox1.Height = _picHeight;

            pictureBox1.Width = Convert.ToInt32(((double)pictureBox1.Width) * (_zoomInt * 0.01));
            pictureBox1.Height = Convert.ToInt32(((double)pictureBox1.Width) * _picRatio);

            pictureBox1.Update();
        }

        private void CenterImage()
        {
            int x = (panel1.Width - pictureBox1.Width) / 2;
            int y = (panel1.Height - pictureBox1.Height) / 2;

            pictureBox1.Location = new Point(x, y);
        }
        private void RefreshGraphDraw()
        {
            try
            {
                using (
                    MemoryStream ms =
                        new MemoryStream(
                            Generator.GenerateGraph(this.CurrentGraph.ToDotFormat(), Enums.GraphReturnType.Png)
                                .ToArray()))
                {
                    var img = Image.FromStream(ms);
                    this.pictureBox1.Image = img;
                    this.OriginalBitmap = this.pictureBox1.Image;
                    pictureBox1.Image = img;
                    pictureBox1.Width = img.Width;
                    pictureBox1.Height = img.Height;
                    _picWidth = pictureBox1.Width;
                    _picHeight = pictureBox1.Height;
                    GetRatio();
                    ZoomPictureBox();
                    this.CurrentBitmap = this.pictureBox1.Image;
                }
            }
            catch (Exception e)
            {
                var result = MessageBox.Show("Não foi possível renderizar o grafo. Gostaria de tentar novamente?\r\n\r\nMOTIVO: " + e.Message + "\r\n\r\nDetalhes do erro: " +
                    e.StackTrace, "Erro ao renderizar grafo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                    RefreshGraphDraw();
            }
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoomInt += scrollSpeed;
                if (_zoomInt > 500)
                {
                    _zoomInt = 500;
                    return;
                }
                ZoomPictureBox();
            }
            else if (e.Delta < 0)
            {
                _zoomInt -= scrollSpeed;
                if (_zoomInt <= 10)
                {
                    _zoomInt = 10;
                    return;
                }
                ZoomPictureBox();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _isPanning = true;
            _startPt = e.Location;
            Cursor = Cursors.SizeAll;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filecontend = string.Empty;
            var file = string.Empty;
            string[] trabalhadores;
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.InitialDirectory = Path.Combine(Application.StartupPath);
                open.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    file = open.FileName;
                    var filestream = open.OpenFile();
                    using (StreamReader reader = new StreamReader(filestream, Encoding.GetEncoding("iso-8859-1")))
                    {
                        filecontend = reader.ReadToEnd();
                    }
                }
            }
            filecontend = filecontend.Replace("\r","");
            trabalhadores = filecontend.Split(',', ';', '\n');
            for (int i = 0; i < trabalhadores.Length; i=i+5)
            {
                trabalhadores[i + 1] = trabalhadores[i+1].Replace('.',',');
                trabalhadores[i + 2] = trabalhadores[i+2].Replace(".", ",");
            }
            CurrentGraph.AddNode("julio", "-15,836073", "-47,912019","estudante",true);
            for (int i = 0; i < trabalhadores.Length; i=i+5){
                if(bool.Parse(trabalhadores[i+4]) == true)
                    CurrentGraph.AddNode(trabalhadores[i],trabalhadores[i+1],trabalhadores[i+2],trabalhadores[i+3],bool.Parse(trabalhadores[i+4]));
            }
            for (int i = 0; i < CurrentGraph.Nodes.Count; i++){
                for (int j = 0; j < CurrentGraph.Nodes.Count; j++){
                    if(i != j)
                        CurrentGraph.AddAdjacency(CurrentGraph.GetNodeById(i),CurrentGraph.GetNodeById(j), converterCord(CurrentGraph.GetNodeById(i).latitude, CurrentGraph.GetNodeById(j).latitude, CurrentGraph.GetNodeById(i).longitude, CurrentGraph.GetNodeById(j).longitude));
                }
            }
            listView1.Columns.Add("Nome",121);
            listView1.Columns.Add("Profissão", 121);
            List<string> Adicionado = new List<string>();
            for (int i = 1 ; i < CurrentGraph.Nodes.Count; i++){
                var item1 = new ListViewItem(new[] { CurrentGraph.GetNodeById(i).Name, CurrentGraph.GetNodeById(i).profissao });
                //var item1 = new ListViewItem(new[] { CurrentGraph.GetNodeById(i).profissao });
                item1.Tag = CurrentGraph.GetNodeById(i);
                var node = (Node)item1.Tag;
                //if (!Adicionado.Contains(node.profissao)) {
                    listView1.Items.Add(item1);
                    //Adicionado.Add(node.profissao);
                //}
            }
            BaseGraph = CurrentGraph.Copy();
            RefreshGraphDraw();
        }

        private double converterCord(string latA, string LatB, string LonA, string LongB)
        {
            var sCoord = new GeoCoordinate(double.Parse(latA), double.Parse(LonA));
            var eCoord = new GeoCoordinate(double.Parse(LatB), double.Parse(LongB));
            return sCoord.GetDistanceTo(eCoord)/1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Node> nodeselected = new List<Node>();

            nodeselected.Add(CurrentGraph.GetNodeById(0));

            //foreach (ListViewItem item in listView1.SelectedItems) {
            //    var node = (Node)item.Tag;
            //    nodeselected.Add(CurrentGraph.GetNodeById(node.GetId()));
            //}

            foreach (ListViewItem item in listView1.SelectedItems) {
                var node = (Node)item.Tag;
                foreach (var item2 in CurrentGraph.Nodes) {
                    if (item2.Value.Name == node.Name) {
                        nodeselected.Add(CurrentGraph.GetNodeById(item2.Value.GetId()));
                    }
                }
            }

            Queue<Node> nodeFila = new Queue<Node>();
            nodeFila.Enqueue(CurrentGraph.GetNodeById(0));
            Node nodePai= null;
            Node inicio=null;
            Node fim=null;
            List<int> visited = new List<int>();
            List<string> visitedProf = new List<string>();
            double distancia = double.MaxValue;
            double distanciaAux = 0;
            double distanciaTotal = 0;
            var flag = 0;
            while (nodeFila.Count > 0 && visited.Count <= nodeselected.Count) {
                nodePai = nodeFila.Dequeue();
                foreach (var item in nodeselected){
                    if (!visitedProf.Contains(item.profissao) && nodePai.profissao != item.profissao && nodePai != null && nodePai.Name != item.Name && !visited.Contains(nodePai.GetId()) && !visited.Contains(item.GetId()) && CurrentGraph.GetNodeById(nodePai.GetId()).AdjacencyList[CurrentGraph.GetNodeById(item.GetId())].Cost <= distancia) {
                        distancia = CurrentGraph.GetNodeById(nodePai.GetId()).AdjacencyList[CurrentGraph.GetNodeById(item.GetId())].Cost;
                        if (double.Parse(textBox1.Text) >= distancia) {
                            distanciaAux += distancia;
                            if (distanciaAux <= double.Parse(textBox1.Text) && !visitedProf.Contains(item.profissao)) {
                                inicio = CurrentGraph.Nodes[CurrentGraph.Nodes.Keys.ToArray()[nodePai.GetId()]];
                                fim = CurrentGraph.Nodes[CurrentGraph.Nodes.Keys.ToArray()[item.GetId()]];
                                Steps = CurrentGraph.Dijkstra(inicio, fim);
                            }
                        }
                     }
                 }
                nodeFila.Enqueue(fim);
                if (inicio != null) {
                    if (!visitedProf.Contains(fim.profissao)) {
                        distanciaTotal += inicio.AdjacencyList[fim].Cost;
                    }
                    flag = 0;
                    visited.Add(inicio.GetId());
                    visitedProf.Add(fim.profissao);
                    distancia = double.MaxValue;
                    CurrentStep = Steps.Count - 1;
                    CurrentGraph = Steps[CurrentStep];
                    RefreshGraphDraw();
                } else {
                    MessageBox.Show("Não existe nenhum serviço nas proximidades");
                    return;
                }

            }
            if(inicio != null) {
                inicio = CurrentGraph.Nodes[CurrentGraph.Nodes.Keys.ToArray()[nodePai.GetId()]];
                fim = CurrentGraph.Nodes[CurrentGraph.Nodes.Keys.ToArray()[0]];
                Steps = CurrentGraph.Dijkstra(inicio, fim);
                CurrentStep = Steps.Count - 1;
                CurrentGraph = Steps[CurrentStep];
                RefreshGraphDraw();
            }
            CurrentGraph = BaseGraph.Copy();
            distanciaTotal += inicio.AdjacencyList[fim].Cost;
            MessageBox.Show("Total Percorrido "+distanciaTotal.ToString("F")+" KM");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Steps.Clear();
            CurrentStep = -1;
            //
            //RefreshGraphDraw();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                //Cursor = Cursors.Hand;
                Cursor = Cursors.SizeAll;
                Control c = (Control)sender;
                c.Left = (c.Left + e.X) - _startPt.X;
                c.Top = (c.Top + e.Y) - _startPt.Y;
                c.BringToFront();

            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isPanning = false;
            Cursor = Cursors.Default;
        }

    }

}