using Neural_Network.Interfaces;

namespace Neural_Network
{
    public class Layer
    {
        public Neuron[] Neurons { get; }

        public Layer(int inputSize, int neuronCount, IActivationFunction activationFunction, Random? random = null)
        {
            if (inputSize <= 0)
                throw new ArgumentException("Input size must be positive", nameof(inputSize));
            if (neuronCount <= 0)
                throw new ArgumentException("Neuron count must be positive", nameof(neuronCount));

            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(inputSize, activationFunction, random);
            }
        }

        public double[] Forward(double[] inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (inputs.Length != Neurons[0].Weights.Length)
                throw new ArgumentException($"Expected {Neurons[0].Weights.Length} inputs, but got {inputs.Length}");

            return Neurons.Select(neuron => neuron.FeedForward(inputs)).ToArray();
        }

        public double[] BackpropagateOutput(double[] errors, double learningRate = 0.01)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));
            if (errors.Length != Neurons.Length)
                throw new ArgumentException($"Expected {Neurons.Length} errors, but got {errors.Length}");

            var deltas = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                var neuron = Neurons[i];
                deltas[i] = errors[i] * neuron.ActivateDerivative(neuron.InputSum);
                UpdateNeuronWeights(neuron, deltas[i], learningRate);
            }

            return deltas;
        }

        public double[] BackpropagateHidden(double[] outputLayerErrors, Layer nextLayer, double learningRate)
        {
            if (outputLayerErrors == null)
                throw new ArgumentNullException(nameof(outputLayerErrors));
            if (nextLayer == null)
                throw new ArgumentNullException(nameof(nextLayer));
            if (outputLayerErrors.Length != nextLayer.Neurons.Length)
                throw new ArgumentException($"Expected {nextLayer.Neurons.Length} errors, but got {outputLayerErrors.Length}");

            var errors = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                double error = 0.0;
                for (int j = 0; j < nextLayer.Neurons.Length; j++)
                {
                    error += nextLayer.Neurons[j].Weights[i] * outputLayerErrors[j];
                }
                errors[i] = error;

                var neuron = Neurons[i];
                double delta = error * neuron.ActivateDerivative(neuron.InputSum);
                UpdateNeuronWeights(neuron, delta, learningRate);
            }

            return errors;
        }

        private void UpdateNeuronWeights(Neuron neuron, double delta, double learningRate)
        {
            for (int j = 0; j < neuron.Weights.Length; j++)
            {
                if (double.IsNaN(neuron.LastInputs[j]) || double.IsInfinity(neuron.LastInputs[j]))
                    throw new ArgumentException($"Invalid input at index {j}: NaN or Infinity");
                neuron.Weights[j] -= learningRate * delta * neuron.LastInputs[j];
            }
            neuron.Bias -= learningRate * delta;
        }
    }
}