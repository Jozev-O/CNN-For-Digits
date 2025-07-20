namespace Neural_Network.Interfaces
{
    public interface IActivationFunction
    {
        double Activate(double x);
        double Derivative(double x);
    }
}
