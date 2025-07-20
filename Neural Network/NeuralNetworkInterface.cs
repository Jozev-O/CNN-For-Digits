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
        private IActivationFunction _activationFunction = new SigmoidActivation();
        public string ModelPath { get; private set; } = "network_model.json";
        public int TrainEpochs
        {
            get;
            set
            {
                if (value > 1000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                else
                {
                    TrainEpochs = value;
                }
            }
        } = 10;
        public NeuralNetworkInterface(INeuralNetworkSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            InitializeNetwork();
        }
        public NeuralNetworkInterface(INeuralNetworkSerializer serializer, string filepath)
        {
            ModelPath = filepath;
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            InitializeNetwork();
        }
        public void SetReLU()
        {
            _activationFunction = new LeakyReLU();
            InitializeNetwork();
        }
        public void SetSigmoid()
        {
            _activationFunction = new SigmoidActivation();
            InitializeNetwork();
        }
        private string InitializeNetwork()
        {
            var actFunc = _activationFunction;
            var Softmax = new SoftmaxNormalization();

            if (_serializer.TryLoad(ModelPath, actFunc, Softmax, out _network))
            {
                return $"Модель загружена: {_network.Hidden.Neurons.Length} скрытых нейронов, {_network.Output.Neurons.Length} выходных нейронов";
            }
            else
            {
                _network = new NeuralNetwork(784, 128, 10, actFunc, Softmax);
                return $"Новая модель создана: {_network.Hidden.Neurons.Length} скрытых нейронов, {_network.Output.Neurons.Length} выходных нейронов";
            }
        }

        public void Train(double[] input, int correctLabel)
        {
            if (_network == null)
            {
                throw new Exception("Сеть не инициализирована");
            }

            double[] doubles = new double[10];

            doubles[correctLabel - 1] = 1;


            if (correctLabel >= 1 && correctLabel <= 10)
            {
                _network.Train(input, doubles);
                _serializer.Save(_network, ModelPath);
            }
            else
            {
                throw new Exception("Неверная метка");
            }

        }


        public string Predict(double[] input, int correctLabel, out int predictedLabel)
        {
            if (_network == null)
            {
                throw new Exception("Сеть не инициализирована");
            }
            var predictions = new List<bool>();
            var sb = new StringBuilder();

            var output = _network.Predict(input);
            predictedLabel = MathUtils.ArgMax(output);

            sb.AppendLine($"Предсказание: {predictedLabel} | Ожидаемая метка: {correctLabel}");

            sb.AppendLine($"Выходы сети: [{string.Join(", ", output.Select(o => o.ToString("0.00")))}]");

            var countOfErr = predictions.FindAll(e => e == false);

            sb.AppendLine($"Количество ошибок --> {countOfErr.Count}/{predictions.Count}");
            sb.AppendLine($"Процент ошибок --> {(double)countOfErr.Count / predictions.Count} %");
            return sb.ToString();
        }

        public string SaveModel(string filepath = null)
        {
            if (_network == null)
            {
                throw new Exception("Сеть не инициализирована");
            }
            if (filepath != null)
            {
                ModelPath = filepath;
            }
            return _serializer.Save(_network, ModelPath);

        }
        ~NeuralNetworkInterface()
        {
            SaveModel();
        }
    }
}