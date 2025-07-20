namespace TempTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int target = int.Parse(Console.ReadLine());

            double[] doubles = new double[10];

            doubles[target] = 1;

            Console.WriteLine(string.Join(" ", doubles.Select(o => o.ToString("0.00"))));
        }
    }
}
