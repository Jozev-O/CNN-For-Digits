using Neural_Network;
using Neural_Network.JsonNeuralNetworkSerializer;
using System.Text;

namespace CNN_For_Digits
{
    public partial class Form1 : Form
    {
        static readonly string trainImagesPath = @"C:\Projects\CNN For Digits\MNIST Dataset\OriginalFiles\train-images.idx3-ubyte";
        static readonly string trainLabelsPath = @"C:\Projects\CNN For Digits\MNIST Dataset\OriginalFiles\train-labels.idx1-ubyte";

        static readonly string testImagesPath = @"C:\Projects\CNN For Digits\MNIST Dataset\OriginalFiles\t10k-images.idx3-ubyte";
        static readonly string testLabelsPath = @"C:\Projects\CNN For Digits\MNIST Dataset\OriginalFiles\t10k-labels.idx1-ubyte";

        MnistRepo mnistRepo;
        NeuralNetworkInterface NN;
        int globalRndImg;

        public Form1()
        {
            InitializeComponent();
            mnistRepo = new(trainImagesPath, trainLabelsPath);
        }

        private void Random_Img_button_Click(object sender, EventArgs e)
        {
            globalRndImg = new Random().Next(mnistRepo.ImagesCount);

            Bitmap bitMap = MakeBitmap(mnistRepo.Images[globalRndImg], 10);
            pictureBox1.Image = bitMap;

            width.Text = pictureBox1.Width.ToString();
            height.Text = pictureBox1.Height.ToString();

            label2.Text = $"Сurrent namber -> {mnistRepo.Labels[globalRndImg]}";

            toolStripStatusLabel1.Text = NN.ModelPath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Images Count: {mnistRepo.ImagesCount}");
            sb.AppendLine($"Label Count: {mnistRepo.LabelCount}");
            label1.Text = sb.ToString();
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

        private void тестовыйНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mnistRepo = new(testImagesPath, testLabelsPath);
            Form1_Load(sender, e);
        }

        private void обучающийНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mnistRepo = new(trainImagesPath, trainLabelsPath);
            Form1_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            NN ??= new NeuralNetworkInterface(new JsonNeuralNetworkSerializer());

            label3.Text = NN.Predict(
                mnistRepo.Images[globalRndImg].ConvertBytesToDoubles(),
                mnistRepo.Labels[globalRndImg],
                out var predicted
                );
            textBox1.Text = predicted.ToString();


            Random_Img_button_Click(sender, e);
        }

        private void сохранитьМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NN == null)
            {
                MessageBox.Show("Нейросеть не инициализированна");
            }
            else
            {
                var saveFileDialog = new SaveFileDialog();

                // Настройки диалога
                //saveFileDialog.Filter = "JSON files(*.json) | *.json | All files(*.*) | *.* ";
                //saveFileDialog.FilterIndex = 1;
                saveFileDialog.Title = "Сохранить файл как...";
                saveFileDialog.FileName = @"network_model.json";
                //saveFileDialog.CheckFileExists = true;
                //saveFileDialog.CheckPathExists = true;

                // Показать диалог и получить результат
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    MessageBox.Show($"Сохраненно по пути {NN.SaveModel(filePath.IsValidPath() ? filePath : null)}");
                }
            }
        }

        private void загрузитьМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создание диалога
            var openFileDialog = new OpenFileDialog();

            // Настройка параметров
            openFileDialog.Title = "Выберите файл";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //openFileDialog.Filter = "JSON files(*.json) | *.json | All files(*.*) | *.* ";
            //openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;

            // Показ диалога

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                NN = new NeuralNetworkInterface(new JsonNeuralNetworkSerializer(), selectedFilePath);
            }
        }

        private void reluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NN.SetReLU();
        }

        private void sigmoidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NN.SetSigmoid();
        }
    }
}