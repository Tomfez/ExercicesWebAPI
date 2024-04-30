using Microsoft.AspNetCore.Mvc;
using JobOverview.Entities;
using JobOverview.Service;
using Microsoft.AspNetCore.Authorization;
using JobOverview.Tools;

namespace JobOverview.Controllers
{
    // api/Filieres/BIOV/Equipes
    [Route("api/Filieres/{codeFiliere}/[controller]")]
    [ApiController]
    [Authorize(Policy = "GererEquipes")]
    public class EquipesController(IServiceEquipes service, ILogger<EquipesController> logger) : ControllerBase
    {
        private readonly IServiceEquipes _service = service;
        private readonly ILogger<EquipesController> _logger = logger;

        #region GET
        // GET: api/Filieres/BIOH/Equipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipe>>?> GetEquipes(string codeFiliere)
        {
            var res = await _service.GetEquipes(codeFiliere);
            return res.ConvertToObjectResult();
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
        #endregion

        #region POST
        // POST: api/Filieres/BIOH/Equipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipe>> PostEquipe(string codeFiliere, Equipe equipe)
        {
            ServiceResult<Equipe?> res = await _service.PostEquipe(codeFiliere, equipe);

            // Renvoie une réponse de code 201 avec l'en-tête 
            // "location: <url d'accès à l’équipe>" et un corps contenant l’équipe
            string uri = Url.Action(nameof(GetEquipe),
                    new { codeFiliere = res.Data?.CodeFiliere, codeEquipe = res.Data?.Code }) ?? "";

            return res.ConvertToObjectResult(uri);
        }

        // POST: api/Filieres/BIOV/Equipes/BIOV_MKT
        [HttpPost("{nomEquipe}")]
        public async Task<ActionResult<Personne>> PostPersonne(string codeFiliere, string nomEquipe, Personne personne)
        {
            ServiceResult<Personne?> res = await _service.PostPersonne(nomEquipe, personne);

            // Renvoie une réponse de code 201 avec l'en-tête 
            // "location: <url d'accès à l’équipe de la personne>" et un corps contenant l’équipe
            string uri = Url.Action(nameof(GetEquipe), new { codeFiliere, nomEquipe }) ?? "";

            return res.ConvertToObjectResult(uri);

        }
        #endregion


        // PUT: api/Equipes?manager=x
        [HttpPut("{codeEquipe}")]
        public async Task<ActionResult> PutEquipe(string codeEquipe, [FromQuery] string manager)
        {
            ServiceResult<int> res = await _service.PutPersonne(codeEquipe, manager);
            return res.ConvertToObjectResult();
        }

    }
}
