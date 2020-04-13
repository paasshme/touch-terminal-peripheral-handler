using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using PeripheralTools;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation
{
        
    /// <summary>
    /// Static Factory pattern that is used to create instances of the different peripherals using System.Reflection
    /// </summary>
    public class PeripheralFactory
    {
        private static string PROJECT_PATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        // Default Windows path
        public static string CONFIGURATION_FILE_PATH = PROJECT_PATH + "/Config.xml";

        private const string DLL_EXTENSION = ".dll"; 

        private static IConfigReader configReader;

        // Dictionnarry that match the type string (e.g. "RandomDevice") with the instance of this type
        private static Dictionary<string, IDevice> devicesDictionnary;

        // A peripheral event handler lookalike, this object will be given to every peripheral instance so that they can communicate
        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = PeripheralEventHandlerProxy.GetInstance();

 

        /// <summary>
        /// Static block that handle Linux path correctly 
        /// </summary>
        static PeripheralFactory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                CONFIGURATION_FILE_PATH = "Config.xml";
            }
        }

        /// <summary>
        /// Init the factory by loading every dll and every instances found in the config file 
        /// It fills the devicesDictionnary with an instance of each type.
        /// Must be called once at the start of the application
        /// </summary>
        /// <exception cref="System.ArgumentException">  Thrown when the package of instance is incorrect </exception>
        /// <exception cref="System.ArgumentNullException"> Thrown when the instance name or the package is null  </exception>
        /// <exception cref="System.IO.FileNotFoundException"> Thrown when  the library is not found </exception>
        /// <exception cref="System.IO.FileLoadException"> Thrown when the System.Reflection don't manage to load correctly the library </exception>
        /// <exception cref="System.BadImageFormatException"> Thrown when library doesn't match a understandable format </exception>
        /// <exception cref="System.NotSupportedException"> Thrown when the type of the creatd instance is not supported</exception>
        /// <exception cref="System.Reflection.TargetInvocationException"> Thrown when the constructor of the instance threw an exception </exception>
        /// <exception cref="System.MethodAccessException"> Thrown when  the contructor of the instance is private </exception>
        /// <exception cref="System.MemberAccessException"> Thrown when  a member of the instance is not accessible </exception>
        /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException"> Thrown when an invalid COM object is used</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException"> Thrown when an HRESULT is returned from a COM Method call </exception>
        /// <exception cref="System.MissingMethodException"> Thrown when a method is missing in the instance class </exception>
        /// <exception cref="System.TypeLoadException">  Thrown when the type cannot be load</exception>

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
                        //effective loading
                        Assembly.Load(assemblyName);
                    }

                    catch (Exception ex)
                    {
                        if (ex is FileNotFoundException || ex is ArgumentNullException)
                        {
                            Console.WriteLine("Missing .dll file : " + assemblyName);
                        }
                        if (ex is FileLoadException)
                        {
                            Console.WriteLine("Couldn't load the .dll file : " + assemblyName);
                        }
                        if (ex is BadImageFormatException)
                        {
                            Console.WriteLine("invalid .dll file : " + assemblyName);
                        }
                        Console.WriteLine(ex.Message);
                    }
                }

                // Getting all the peripheral types from the current assembly
                ArrayList instances = configReader.GetAllInstancesFromOneDll(aLibraryName);

                // Browsing through each instance node
                foreach (string instanceName in instances)
                {
                    string[] parsedPath = aLibraryName.Split("/");
                    string packageOfInstance = parsedPath[parsedPath.Length - 1];

                    // Getting the constructor parameters
                    object[] objectParameters = configReader.GetParametersForOneInstance(aLibraryName, instanceName);
                    Type typeOfInstance = null;
                    try
                    {
                        // Getting the type of the peripheral to create
                        typeOfInstance = assembly.GetType(packageOfInstance + "." + instanceName);
                        // Creating the peripheral with right constructor parameters
                        // Build so as to the instance is an IDevice
                        var instance = Activator.CreateInstance(typeOfInstance, objectParameters) as IDevice;

                        // Adding the event handler to the new instance
                        instance.eventHandler = PeripheralEventHandlerProxy.GetInstance();
                        // Adding the instance to the dictionnary
                        devicesDictionnary.Add(instanceName, instance);

                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine("Null argument in " + instanceName + " handling : or " + packageOfInstance + " handling");
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("Incorrect argument : " + packageOfInstance + "." + instanceName);
                        Console.WriteLine(e.Message);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine("Couldn't find .dll file : " + assembly);
                        Console.WriteLine(e.Message);
                    }
                    catch (FileLoadException e)
                    {
                        Console.WriteLine("Couldn't load .dll file : " + assembly);
                        Console.WriteLine(e.Message);
                    }
                    catch (BadImageFormatException e)
                    {
                        Console.WriteLine("invalid .dll file : " + assembly);
                        Console.WriteLine(e.Message);
                    }
                    catch (NotSupportedException e)
                    {
                        Console.WriteLine("Type " + typeOfInstance + " isn't handled correctly");
                        Console.WriteLine(e.Message);
                    }
                    catch (TargetInvocationException e)
                    {
                        Console.WriteLine("Invalid target on instance creation : " + typeOfInstance + 
                            ": the invoked constructor threw an exception.");
                        Console.WriteLine(e.Message);
                    }
                    catch (MissingMethodException e)
                    {
                        Console.WriteLine("Constructor of : " + typeOfInstance + 
                            " isn't defined with those parameters");
                        Console.WriteLine(e.Message);
                    }
                    catch (MethodAccessException e)
                    {
                        Console.WriteLine("Constructor of +" + typeOfInstance + " is private");
                        Console.WriteLine(e.Message);
                    }
                    catch (MemberAccessException e)
                    {
                        Console.WriteLine("Couldn't access to member of class : " + typeOfInstance);
                        Console.WriteLine(e.Message);
                    }
                    catch (InvalidComObjectException e)
                    {
                        Console.WriteLine("Object " + typeOfInstance + " isn't used properly");
                        Console.WriteLine(e.Message);
                    }
                    catch (COMException e)
                    {
                        Console.WriteLine("Object " + typeOfInstance + " isn't used properly");
                        Console.WriteLine(e.Message);
                    }
                    catch (TypeLoadException e)
                    {
                        Console.WriteLine("couldn't load type : " + typeOfInstance);
                        Console.WriteLine(e.Message);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandled exception type" + ex.GetType());
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Method that gets the name of the type of every peripheral instance created 
        /// </summary>
        /// <returns>A string list that contains all the types/instance names </returns>
        public static IList<string> GetAllInstanceNames()
        {
            return new List<string>(devicesDictionnary.Keys);

        }

        /// <summary>
        /// Static setter to give the eventHandler when it will be ready to peripheral 
        /// </summary>
        /// <param name="peripheralEventHandler"></param>
        public static void SetHandler(PeripheralEventHandler peripheralEventHandler)
        {
            peripheralEventHandlerProxy.SetEventHandler(peripheralEventHandler);
        }

        /// <summary>
        /// return the instance of the peripheral whose name is in parameter
        /// </summary>
        /// <param name="instance"> Name of the type of the peripheral instance (e.g. RandomDevice) </param>
        /// <returns> The instance matching the name, as an IDevice </returns>
        /// <exception cref="InexistantObjectException">Thrown when the object is not found in the dictionnary </exception>
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

        /// <summary>
        /// Method that returns all the effective method of a peripheral type
        /// Efective methods = method implemetned from object are removed/ 
        /// </summary> 
        /// <param name="objectType"> type of the peripheral </param>
        /// <returns> a list containing every method (object method info)</returns>
        public static HashSet<MethodInfo> FindMethods(Type objectType)
        {
            HashSet<MethodInfo> methodListResult = new HashSet<MethodInfo>();
            //Get every interfaces implemented by this type
            Type[] everyInterfaces = objectType.GetInterfaces();

            //Browsing through each interface implemented
            foreach (Type anInterface in everyInterfaces)
            {
                //Prevent user from calling Idipsose methods since it needs to be called only by the system.
                if(anInterface.Name.Equals("IDisposable"))
                {
                    continue;
                }

                //getting the method of the current interface
                MethodInfo[] methodsOfTheObject = anInterface.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                //Before adding each method , check if it is already in the returned list
                foreach (MethodInfo method in methodsOfTheObject)
                {
                    methodListResult.Add(method);
                }
            }

            return methodListResult;
        }

    }
}
