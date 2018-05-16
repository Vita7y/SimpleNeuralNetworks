using NeuralNet.NeuralNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NeuralNet
{
    public partial class Form1 : Form
    {
        NeuralNetwork nn = new NeuralNetwork(0.9, new int[] { 3, 3, 3 });

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NetworkHelper.ToTreeView(treeView1, nn);
            NetworkHelper.ToPictureBox(pictureBox1, nn, 400, 100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> ins = new List<double>();
            ins.Add(0.9);
            ins.Add(0.1);
            ins.Add(0.8);

            nn.Run(ins);

            NetworkHelper.ToTreeView(treeView1, nn);
            NetworkHelper.ToPictureBox(pictureBox1, nn,400, 100);
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
                nn.Train(ins, ots);

            NetworkHelper.ToTreeView(treeView1, nn);
            NetworkHelper.ToPictureBox(pictureBox1, nn,400, 100);
        }
    }

}