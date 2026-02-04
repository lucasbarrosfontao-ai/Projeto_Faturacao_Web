using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int Id_Produto { get; set; }

        [Required]
        public int Id_Fornecedor { get; set; }

        [ForeignKey("Id_Fornecedor")]
        public Fornecedor? Fornecedor { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Referencia { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Unidade_Medida { get; set; } = string.Empty;

        [Required]
        public decimal IVA { get; set; }

        [Required]
        public decimal Stock_Atual { get; set; }
    }
    
}