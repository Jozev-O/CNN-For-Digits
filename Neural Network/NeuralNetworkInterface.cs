using Neural_Network.ActivationFuncs;
using Neural_Network.Interfaces;
using Neural_Network.JsonNeuralNetworkSerializer.Interfaces;
using Neural_Network.NormalizationFuncs;
using System.Text;

namespace Neural_Network
{
    public class NeuralNetworkInterface
    {
        private readonly INeuralNetworkSerializer _serializer;
        private NeuralNetwork _network;
        private IActivationFunction _activationFunction;
        private readonly INormalizationFunction _normalizationFunction;
        private readonly int _inputSize;
        private readonly int _hiddenSize;
        private readonly int _outputSize;

        public string ModelPath { get; private set; }

        private int _trainEpochs = 10;
        public int TrainEpochs
        {
            get => _trainEpochs;
            set
            {
                if (value <= 0 || value > 1000)
                    throw new ArgumentOutOfRangeException(nameof(value), "Train epochs must be between 1 and 1000");
                _trainEpochs = value;
            }
        }

        /// <summary>
        /// Инициализирует интерфейс для работы с нейронной сетью.
        /// </summary>
        /// <param name="serializer">Сериализатор для сохранения/загрузки модели.</param>
        /// <param name="inputSize">Количество входов (например, 784 для MNIST).</param>
        /// <param name="hiddenSize">Количество нейронов в скрытом слое.</param>
        /// <param name="outputSize">Количество выходов (например, 10 для цифр).</param>
        /// <param name="modelPath">Путь к файлу модели.</param>
        public NeuralNetworkInterface(
            INeuralNetworkSerializer serializer,
            int inputSize = 784,
            int hiddenSize = 128,
            int outputSize = 10,
            string modelPath = "network_model.json")
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _inputSize = inputSize > 0 ? inputSize : throw new ArgumentException("Input size must be positive", nameof(inputSize));
            _hiddenSize = hiddenSize > 0 ? hiddenSize : throw new ArgumentException("Hidden size must be positive", nameof(hiddenSize));
            _outputSize = outputSize > 0 ? outputSize : throw new ArgumentException("Output size must be positive", nameof(outputSize));
            ModelPath = modelPath ?? throw new ArgumentNullException(nameof(modelPath));
            _activationFunction = new SigmoidActivation();
            _normalizationFunction = new SoftmaxNormalization();
            InitializeNetwork();
        }
        //public NeuralNetworkInterface(INeuralNetworkSerializer serializer)
        //{
        //    _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        //    InitializeNetwork();
        //}
        //public NeuralNetworkInterface(INeuralNetworkSerializer serializer, string filepath)
        //{
        //    ModelPath = filepath;
        //    _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        //    InitializeNetwork();
        //}

        /// <summary>
        /// Устанавливает LeakyReLU как функцию активации и пересоздаёт сеть.
        /// </summary>
        public void SetReLU()
        {
            _activationFunction = new LeakyReLU();
            InitializeNetwork();
        }

        /// <summary>
        /// Устанавливает Sigmoid как функцию активации и пересоздаёт сеть.
        /// </summary>
        public void SetSigmoid()
        {
            _activationFunction = new SigmoidActivation();
            InitializeNetwork();
        }

        /// <summary>
        /// Инициализирует сеть: пытается загрузить модель или создаёт новую.
        /// </summary>
        /// <returns>Сообщение о результате инициализации.</returns>
        private string InitializeNetwork()
        {
            // Пытаемся загрузить модель; если не удалось, создаём новую с заданными параметрами
            if (_serializer.TryLoad(ModelPath, _activationFunction, _normalizationFunction, out _network))
            {
                return $"Модель загружена: {_network.Hidden.Neurons.Length} скрытых нейронов, {_network.Output.Neurons.Length} выходных нейронов";
            }
            else
            {
                _network = new NeuralNetwork(_inputSize, _hiddenSize, _outputSize, _activationFunction, _normalizationFunction, new Random());
                return $"Новая модель создана: {_network.Hidden.Neurons.Length} скрытых нейронов, {_network.Output.Neurons.Length} выходных нейронов";
            }
        }

        /// <summary>
        /// Обучает сеть на одном примере, сохраняя модель после обучения.
        /// </summary>
        /// <param name="input">Входные данные (например, пиксели изображения).</param>
        /// <param name="correctLabel">Правильная метка (например, 0-9 для MNIST).</param>
        public void Train(double[] input, int correctLabel)
        {
            if (_network == null)
                throw new InvalidOperationException("Network is not initialized");
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (correctLabel < 0 || correctLabel >= _outputSize)
                throw new ArgumentException($"Label must be between 0 and {_outputSize - 1}, got {correctLabel}");


            // Создаём one-hot вектор для целевой метки
            var oneHotTarget = CreateOneHotVector(correctLabel, _outputSize);
            _network.Train(input, oneHotTarget);
            // Сохраняем модель после обучения, чтобы не потерять прогресс
            _serializer.Save(_network, ModelPath);
        }

        /// <summary>
        /// Выполняет предсказание и возвращает отчёт о результате.
        /// </summary>
        /// <param name="input">Входные данные.</param>
        /// <param name="correctLabel">Ожидаемая метка для оценки точности.</param>
        /// <param name="predictedLabel">Предсказанная метка.</param>
        /// <returns>Отчёт с предсказанием, выходами и точностью.</returns>
        public string Predict(double[] input, int correctLabel, out int predictedLabel)
        {
            if (_network == null)
                throw new InvalidOperationException("Network is not initialized");
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            var predictions = new List<bool>();  // Доделать метрики

            var sb = new StringBuilder();
            var output = _network.Predict(input);

            predictedLabel = MathUtils.ArgMax(output);

            // Формируем отчёт: предсказание и выходы сети
            sb.AppendLine($"Предсказание: {predictedLabel} | Ожидаемая метка: {correctLabel}");
            sb.AppendLine($"Выходы сети: [{string.Join(", ", output.Select(o => o.ToString("0.00")))}]");

            var countOfErr = predictions.FindAll(e => e == false);

            sb.AppendLine($"Количество ошибок --> {countOfErr.Count}/{predictions.Count}");
            sb.AppendLine($"Процент ошибок --> {(double)countOfErr.Count / predictions.Count} %");
            return sb.ToString();
        }

        /// <summary>
        /// Сохраняет модель в указанный файл или по умолчанию.
        /// </summary>
        /// <param name="filepath">Путь для сохранения (опционально).</param>
        /// <returns>Сообщение о результате сохранения.</returns>
        public string SaveModel(string filepath = null)
        {
            if (_network == null)
                throw new InvalidOperationException("Network is not initialized");
            if (filepath != null)
                ModelPath = filepath;
            return _serializer.Save(_network, ModelPath);
        }
        /// <summary>
        /// Создаёт one-hot вектор для целевой метки.
        /// </summary>
        /// <param name="label">Метка (например, 0-9).</param>
        /// <param name="outputSize">Размер выходного слоя.</param>
        /// <returns>One-hot вектор.</returns>
        private static double[] CreateOneHotVector(int label, int outputSize)
        {
            var vector = new double[outputSize];
            vector[label] = 1.0;
            return vector;
        }
    }
}