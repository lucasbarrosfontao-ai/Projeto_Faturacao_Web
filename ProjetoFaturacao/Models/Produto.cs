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

        [ForeignKey("Id_Fornecedor") ] 

        public Fornecedor? Fornecedor { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Referencia { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        [Required]
        public decimal Preco_Custo { get; set; }

        public decimal Preco_Venda { get; set; }

        public string Unidade_Medida { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "IVA não pode ser negativo.")]
        public decimal IVA { get; set; }

        [Required]
        public int Stock_Atual { get; set; }
    }
    
}