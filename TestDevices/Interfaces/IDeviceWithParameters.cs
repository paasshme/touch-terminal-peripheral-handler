using IDeviceLib;

namespace TestDevice.Interfaces
{
   public interface IDeviceWithParameters : IDevice
    {
        void MethodWithParameters(string aString);
        void MethodWithTwoParameters(string aString, string bString);

    }
}
