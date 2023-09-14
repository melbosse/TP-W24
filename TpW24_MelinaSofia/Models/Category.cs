using System;
using System.Collections.Generic;

namespace TpW24_MelinaSofia.Models
{
    public partial class Category
    {
        public Category()
        {
            Sujets = new HashSet<Sujet>();
        }

        public int CatId { get; set; }
        public string Nom { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool? Actif { get; set; }

        public virtual ICollection<Sujet> Sujets { get; set; }
    }
}
