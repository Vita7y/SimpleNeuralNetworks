using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NeuralNet;
using NeuralNetwork.NeuralNet;
using NeuralNetwork.Utils;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        NeuralNetwork.NeuralNet.NeuralNetwork _neuralNetwork;
        private nnParameters _param;

        public Form1()
        {
            InitializeComponent();

            _param = new nnParameters
            {
                LearningRate = 0.9,
                Layers = new List<int>() { 3, 3, 3 },
                InitValues = new List<double>() { 0.9, 0.1, 0.8},

                TrainCycles = 100000,
                TrainInput = new []{0.9,0.1,0.8},
                TrainOutput = new []{0.726, 0.708, 0.778}
            };

            propertyGrid.SelectedObject = _param;
            _neuralNetwork = new NeuralNet.NeuralNetwork(_param.LearningRate, _param.Layers);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork, 400, 100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _neuralNetwork.Run(_param.InitValues.ToArray());

            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork,400, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _neuralNetwork = new NeuralNetwork.NeuralNet.NeuralNetwork(_param.LearningRate, _param.Layers);
            for (int i = 0; i <_param.TrainCycles; i++)
                NeuralNetwork.NeuralNet.NeuralNetwork.Train(_neuralNetwork, _param.TrainInput, _param.TrainOutput);

            NetworkHelper.ToTreeView(treeView1, _neuralNetwork);
            NetworkHelper.ToPictureBox(pictureBox1, _neuralNetwork,400, 100);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _param.SerializeToFile(saveFileDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _param = SerializeHelper.DeserializeFromFile<nnParameters>(openFileDialog.FileName);
                propertyGrid.SelectedObject = _param;
            }
        }

        private void loadNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _neuralNetwork = SerializeHelper.DeserializeFromFile<NeuralNet.NeuralNetwork>(openFileDialog.FileName);
                propertyGrid.SelectedObject = _param;
            }
        }

        private void saveNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _neuralNetwork.SerializeToFile(saveFileDialog.FileName);
            }
        }
    }

}