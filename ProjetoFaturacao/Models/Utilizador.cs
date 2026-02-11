using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Utilizadores")]
    public class Utilizador
    {
        [Key]
        public int Id_Utilizador {get; set;}

        [Required]
        public string Nome_Utilizador {get;set;} = string.Empty;

        [Required]
        public string Palavra_Passe {get;set;} = string.Empty;

        public string? Email_Utilizador{get;set;}

        public string? Codigo_Recuperacao { get; set; }

    }
}