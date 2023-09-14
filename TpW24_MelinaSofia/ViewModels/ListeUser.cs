using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TpW24_MelinaSofia.ViewModels
{
    public class ListeUser
    {
        [Key, Column(Order = 0)]
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int TotalSujets { get; set; }
        public int TotalMsg { get; set; }
        public DateTime DateMsg { get; set; }
        public DateTime DateSujet { get; set; }
        public DateTime DateLastAction { get; set; }
    }
}