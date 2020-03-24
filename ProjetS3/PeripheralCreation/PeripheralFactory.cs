using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using IDeviceLib;
using ProjetS3.PeripheralRequestHandler;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ProjetS3.PeripheralCreation
{
    /*
     * Object that creates instances of the different peripherals
     */
    public class PeripheralFactory
    {


        private static string PROJECT_PATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        //Default windows path
        public static string CONFIGURATION_FILE_PATH = PROJECT_PATH + "/Config.xml";

        private const string DLL_EXTENSION = ".dll"; 

        private static XMLConfigReader configReader;

        //Dictionnarry that match the type string (e.g. "RandomDevice") with the instance of this type
        private static Dictionary<string, IDevice> devicesDictionnary;

        //A peripheral event handler lookalike, this object will be given to every peripheral instance so that they can communicate
        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = PeripheralEventHandlerProxy.GetInstance();

        /*
         * Method that creates an instance for every node instance in the configuration file.
         * When executed, it fills the devicesDictionnary with an instance of each type.
         */

        //Handle linux path correctly
        static PeripheralFactory()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                CONFIGURATION_FILE_PATH = "Config.xml";
            }
        }
        public static void Init()
        {
            devicesDictionnary = new Dictionary<string, IDevice>();
            
            configReader = new XMLConfigReader(CONFIGURATION_FILE_PATH);


            ArrayList listOfEveryDllByName = configReader.GetAllDllName();

            //Browsing through each library in configuration file
            foreach (string aLibraryName in listOfEveryDllByName)
            {
                Assembly assembly = null;

                string absolutePath = Regex.Replace(PROJECT_PATH, "\\\\", "/");

                //Loading the current .dll
                assembly = Assembly.LoadFrom(absolutePath + aLibraryName + DLL_EXTENSION);

                //Load all the dependencies of the current assembly
                foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
                {

                    try
                    {
                        //effictive loading
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

                //Getting all the peripheral types from the curretn assembly
                ArrayList instances = configReader.GetAllInstancesFromOneDll(aLibraryName);

                //Browsing through each instance node
                foreach (string instanceName in instances)
                {
                    string[] parsedPath = aLibraryName.Split("/");
                    string packageOfInstance = parsedPath[parsedPath.Length - 1];

                    //Getting the constructor parameters
                    Object[] objectParameters = configReader.GetParametersForOneInsance(aLibraryName, instanceName);
                    Type typeOfInstance = null;
                    try
                    {
                        //Getting the type of the peripheral to create
                        typeOfInstance = assembly.GetType(packageOfInstance + "." + instanceName);
                        //Creating the peripheral with right constructor parameters
                        //Build so as to the instance is an IDevice
                        var instance = Activator.CreateInstance(typeOfInstance, objectParameters) as IDevice;

                        //adding the event handler to the new instance
                        instance.eventHandler = PeripheralEventHandlerProxy.GetInstance();
                        //Adding the instance to the dictionnary
                        devicesDictionnary.Add(instanceName, instance);

                    }
                    catch(Exception ex)
                    {
                        //Accurate exception management
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
        /*
         * Method that gets the name of the type of every peripheral instance created 
         * @return a list of string that contains all the types
         */
        public static IList<string> GetAllInstanceNames()
        {
            return new List<string>(devicesDictionnary.Keys);

        }
        public static void SetHandler(PeripheralEventHandler peripheralEventHandler)
        {
            peripheralEventHandlerProxy.SetEventHandler(peripheralEventHandler);
        }

        /*
         * Method that returns the instance of the peripheral whose name is in parameter
         * @param Name of the type of the peripheral instance (e.g. RandomDevice)
         * @return the instance of the good peripheral, which is an IDevice
         */
        public static IDevice GetInstance(string instance)
        {
            IDevice device;
            //Checks if the string in parameters is in the dictionnray keys
            if(devicesDictionnary.TryGetValue(instance, out device))
            {
                return device;
            }
            //Exception is thrown if object doesn't exists
            throw new InexistantObjectException("Object Not found : " + instance);
        }

        /*
         * Method that returns all the effective method of a peripheral type
         * Efective methods = method implemetned from object are removed
         * @param the peripheral type
         * @return a list containing every method (object method info)
         */
        public static List<MethodInfo> FindMethods(Type objectType)
        {
            List<MethodInfo> methodListResult = new List<MethodInfo>();
            //Get every interfaces implemented by this type
            Type[] everyInterfaces = objectType.GetInterfaces();

            //Browsing through each interface implemented
            foreach (Type anInterface in everyInterfaces)
            {
                //getting the method of the effective method of the interface
                MethodInfo[] methodsOfTheObject = anInterface.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                //Before adding each method , check if it already in the returned list
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
