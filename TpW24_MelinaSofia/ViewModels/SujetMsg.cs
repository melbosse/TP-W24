using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TpW24_MelinaSofia.Models;

namespace TpW24_MelinaSofia.ViewModels
{
    public class SujetMsg
    {
        [Key, Column(Order = 0)]

        public int SujetId { get; set; }
        public int? CatId { get; set; }
        public string? UserId { get; set; }
        public AspNetUser? UserName { get; set; }
        public string? Titre { get; set; }
        public string? Texte { get; set; }
        public DateTime Date { get; set; }
        public int? Vues { get; set; }
        public bool? Actif { get; set; }
        public Message? DernierMsg { get; set; }
        public int TotalMessages { get; set; }
    }
}