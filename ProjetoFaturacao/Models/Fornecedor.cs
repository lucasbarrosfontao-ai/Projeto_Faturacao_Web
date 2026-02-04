using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Fornecedores")]
    public class Fornecedor
    {
        [Key]
        public int Id_Fornecedor { get; set; }

        [Required]
        public string Nome_Empresa { get; set; } = string.Empty;

        [Required]
        public string NIPC { get; set; } = string.Empty;

        public string Nome_Representante { get; set; } = string.Empty;

        public string Contato { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Rua { get; set; } = string.Empty;

        public string Localidade { get; set; } = string.Empty;

        public string Codigo_Postal { get; set; } = string.Empty;
    }
}