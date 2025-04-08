using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studenti.Models;

namespace Studenti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class StudentController : ControllerBase
    {
        public FakultetContext Context { get; set; }

        public StudentController(FakultetContext context)
        {
            Context = context;
        }

        [Route("Studenti")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi([FromQuery] int[] rokIDs)
        {
            var studenti = Context.Studenti
                .Include(p => p.StudentPredmet!/* .Where(p => p.Student.Indeks == 18309)*/)
                .ThenInclude(p => p.IspitniRok!)
                .Include(p => p.StudentPredmet!)
                .ThenInclude(p => p.Predmet);
            
            var student = await studenti.ToListAsync();


            return Ok
            (
                student.Select(p => 
                new 
                {
                    Indeks = p.Indeks,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    Predmeti = p.StudentPredmet!
                    .Where(q => rokIDs.Contains(q.IspitniRok!.ID))
                    .Select
                    (
                        q => new 
                        {
                            Predmet = q.Predmet!.Naziv,
                            GodinaPredmeta = q.Predmet.Godina,
                            IspitniRok = q.IspitniRok!.Naziv,
                            Ocena = q.Ocena
                        })
                }).ToList()
            );
        }

        [Route("DodatiStudenta")]
        [HttpPost]
        public async Task<ActionResult> DodajStudenta([FromBody] Student student)
        {
            if(student.Indeks < 10000 || student.Indeks > 20000 )
            {
                return BadRequest("Pogresan ID");
            }

            if(string.IsNullOrWhiteSpace(student.Ime) || student.Ime.Length > 50)
            {
                return BadRequest("Pogresno ime!");
            }

            if(string.IsNullOrWhiteSpace(student.Prezime) || student.Prezime.Length > 50)
            {
                return BadRequest("Pogresno prezime!");
            }

            try
            {
                Context.Studenti.Add(student);
                await Context.SaveChangesAsync();
                return Ok($"Student je dodat! ID je {student.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            

        }
        [Route("StudentiPretraga/{rokovi}/{predmetID}")]
        [HttpGet]
        public async Task<ActionResult> StudentiPretraga(string rokovi, int predmetID)
        {
            var rokIDs= rokovi.Split('a')
                .Where(x=> int.TryParse(x, out _))
                .Select(int.Parse)
                .ToList();

            var studenti = Context.StudentiPredmeti
                .Include(p => p.Student!/* .Where(p => p.Student.Indeks == 18309)*/)
                .Include(p => p.IspitniRok!)
                .Include(p => p.Predmet)
                .Where(p=> p.Predmet!.ID==predmetID
                && rokIDs.Contains(p.IspitniRok!.ID));
            
            var student = await studenti.ToListAsync();


            return Ok
            (
                student.Select(p => 
                new 
                {
                    Indeks = p.Student!.Indeks,
                    Ime = p.Student.Ime,
                    Prezime = p.Student.Prezime,
                    Predmeti = p.Predmet!.Naziv!,
                    Rok = p.IspitniRok!.Naziv,
                    Ocena = p.Ocena
                }).ToList()
            );
        }
        [Route("StudentiPretragaFromBody/{predmetID}")]
        [HttpPut]
        public async Task<ActionResult> StudentiPretragaFromBody([FromRoute]int predmetID, [FromBody]int[] rokIDs)
        {
            

            var studenti = Context.StudentiPredmeti
                .Include(p => p.Student!/* .Where(p => p.Student.Indeks == 18309)*/)
                .Include(p => p.IspitniRok!)
                .Include(p => p.Predmet)
                .Where(p=> p.Predmet!.ID==predmetID
                && rokIDs.Contains(p.IspitniRok!.ID));
            
            var student = await studenti.ToListAsync();


            return Ok
            (
                student.Select(p => 
                new 
                {
                    Indeks = p.Student!.Indeks,
                    Ime = p.Student.Ime,
                    Prezime = p.Student.Prezime,
                    Predmeti = p.Predmet!.Naziv!,
                    Rok = p.IspitniRok!.Naziv,
                    Ocena = p.Ocena
                }).ToList()
            );
        }


        [Route("PromenitiStudenta/{indeks}/{ime}/{prezime}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int indeks, string ime, string prezime)
        {
            if(indeks < 10000 || indeks > 20000 )
            {
                return BadRequest("Pogresan ID");
            }

            try
            {
                var student = Context.Studenti.Where(p => p.Indeks == indeks).FirstOrDefault();
                if(student != null)
                {
                    student.Ime = ime;
                    student.Prezime = prezime;

                    await Context.SaveChangesAsync();
                    return Ok($"Student uspesno promenjen! ID: {student.ID}");
                }
                else
                {
                    return BadRequest("Student nije pronadjen!");
                }
            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
        [Route("PromenaFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniBody([FromBody] Student student)
        {
            if(student.ID <= 0)
            {
                return BadRequest("Pogresan ID");
            }
            if(student.Indeks < 10000 || student.Indeks > 20000 )
            {
                return BadRequest("Pogresan ID");
            }

            if(string.IsNullOrWhiteSpace(student.Ime) || student.Ime.Length > 50)
            {
                return BadRequest("Pogresno ime!");
            }

            if(string.IsNullOrWhiteSpace(student.Prezime) || student.Prezime.Length > 50)
            {
                return BadRequest("Pogresno prezime!");
            }
            try
            {
                // var studentZaPromenu = await Context.Studenti.FindAsync(student.ID);
                // studentZaPromenu.Indeks = student.Indeks;
                // studentZaPromenu.Ime = student.Ime;
                // studentZaPromenu.Prezime = student.Prezime;

                // await Context.SaveChangesAsync();
                // return Ok($"Student je uspesno promenjen! ID: {studentZaPromenu.ID}");

                Context.Studenti.Update(student);

                await Context.SaveChangesAsync();
                return Ok($"Student je uspesno promenjen! ID: {student.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("IzbrisatiStudenta/{id}")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisi(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Pogresan ID");
            }
            try
            {
                var student = await Context.Studenti.FindAsync(id);
                int indeks = student!.Indeks;
                Context.Studenti.Remove(student);
                await Context.SaveChangesAsync();
                return Ok($"Uspesno Izbrisan student sa indeksom: {indeks}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
