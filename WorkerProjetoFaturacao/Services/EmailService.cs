using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
        var user = _config["SMTP_USER"];
            Console.WriteLine($"Tentando SMTP com o utilizador: {user}");
    }
    

    public async Task EnviarFaturaEmailAsync(string emailDestinatario, string nomeCliente, byte[] pdfBytes, string numeroFatura)
    {
        var message = new MimeMessage();
        // Configura o Remetente (Podes pôr o teu nome ou da empresa da PAP)
        message.From.Add(new MailboxAddress("FaturaFlow - Sistema Automático", "no-reply@faturaflow.com"));
        message.To.Add(new MailboxAddress(nomeCliente, emailDestinatario));
        message.Subject = $"Fatura Disponível - Nº {numeroFatura}";

        // Criar o corpo do email
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <h2 style='color: #2e6c80;'>Olá {nomeCliente},</h2>
                <p>Agradecemos a sua preferência.</p>
                <p>Em anexo enviamos a fatura <strong>{numeroFatura}</strong> em formato PDF.</p>
                <br>
                <p>Atentamente,<br>A Equipa de Faturação</p>"
        };

        // Adicionar o Anexo (O PDF que gerámos)
        bodyBuilder.Attachments.Add($"Fatura_{numeroFatura.Replace("/", "-")}.pdf", pdfBytes);

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            // Ligar ao servidor SMTP (Gmail, Outlook ou Mailtrap para testes)
            // Estes valores virão do Docker Compose/appsettings
            var host = _config["SMTP_HOST"] ?? "smtp.mailtrap.io";
            var port = int.Parse(_config["SMTP_PORT"] ?? "2525");
            var user = _config["SMTP_USER"];
            var pass = _config["SMTP_PASS"];

            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(user, pass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao enviar email: {ex.Message}");
        }
    }
    public async Task EnviarEmailRecuperacaoAsync(string emailDestinatario, int codigo)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("FaturaFlow Sistema", _config["SMTP_USER"]));
        message.To.Add(new MailboxAddress("Utilizador", emailDestinatario));
        message.Subject = "Código de Recuperação de Senha";

        message.Body = new TextPart("plain")
        {
            Text = $@"Olá,
            
            Você solicitou a recuperação de senha no sistema FaturaFlow.
            O seu código de verificação é: {codigo}
            
            Se não solicitou esta alteração, ignore este email."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["SMTP_HOST"], int.Parse(_config["SMTP_PORT"]!), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config["SMTP_USER"], _config["SMTP_PASS"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}