using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Service;
using Version = JobOverview.Entities.Version;

namespace JobOverview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogicielsController : ControllerBase
    {
        private readonly IServiceLogiciels _service;

        public LogicielsController(IServiceLogiciels service)
        {
            _service = service;
        }

        // GET: api/Logiciels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logiciel>>> GetLogiciels([FromQuery] string? codeFiliere)
        {
            var logiciels = await _service.GetLogiciels(codeFiliere);
            return Ok(logiciels);
        }

        // GET: api/Logiciels/GENOMICA
        [HttpGet("{code}")]
        public async Task<ActionResult<Logiciel>> GetLogiciel(string code)
        {
            var logiciel = await _service.GetLogiciel(code);

            if (logiciel == null)
            {
                return NotFound();
            }

            return Ok(logiciel);
        }

        // GET: api/Logiciels/GENOMICA/versions?millesime=2023
        [HttpGet("{code}/versions")]
        public async Task<ActionResult<Version?>> GetVersions(string code, [FromQuery] short? millesime)
        {
            var versions = await _service.GetVersionsLogiciel(code, millesime);

            if (versions == null)
            {
                return NotFound();
            }

            return Ok(versions);
        }

        // GET: api/Logiciels/GENOMICA/Versions/1.00/Releases/30
        [HttpGet("{codeLogiciel}/Versions/{numVersion}/Releases/{numRelease}")]
        public async Task<ActionResult<Release?>> GetRelease(string codeLogiciel, float numVersion, short numRelease)
        {
            var release = await _service.GetRelease(codeLogiciel, numVersion, numRelease);

            if (release == null)
                return NotFound();

            return Ok(release);
        }

        // POST: api/Logiciels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{codeLogiciel}/Versions/{numVersion}/Releases")]
        public async Task<ActionResult<Logiciel>> PostRelease(string codeLogiciel, float numVersion, [FromForm] FormRelease formRel)
        {
            Release release = new Release()
            {
                Numero = formRel.Numero,
                NumeroVersion = numVersion,
                CodeLogiciel = codeLogiciel,
                DatePubli = formRel.DatePubli
            };

            if (formRel.Notes != null)
            {
                using StreamReader reader = new(formRel.Notes.OpenReadStream());
                release.Notes = await reader.ReadToEndAsync();
            }

            Release res = await _service.PostRelease(codeLogiciel, numVersion, release);

            object key = new { codeLogiciel = res.CodeLogiciel, numVersion = res.NumeroVersion, numRelease = res.Numero };
            string uri = Url.Action(nameof(PostRelease), key) ?? "";
            return Created(uri, res);
        }

        //// PUT: api/Logiciels/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLogiciel(string id, Logiciel logiciel)
        //{
        //    if (id != logiciel.Code)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(logiciel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LogicielExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Logiciels
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Logiciel>> PostLogiciel(Logiciel logiciel)
        //{
        //    _context.Logiciels.Add(logiciel);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (LogicielExists(logiciel.Code))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetLogiciel", new { id = logiciel.Code }, logiciel);
        //}

        //// DELETE: api/Logiciels/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLogiciel(string id)
        //{
        //    var logiciel = await _context.Logiciels.FindAsync(id);
        //    if (logiciel == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Logiciels.Remove(logiciel);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool LogicielExists(string id)
        //{
        //    return _context.Logiciels.Any(e => e.Code == id);
        //}
    }
}
