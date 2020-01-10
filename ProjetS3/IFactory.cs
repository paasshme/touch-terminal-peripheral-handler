
//deprecated
namespace ProjetS3
{
    public interface IFactory
    {
        object create(string ObjectName, string MethodName, object[] parameters);
    }
}
