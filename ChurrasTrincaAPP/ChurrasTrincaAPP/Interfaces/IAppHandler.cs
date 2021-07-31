using System.Threading.Tasks;

namespace ChurrasTrincaAPP.Interfaces
{
    public interface IAppHandler
    {
        Task<bool> LaunchApp(string uri);
    }
}
