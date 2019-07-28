using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(100, MinimumLength=3, ErrorMessage="Local é entre 3 e 100 caracteres!")]
        public string Local { get; set; }
        public string DataEvento  { get; set; }
        [Required (ErrorMessage = "O campo {0} é obrigatório!")]
        public string Tema { get; set; }
        [Range(2, 12000, ErrorMessage="Quantidade de pessoas é entre 2 e 12000 pessoas!")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        [Phone]
        public string Telefone { get; set; }
        [Required(ErrorMessage="O campo {0} é obrigatório!"), EmailAddress(ErrorMessage="O campo email deve receber um email válido!")]
        public string Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        // [Required(ErrorMessage="O campo {0} é obrigatório!")]
        public List<PalestranteDto> Palestrantes{ get; set; }
    }
}