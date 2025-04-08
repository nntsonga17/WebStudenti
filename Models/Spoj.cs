using System.ComponentModel.DataAnnotations;

namespace Studenti.Models
{
    public class Spoj
    {
        public int ID { get; set; }

        [Range(5,10)]
        public int Ocena { get; set; }

        public IspitniRok? IspitniRok { get; set; }

        public Student? Student { get; set; }
        public Predmet? Predmet { get; set; }

        
    }
}