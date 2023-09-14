using System;
using System.Collections.Generic;

namespace TpW24_MelinaSofia.Models
{
    public partial class Sujet
    {
        public Sujet()
        {
            Messages = new HashSet<Message>();
        }

        public int SujetId { get; set; }
        public int? CatId { get; set; }
        public string? UserId { get; set; }
        public string Titre { get; set; } = null!;
        public string Texte { get; set; } = null!;
        public DateTime Date { get; set; }
        public int? Vues { get; set; }
        public bool? Actif { get; set; }

        public virtual Category? Cat { get; set; }
        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
