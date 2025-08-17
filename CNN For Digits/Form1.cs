using Neural_Network;
using Neural_Network.JsonNeuralNetworkSerializer;
using System.Text;

namespace CNN_For_Digits
{
    public partial class Form1 : Form
    {

        // Относительные пути к файлам MNIST относительно папки проекта
        //private static readonly string TrainImagesPath = Path.Combine("MNIST Dataset", "train-images.idx3-ubyte");
        //private static readonly string TrainLabelsPath = Path.Combine("MNIST Dataset", "train-labels.idx1-ubyte");
        //private static readonly string TestImagesPath = Path.Combine("MNIST Dataset", "t10k-images.idx3-ubyte");
        //private static readonly string TestLabelsPath = Path.Combine("MNIST Dataset", "t10k-labels.idx1-ubyte");

        static readonly string TrainImagesPath = @"C:\Projects\CNN For Digits\MNIST Dataset\train-images.idx3-ubyte";
        static readonly string TrainLabelsPath = @"C:\Projects\CNN For Digits\MNIST Dataset\train-labels.idx1-ubyte";

        static readonly string TestImagesPath = @"C:\Projects\CNN For Digits\MNIST Dataset\t10k-images.idx3-ubyte";
        static readonly string TestLabelsPath = @"C:\Projects\CNN For Digits\MNIST Dataset\t10k-labels.idx1-ubyte";

        private MnistRepo _mnistRepo;
        private NeuralNetworkInterface _neuralNetworkInterface;
        private int _currentImageIndex;

        public Form1()
        {
            InitializeComponent();
            // Инициализация репозитория MNIST с обучающим набором данных
            _mnistRepo = new MnistRepo(TrainImagesPath, TrainLabelsPath);
            UpdateDatasetInfo();
        }

        // Обновляет информацию о наборе данных в UI (количество изображений и меток)
        private void UpdateDatasetInfo()
        {
            if (_mnistRepo == null)
            {
                MessageBox.Show("Репозиторий данных не инициализирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Images Count: {_mnistRepo.ImagesCount}");
            sb.AppendLine($"Label Count: {_mnistRepo.LabelCount}");
            label1.Text = sb.ToString();
        }

        // Обрабатывает нажатие кнопки для отображения случайного изображения
        private void Random_Img_button_Click(object sender, EventArgs e)
        {

            if (_mnistRepo == null)
            {
                MessageBox.Show("Репозиторий данных не инициализирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Выбираем случайное изображение из набора данных
            _currentImageIndex = new Random().Next(_mnistRepo.ImagesCount);
            pictureBox1.Image = MakeBitmap(_mnistRepo.Images[_currentImageIndex], 10);

            // Обновляем размеры изображения и метку
            width.Text = pictureBox1.Width.ToString();
            height.Text = pictureBox1.Height.ToString();
            label2.Text = $"Текущая цифра: {_mnistRepo.Labels[_currentImageIndex]}";

            // Обновляем путь к модели, если нейросеть инициализирована
            toolStripStatusLabel1.Text = _neuralNetworkInterface?.ModelPath ?? "Модель не загружена";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDatasetInfo();
        }

        public static Bitmap MakeBitmap(byte[] dImage, int mag)
        {
            int size = 28; // MNIST images are always 28x28
            int width = size * mag;
            int height = size * mag;

            Bitmap result = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(result))
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        int pixel = dImage[y * size + x];
                        //int intensity = 255 - pixel;
                        //int intensity = pixel > 0 ? 0 : 255;
                        int intensity = pixel;
                        Color color = Color.FromArgb(intensity, intensity, intensity);
                        using (SolidBrush brush = new SolidBrush(color))
                        {
                            gr.FillRectangle(brush, x * mag, y * mag, mag, mag);
                        }
                    }
                }
            }
            return result;
        }

        // Загружает тестовый набор данных
        private void тестовыйНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mnistRepo = new(TestImagesPath, TestLabelsPath);
            UpdateDatasetInfo();
        }

        // Загружает обучающий набор данных
        private void обучающийНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mnistRepo = new(TrainImagesPath, TrainLabelsPath);
            UpdateDatasetInfo();
        }

        // Выполняет предсказание нейросети для текущего изображения
        private void button2_Click(object sender, EventArgs e)
        {
            if (_mnistRepo == null)
            {
                MessageBox.Show("Репозиторий данных не инициализирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _neuralNetworkInterface ??= new NeuralNetworkInterface(new JsonNeuralNetworkSerializer());

            label3.Text = _neuralNetworkInterface.Predict(
                _mnistRepo.Images[_currentImageIndex].ConvertBytesToDoubles(),
                _mnistRepo.Labels[_currentImageIndex],
                out var predicted
                );
            textBox1.Text = predicted.ToString();


            Random_Img_button_Click(sender, e);
        }

        // Сохраняет модель нейросети в файл
        private void сохранитьМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_neuralNetworkInterface == null)
            {
                MessageBox.Show("Нейросеть не инициализирована.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                using SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Сохранить модель нейросети",
                    FileName = "network_model.json",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    MessageBox.Show($"Модель сохранена по пути: {_neuralNetworkInterface.SaveModel(filePath)}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Загружает модель нейросети из файла
        private void загрузитьМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл модели",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _neuralNetworkInterface = new NeuralNetworkInterface(new JsonNeuralNetworkSerializer(), modelPath: openFileDialog.FileName);
                toolStripStatusLabel1.Text = _neuralNetworkInterface.ModelPath;
                MessageBox.Show("Модель успешно загружена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void reluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_neuralNetworkInterface != null)
            {
                _neuralNetworkInterface.SetReLU();
            }
        }

        private void sigmoidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_neuralNetworkInterface != null)
            {
                _neuralNetworkInterface.SetSigmoid();
            }
        }

        private void обучениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}