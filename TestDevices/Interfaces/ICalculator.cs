using PeripheralTools;


namespace TestDevice.Interfaces
{
    public interface ICalculator : IDevice
    {
        void add(int a, int b);
    }
}
