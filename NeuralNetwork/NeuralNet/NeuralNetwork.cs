using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NeuralNet.NeuralNet;

namespace NeuralNetwork.NeuralNet
{
    public class NeuralNetwork
    {
        public NeuralNetwork()
        { }

        public NeuralNetwork(double learningRate, IReadOnlyList<int> layers)
        {
            Init(learningRate, layers);
        }

        public void Init(double learningRate, IReadOnlyList<int> layers)
        {
            if (layers.Count < 2)
                return;

            LearningRate = learningRate;
            Layers = new List<Layer>();

            for (var l = 0; l < layers.Count; l++)
            {
                var layer = new Layer(layers[l]);
                Layers.Add(layer);

                for (var n = 0; n < layers[l]; n++)
                    layer.Neurons.Add(new Neuron());

                layer.Neurons.ForEach(nn =>
                {
                    if (l == 0)
                        nn.Bias = 0;
                    else
                        for (var d = 0; d < layers[l - 1]; d++)
                            nn.Dendrites.Add(new Dendrite());
                });
            }
        }

        public List<Layer> Layers { get; set; }

        public double LearningRate { get; set; }

        [XmlIgnore]
        public int LayerCount => Layers.Count;

        private static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double[] Run(double[] input)
        {
            if (input.Length != Layers[0].NeuronCount) return null;

            for (var l = 0; l < Layers.Count; l++)
            {
                var layer = Layers[l];

                for (var n = 0; n < layer.Neurons.Count; n++)
                {
                    var neuron = layer.Neurons[n];

                    if (l == 0)
                    {
                        neuron.Value = input[n];
                    }
                    else
                    {
                        neuron.Value = 0;
                        for (var np = 0; np < Layers[l - 1].Neurons.Count; np++)
                            neuron.Value = neuron.Value + Layers[l - 1].Neurons[np].Value * neuron.Dendrites[np].Weight;

                        neuron.Value = Sigmoid(neuron.Value + neuron.Bias);
                    }
                }
            }

            var last = Layers[Layers.Count - 1];
            var numOutput = last.Neurons.Count;
            var output = new double[numOutput];
            for (var i = 0; i < last.Neurons.Count; i++)
                output[i] = last.Neurons[i].Value;

            return output;
        }

        public static bool Train(NeuralNetwork nn, double[] input, double[] output)
        {
            if (input.Length != nn.Layers[0].Neurons.Count
                || output.Length != nn.Layers[nn.Layers.Count - 1].Neurons.Count)
                return false;

            nn.Run(input);

            for (var i = 0; i < nn.Layers[nn.Layers.Count - 1].Neurons.Count; i++)
            {
                var neuron = nn.Layers[nn.Layers.Count - 1].Neurons[i];

                neuron.Delta = neuron.Value * (1 - neuron.Value) * (output[i] - neuron.Value);

                for (var j = nn.Layers.Count - 2; j >= 1; j--)
                for (var k = 0; k < nn.Layers[j].Neurons.Count; k++)
                {
                    var n = nn.Layers[j].Neurons[k];

                    n.Delta = n.Value *
                              (1 - n.Value) *
                              nn.Layers[j + 1].Neurons[i].Dendrites[k].Weight *
                              nn.Layers[j + 1].Neurons[i].Delta;
                }
            }

            for (var i = nn.Layers.Count - 1; i >= 1; i--)
            for (var j = 0; j < nn.Layers[i].Neurons.Count; j++)
            {
                var n = nn.Layers[i].Neurons[j];
                n.Bias = n.Bias + nn.LearningRate * n.Delta;

                for (var k = 0; k < n.Dendrites.Count; k++)
                    n.Dendrites[k].Weight = n.Dendrites[k].Weight +
                                            nn.LearningRate * nn.Layers[i - 1].Neurons[k].Value * n.Delta;
            }

            return true;
        }
    }
}