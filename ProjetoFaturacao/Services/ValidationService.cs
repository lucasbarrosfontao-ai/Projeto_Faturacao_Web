using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidationService
{
    #region Validar Pessoas

    // Validate NI (NIF/NIPC) using modulo 11
    public bool IsValidNI(string ni)
    {
        if (string.IsNullOrWhiteSpace(ni) || !ni.All(char.IsDigit))
            return false;

        if (ni.Length != 9) return false; // Keep length check

        int[] weights = { 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (ni[i] - '0') * weights[i];
        }
        int remainder = sum % 11;
        int checkDigit = remainder < 2 ? 0 : 11 - remainder;
        return checkDigit == (ni[8] - '0');
    }

    // Validate name (mandatory, not empty)
    public bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    // Validate postal code (optional, format XXXX-XXX)
    public bool IsValidPostalCode(string? postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode)) return true; // Optional
        return Regex.IsMatch(postalCode, @"^\d{4}-\d{3}$");
    }

    // Validate phone (optional, any national/international, 7-15 digits)
    public bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return true; // Optional
        string cleaned = Regex.Replace(phone, @"[^\d+]", ""); // Remove non-digits except +
        return Regex.IsMatch(cleaned, @"^[\+]?\d{7,15}$"); // Starts with optional +, 7-15 digits
    }

    // Validate email (optional)
    public bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return true; // Optional
        return new EmailAddressAttribute().IsValid(email);
    }

    // Comprehensive validation for person (client/supplier)
    public List<string> ValidatePerson(string ni, string name, string? contact, string? postalCode, string? email)
    {
        var errors = new List<string>();

        if (!IsValidNI(ni))
            errors.Add("NI inválido (deve seguir o algoritmo módulo 11).");

        if (!IsValidName(name))
            errors.Add("Nome é obrigatório.");

        if (!IsValidPhone(contact))
            errors.Add("Contato inválido (formato português esperado).");

        if (!IsValidPostalCode(postalCode))
            errors.Add("Código postal inválido (formato esperado: XXXX-XXX).");

        if (!IsValidEmail(email))
            errors.Add("Email inválido.");

        return errors;
    }

    #endregion

    #region Validar Produtos

    public class ValidationResult
    {
        public List<string> Errors { get; set; } = new();
        public List<string> Confirmations { get; set; } = new();
    }

    public ValidationResult ValidateProduto(string nome, string referencia, int idFornecedor, decimal precoCusto, decimal precoVenda, string unidadeMedida, decimal iva, int stock)
    {
        var result = new ValidationResult();

        // Mandatory fields
        if (string.IsNullOrWhiteSpace(nome))
            result.Errors.Add("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(referencia))
            result.Errors.Add("Referência é obrigatória.");
        if (idFornecedor <= 0)
            result.Errors.Add("ID do fornecedor é obrigatório e deve ser válido.");
        if (precoCusto < 0)
            result.Errors.Add("Preço de custo não pode ser negativo.");
        if (precoVenda < 0)
            result.Errors.Add("Preço de venda não pode ser negativo.");
        if (string.IsNullOrWhiteSpace(unidadeMedida))
            result.Errors.Add("Unidade de medida é obrigatória.");
        // Confirmations
        if (stock < 0)
            result.Confirmations.Add("O stock é negativo. Deseja confirmar?");
        if (precoCusto > precoVenda && precoVenda > 0)
            result.Confirmations.Add("O preço de custo é maior que o preço de venda. Deseja confirmar?");

        return result;
    }

    #endregion
}
