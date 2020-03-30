using PeripheralTools;

namespace TestDevices
{
    public class FakeEventHandler: IPeripheralEventHandler 
    {
        public void putPeripheralEventInQueue(string a, string b, string c)
        {
            System.Console.WriteLine(a+b+c);
        }
        
    }
}