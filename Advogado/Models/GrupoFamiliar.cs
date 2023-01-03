using System;
using System.ComponentModel.DataAnnotations;

namespace Advogado.Models
{
    public class GrupoFamiliar
    {
        public int GrupoFamiliarId { get; set; }

        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(14, ErrorMessage = "Máximo 14 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cpf { get; set; }
       
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataNascimento { get; set; }

        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string GrauParentesco { get; set; }

        public int CasoId { get; set; }
        public Caso Caso { get; set; }

        public bool Ativo { get; set; }
    }
}