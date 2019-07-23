using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")] // rota da requisição
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventoController(IProAgilRepository repo) // injeção de dependencia
        {
            _repo = repo;
        }

        // método get
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // chamada assincrona
                var results = await _repo.GetAllEventosAsync
                (false); // await é utilizado para ele esperar este dado retornar do banco de dados para dps ir para proxima linha
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
        }

        // busca por id
        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                // chamada assincrona
                var results = await _repo.GetEventosAsyncById
                (eventoId, true); // await é utilizado para ele esperar este dado retornar do banco de dados para dps ir para proxima linha
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
        }

        // busca por tema
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repo.GetAllEventosAsyncByTema(tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
        }

        // adiciona um evento
        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repo.Add(model);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
            return BadRequest();
        }

        // atualiza um evento
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try
            {
                // cria um objeto evento recebendo id
                var evento = await _repo.GetEventosAsyncById(eventoId, false);
                if (evento == null) return NotFound(); // se id for nulo retorna 404
                model.Id = eventoId; // pega o id da rota e passa para o modelo
                _repo.Update(model); // salva o modelo
                 if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
           return BadRequest();
        }

        // deletando evento
        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repo.GetEventosAsyncById(eventoId, false);
                if (evento == null)
                {
                    return NotFound("Evento não encontrado");
                }

                _repo.Delete(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
            return BadRequest();
        }
    }
}