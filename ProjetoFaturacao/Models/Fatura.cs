using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFaturacao.Models
{
    [Table("Faturas")]
    public class Fatura
    {
        [Key]
        public int Id_Fatura { get; set; }

        [Required]
        public int Id_Cliente { get; set; }

        [ForeignKey("Id_Cliente")]
        public Cliente? Cliente { get; set; }

        [Required]
        [MaxLength(50)]
        public string Numero_Fatura { get; set; } = string.Empty;

        [Required]
        public DateTime Data_Emissao { get; set; }

        [Required]
        public decimal Valor_Total_Sem_IVA { get; set; }

        [Required]
        public decimal Valor_Total_IVA { get; set; }

        [Required]
        public decimal Valor_Total_Pagar { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Emitida";

        public virtual ICollection<LinhaFatura>? LinhasFatura { get; set; }
    }
}