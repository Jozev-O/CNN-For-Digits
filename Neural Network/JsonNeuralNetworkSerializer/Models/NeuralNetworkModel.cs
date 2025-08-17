//namespace Neural_Network.JsonNeuralNetworkSerializer.Models
//{
//    public class NeuralNetworkModel
//    {
//        public int InputSize { get; set; }
//        public int HiddenSize { get; set; }
//        public int OutputSize { get; set; }
//        public double[][] HiddenWeights { get; set; }
//        public double[] HiddenBiases { get; set; }
//        public double[][] OutputWeights { get; set; }
//        public double[] OutputBiases { get; set; }
//    }
//}
using System;

namespace Neural_Network.JsonNeuralNetworkSerializer.Models
{
    /// <summary>
    /// Represents the serialized structure of a neural network, storing layer sizes and weights.
    /// </summary>
    public class NeuralNetworkModel
    {
        /// <summary>
        /// Version of the model for compatibility checking during deserialization.
        /// </summary>
        public int Version { get; init; } = 1;

        /// <summary>
        /// Number of inputs to the network (e.g., 784 for MNIST pixels).
        /// </summary>
        public int InputLayerSize { get; init; }

        /// <summary>
        /// Number of neurons in the hidden layer.
        /// </summary>
        public int HiddenLayerNeuronCount { get; init; }

        /// <summary>
        /// Number of neurons in the output layer (e.g., 10 for MNIST digits).
        /// </summary>
        public int OutputLayerNeuronCount { get; init; }

        /// <summary>
        /// Weights for each neuron in the hidden layer (matrix: HiddenLayerNeuronCount x InputLayerSize).
        /// </summary>
        public double[][] HiddenLayerWeights { get; init; }

        /// <summary>
        /// Biases for each neuron in the hidden layer (array: HiddenLayerNeuronCount).
        /// </summary>
        public double[] HiddenLayerBiases { get; init; }

        /// <summary>
        /// Weights for each neuron in the output layer (matrix: OutputLayerNeuronCount x HiddenLayerNeuronCount).
        /// </summary>
        public double[][] OutputLayerWeights { get; init; }

        /// <summary>
        /// Biases for each neuron in the output layer (array: OutputLayerNeuronCount).
        /// </summary>
        public double[] OutputLayerBiases { get; init; }

        /// <summary>
        /// Initializes a new instance of the neural network model with validation.
        /// </summary>
        /// <param name="version">Model version for compatibility.</param>
        /// <param name="inputLayerSize">Number of inputs.</param>
        /// <param name="hiddenLayerNeuronCount">Number of hidden neurons.</param>
        /// <param name="outputLayerNeuronCount">Number of output neurons.</param>
        /// <param name="hiddenLayerWeights">Weights for hidden layer neurons.</param>
        /// <param name="hiddenLayerBiases">Biases for hidden layer neurons.</param>
        /// <param name="outputLayerWeights">Weights for output layer neurons.</param>
        /// <param name="outputLayerBiases">Biases for output layer neurons.</param>
        public NeuralNetworkModel(
            int version,
            int inputLayerSize,
            int hiddenLayerNeuronCount,
            int outputLayerNeuronCount,
            double[][] hiddenLayerWeights,
            double[] hiddenLayerBiases,
            double[][] outputLayerWeights,
            double[] outputLayerBiases)
        {
            if (version < 1)
                throw new ArgumentException("Version must be positive", nameof(version));
            if (inputLayerSize <= 0)
                throw new ArgumentException("Input layer size must be positive", nameof(inputLayerSize));
            if (hiddenLayerNeuronCount <= 0)
                throw new ArgumentException("Hidden layer neuron count must be positive", nameof(hiddenLayerNeuronCount));
            if (outputLayerNeuronCount <= 0)
                throw new ArgumentException("Output layer neuron count must be positive", nameof(outputLayerNeuronCount));
            if (hiddenLayerWeights == null || hiddenLayerWeights.Length != hiddenLayerNeuronCount)
                throw new ArgumentException("Hidden weights must match hidden neuron count", nameof(hiddenLayerWeights));
            if (hiddenLayerBiases == null || hiddenLayerBiases.Length != hiddenLayerNeuronCount)
                throw new ArgumentException("Hidden biases must match hidden neuron count", nameof(hiddenLayerBiases));
            if (outputLayerWeights == null || outputLayerWeights.Length != outputLayerNeuronCount)
                throw new ArgumentException("Output weights must match output neuron count", nameof(outputLayerWeights));
            if (outputLayerBiases == null || outputLayerBiases.Length != outputLayerNeuronCount)
                throw new ArgumentException("Output biases must match output neuron count", nameof(outputLayerBiases));
            if (hiddenLayerWeights.Any(w => w == null || w.Length != inputLayerSize))
                throw new ArgumentException("Each hidden weights array must match input layer size", nameof(hiddenLayerWeights));
            if (outputLayerWeights.Any(w => w == null || w.Length != hiddenLayerNeuronCount))
                throw new ArgumentException("Each output weights array must match hidden neuron count", nameof(outputLayerWeights));

            // Инициализируем свойства: они неизменяемы после создания, чтобы защитить данные модели
            Version = version;
            InputLayerSize = inputLayerSize;
            HiddenLayerNeuronCount = hiddenLayerNeuronCount;
            OutputLayerNeuronCount = outputLayerNeuronCount;
            HiddenLayerWeights = hiddenLayerWeights;
            HiddenLayerBiases = hiddenLayerBiases;
            OutputLayerWeights = outputLayerWeights;
            OutputLayerBiases = outputLayerBiases;
        }
    }
}