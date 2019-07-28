using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Reposirory;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public readonly ProAgilContext _context;

        // GERAIS
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // toda query de restreamento não irá travar o entity
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity); // passando a entidade para salvar
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity); // atualizando
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity); // deletando
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0; // se houver alguma mudança retorna true
        }

        // Eventos    
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }
            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }
            query = query.OrderByDescending(c => c.DataEvento)
            .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventosAsyncById(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }
            query = query.OrderBy(c => c.Id)
            .Where(c => c.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }

        // Busca palestrante 
        public async Task<Palestrante[]> GetPalestranteAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(p => p.Nome);

            return await query.ToArrayAsync();
        }
        // Palestrante by id
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }
            query = query.OrderBy(p => p.Nome)
            .Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrnateAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }
            query = query.Where(p => p.Nome.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}