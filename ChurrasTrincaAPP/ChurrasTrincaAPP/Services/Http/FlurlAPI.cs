using ChurrasTrincaAPP.Helpers;
using ChurrasTrincaAPP.Interfaces;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ChurrasTrincaAPP.Services.Http
{
    public class FlurlAPI<T> : IFlurlAPI<T> where T : class
    {
        public async Task<List<T>> GetAllItemsAsync(string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                //.AppendPathSegment(entity)
                .WithHeader("Accept", "application/json")
                .WithOAuthBearerToken(token)
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsAsync(string entity, string parameter)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsAsync(string entity, string parameter, int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsAsync(string entity, string parameter1, string parameter2, int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter1}/{parameter2}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsAsync(string entity, int id, string parameter, string value)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{id}")
                .SetQueryParam(parameter, value)
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsByMultiplesIdAsync(string entity, string parameter, int id1, int id2)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter}/{id1}/{id2}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsWithParameterAsync(string entity, string action, string parameter, string value)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{action}")
                .SetQueryParam(parameter, value)
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsByParamsAsync(string entity, string action, int id1, int id2, int id3)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{action}")
                .SetQueryParams
                (
                    new
                    {
                        CategoriaID = id1,
                        ProfissionalID = id2,
                        StatusOrcamentoID = id3
                    }
                )
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<byte[]> GetFileAsync(string file, string token)
        {
            var result =
                await Constants.BaseAddressPDF
                .AppendPathSegment($"{file}")
                .WithHeader("Accept", "application/json")
                //.WithOAuthBearerToken(token)
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(5))
                .GetBytesAsync();
            return result;
        }

        public async Task<T> GetItemAsync(string entity, string parameter, int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> GetItemDoubleIdAsync(string entity, string parameter, int id1, int id2)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{parameter}/{id1}/{id2}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public Task<List<T>> GetAllItemsByIdAsync(string entity, string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GettemDoubleIdAsync(string entity, string parameter, int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetItemsDynamicAsync(string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result = 
                await Constants.BaseAddressAPI
                .WithHeader("Accept", "application/json")
                .WithOAuthBearerToken(token)
                .WithTimeout(TimeSpan.FromMinutes(2))
                .AllowHttpStatus("400-500")
                .GetAsync()
                .ReceiveJson<T>();
            return result;
        }

        public async Task<T> GetItemByIdAsync(string entity, int id, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (string.IsNullOrWhiteSpace(entity))
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{id}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .AllowHttpStatus("400-500")
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .GetJsonAsync<T>();
                return result;
            }
            else
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{entity}/{id}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .AllowHttpStatus("400-500")
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .GetJsonAsync<T>();
                return result;
            }
        }

        public async Task<T> GetItemByEmailAsync(string entity, string email)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{email}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> GetItemByEmailAsync(string entity, string action, string emailParameter)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{action}")
                .SetQueryParams
                (
                    new
                    {
                        email = emailParameter
                    }
                )
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> GetItemByLoginAsync(string entity, string action, string emailParameter, string senhaParameter)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{action}")
                .SetQueryParams
                (
                    new
                    {
                        email = emailParameter,
                        senha = senhaParameter
                    }
                )
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> GetItemByParameterAsync(string entity, string action, int id1, int id2)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{action}")
                .SetQueryParams
                (
                    new
                    {
                        PedidoID = id1,
                        ProdutoID = id2
                    }
                )
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> AddItemAsync(string entity, T obj, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (string.IsNullOrWhiteSpace(entity))
            {
                var result =
                    await Constants.BaseAddressAPI
                    //.AppendPathSegment($"{entity}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .AllowHttpStatus("400-500")
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .PostJsonAsync(obj)
                    .ReceiveJson<T>();
                return result;
            }
            else
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{entity}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .AllowHttpStatus("400-500")
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .PostJsonAsync(obj)
                    .ReceiveJson<T>();
                return result;
            }
        }

        public async Task<T> PostItemAsync(string entity, Dictionary<string, object> data)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result = await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .PostJsonAsync(data)
                .ReceiveJson<T>();
            return result;
        }

        public async Task<T> UpdateItemAsync(string entity, int id, T obj, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (string.IsNullOrWhiteSpace(entity))
            {
                var result =
                    await Constants.BaseAddressAPI
                    //.AppendPathSegment($"{id}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .AllowHttpStatus("400-500")
                    .PutJsonAsync(obj);
                //.ReceiveJson<T>();
                return null;
            }
            else
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{entity}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .AllowHttpStatus("400-500")
                    .PutJsonAsync(obj);
                //.ReceiveJson<T>();
                return null;
            }
        }

        public async Task<T> DeleteItemAsync(string entity, int id, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (string.IsNullOrWhiteSpace(entity))
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{id}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .AllowHttpStatus("400-500")
                    .DeleteAsync();
                //.ReceiveJson<T>();
                return null;
            }
            else
            {
                var result =
                    await Constants.BaseAddressAPI
                    .AppendPathSegment($"{entity}/{id}")
                    .WithHeader("Accept", "application/json")
                    .WithOAuthBearerToken(token)
                    .WithTimeout(TimeSpan.FromMinutes(2))
                    .AllowHttpStatus("400-500")
                    .DeleteAsync();
                //.ReceiveJson<T>();
                return null;
            }
        }

        public async Task<T> UploadItemAsync(string entity, Stream file, int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/upload/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .PostJsonAsync(file)
                .ReceiveJson<T>();
            return result;
        }
    }
}
