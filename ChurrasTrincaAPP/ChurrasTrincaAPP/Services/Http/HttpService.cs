using System;
using System.Net.Http;

namespace ChurrasTrincaAPP.Services.Http
{
    sealed class HttpService
    {
        #region [ Properties ]
        private static Lazy<HttpService> _lazy = new Lazy<HttpService>(() => new HttpService());
        public static HttpService Current => _lazy.Value;
        private HttpClient _httpClient;
        #endregion

        private HttpService()
        {
            _httpClient = new HttpClient();
        }
    }
}
