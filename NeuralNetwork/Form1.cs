using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using NeuralNetwork.NeuralNet;
using NeuralNetwork.Utils;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        NeuralNet.NeuralNetwork _neuralNetwork;
        private nnParameters _param;
        OpenFileDialog openImageDialog;
        private BackgroundWorker _trainWorker;


        public Form1()
        {
            InitializeComponent();

            _param = new nnParameters
            {
                LearningRate = 0.9,
                Layers = new List<int>() {3, 3, 3},
                InitValues = new List<double>() {0.9, 0.1, 0.8},

                TrainCycles = 100000,
                TrainInput = new[] {0.9, 0.1, 0.8},
                TrainOutput = new[] {0.726, 0.708, 0.778}
            };

            propertyGridTrain.SelectedObject = _param;
            _neuralNetwork = new NeuralNet.NeuralNetwork(_param.LearningRate, _param.Layers);
            propertyGridNN.SelectedObject = _neuralNetwork;

            _trainWorker = new BackgroundWorker();
            _trainWorker.DoWork += TrainWorkerOnDoWork;
            _trainWorker.ProgressChanged+= TrainWorkerOnProgressChanged;
            _trainWorker.RunWorkerCompleted+= TrainWorkerOnRunWorkerCompleted;
        }

        private void TrainWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            

        }

        private void TrainWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            

        }

        private void TrainWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _neuralNetwork = new NeuralNet.NeuralNetwork(_param.LearningRate, _param.Layers);
            for (int i = 0; i < _param.TrainCycles; i++)
            {
                NeuralNet.NeuralNetwork.Train(_neuralNetwork, _param.TrainInput, _param.TrainOutput);
                _trainWorker.ReportProgress();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            _neuralNetwork.Run(_param.InitValues.ToArray());
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {

            propertyGridNN.SelectedObject = _neuralNetwork;
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
                propertyGridTrain.SelectedObject = _param;
            }
        }

        private void loadNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _neuralNetwork = SerializeHelper.DeserializeFromFile<NeuralNet.NeuralNetwork>(openFileDialog.FileName);
                propertyGridTrain.SelectedObject = _param;
            }
        }

        private void saveNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _neuralNetwork.SerializeToFile(saveFileDialog.FileName);
            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var decoder = BitmapDecoder.Create(new Uri(openImageDialog.FileName), BitmapCreateOptions.None, BitmapCacheOption.None);
                    if (decoder.Frames.Count > 0)
                    {
                        var image = decoder.Frames[0];
                        pictureBox.Load(openImageDialog.FileName);

                        var width = image.PixelWidth;
                        var pixelCount = width * width;
                        var pixelData = new double[pixelCount];
                        image.CopyPixels(pixelData, width * 4, 0);
                        for (var i = 0; i < pixelData.Length; i++)
                            pixelData[i] = pixelData[i] != 0 ? 1 : 0;
                        _param.Layers.Clear();
                        _param.Layers.AddRange(new[] {pixelData.Length, pixelData.Length, 1});
                        _param.InitValues.Clear();
                        _param.InitValues.AddRange(pixelData.ToList());
                        _param.TrainInput = pixelData;
                        _param.TrainOutput = new[] {0.0};
                        propertyGridTrain.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}