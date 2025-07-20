using Neural_Network.Interfaces;

namespace Neural_Network
{
    public class Neuron
    {
        public double[] Weights { get; set; }
        public double Bias { get; set; }
        public double Output { get; set; }
        public double InputSum { get; set; }
        public double[] LastInputs { get; set; }

        private readonly IActivationFunction _activationFunction;

        public Neuron(int inputCount, IActivationFunction activationFunction, Random? random = null)
        {
            if (inputCount <= 0)
                throw new ArgumentException("Input count must be positive", nameof(inputCount));

            _activationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            random ??= new Random();

            Weights = new double[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                Weights[i] = random.NextDouble() * 2 - 1; // [-1, 1]
            }

            Bias = random.NextDouble() * 2 - 1;
        }

        public double FeedForward(double[] inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (inputs.Length != Weights.Length)
                throw new ArgumentException($"Expected {Weights.Length} inputs, but got {inputs.Length}");

            // Сохраняем входы без копирования, если это безопасно
            LastInputs = inputs;

            InputSum = Bias;
            for (int i = 0; i < Weights.Length; i++)
            {
                if (double.IsNaN(inputs[i]) || double.IsInfinity(inputs[i]))
                    throw new ArgumentException($"Input at index {i} is invalid: NaN or Infinity");
                InputSum += inputs[i] * Weights[i];
            }

            Output = _activationFunction.Activate(InputSum);
            return Output;
        }

        public double ActivateDerivative(double x)
        {
            return _activationFunction.Derivative(x);
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
