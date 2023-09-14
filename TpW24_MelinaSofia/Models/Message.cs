using System;
using System.Collections.Generic;

namespace TpW24_MelinaSofia.Models
{
    public partial class Message
    {
        public int MsgId { get; set; }
        public int? SujetId { get; set; }
        public string? UserId { get; set; }
        public string Texte { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool? Actif { get; set; }
        public string? Description { get; set; }

        public virtual Sujet? Sujet { get; set; }
        public virtual AspNetUser? User { get; set; }
    }
}
