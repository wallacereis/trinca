using System.Collections.Generic;

namespace ChurrasTrincaAPP.Models
{
    public class Root
    {
        /* EF Relations - ManyToOne */
        public List<BBQ> data { get; set; }
        public Root()
        {
            data = new List<BBQ>();
        }
    }
}
