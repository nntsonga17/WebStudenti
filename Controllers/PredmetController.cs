using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studenti.Models;

namespace Studenti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PredmetController : ControllerBase
    {
        public FakultetContext Context { get; set; }

        public PredmetController(FakultetContext context)
        {
            Context = context;
        }
        [Route("PreuzmiPredmete")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi()
        {
            return Ok(await Context.Predmeti.Select(p => new { p.ID, p.Naziv}).ToListAsync());
        }
    }
}