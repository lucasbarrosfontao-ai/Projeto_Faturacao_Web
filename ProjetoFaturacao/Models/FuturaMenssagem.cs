using System.ComponentModel.DataAnnotations;
public class FaturaMessage
{
    public int Id_Fatura { get; set; }
    public string EmailCliente { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
}
public class RecuperacaoMessage
{
    public int NumeroRecuperacao {get; set;}
    public string EmailUtilizador {get;set;} = string.Empty;

}