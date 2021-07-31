using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurrasTrincaAPP.Models
{
    public class BBQ
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public decimal value_per_person { get; set; }

        /* EF Relations - ManyToOne */
        public List<Participant> Participants { get; set; }
        public BBQ()
        {
            Participants = new List<Participant>();
        }

        /* Support Properties */
        public int TotalParticipantes => Participants.Count;
        public decimal ValorTotalPago => Participants.Select(b => b.value_paid).Sum();
    }
}
