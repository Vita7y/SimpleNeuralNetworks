using NeuralNet.NeuralNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NeuralNet
{
    public partial class Form1 : Form
    {
        NeuralNetwork _neuralNetwork;

        public Form1()
        {
            InitializeComponent();

            _neuralNetwork = new NeuralNetwork(0.9, new[] { 3, 3, 3 });
            propertyGrid.SelectedObject = _neuralNetwork;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork, 400, 100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> ins = new List<double>();
            ins.Add(0.9);
            ins.Add(0.1);
            ins.Add(0.8);

            _neuralNetwork.Run(ins);

            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork,400, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<double> ins = new List<double>();
            ins.Add(0.9);
            ins.Add(0.1);
            ins.Add(0.8);

            List<double> ots = new List<double>();
            ots.Add(0.726);
            ots.Add(0.708);
            ots.Add(0.778);

            for(int i = 0; i <100000; i++)
                _neuralNetwork.Train(ins, ots);

            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork,400, 100);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }

}