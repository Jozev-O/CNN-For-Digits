namespace Neural_Network.JsonNeuralNetworkSerializer.Models
{
    public class NeuralNetworkModel
    {
        public int InputSize { get; set; }
        public int HiddenSize { get; set; }
        public int OutputSize { get; set; }
        public double[][] HiddenWeights { get; set; }
        public double[] HiddenBiases { get; set; }
        public double[][] OutputWeights { get; set; }
        public double[] OutputBiases { get; set; }
    }
}
