namespace CNN_For_Digits
{
    static class Additions
    {
        public static double[] ConvertBytesToDoubles(this byte[] byteArray)
        {
            double[] doubleArray = new double[byteArray.Length];
            for (int i = 0; i < byteArray.Length; i++)
            {
                doubleArray[i] = (double)byteArray[i]; // 0..255 → 0.0..255.0
            }
            return doubleArray;
        }
        public static bool IsValidPath(this string path)
        {
            try
            {
                // Простая проверка с помощью Path.GetFullPath
                string fullPath = Path.GetFullPath(path);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException)
            {
                return false;
            }
        }
    }
}
