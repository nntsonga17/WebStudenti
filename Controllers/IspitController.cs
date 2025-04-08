using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studenti.Models;

namespace Studenti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class IspitController : ControllerBase
    {
        public FakultetContext Context { get; set; }

        public IspitController(FakultetContext context)
        {
            Context = context;
        }
        [Route("IspitniRokovi")]
        [HttpGet]
        public async Task<ActionResult> Rokovi()
        {
            
            return Ok(await Context.IspitniRokovi.Select(p =>
            new 
            {
                ID = p.ID,
                Naziv = p.Naziv
            }).ToListAsync());
        }
        [Route("DodajPolozeniIspit/{indeks}/{idPredmeta}/{idRoka}/{ocena}")]
        [HttpPost]
        public async Task<ActionResult> DodajIspit(int indeks, int idPredmeta, int idRoka, int ocena)
        {
            if(indeks < 10000 || indeks > 20000)
            {
                BadRequest("Pogresan broj indeksa");
            }
            //...provere

            try
            {
                var student = await Context.Studenti.Where(p => p.Indeks == indeks).FirstOrDefaultAsync();
                var predmet = await Context.Predmeti.Where(p => p.ID == idPredmeta).FirstOrDefaultAsync();
                var ispitniRok = await Context.IspitniRokovi.FindAsync(idRoka); //samo za id

                var polozioRok = Context.StudentiPredmeti
                .Include(x=>x.Student)
                .Include(x=>x.Predmet)
                .Where(x=>x.Student!.ID==student!.ID
                && x.Predmet!.ID==predmet!.ID)
                .FirstOrDefault();

                if(polozioRok!=null)
                {
                    return BadRequest("Student je vec polozio taj ispit");
                }
                else
                {
                    Spoj s = new Spoj
                    {
                        Student = student,
                        Predmet = predmet,
                        IspitniRok = ispitniRok,
                        Ocena = ocena
                    };

                    Context.StudentiPredmeti.Add(s);
                    await Context.SaveChangesAsync();

                    var podaciOStudentu = await Context.StudentiPredmeti
                            .Include(p => p.Student)
                            .Include(p => p.Predmet)
                            .Include(p => p.IspitniRok)
                            .Where(p => p.Student!.Indeks == indeks)
                            .Select(p =>
                            new
                            {
                                Indeks = p.Student!.Indeks,
                                Ime = p.Student.Ime,
                                Prezime = p.Student.Prezime,
                                Predmet = p.Predmet!.Naziv,
                                IspitniRok = p.IspitniRok!.Naziv,
                                Ocena = p.Ocena
                            }).ToListAsync();
                    return Ok(podaciOStudentu);
                    }

                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
   
}            