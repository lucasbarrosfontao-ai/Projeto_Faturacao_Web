using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id_Cliente { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF deve ter 9 dígitos.")]
        public string NIF { get; set; } = string.Empty;

        public string Contato { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Morada { get; set; } = string.Empty;

        public string Localidade { get; set; } = string.Empty;

        public string Codigo_Postal { get; set; } = string.Empty;

        public bool Ativo {get; set; } = true;
    }
}