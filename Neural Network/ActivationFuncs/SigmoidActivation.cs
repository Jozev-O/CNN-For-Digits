using Neural_Network.Interfaces;

namespace Neural_Network.ActivationFuncs
{
    public class SigmoidActivation : IActivationFunction
    {
        public double Activate(double x)
        {
            // Проверяем на переполнение экспоненты
            if (double.IsNaN(x) || double.IsInfinity(x))
                throw new ArgumentException("Invalid input for activation function: NaN or Infinity");

            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public double Derivative(double x)
        {
            double sigmoid = Activate(x);
            return sigmoid * (1 - sigmoid);
        }
    }
}
