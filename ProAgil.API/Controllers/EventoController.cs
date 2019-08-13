using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0]; // arquivo todo arquivo vem como array
                var folderName = Path.Combine("Resources", "Images"); // diretorio que será o novo diretorio para a imagem

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName); // combina o diretorio onde ele quer armazenar com o caminho do arquivo

                if (file.Length > 0) // se o arquivo selecionado existir logo será maior que 0
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName; // pega o nome do arquivo do header
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim()); // substitui as aspas duplas do nome do arquivo e os espaços

                    // salva o arquivo no novo diretorio
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados Falhou!\n {ex.Message}");
            }

            return BadRequest("Erro ao tentar realizar upload!");
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

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();


                if (idLotes.Count != 0)
                {
                    // verifica os lotes e redes que foram passados
                    model.Lotes.ForEach(item => idLotes.Add(item.Id));
                    // procura os lotes passados
                    var lotes = evento.Lotes.Where(lote => !idLotes.Contains(lote.Id)).ToArray();
                    if (lotes.Length > 0) _repo.DeleteRange(lotes); // deleta os lotes não encontrados
                }
                if (idRedesSociais.Count != 0)
                {
                    model.RedesSociais.ForEach(item => idRedesSociais.Add(item.Id));
                    // procura pelas redes sociais informadas
                    var redesSociais = evento.RedesSociais.Where(redeSocial => !idRedesSociais.Contains(redeSocial.Id)).ToArray();
                    if (redesSociais.Length > 0) _repo.DeleteRange(redesSociais); // deleta os lotes não encontrados
                }

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