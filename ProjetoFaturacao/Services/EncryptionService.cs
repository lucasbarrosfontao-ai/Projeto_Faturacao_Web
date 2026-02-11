using BCrypt.Net;

namespace ProjetoFaturacao.Services
{
    
    public class EncryptionService
    {
        // Gera o Hash seguro (com Salt automático)
        public string GerarHash(string senhaLimpa)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(senhaLimpa);
        }

        // Verifica se a senha bate com o Hash
        public bool VerificarSenha(string senhaDigitada, string hashSalvoNoBanco)
        {
            
            if (string.IsNullOrEmpty(senhaDigitada) || string.IsNullOrEmpty(hashSalvoNoBanco))
                return false;

            try
            {
                // A biblioteca faz a mágica de extrair o salt do hash e comparar
                return BCrypt.Net.BCrypt.EnhancedVerify(senhaDigitada, hashSalvoNoBanco);
            }
            catch
            {
                return false;
            }
        }
    }
}
