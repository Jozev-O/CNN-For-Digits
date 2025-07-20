using Neural_Network.Interfaces;

namespace Neural_Network.JsonNeuralNetworkSerializer.Interfaces
{
    public interface INeuralNetworkSerializer
    {
        string Save(NeuralNetwork network, string filePath);
        NeuralNetwork Load(string filePath, IActivationFunction activationFunction, INormalizationFunction normalizationFunction, Random? random = null);
        bool TryLoad(string filePath, IActivationFunction activationFunction, INormalizationFunction normalizationFunction, out NeuralNetwork network, Random? random = null);
    }
}
