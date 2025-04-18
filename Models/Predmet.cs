using System.ComponentModel.DataAnnotations;

namespace Studenti.Models
{
    public class Predmet
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string? Naziv { get; set; }

        [Range(1,5)]
        public int Godina { get; set; }

        public List<Spoj>? PredmetStudent { get; set; }
    }
}