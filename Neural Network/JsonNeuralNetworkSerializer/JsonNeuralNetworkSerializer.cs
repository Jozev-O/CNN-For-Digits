using Neural_Network.JsonNeuralNetworkSerializer.Interfaces;
using Neural_Network.JsonNeuralNetworkSerializer.Models;
using Neural_Network.Interfaces;
using Newtonsoft.Json;

namespace Neural_Network.JsonNeuralNetworkSerializer
{
    public class JsonNeuralNetworkSerializer : INeuralNetworkSerializer
    {
        public string Save(NeuralNetwork network, string filePath)
        {
            if (network == null)
                throw new ArgumentNullException(nameof(network));
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                var modelData = new NeuralNetworkModel
                {
                    InputSize = network.Hidden.Neurons[0].Weights.Length,
                    HiddenSize = network.Hidden.Neurons.Length,
                    OutputSize = network.Output.Neurons.Length,
                    HiddenWeights = network.Hidden.Neurons.Select(n => n.Weights).ToArray(),
                    HiddenBiases = network.Hidden.Neurons.Select(n => n.Bias).ToArray(),
                    OutputWeights = network.Output.Neurons.Select(n => n.Weights).ToArray(),
                    OutputBiases = network.Output.Neurons.Select(n => n.Bias).ToArray()
                };

                string json = JsonConvert.SerializeObject(modelData, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Модель успешно сохранена в {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении модели: {ex.Message}");
                throw;
            }
        }

        public NeuralNetwork Load(string filePath, IActivationFunction activationFunction, INormalizationFunction normalizationFunction, Random? random = null)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл модели не найден: {filePath}");
            if (activationFunction == null)
                throw new ArgumentNullException(nameof(activationFunction));
            if (normalizationFunction == null)
                throw new ArgumentNullException(nameof(normalizationFunction));

            try
            {
                string json = File.ReadAllText(filePath);
                var modelData = JsonConvert.DeserializeObject<NeuralNetworkModel>(json)
                    ?? throw new InvalidDataException("Неверный формат файла модели");

                ValidateModelData(modelData);

                var network = new NeuralNetwork(
                    modelData.InputSize,
                    modelData.HiddenSize,
                    modelData.OutputSize,
                    activationFunction,
                    normalizationFunction,
                    random);

                for (int i = 0; i < modelData.HiddenSize; i++)
                {
                    network.Hidden.Neurons[i].Weights = modelData.HiddenWeights[i]
                        ?? throw new InvalidDataException($"Hidden weights at index {i} are null");
                    network.Hidden.Neurons[i].Bias = modelData.HiddenBiases[i];
                }

                for (int i = 0; i < modelData.OutputSize; i++)
                {
                    network.Output.Neurons[i].Weights = modelData.OutputWeights[i]
                        ?? throw new InvalidDataException($"Output weights at index {i} are null");
                    network.Output.Neurons[i].Bias = modelData.OutputBiases[i];
                }

                Console.WriteLine($"Модель успешно загружена из {filePath}");
                return network;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке модели: {ex.Message}");
                throw;
            }
        }

        public bool TryLoad(string filePath, IActivationFunction activationFunction, INormalizationFunction normalizationFunction, out NeuralNetwork network, Random? random = null)
        {
            network = null;

            try
            {
                network = Load(filePath, activationFunction, normalizationFunction, random);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                return false;
            }
        }

        private void ValidateModelData(NeuralNetworkModel modelData)
        {
            if (modelData == null)
                throw new InvalidDataException("Model data is null");
            if (modelData.HiddenWeights == null || modelData.OutputWeights == null)
                throw new InvalidDataException("Weights data is missing");
            if (modelData.HiddenBiases == null || modelData.OutputBiases == null)
                throw new InvalidDataException("Biases data is missing");
            if (modelData.InputSize <= 0 || modelData.HiddenSize <= 0 || modelData.OutputSize <= 0)
                throw new InvalidDataException("Invalid model dimensions");
            if (modelData.HiddenWeights.Length != modelData.HiddenSize ||
                modelData.OutputWeights.Length != modelData.OutputSize)
                throw new InvalidDataException("Weights array size mismatch");
            if (modelData.HiddenBiases.Length != modelData.HiddenSize ||
                modelData.OutputBiases.Length != modelData.OutputSize)
                throw new InvalidDataException("Biases array size mismatch");
        }
    }
}