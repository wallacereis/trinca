using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChurrasTrincaAPP.Interfaces
{
    public interface IFlurlAPI<T> where T : class
    {
        /*-------------------------- List Result --------------------------*/
        Task<List<T>> GetAllItemsAsync(string token);
        Task<List<T>> GetAllItemsAsync(string entity, string parameter);
        Task<List<T>> GetAllItemsAsync(string entity, string parameter, int id);
        Task<List<T>> GetAllItemsAsync(string entity, string parameter1, string parameter2, int id);
        Task<List<T>> GetAllItemsAsync(string entity, int id, string parameter, string value);
        Task<List<T>> GetAllItemsWithParameterAsync(string entity, string action, string parameter, string value);
        Task<List<T>> GetAllItemsByMultiplesIdAsync(string entity, string parameter, int id1, int id2);

        /*-------------------------- Unique Result --------------------------*/
        Task<T> GetItemsDynamicAsync(string token);
        Task<T> GetItemAsync(string entity, string parameter, int id);
        Task<T> GettemDoubleIdAsync(string entity, string parameter, int id1, int id2);
        Task<T> GetItemByIdAsync(string entity, int id, string token);
        Task<T> GetItemByEmailAsync(string entity, string email);
        Task<T> GetItemByEmailAsync(string entity, string action, string email);
        Task<T> GetItemByLoginAsync(string entity, string action, string emailParameter, string senhaParameter);

        /*-------------------------- CRUD Result --------------------------*/
        Task<T> AddItemAsync(string entity, T obj, string token);
        Task<T> UpdateItemAsync(string entity, int id, T obj, string token);
        Task<T> DeleteItemAsync(string entity, int id, string token);

        /*-------------------------- File Result --------------------------*/
        Task<byte[]> GetFileAsync(string file, string token);

        /*-------------------------- Upload Result --------------------------*/
        Task<T> UploadItemAsync(string entity, Stream file, int id);

        /*-------------------------- Post Item Result --------------------------*/
        Task<T> PostItemAsync(string entity, Dictionary<string, object> data);
    }
}
