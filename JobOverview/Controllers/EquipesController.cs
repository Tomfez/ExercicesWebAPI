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

namespace JobOverview.Controllers
{
    [Route("api/Filieres/{codeFiliere}/[controller]")]
    [ApiController]
    public class EquipesController : ControllerBase
    {
        private readonly IServiceEquipes _service;

        public EquipesController(IServiceEquipes service)
        {
            _service = service;
        }

        // GET: api/Filieres/BIOH/Equipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipe>?>> GetEquipes(string codeFiliere)
        {
            return Ok(await _service.GetEquipes(codeFiliere));
        }

        // GET: api/Filieres/BIOH/Equipes/BIOH_DEV
        [HttpGet("{nomEquipe}")]
        public async Task<ActionResult<Equipe?>> GetEquipe(string codeFiliere, string nomEquipe)
        {
            var equipe = await _service.GetEquipe(codeFiliere, nomEquipe);

            if (equipe == null)
            {
                return NotFound();
            }

            return Ok(equipe);
        }

        // PUT: api/Equipes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEquipe(string id, Equipe equipe)
        //{
        //    if (id != equipe.Code)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(equipe).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EquipeExists(id))
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

        //// POST: api/Equipes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Equipe>> PostEquipe(Equipe equipe)
        //{
        //    _context.Equipes.Add(equipe);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (EquipeExists(equipe.Code))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetEquipe", new { id = equipe.Code }, equipe);
        //}

        //// DELETE: api/Equipes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEquipe(string id)
        //{
        //    var equipe = await _context.Equipes.FindAsync(id);
        //    if (equipe == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Equipes.Remove(equipe);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EquipeExists(string id)
        //{
        //    return _context.Equipes.Any(e => e.Code == id);
        //}
    }
}
