using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TpW24_MelinaSofia.Models;

namespace TpW24_MelinaSofia.ViewModels
{
    public class Catsuj
    {
        [Key, Column(Order = 0)]
        public int CatId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public IEnumerable<Sujet> Sujets { get; set; }
    }
}