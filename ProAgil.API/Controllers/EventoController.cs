using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")] // rota da requisição
    [ApiController] // caso não especifique o apicontroller será necessário passar o frombody nos parametros dos métodos
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        public readonly IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper) // injeção de dependencia
        {
            _mapper = mapper;
            _repo = repo;
        }

        // método get
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // chamada assincrona
                var eventos = await _repo.GetAllEventosAsync
                (false); // await é utilizado para ele esperar este dado retornar do banco de dados para dps ir para proxima linha
                var results = _mapper.Map<EventoDto[]>(eventos);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados Falhou!\n {ex.Message}");
            }
        }

        // busca por id
        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                // chamada assincrona
                var evento = await _repo.GetEventosAsyncById
                (eventoId, true); // await é utilizado para ele esperar este dado retornar do banco de dados para dps ir para proxima linha
                var results = _mapper.Map<EventoDto>(evento);

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
                var eventos = await _repo.GetAllEventosAsyncByTema(tema, true);

                var results = _mapper.Map<EventoDto>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados Falhou!");
            }
        }

        // adiciona um evento
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados Falhou!\n {ex.Message}");
            }
            return BadRequest();
        }

        // atualiza um evento
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, EventoDto model)
        {
            try
            {
                // cria um objeto evento recebendo id
                var evento = await _repo.GetEventosAsyncById(eventoId, false);
                if (evento == null) return NotFound(); // se id for nulo retorna 404
                _mapper.Map(model, evento);
                
               // model.Id = eventoId; // pega o id da rota e passa para o modelo
                _repo.Update(evento); // salva o modelo
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
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