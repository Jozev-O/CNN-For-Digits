using Neural_Network.Interfaces;
using Neural_Network.NormalizationFuncs;

namespace Neural_Network
{
    public class NeuralNetwork
    {
        public Layer Hidden { get; }
        public Layer Output { get; }
        public double LearningRate { get; set; } = 0.1;
        private readonly INormalizationFunction _normalizationFunction;

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize,
            IActivationFunction activationFunction,
            INormalizationFunction normalizationFunction = null,
            Random? random = null)
        {
            if (inputSize <= 0 || hiddenSize <= 0 || outputSize <= 0)
                throw new ArgumentException("Input, hidden, and output sizes must be positive");

            Hidden = new Layer(inputSize, hiddenSize, activationFunction, random);
            Output = new Layer(hiddenSize, outputSize, activationFunction, random);
            _normalizationFunction = normalizationFunction ?? new SoftmaxNormalization();
        }

        public double[] Predict(double[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Length != Hidden.Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Hidden.Neurons[0].Weights.Length} inputs, but got {input.Length}");

            var hiddenOutputs = Hidden.Forward(input);
            var output = Output.Forward(hiddenOutputs);
            return _normalizationFunction.Normalize(output);
        }

        public void Train(double[] input, double[] target)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (input.Length != Hidden.Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Hidden.Neurons[0].Weights.Length} inputs, but got {input.Length}");
            if (target.Length != Output.Neurons.Length)
                throw new ArgumentException($"Expected {Output.Neurons.Length} targets, but got {target.Length}");

            // Прямой проход
            var hiddenOutputs = Hidden.Forward(input);
            var outputs = Output.Forward(hiddenOutputs);
            var normalizedOutputs = _normalizationFunction.Normalize(outputs);

            // Вычисляем ошибку выходного слоя
            var outputErrors = new double[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                if (double.IsNaN(target[i]) || double.IsInfinity(target[i]))
                    throw new ArgumentException($"Invalid target at index {i}: NaN or Infinity");
                outputErrors[i] = normalizedOutputs[i] - target[i];
            }

            // Обратное распространение
            var outputDeltas = Output.BackpropagateOutput(outputErrors, LearningRate);
            Hidden.BackpropagateHidden(outputDeltas, Output, LearningRate);
        }
    }
}