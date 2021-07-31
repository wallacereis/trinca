using System.Collections.Generic;

namespace ChurrasTrincaAPP.Models
{
    public class Response<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public string exception { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public Error()
        {
            this.errors = new List<string>();
        }

        public List<string> errors { get; set; }
    }
}
