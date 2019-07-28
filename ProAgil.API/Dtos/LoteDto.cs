using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage="O campo {0} é obrigatório!")]
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        [Range(2, 12000, ErrorMessage="A quantidade deve ser entre 2 e 12000!")]
        public int Quantidade { get; set; }
    }
}