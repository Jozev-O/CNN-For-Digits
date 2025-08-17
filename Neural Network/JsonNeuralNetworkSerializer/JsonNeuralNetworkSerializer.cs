using Neural_Network.JsonNeuralNetworkSerializer.Interfaces;
using Neural_Network.JsonNeuralNetworkSerializer.Models;
using Neural_Network.Interfaces;
using Newtonsoft.Json;

namespace Neural_Network.JsonNeuralNetworkSerializer
{
    public class JsonNeuralNetworkSerializer : INeuralNetworkSerializer
    {
        /// <summary>
        /// Saves the neural network to a JSON file.
        /// </summary>
        /// <param name="network">The network to save.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The file path if successful.</returns>
        public string Save(NeuralNetwork network, string filePath)
        {
            ValidateFilePath(filePath);
            if (network == null)
                throw new ArgumentNullException(nameof(network));
            if (network.Hidden.Neurons.Length == 0 || network.Output.Neurons.Length == 0)
                throw new ArgumentException("Network layers cannot be empty");


            try
            {
                // Создаём модель через конструктор, чтобы валидировать данные на этапе создания
                var serializedModel = new NeuralNetworkModel(
                    version: 1,
                    inputLayerSize: network.Hidden.Neurons[0].Weights.Length,
                    hiddenLayerNeuronCount: network.Hidden.Neurons.Length,
                    outputLayerNeuronCount: network.Output.Neurons.Length,
                    hiddenLayerWeights: network.Hidden.Neurons.Select(n => n.Weights).ToArray(),
                    hiddenLayerBiases: network.Hidden.Neurons.Select(n => n.Bias).ToArray(),
                    outputLayerWeights: network.Output.Neurons.Select(n => n.Weights).ToArray(),
                    outputLayerBiases: network.Output.Neurons.Select(n => n.Bias).ToArray());

                string json = JsonConvert.SerializeObject(serializedModel, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Модель успешно сохранена в {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save model: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads the neural network from a JSON file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="activationFunction">Activation function.</param>
        /// <param name="normalizationFunction">Normalization function.</param>
        /// <param name="random">Random (optional, not used for loading).</param>
        /// <returns>The loaded network.</returns>
        public NeuralNetwork Load(
            string filePath,
            IActivationFunction activationFunction,
            INormalizationFunction normalizationFunction,
            Random? random = null
            )
        {
            ValidateFilePath(filePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Model file not found: {filePath}");
            if (activationFunction == null)
                throw new ArgumentNullException(nameof(activationFunction));
            if (normalizationFunction == null)
                throw new ArgumentNullException(nameof(normalizationFunction));

            try
            {
                string json = File.ReadAllText(filePath);
                var serializedModel = JsonConvert.DeserializeObject<NeuralNetworkModel>(json)
               ?? throw new InvalidDataException("Invalid model format");

                // Проверяем данные после десериализации, чтобы убедиться в целостности
                ValidateModelData(serializedModel);


                // Создаём сеть с сохранёнными весами и bias через новый конструктор
                var network = new NeuralNetwork(
                    serializedModel.HiddenLayerWeights,
                    serializedModel.HiddenLayerBiases,
                    serializedModel.OutputLayerWeights,
                    serializedModel.OutputLayerBiases,
                    activationFunction,
                    normalizationFunction);

                Console.WriteLine($"Модель успешно загружена из {filePath}");
                return network;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load model: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tries to load the network without throwing exceptions.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="activationFunction">Activation function.</param>
        /// <param name="normalizationFunction">Normalization function.</param>
        /// <param name="network">Out: loaded network.</param>
        /// <returns>True if successful.</returns>
        public bool TryLoad(
            string filePath,
            IActivationFunction activationFunction,
            INormalizationFunction normalizationFunction,
            out NeuralNetwork network,
            Random? random = null)
        {
            network = null;

            try
            {
                network = Load(filePath, activationFunction, normalizationFunction);
                return true;
            }
            catch
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates the deserialized model data.
        /// </summary>
        private void ValidateModelData(NeuralNetworkModel modelData)
        {
            if (modelData == null)
                throw new InvalidDataException("Model data is null");
            if (modelData.Version != 1)
                throw new InvalidDataException($"Unsupported model version: {modelData.Version}");
            // Остальная валидация в конструкторе NeuralNetworkModel
        }
        /// <summary>
        /// Validates the file path.
        /// </summary>
        private void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null, empty, or whitespace", nameof(filePath));
        }
    }
}