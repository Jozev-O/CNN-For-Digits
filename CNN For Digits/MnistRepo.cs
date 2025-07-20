namespace CNN_For_Digits
{
    class MnistRepo
    {
        public byte[][] Images { get; set; }
        public byte[] Labels { get; set; }
        public int LabelCount { get; set; }
        public int ImagesCount { get; set; }
        public MnistRepo(string trainImagesPath, string trainLabelsPath)
        {
            var (images, labels) = MnistReader.Load(trainImagesPath, trainLabelsPath);
            Labels = labels;
            Images = images;
            LabelCount = labels.Length;
            ImagesCount = images.Length;
        }

    }
}
