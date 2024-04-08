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
    [Route("api/[controller]")]
    [ApiController]
    public class TachesController : ControllerBase
    {
        private readonly IServiceTaches _service;

        public TachesController(IServiceTaches service)
        {
            _service = service;
        }

        #region GET
        // GET: api/Taches?personne=x&logiciel=y&version=z
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tache>>> GetTaches(
            [FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
        {
            List<Tache> res = await _service.GetTaches(personne, logiciel, version);

            if (res.Count == 0)
                return NotFound();

            return Ok(res);
        }

        // GET: api/Taches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tache?>> GetTache(int id)
        {
            Tache? tache = await _service.GetTache(id);

            if (tache == null)
                return NotFound();

            return Ok(tache);
        }

        // GET: api/Taches/44/Travaux
        [HttpGet("{idTache}/Travaux")]
        public async Task<ActionResult<IEnumerable<Travail>>> GetTravaux(int idTache)
        {
            return await _service.GetTravaux(idTache);
        }


        // GET: api/Personnes/RBEAUMONT
        [HttpGet("/api/personnes/{pseudo}")]
        public async Task<ActionResult<Personne?>> GetPersonne(string pseudo)
        {
            var personne = await _service.GetPersonne(pseudo);

            if (personne == null)
                return NotFound();

            return Ok(personne);
        }
        #endregion

        #region POST
        // POST: api/Taches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Tache>> PostTache(Tache tache)
        //{
        //    try
        //    {
        //        await _service.PostTache(tache);
        //        return CreatedAtAction(nameof(GetTache), new { id = tache.Id }, tache);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.CustomResponseForError(ex);
        //    }
        //}

        // POST: api/Taches/44/Travaux
        [HttpPost("{idTache}/Travaux")]
        public async Task<ActionResult<Tache>> PostTravail([FromRoute] int idTache, Travail travail)
        {
            try
            {
                await _service.PostTravail(idTache, travail);
                return CreatedAtAction(nameof(GetTravaux), new { idTache }, travail);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
        #endregion

        #region DELETE
        // DELETE: api/Taches/45/Travaux/2023-11-23
        [HttpDelete("{idTache}/Travaux/{date}")]
        public async Task<IActionResult> DeleteTravail(int idTache, DateOnly date)
        {
            try
            {
                await _service.DeleteTravail(idTache, date);
                return NoContent();
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }

        // DELETE: api/Taches?personne=x&logiciel=y&version=z
        [HttpDelete]
        public async Task<IActionResult> DeleteTaches(
            [FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
        {
            try
            {
                int nbSupr = await _service.DeleteTaches(personne, logiciel, version);
                return Ok(nbSupr + " tâches supprimées");
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
        #endregion

        //private bool TacheExists(int id)
        //{
        //    return _context.Taches.Any(e => e.Id == id);
        //}

        // PUT: api/Taches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTache(int id, Tache tache)
        //{
        //    if (id != tache.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tache).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TacheExists(id))
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

        // PUT: api/Taches/5
        [HttpPut]
        public async Task<ActionResult<Tache>> PutTache(Tache tache)
        {
            try
            {
                Tache result = await _service.PutPostTache(tache);

                // Si elle est ajoutée, EF renvoie 0
                if(result.Id == 0)
                    return CreatedAtAction(nameof(GetTache), new { result.Id }, result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
    }
}
