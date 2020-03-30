namespace PeripheralTools
{
    public interface IDevice
    {
         IPeripheralEventHandler eventHandler {get; set;}
         void Start();

         void Stop();

    }
}