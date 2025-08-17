using Neural_Network;
using Neural_Network.JsonNeuralNetworkSerializer;
using System.Text;

namespace CNN_For_Digits
{
    public partial class Form1 : Form
    {

        // ������������� ���� � ������ MNIST ������������ ����� �������
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
            // ������������� ����������� MNIST � ��������� ������� ������
            _mnistRepo = new MnistRepo(TrainImagesPath, TrainLabelsPath);
            UpdateDatasetInfo();
        }

        // ��������� ���������� � ������ ������ � UI (���������� ����������� � �����)
        private void UpdateDatasetInfo()
        {
            if (_mnistRepo == null)
            {
                MessageBox.Show("����������� ������ �� ���������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Images Count: {_mnistRepo.ImagesCount}");
            sb.AppendLine($"Label Count: {_mnistRepo.LabelCount}");
            label1.Text = sb.ToString();
        }

        // ������������ ������� ������ ��� ����������� ���������� �����������
        private void Random_Img_button_Click(object sender, EventArgs e)
        {

            if (_mnistRepo == null)
            {
                MessageBox.Show("����������� ������ �� ���������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // �������� ��������� ����������� �� ������ ������
            _currentImageIndex = new Random().Next(_mnistRepo.ImagesCount);
            pictureBox1.Image = MakeBitmap(_mnistRepo.Images[_currentImageIndex], 10);

            // ��������� ������� ����������� � �����
            width.Text = pictureBox1.Width.ToString();
            height.Text = pictureBox1.Height.ToString();
            label2.Text = $"������� �����: {_mnistRepo.Labels[_currentImageIndex]}";

            // ��������� ���� � ������, ���� ��������� ����������������
            toolStripStatusLabel1.Text = _neuralNetworkInterface?.ModelPath ?? "������ �� ���������";
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

        // ��������� �������� ����� ������
        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mnistRepo = new(TestImagesPath, TestLabelsPath);
            UpdateDatasetInfo();
        }

        // ��������� ��������� ����� ������
        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mnistRepo = new(TrainImagesPath, TrainLabelsPath);
            UpdateDatasetInfo();
        }

        // ��������� ������������ ��������� ��� �������� �����������
        private void button2_Click(object sender, EventArgs e)
        {
            if (_mnistRepo == null)
            {
                MessageBox.Show("����������� ������ �� ���������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // ��������� ������ ��������� � ����
        private void ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_neuralNetworkInterface == null)
            {
                MessageBox.Show("��������� �� ����������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                using SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "��������� ������ ���������",
                    FileName = "network_model.json",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    MessageBox.Show($"������ ��������� �� ����: {_neuralNetworkInterface.SaveModel(filePath)}",
                        "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // ��������� ������ ��������� �� �����
        private void ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "�������� ���� ������",
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
                MessageBox.Show("������ ������� ���������.", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}