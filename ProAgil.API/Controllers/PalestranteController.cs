using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public PalestranteController(IProAgilRepository repo)
        {
            this._repo = repo;
        }

        // método bucar todos os palestrantes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetPalestranteAsync(false);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de conexão!");
            }
        }
        // buscar palestrante por id
        [HttpGet("{palestranteId}")]
        public async Task<IActionResult> Get(int palestranteId)
        {
            try
            {
                var results = await _repo.GetPalestranteAsyncById(palestranteId, false);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de conexão!");
            }
        }

        // método para buscar um palestrnate por nome
        [HttpGet("getByName/{palestranteNome}")]
        public async Task<IActionResult> Get(string palestranteNome)
        {
            try
            {
                var results = await _repo.GetAllPalestrnateAsyncByName(palestranteNome, false);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de conexão!");
            }
        }

        // método para adicionar um palestrante
        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                _repo.Add(model);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/palestrante/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de conexão!");
            }
            return BadRequest();
        }
        // método para alterar um palestrante
        [HttpPut("{PalestranteId}")]
        public async Task<IActionResult> Put(int PalestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await _repo.GetPalestranteAsyncById(PalestranteId, false);
                if (palestrante == null)
                {
                    return NotFound();
                }
                _repo.Update(model);
                if(await _repo.SaveChangesAsync()){
                    return Created($"/api/palestrante/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
           return BadRequest();
        }

        // deletando palestrante
        [HttpDelete("{palestranteId}")]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try
            {
                var palestrante = await _repo.GetEventosAsyncById(palestranteId, false);
                if (palestrante == null)
                {
                    return NotFound("Palestrante não encontrado!");
                }
                _repo.Delete(palestrante);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de conexão!");
            }
            return BadRequest();
        }
    }
}