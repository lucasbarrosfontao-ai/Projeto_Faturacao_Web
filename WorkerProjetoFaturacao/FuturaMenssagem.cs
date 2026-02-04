using System.ComponentModel.DataAnnotations;
public class FaturaMessage
{
    public int FaturaId { get; set; }
    public string EmailCliente { get; set; }
    public string NomeCliente { get; set; }
}