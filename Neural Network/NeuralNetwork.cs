using Neural_Network.Interfaces;

namespace Neural_Network
{
    public class NeuralNetwork
    {
        public Layer Hidden { get; }
        public Layer Output { get; }
        private double _learningRate = 0.1;
        public double LearningRate
        {
            get => _learningRate;
            set
            {
                if (value <= 0 || double.IsNaN(value) || double.IsInfinity(value))
                    throw new ArgumentException("Learning rate must be positive and finite");
                _learningRate = value;
            }
        }
        private readonly INormalizationFunction _normalizationFunction;

        /// <summary>
        /// Инициализирует нейронную сеть с одним скрытым и одним выходным слоем.
        /// </summary>
        /// <param name="inputSize">Количество входов (например, 784 для MNIST).</param>
        /// <param name="hiddenSize">Количество нейронов в скрытом слое.</param>
        /// <param name="outputSize">Количество нейронов в выходном слое (например, 10 для цифр).</param>
        /// <param name="activationFunction">Функция активации для нейронов.</param>
        /// <param name="normalizationFunction">Функция нормализации выходов (например, Softmax).</param>
        /// <param name="random">Генератор случайных чисел для инициализации весов.</param>
        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize,
            IActivationFunction activationFunction,
            INormalizationFunction normalizationFunction,
            Random random)
        {
            if (inputSize <= 0 || hiddenSize <= 0 || outputSize <= 0)
                throw new ArgumentException("Input, hidden, and output sizes must be positive");


            ArgumentNullException.ThrowIfNull(activationFunction);
            ArgumentNullException.ThrowIfNull(normalizationFunction);


            // Инициализируем слои: скрытый слой обрабатывает входы, выходной даёт предсказания
            Hidden = new Layer(inputSize, hiddenSize, activationFunction, random);
            Output = new Layer(hiddenSize, outputSize, activationFunction, random);
            _normalizationFunction = normalizationFunction;
        }

        /// <summary>
        /// Initializes a neural network with predefined weights and biases for deserialization.
        /// </summary>
        public NeuralNetwork(
            double[][] hiddenWeights, double[] hiddenBiases,
            double[][] outputWeights, double[] outputBiases,
            IActivationFunction activationFunction,
            INormalizationFunction normalizationFunction)
        {
            if (hiddenWeights == null || hiddenBiases == null || outputWeights == null || outputBiases == null)
                throw new ArgumentNullException("Weights and biases cannot be null");

            ArgumentNullException.ThrowIfNull(activationFunction);
            ArgumentNullException.ThrowIfNull(normalizationFunction);


            Hidden = new Layer(hiddenWeights, hiddenBiases, activationFunction);
            Output = new Layer(outputWeights, outputBiases, activationFunction);
            _normalizationFunction = normalizationFunction;
        }


        /// <summary>
        /// Выполняет предсказание: пропускает входы через сеть и возвращает нормализованные выходы.
        /// </summary>
        /// <param name="input">Входные данные (например, пиксели изображения).</param>
        /// <returns>Нормализованные выходы (например, вероятности для каждой цифры).</returns>
        public double[] Predict(double[] input)
        {
            ValidateInput(input);
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Length != Hidden.Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Hidden.Neurons[0].Weights.Length} inputs, but got {input.Length}");

            // Прямой проход: входы -> скрытый слой -> выходной слой -> нормализация
            var hiddenLayerOutputs = Hidden.Forward(input);
            var outputLayerOutputs = Output.Forward(hiddenLayerOutputs);
            return _normalizationFunction.Normalize(outputLayerOutputs);
        }

        /// <summary>
        /// Обучает сеть на одном примере, корректируя веса на основе ошибки.
        /// </summary>
        /// <param name="input">Входные данные (например, пиксели изображения).</param>
        /// <param name="target">Целевые значения (например, one-hot вектор для цифры).</param>
        public void Train(double[] input, double[] target)
        {
            ValidateInput(input);
            ArgumentNullException.ThrowIfNull(target, nameof(target));

            if (target.Length != Output.Neurons.Length)
                throw new ArgumentException($"Expected {Output.Neurons.Length} targets, but got {target.Length}");
            if (target.Any(double.IsNaN) || target.Any(double.IsInfinity))
                throw new ArgumentException("Target contains NaN or Infinity values");


            //if (input.Length != Hidden.Neurons[0].Weights.Length)
            //    throw new ArgumentException($"Expected {Hidden.Neurons[0].Weights.Length} inputs, but got {input.Length}");


            // Прямой проход: вычисляем выходы сети для предсказания
            var hiddenLayerOutputs = Hidden.Forward(input);
            var outputLayerOutputs = Output.Forward(hiddenLayerOutputs);
            var normalizedOutputs = _normalizationFunction.Normalize(outputLayerOutputs);

            // Вычисляем потери: разница между предсказанием и целью
            var outputLosses = new double[outputLayerOutputs.Length];
            for (int i = 0; i < outputLayerOutputs.Length; i++)
            {
                outputLosses[i] = normalizedOutputs[i] - target[i];
            }

            // Обратное распространение: корректируем веса, начиная с выходного слоя
            var outputDeltas = Output.BackpropagateOutput(outputLosses, LearningRate);
            Hidden.BackpropagateHidden(outputDeltas, Output, LearningRate);
        }

        /// <summary>
        /// Проверяет входные данные на корректность.
        /// </summary>
        private void ValidateInput(double[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (Hidden == null || Hidden.Neurons.Length == 0)
                throw new InvalidOperationException("Hidden layer is not initialized");
            if (input.Length != Hidden.Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Hidden.Neurons[0].Weights.Length} inputs, but got {input.Length}");
        }
    }
}