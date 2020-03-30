namespace PeripheralTools
{
    /**
     * Service representing every device used
     * Any device must be implementing this interface or one of this implementation
     * 
     */
    public interface IDevice
    {
        // object allowing the communication with the microservice
         IPeripheralEventHandler eventHandler {get; set;}

        //Basics methods common to every devices
         void Start();

         void Stop();

    }
}