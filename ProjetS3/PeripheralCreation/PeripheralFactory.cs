using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using IDeviceLib;
using ProjetS3.PeripheralRequestHandler;
using System.IO;

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
                assembly = Assembly.LoadFrom(aLibraryName + DLL_EXTENSION);
                foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
                {
                    try
                    {
                        Assembly.Load(assemblyName);
                    }

                    catch (Exception ex)
                    {
                        if(ex is FileNotFoundException || ex is ArgumentNullException)
                        {
                            Console.WriteLine("Missing .dll file : " + assemblyName);
                        }
                        if(ex is FileLoadException)
                        {
                            Console.WriteLine("Couldn't load the .dll file : " + assemblyName);
                        }
                        if(ex is BadImageFormatException)
                        {
                            Console.WriteLine("invalid .dll file : " + assemblyName);
                        }
                        Console.WriteLine(ex.Message);

                    }
                }

                ArrayList instances = configReader.GetAllInstancesFromOneDll(aLibraryName);

                foreach (string instanceName in instances)
                {
                    string[] parsedPath = aLibraryName.Split("/");
                    string packageOfInstance = parsedPath[parsedPath.Length - 1];

                    Object[] objectParameters = configReader.GetParametersForOneInsance(aLibraryName, instanceName);
                    Type typeOfInstance = null;
                    try
                    {
                        typeOfInstance = assembly.GetType(packageOfInstance + "." + instanceName);
                        var instance = Activator.CreateInstance(typeOfInstance, objectParameters) as IDevice;

                        instance.eventHandler = PeripheralEventHandlerProxy.GetInstance();
                        devicesDictionnary.Add(instanceName, instance);

                    }
                    catch(Exception ex)
                    {
                        switch (ex.GetType().ToString())
                        {
                            case "System.ArgumentException":
                                Console.WriteLine("Incorrect argument : " + packageOfInstance + "." + instanceName);
                                break;
                            case "Sytem.ArgumentNullException":
                                Console.WriteLine("Null argument in " + instanceName + " handling : or " + packageOfInstance + " handling");
                                break;
                            case "System.IO.FileNotFoundException":
                                Console.WriteLine("Couldn't find .dll file : " + assembly);
                                break;
                            case "System.IO.FileLoadException":
                                Console.WriteLine("Couldn't load .dll file : " + assembly);
                                break;
                            case "System.BadImageFormatException":
                                Console.WriteLine("invalid .dll file : " + assembly);
                                break;
                            case "System.NotSupportedException":
                                Console.WriteLine("Type " + typeOfInstance + " isn't handled correctly");
                                break;
                            case "System.Reflection.TargetInvocationException":
                                Console.WriteLine("Invalid target on instance creation : " + typeOfInstance + ": the invoked constructor threw an exception.");
                                break;
                            case "System.MethodAccesException":
                                Console.WriteLine("Constructor of +" + typeOfInstance + " is private");
                                break;
                            case "System.MemberAccesException":
                                Console.WriteLine("Couldn't access to member of class : " + typeOfInstance);
                                break;
                            case "System.Runtime.InteropServices.InvalidComObjectException":
                                Console.WriteLine("Object " + typeOfInstance + " isn't used properly");
                                break;
                            case "System.Runtime.InteropServices.COMExpcetion":
                                Console.WriteLine("Object " + typeOfInstance + " isn't used properly");
                                break;
                            case "System.MissingMethodException":
                                Console.WriteLine("Constructor of : " + typeOfInstance + " isn't defined with those parameters");
                                break;
                            case "System.TypeLoadException":
                                Console.WriteLine("couldn't load type : " + typeOfInstance);
                                break;
                            default:
                                System.Console.WriteLine("Unhandled exception type"+ex.GetType());
                                break;


                        }
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
            IDevice device;
            if(devicesDictionnary.TryGetValue(instance, out device))
            {
                return device;
            }
            throw new InexistantObjectException("Object Not found : " + instance);
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
