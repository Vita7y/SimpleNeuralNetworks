using System.Collections.Generic;
using System.Xml.Serialization;

namespace NeuralNet.NeuralNet
{
    public class Layer
    {
        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        public Layer(int numNeurons)
        {
            Neurons = new List<Neuron>(numNeurons);
        }

        public List<Neuron> Neurons { get; set; }

        [XmlIgnore]
        public int NeuronCount
        {
            get
            {
                return Neurons.Count;
            }
        }

    }
}
