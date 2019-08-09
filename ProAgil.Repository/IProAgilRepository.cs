using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        // geral
        // metodos abstratos:
        void Add<T>(T entity) where T: class;  // adicionar vai receber uma entidade onde 'T' será uma classe
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;

        void DeleteRange<T> (T[] entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Eventos 
        Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);
        Task<Evento> GetEventosAsyncById(int eventoId, bool includePalestrantes);

        // palestrante
        Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos);
        Task<Palestrante[]> GetPalestranteAsync(bool includeEventos);
        Task<Palestrante[]> GetAllPalestrnateAsyncByName(string name, bool includeEventos);
    }
}