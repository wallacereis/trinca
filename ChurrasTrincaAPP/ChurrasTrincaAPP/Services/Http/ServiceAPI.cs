using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using ChurrasTrincaAPP.Helpers;

namespace ChurrasTrincaAPP.Services.Http
{
    public class ServiceAPI<T> where T : class
    {
        private HttpClient client = new HttpClient();

        //Obtém a lista de Bancos Brasileiros
        public static async Task<List<T>> GetBancosAsync()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"{Constants.BaseAddressBB}/bancos.json").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<T>>(
                       await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }
            return null;
        }

        //Obtém o conjunto de Unidades da Federação do Brasil - Referência IBGE
        public static async Task<List<T>> GetEstadosAsync()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"{Constants.BaseAddressIBGE}/localidades/estados").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<T>>(
                       await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }
            return null;
        }

        //Obtém o conjunto de municípios do Brasil a partir dos identificadores das Unidades da Federação - Referência IBGE
        public static async Task<List<T>> GetMunicipiosAsync(int EstadoID)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"{Constants.BaseAddressIBGE}/localidades/estados/"+EstadoID+"/municipios").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<T>>(
                       await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }
            return null;
        }

        //Upload Files to Server
        public static async Task<bool> UploadFileAsync(Stream image, string fileName, string urlUpload)
        {
            HttpContent fileStreamContent = new StreamContent(image);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent);
                var response = await client.PostAsync(urlUpload, formData);
                if (response.IsSuccessStatusCode)
                    return response.IsSuccessStatusCode;
                else
                {
                    int statusCode = (int)response.StatusCode;
                    string reasonPhrase = response.ReasonPhrase;
                    string requestMessage = response.RequestMessage.Headers.ToString();
                    return false;
                }
            }
        }
    }
}
