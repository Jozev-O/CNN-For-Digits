using Neural_Network.Interfaces;

namespace Neural_Network
{
    public class Layer
    {
        public Neuron[] Neurons { get; }


        /// <summary>
        /// Инициализирует слой с заданным количеством нейронов.
        /// </summary>
        /// <param name="inputSize">Размер входа (выход предыдущего слоя).</param>
        /// <param name="neuronCount">Количество нейронов в этом слое.</param>
        /// <param name="activationFunction">Функция активации для всех нейронов.</param>
        /// <param name="random">Генератор случайных чисел; один на слой для разнообразия весов и reproducibility.</param>
        public Layer(int inputSize, int neuronCount, IActivationFunction activationFunction, Random random)
        {
            if (inputSize <= 0)
                throw new ArgumentException("Input size must be positive", nameof(inputSize));
            if (neuronCount <= 0)
                throw new ArgumentException("Neuron count must be positive", nameof(neuronCount));

            ArgumentNullException.ThrowIfNull(random);

            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < neuronCount; i++)
            {
                // Создаём нейроны с одним random, чтобы веса были разными и сеть не "застряла" в симметрии
                Neurons[i] = new Neuron(inputSize, activationFunction, random);
            }
        }

        /// <summary>
        /// Initializes a layer with predefined weights and biases for deserialization.
        /// </summary>
        /// <param name="weights">Array of weight arrays for each neuron.</param>
        /// <param name="biases">Array of biases for each neuron.</param>
        /// <param name="activationFunction">Activation function for neurons.</param>
        public Layer(double[][] weights, double[] biases, IActivationFunction activationFunction)
        {
            if (weights == null || biases == null)
                throw new ArgumentNullException("Weights and biases cannot be null");
            if (weights.Length == 0 || weights.Length != biases.Length)
                throw new ArgumentException("Weights and biases must match in length and be non-empty");
            if (weights.Any(w => w == null))
                throw new ArgumentException("Weight arrays cannot contain null");
            if (activationFunction == null)
                throw new ArgumentNullException(nameof(activationFunction));

            Neurons = new Neuron[weights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                Neurons[i] = new Neuron(weights[i], biases[i], activationFunction);
            }
        }

        /// <summary>
        /// Выполняет прямой проход: пропускает входы через все нейроны.
        /// </summary>
        /// <param name="inputs">Входы от предыдущего слоя.</param>
        /// <returns>Выходы слоя (входы для следующего).</returns>
        public double[] Forward(double[] inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (inputs.Length != Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Neurons[0].Weights.Length} inputs, but got {inputs.Length}");

            // Используем for-loop вместо LINQ для лучшей производительности в больших сетях
            var outputs = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                // Каждый нейрон вычисляет свой выход независимо, чтобы слой агрегировал предсказания
                outputs[i] = Neurons[i].FeedForward(inputs);
            }
            return outputs;
        }

        /// <summary>
        /// Вычисляет дельты для выходного слоя и обновляет веса.
        /// </summary>
        /// <param name="errors">Ошибки (разница между предсказанием и target).</param>
        /// <param name="learningRate">Скорость обучения для корректировки весов.</param>
        /// <returns>Дельты для распространения на предыдущий слой.</returns>
        public double[] BackpropagateOutput(double[] errors, double learningRate)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));
            if (errors.Length != Neurons.Length)
                throw new ArgumentException($"Expected {Neurons.Length} errors, but got {errors.Length}");

            var deltas = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                var neuron = Neurons[i];

                // Вычисляем дельту как ошибка * производная, чтобы учесть, насколько активация влияет на ошибку
                deltas[i] = errors[i] * neuron.ComputeDerivative(neuron.WeightedSum);

                // Делегируем обновление нейрону, чтобы сохранить инкапсуляцию и избежать дублирования логики
                neuron.UpdateWeights(deltas[i], learningRate);
            }

            return deltas;
        }

        /// <summary>
        /// Вычисляет дельты для скрытого слоя и обновляет веса.
        /// </summary>
        /// <param name="nextLayerDeltas">Дельты от следующего слоя.</param>
        /// <param name="nextLayer">Следующий слой для доступа к весам.</param>
        /// <param name="learningRate">Скорость обучения.</param>
        /// <returns>Дельты для распространения на предыдущий слой.</returns>
        public double[] BackpropagateHidden(double[] outputLayerErrors, Layer nextLayer, double learningRate)
        {
            if (outputLayerErrors == null)
                throw new ArgumentNullException(nameof(outputLayerErrors));
            if (nextLayer == null)
                throw new ArgumentNullException(nameof(nextLayer));
            if (outputLayerErrors.Length != nextLayer.Neurons.Length)
                throw new ArgumentException($"Expected {nextLayer.Neurons.Length} errors, but got {outputLayerErrors.Length}");

            var deltas = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                double error = 0.0;
                for (int j = 0; j < nextLayer.Neurons.Length; j++)
                {
                    // Суммируем взвешенные дельты от следующего слоя, чтобы "пропагандировать" ошибку назад
                    error += nextLayer.Neurons[j].Weights[i] * outputLayerErrors[j];
                }
                errors[i] = error;

                var neuron = Neurons[i];
                // Дельта = propagated error * производная, чтобы скорректировать вклад этого нейрона в общую ошибку
                deltas[i] = error * neuron.ComputeDerivative(neuron.WeightedSum);
                // Делегируем обновление, чтобы нейрон сам управлял своими весами
                neuron.UpdateWeights(deltas[i], learningRate);
        }

            return deltas;
        }
    }
}