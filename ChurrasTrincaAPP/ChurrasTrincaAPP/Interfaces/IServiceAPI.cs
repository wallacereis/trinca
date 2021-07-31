using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChurrasTrincaAPP.Interfaces
{
    public interface IServiceAPI<T> where T : class
    {
        Task<bool> UploadFileAsync(Stream image, string fileName, string urlUpload);
        Task<List<T>> GetEstadosAsync();
        Task<List<T>> GetMunicipiosAsync(int EstadoID);
    }
}
