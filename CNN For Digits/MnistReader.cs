namespace CNN_For_Digits
{
    public static class MnistReader
    {
        public static (byte[][] images, byte[] labels) Load(string imagePath, string labelPath)
        {
            var images = ReadImages(imagePath);
            var labels = ReadLabels(labelPath);
            return (images, labels);
        }
        private static byte[] ReadLabels(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);

            int magic = ReadBigEndianInt32(br);
            if (magic != 2049)
                throw new Exception($"Invalid magic number: {magic}");

            int numLabels = ReadBigEndianInt32(br);
            return br.ReadBytes(numLabels);
        }
        private static byte[][] ReadImages(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);

            int magic = ReadBigEndianInt32(br);
            if (magic != 2051)
                throw new Exception($"Invalid magic number: {magic}");

            int numImages = ReadBigEndianInt32(br);
            int numRows = ReadBigEndianInt32(br);
            int numCols = ReadBigEndianInt32(br);

            Console.WriteLine($"magic: {magic}, images: {numImages}, rows: {numRows}, cols: {numCols}");

            byte[][] images = new byte[numImages][];
            for (int i = 0; i < numImages; i++)
            {
                images[i] = br.ReadBytes(numRows * numCols);
            }

            return images;
        }
        private static int ReadBigEndianInt32(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            if (bytes.Length < 4) throw new EndOfStreamException("Unexpected end of file");
            return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        }

    }
}
