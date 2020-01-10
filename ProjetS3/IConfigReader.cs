using System.Collections;

namespace ProjetS3
{
    interface IConfigReader
    {
        //dll name: example (without the .dll) but with the path
        ArrayList GetAllDllName();

        //only object name
        ArrayList GetAllInstancesFromOneDll(string libName);
    }
}
