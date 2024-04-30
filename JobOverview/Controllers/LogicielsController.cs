using Microsoft.AspNetCore.Mvc;
using JobOverview.Entities;
using JobOverview.Service;
using Version = JobOverview.Entities.Version;
using JobOverview.Tools;

namespace JobOverview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogicielsController(IServiceLogiciels service, ILogger<LogicielsController> logger) : ControllerBase
    {
        private readonly IServiceLogiciels _service = service;
        private readonly ILogger<LogicielsController> _logger = logger;

        #region GET
        // GET: api/Logiciels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logiciel>>> GetLogiciels([FromQuery] string? codeFiliere)
        {
            var logiciels = await _service.GetLogiciels(codeFiliere);
            return logiciels.ConvertToObjectResult();
        }

        // GET: api/Logiciels/GENOMICA
        [HttpGet("{code}")]
        public async Task<ActionResult<Logiciel>> GetLogiciel(string code)
        {
            var logiciel = await _service.GetLogiciel(code);
            return logiciel.ConvertToObjectResult();
        }

        // GET: api/Logiciels/GENOMICA/versions?millesime=2023
        [HttpGet("{code}/versions")]
        public async Task<ActionResult<Version?>> GetVersions(string code, [FromQuery] short? millesime)
        {
            var versions = await _service.GetVersionsLogiciel(code, millesime);
            return versions.ConvertToObjectResult();
        }

        // GET: api/Logiciels/GENOMICA/Versions/1.00/Releases/30
        [HttpGet("{codeLogiciel}/Versions/{numVersion}/Releases/{numRelease}")]
        public async Task<ActionResult<Release?>> GetRelease(string codeLogiciel, float numVersion, short numRelease)
        {
            var release = await _service.GetRelease(codeLogiciel, numVersion, numRelease);
            return release.ConvertToObjectResult();
        }
        #endregion

        #region POST
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

            var res = await _service.PostRelease(codeLogiciel, numVersion, release);

            object key = new { codeLogiciel = res.Data?.CodeLogiciel, numVersion = res.Data?.NumeroVersion, numRelease = res.Data?.Numero };

            string uri = Url.Action(nameof(PostRelease), key) ?? "";

            return res.ConvertToObjectResult(uri);
        }

        // POST:api/Logiciels/GENOMICA/versions
        [HttpPost("{codeLogiciel}/versions/")]
        public async Task<ActionResult<Version>> PostVersion(string codeLogiciel, Version version)
        {
            var res = await _service.PostVersion(codeLogiciel, version);
            return res.ConvertToObjectResult();
        }

        #endregion
    }
}
