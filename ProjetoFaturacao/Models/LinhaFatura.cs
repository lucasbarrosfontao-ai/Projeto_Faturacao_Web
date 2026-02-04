using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Linhas_fatura")]
    public class LinhaFatura
    {
        [Key]
        public int Id_Linha { get; set; }

        [Required]
        public int Id_Fatura { get; set; }

        [ForeignKey("Id_Fatura")]
        public Fatura? Fatura { get; set; }

        [Required]
        public int Id_Produto { get; set; }

        [ForeignKey("Id_Produto")]
        public Produto? Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal Preco_Unitario { get; set; }

        [Required]
        public decimal Taxa_IVA { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}