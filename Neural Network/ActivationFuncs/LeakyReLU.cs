using Neural_Network.Interfaces;

namespace Neural_Network.ActivationFuncs
{
    public class LeakyReLU : IActivationFunction
    {
        private double alpha = 0.01;
        public double Activate(double x)
        {
            // Проверяем на переполнение экспоненты
            if (double.IsNaN(x) || double.IsInfinity(x))
                throw new ArgumentException("Invalid input for activation function: NaN or Infinity");

            return x >= 0 ? x : alpha * x;
        }

        public double Derivative(double x)
        {

            return x >= 0 ? 1.0 : alpha;
        }
    }
}