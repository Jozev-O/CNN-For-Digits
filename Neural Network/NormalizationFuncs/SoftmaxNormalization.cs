using Neural_Network.Interfaces;

namespace Neural_Network.NormalizationFuncs
{
    public class SoftmaxNormalization : INormalizationFunction
    {
        public double[] Normalize(double[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Вычисляем максимум для числовой стабильности
            double max = input.Max();
            double[] exp = new double[input.Length];
            double sum = 0.0;

            for (int i = 0; i < input.Length; i++)
            {
                if (double.IsNaN(input[i]) || double.IsInfinity(input[i]))
                    throw new ArgumentException($"Invalid input at index {i}: NaN or Infinity");
                exp[i] = Math.Exp(input[i] - max);
                sum += exp[i];
            }

            if (sum == 0)
                throw new InvalidOperationException("Sum of exponentials is zero, cannot normalize");

            return exp.Select(v => v / sum).ToArray();
        }
    }

}
