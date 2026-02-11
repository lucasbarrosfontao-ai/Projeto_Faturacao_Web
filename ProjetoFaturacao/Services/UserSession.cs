namespace ProjetoFaturacao.Services
{
    public class UserSession
    {
        // Por padrão é falso. Só vira true quando passar pelo Login.
        public bool IsLoggedIn { get; set; } = false; 
    }
}