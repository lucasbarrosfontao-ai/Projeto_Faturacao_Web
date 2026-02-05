using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
var builder = Host.CreateApplicationBuilder(args);

// 1. Configurar a Connection String
// NOTA: No Docker, o host deve ser "db". Se fores testar localmente, usa "localhost".
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                         ?? "Server=db;Database=projeto_faturacao_db;Uid=root;Pwd=admin;";

// 2. Registar o DbContext (Usando o Pomelo para MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 3. Registar o Worker
builder.Services.AddHostedService<Worker>();

// 4. Se o teu Worker for usar o PdfGeneratorService, podes registá-lo aqui também
// builder.Services.AddSingleton<PdfGeneratorService>();

var host = builder.Build();
host.Run();