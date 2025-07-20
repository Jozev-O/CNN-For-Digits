namespace Neural_Network
{
    public static class MathUtils
    {
        public static int ArgMax(double[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Length == 0)
                throw new ArgumentException("Array is empty");

            int maxIndex = 0;
            double maxValue = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > maxValue)
                {
                    maxValue = array[i];
                    maxIndex = i;
                }
            }

            return maxIndex + 1; // Сохраняем +1 для совместимости с текущей логикой
        }
    }
}