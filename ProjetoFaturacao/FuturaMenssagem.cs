using System.ComponentModel.DataAnnotations;
public class FaturaMessage
{
    
    [Key]
    public int FaturaId { get; set; }
    [Required]
    public required string EmailCliente { get; set; }
}