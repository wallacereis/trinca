using System;
using System.Collections.Generic;
using System.Text;

namespace ChurrasTrincaAPP.Models
{
    public class Participant
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool confirmed { get; set; }
        public decimal value_paid { get; set; }

        /* EF Relations - OneToMany */
        public BBQ BBQ { get; set; }
        public int bbq_id { get; set; }

        /* Support Properties */
        public bool IsConfirmed => confirmed != true;
    }
}
