using System.Collections.Generic;

namespace NeuralNetwork.NeuralNet
{
    public class nnParameters
    {
        public nnParameters()
        {
            Layers = new List<int>();
            InitValues = new List<double>();
        }

        public double LearningRate { get; set; }

        public  List<int> Layers { get; set; }

        public List<double> InitValues { get; set; }

        public int TrainCycles { get; set; }

        public double[] TrainInput { get; set; }

        public double[] TrainOutput { get; set; }
    }
}