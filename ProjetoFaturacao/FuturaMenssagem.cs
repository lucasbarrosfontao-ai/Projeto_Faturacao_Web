using System.ComponentModel.DataAnnotations;
public class FaturaMessage
{
    
    [Key]
    public int FaturaId { get; set; }
    [Required]
    public required string NomeCliente { get; set; }
    [Required]
    public required string EmailCliente { get; set; }
    [Required]
    public decimal ValorTotal { get; set; }
}