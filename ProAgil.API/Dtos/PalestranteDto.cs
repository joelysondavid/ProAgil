using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class PalestranteDto
    {
        public PalestranteDto(int id, string nome, string miniCurriculo, string imagemURL, string telefone, string email) 
        {
            this.Id = id;
                this.Nome = nome;
                this.MiniCurriculo = miniCurriculo;
                this.ImagemURL = imagemURL;
                this.Telefone = telefone;
                this.Email = email;
               
        }
        public int Id { get; set; }
        [Required(ErrorMessage="O campo {0} é obrigatório!")]
        public string Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage="O campo {0} é obrigatório!"),EmailAddress(ErrorMessage="Informe um email válido!")]
        public string Email { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<EventoDto> Eventos { get; set; }
    }
}