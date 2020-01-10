using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using IDeviceLib;

//Few line of comments
namespace ProjetS3
{

    public class PeripheralFactory
    {
        private const string CONFIGURATION_FILE_PATH = "Config.xml";
        private static ConfigReader reader;
        private static Dictionary<string, IDevice> devices;
        private static PeripheralEventHandler peh;
        public static void Init()
        {
            /*if (reader is null || peh is null) // Init already call or WebSocket not connected => TODO change to singleton
                return;*/

            System.Diagnostics.Debug.WriteLine("Init factory");

            devices = new Dictionary<string, IDevice>();
            reader = new ConfigReader(CONFIGURATION_FILE_PATH);

            ArrayList tab = reader.GetAllDllName();

            //Create instances
            //Get every instances for every dll
            foreach (string s in tab)
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(s + ".dll");
                    
                }
                catch (Exception ex)
                {
                    throw new MissingDllException("Missing .dll file", ex);
                }

           
                ArrayList instances = reader.GetAllInstancesFromOneDll(s);

                foreach (string instanceName in instances)
                {


                    try
                    {
                        
                        System.Diagnostics.Debug.WriteLine(s + "." + instanceName);

                        var obj = assembly.CreateInstance(s + "." + instanceName) as IDevice;
                        System.Diagnostics.Debug.WriteLine(peh);
                        obj.eventHandler = peh; //Might crash TODO raise exception
                        devices.Add(instanceName, obj);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Instance is not castable as Idevice", ex);
                    }
                }
            }


            foreach (var o in devices.Values)
                System.Diagnostics.Debug.WriteLine("Devices are "+o);


        }

        public static void SetHandler(PeripheralEventHandler _peh)
        {
            peh = _peh;
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
