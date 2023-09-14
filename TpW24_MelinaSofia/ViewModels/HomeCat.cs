using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TpW24_MelinaSofia.ViewModels
{
    public class HomeCat
    {
        [Key, Column(Order = 0)]
        public int CatId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public bool? Actif { get; set; }
        public int TotalSujets { get; set; }
    }
}