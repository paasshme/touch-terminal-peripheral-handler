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
        private static ConfigReader reader;
        private static Dictionary<string, IDevice> devices;
        private static PeripheralEventHandlerProxy peh = PeripheralEventHandlerProxy.GetInstance();

        public static void Init()
        {
            /*if (reader is null || peh is null) // Init already call or WebSocket not connected => TODO change to singleton
                return;*/

            System.Diagnostics.Debug.WriteLine("Init factory");
            Console.WriteLine("Init factory");

            devices = new Dictionary<string, IDevice>();
            reader = new ConfigReader(CONFIGURATION_FILE_PATH);

            ArrayList tab = reader.GetAllDllName();

            //Create instances
            //Get every instances for every dll
            foreach (string s in tab)
            {
                Console.WriteLine(s);
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(s + ".dll");
                    foreach(AssemblyName name in assembly.GetReferencedAssemblies())
                    {
                        Assembly.Load(name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                  //  throw new MissingDllException("Missing .dll file", ex);
                }


                foreach (var o in assembly.GetTypes())
                {
                    Console.WriteLine(o);
                }
           
                ArrayList instances = reader.GetAllInstancesFromOneDll(s);

                foreach (string instanceName in instances)
                {
                    
                        //System.Diagnostics.Debug.WriteLine(s + "." + instanceName);
                        //Console.WriteLine("[DEBUUUUUUUUUUUUUUG]");
                        //Console.WriteLine(s+"."+instanceName);
                        string[] str = s.Split("/");
                        string si = str[str.Length -1];
                        
                        //Console.WriteLine(si+"."+instanceName);
                        Object[] objectParameters = reader.GetParametersForOneInsance(s,instanceName);
                    
                    try
                    {
                        Console.WriteLine("PARAMETERS : ");
                        foreach(Object o in objectParameters)
                        {
                            Console.WriteLine(o + " : "+ o.GetType());
                        }

                        Console.WriteLine(si);
                        Type t = assembly.GetType(si + "." + instanceName);
                        Console.WriteLine("TYPE IS" + t);
                      var obj = Activator.CreateInstance(assembly.GetType(si + "." + instanceName),objectParameters) as IDevice;
                      // var obj = assembly.CreateInstance(si + "." + instanceName, false, BindingFlags.CreateInstance, null, objectParameters, null, null) as IDevice;
                        Console.WriteLine("OBJ : " + obj);
                        //var oo = obj.GetType();
                        //var or = obj.GetType().GetMethods();
                        
                        obj.eventHandler = PeripheralEventHandlerProxy.GetInstance();
                        devices.Add(instanceName, obj);

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("FACTORY EXCEPTION : "+ex);
                        Console.WriteLine("INSTANCE : " + instanceName);
                       // Console.WriteLine("TYPE : " + oo);
                        //throw new Exception("Instance is not castable as IDevice", ex);
                    }
                    
                     
                }
            }

        }
        public static void SetHandler(PeripheralEventHandler _peh)

        {
            peh.SetEventHandler(_peh);
        }

        public static IDevice GetInstance(string instance)
        {
            foreach (string s in devices.Keys)
                System.Diagnostics.Debug.WriteLine("There is " + s);


            try
            {
                System.Diagnostics.Debug.WriteLine("Ask instance is " + instance);
                return devices[instance];
            }
            catch (Exception ex)
            {
                throw new InexistantObjectException("", ex);
            }
        }

        public static List<MethodInfo> FindMethods(Type objectType)
        {
            List<MethodInfo> result = new List<MethodInfo>();
            Type[] everyInterfaces = objectType.GetInterfaces();

            foreach (Type intf in everyInterfaces)
            {
                MethodInfo[] methodes = intf.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (MethodInfo m in methodes)
                {
                    //Console.WriteLine(m.Name);
                    if (!result.Contains(m))
                        result.Add(m);
                }
            }
            return result;
        }

    }
}
