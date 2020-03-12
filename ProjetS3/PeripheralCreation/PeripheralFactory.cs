using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using IDeviceLib;
using System.IO.Ports;
using ProjetS3.PeripheralRequestHandler;

//Few line of comments
namespace ProjetS3.PeripheralCreation
{

    public class PeripheralFactory
    {
        public static string CONFIGURATION_FILE_PATH = "Config.xml";

        private const string DLL_EXTENSION = ".dll"; 

        private static ConfigReader configReader;

        private static Dictionary<string, IDevice> devicesDictionnary;

        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = PeripheralEventHandlerProxy.GetInstance();


        public static void Init()
        {
            devicesDictionnary = new Dictionary<string, IDevice>();
            configReader = new ConfigReader(CONFIGURATION_FILE_PATH);

            ArrayList listOfEveryDllByName = configReader.GetAllDllName();

            //Create instances
            //Get every instances for every dll
            foreach (string aLibraryName in listOfEveryDllByName)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(aLibraryName + DLL_EXTENSION);
                    foreach(AssemblyName assemblyName in assembly.GetReferencedAssemblies())
                    {
                        Assembly.Load(assemblyName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                  //  throw new MissingDllException("Missing .dll file", ex);
                }
           
                ArrayList instances = configReader.GetAllInstancesFromOneDll(aLibraryName);

                foreach (string instanceName in instances)
                {

                    string[] parsedPath = aLibraryName.Split("/");
                    string packageOfInstance = parsedPath[parsedPath.Length - 1];

                    Object[] objectParameters = configReader.GetParametersForOneInsance(aLibraryName, instanceName);
                    
                    try
                    {
                        Type typeOfInstance = assembly.GetType(packageOfInstance + "." + instanceName);
                        var instance = Activator.CreateInstance(typeOfInstance, objectParameters) as IDevice;

                        instance.eventHandler = PeripheralEventHandlerProxy.GetInstance();
                        devicesDictionnary.Add(instanceName, instance);

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("FACTORY EXCEPTION : "+ex);
                        Console.WriteLine("INSTANCE : " + instanceName);
                        //throw new Exception("Instance is not castable as IDevice", ex);
                    }
                }
            }
        }
        public static IList<string> GetAllInstanceNames()
        {
            return new List<string>(devicesDictionnary.Keys);

        }
        public static void SetHandler(PeripheralEventHandler peripheralEventHandler)
        {
            peripheralEventHandlerProxy.SetEventHandler(peripheralEventHandler);
        }

        public static IDevice GetInstance(string instance)
        {
            try
            {
                return devicesDictionnary[instance];
            }
            catch (Exception ex)
            {
                throw new InexistantObjectException("", ex);
            }
        }

        public static List<MethodInfo> FindMethods(Type objectType)
        {
            List<MethodInfo> methodListResult = new List<MethodInfo>();
            Type[] everyInterfaces = objectType.GetInterfaces();

            foreach (Type anInterface in everyInterfaces)
            {
                MethodInfo[] methodsOfTheObject = anInterface.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (MethodInfo method in methodsOfTheObject)
                {
                    if (!methodListResult.Contains(method))
                    {
                        methodListResult.Add(method);
                    }
                }
            }
            return methodListResult;
        }

    }
}
