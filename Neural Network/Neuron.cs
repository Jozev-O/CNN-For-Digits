using Neural_Network.Interfaces;

namespace Neural_Network
{
    public class Neuron
    {
        public double[] Weights { get; set; }                       // Вес нейрона
        public double Bias { get; set; }                            // Смещение
        public double Output { get; set; }                          // Активированный нейрон
        public double WeightedSum { get; set; }                     // Это взвешенная сумма перед активацией
        public double[] CachedInputs { get; set; }       // Кэш входов для использования в backpropagation

        private readonly IActivationFunction _activationFunction;   // Функция активации для каждого нейрона

        /// <summary>
        /// Инициализирует нейрон с случайными весами и bias.
        /// </summary>
        /// <param name="inputCount">Количество входов (размер предыдущего слоя).</param>
        /// <param name="activationFunction">Функция активации для нелинейности.</param>
        /// <param name="random">Генератор случайных чисел для reproducibility; рекомендуется передавать один на всю сеть.</param>
        public Neuron(int inputCount, IActivationFunction activationFunction, Random random)
        {
            if (inputCount <= 0)
                throw new ArgumentException("Input count must be positive", nameof(inputCount));

            _activationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            random ??= new Random();

            // Инициализируем веса случайно в [-1, 1],
            // чтобы избежать симметрии: если все веса одинаковые,
            // нейроны не будут дифференцироваться при обучении

            Weights = new double[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                Weights[i] = random.NextDouble() * 2 - 1; // [-1, 1]
            }

            Bias = random.NextDouble() * 2 - 1;
        }

        /// <summary>
        /// Initializes a neuron with specific weights and bias for deserialization.
        /// </summary>
        /// <param name="weights">Predefined weights for the neuron.</param>
        /// <param name="bias">Predefined bias for the neuron.</param>
        /// <param name="activationFunction">Activation function for the neuron.</param>
        public Neuron(double[] weights, double bias, IActivationFunction activationFunction)
        {
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (weights.Length == 0)
                throw new ArgumentException("Weights array cannot be empty", nameof(weights));
            if (weights.Any(x => double.IsNaN(x) || double.IsInfinity(x)))
                throw new ArgumentException("Weights contain invalid values", nameof(weights));
            if (double.IsNaN(bias) || double.IsInfinity(bias))
                throw new ArgumentException("Bias must be a valid number", nameof(bias));
            if (activationFunction == null)
                throw new ArgumentNullException(nameof(activationFunction));

            _activationFunction = activationFunction;
            Weights = weights.ToArray(); // Копируем, чтобы избежать внешних изменений
            Bias = bias;
            CachedInputs = new double[weights.Length]; // Инициализируем под размер входов
        }

        /// <summary>
        /// Вычисляет выход нейрона на основе входов (forward pass).
        /// </summary>
        /// <param name="inputs">Входы от предыдущего слоя.</param>
        /// <returns>Активированный выход.</returns>
        public double FeedForward(double[] inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (inputs.Length != Weights.Length)
                throw new ArgumentException($"Expected {Weights.Length} inputs, but got {inputs.Length}");

            // Проверяем все входы на invalid значения заранее,
            // чтобы избежать частичных вычислений и упростить отладку
            if (inputs.Any(double.IsNaN) || inputs.Any(double.IsInfinity))
                throw new ArgumentException("Inputs contain NaN or Infinity values");


            // Кэшируем копию входов для backpropagation
            CachedInputs = [.. inputs];  // Копия предотвращает изменения снаружи

            WeightedSum = Bias;
            for (int i = 0; i < Weights.Length; i++)
            {
                WeightedSum += inputs[i] * Weights[i];
            }

            // Активируем нейрон и возвращаем
            Output = _activationFunction.Activate(WeightedSum);
            return Output;
        }

        /// <summary>
        /// Вычисляет производную функции активации для данного входа.
        /// </summary>
        /// <param name="x">Вход для производной (обычно WeightedSum).</param>
        /// <returns>Производная.</returns>
        public double ComputeDerivative(double x)
        {
            return _activationFunction.Derivative(x);
        }
        /// <summary>
        /// Обновляет веса и bias нейрона на основе дельты (градиента ошибки).
        /// </summary>
        /// <param name="delta">Градиент ошибки для этого нейрона.</param>
        /// <param name="learningRate">Скорость обучения.</param>
        public void UpdateWeights(double delta, double learningRate)
        {
            // Обновляем веса: корректируем на основе входов и ошибки, чтобы минимизировать loss в будущем
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] -= learningRate * delta * CachedInputs[i];  // Используем кэш, чтобы связать с текущим forward pass
            }
            Bias -= learningRate * delta;
        }
    }
}
// Обновление весов (используется при обучении)
//public void UpdateWeights(double[] inputs, double learningRate)
//{
//    for (int i = 0; i < Weights.Length; i++)
//    {
//        Weights[i] += learningRate * Delta * inputs[i];
//    }
//    Bias += learningRate * Delta;
//}
