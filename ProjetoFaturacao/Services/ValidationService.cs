using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidationService
{
    
    #region Validação de Pessoas (Cliente/Fornecedor)

    // Valida NIF/NIPC usando algoritmo módulo 11
    public bool ValidarNIF(string nif)
    {
        if (string.IsNullOrWhiteSpace(nif) || !nif.All(char.IsDigit))
            return false;

        if (nif.Length != 9) return false;

        int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 8; i++)
        {
            soma += (nif[i] - '0') * pesos[i];
        }
        int resto = soma % 11;
        int digitoControlo = resto < 2 ? 0 : 11 - resto;
        return digitoControlo == (nif[8] - '0');
    }

    public bool ValidarNome(string nome)
    {
        return !string.IsNullOrWhiteSpace(nome);
    }

    public bool ValidarCodigoPostal(string? codigoPostal)
    {
        if (string.IsNullOrWhiteSpace(codigoPostal)) return true; // Opcional
        return Regex.IsMatch(codigoPostal, @"^\d{4}-\d{3}$");
    }

    public bool ValidarTelefone(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone)) return true; // Opcional
        string limpo = Regex.Replace(telefone, @"[^\d+]", ""); 
        return Regex.IsMatch(limpo, @"^[\+]?\d{7,15}$");
    }

    public bool ValidarEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return true; // Opcional
        return new EmailAddressAttribute().IsValid(email);
    }

    // Validação completa para a página de Clientes/Fornecedores
    public List<string> ValidarPessoa(string nif, string nome, string? contato, string? codigoPostal, string? email)
    {
        var erros = new List<string>();

        if (!ValidarNIF(nif))
            erros.Add("NIF/NIPC inválido (deve seguir o algoritmo módulo 11).");

        if (!ValidarNome(nome))
            erros.Add("Nome é obrigatório.");

        if (!ValidarTelefone(contato))
            erros.Add("Contato inválido.");

        if (!ValidarCodigoPostal(codigoPostal))
            erros.Add("Código postal inválido (formato esperado: XXXX-XXX).");

        if (!ValidarEmail(email))
            erros.Add("Email inválido.");

        return erros;
    }

    #endregion

    #region Validação de Produtos

    public class ResultadoValidacao
    {
        public List<string> Erros { get; set; } = new();
        public List<string> Confirmacoes { get; set; } = new();
    }

    public ResultadoValidacao ValidarProduto(string nome, string referencia, int idFornecedor, decimal precoCusto, decimal precoVenda, string unidadeMedida, decimal iva, int stock)
    {
        var resultado = new ResultadoValidacao();

        if (string.IsNullOrWhiteSpace(nome))
            resultado.Erros.Add("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(referencia))
            resultado.Erros.Add("Referência é obrigatória.");
        if (idFornecedor <= 0)
            resultado.Erros.Add("ID do fornecedor é obrigatório.");
        if (precoCusto < 0)
            resultado.Erros.Add("Preço de custo não pode ser negativo.");
        if (precoVenda < 0)
            resultado.Erros.Add("Preço de venda não pode ser negativo.");
        if (string.IsNullOrWhiteSpace(unidadeMedida))
            resultado.Erros.Add("Unidade de medida é obrigatória.");
        
        if (stock < 0)
            resultado.Confirmacoes.Add("O stock é negativo. Deseja confirmar?");
        if (precoCusto > precoVenda && precoVenda > 0)
            resultado.Confirmacoes.Add("O preço de custo é maior que o preço de venda. Deseja confirmar?");

        return resultado;
    }

    #endregion

    #region Validação de Faturas

    public static class VerificacaoFaturas
    {
        public static bool VerificarCamposFatura(int idCliente, string numeroFatura, DateTime dataFatura, int qtdItens, out string mensagemErro)
        {
            mensagemErro = "";

            if (idCliente <= 0)
            {
                mensagemErro = "Por favor, selecione um cliente.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(numeroFatura))
            {
                mensagemErro = "Por favor, insira o número da fatura.";
                return false;
            }

            if (dataFatura.Date > DateTime.Now.Date)
            {
                mensagemErro = "A data da fatura não pode ser futura.";
                return false;
            }

            if (qtdItens <= 0)
            {
                mensagemErro = "A lista de compras está vazia. Adicione pelo menos um produto.";
                return false;
            }

            return true;
        }

        public static bool VerificarCamposInserirProduto(int idProduto, decimal preco, int quantidade, out string mensagemErro)
        {
            mensagemErro = "";

            if (idProduto <= 0)
            {
                mensagemErro = "Por favor, selecione um produto.";
                return false;
            }

            if (preco <= 0)
            {
                mensagemErro = "O preço unitário deve ser maior que zero.";
                return false;
            }

            if (quantidade <= 0)
            {
                mensagemErro = "A quantidade deve ser maior que zero.";
                return false;
            }

            return true;
        }
    }

    #endregion
}