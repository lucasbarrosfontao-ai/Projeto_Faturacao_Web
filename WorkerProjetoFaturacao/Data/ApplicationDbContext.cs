using ProjetoFaturacao.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Fatura> Faturas { get; set; }
    public DbSet<LinhaFatura> LinhasFatura { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aqui mapeamos os nomes exatos das tabelas e chaves para bater com o teu SQL
        modelBuilder.Entity<LinhaFatura>().ToTable("Linhas_fatura");
        
        // Exemplo de como garantir que o EF sabe qual é a PK
        modelBuilder.Entity<Fatura>().HasKey(f => f.Id_Fatura);
        modelBuilder.Entity<Cliente>().HasKey(c => c.Id_Cliente);
        modelBuilder.Entity<Produto>().HasKey(p => p.Id_Produto);
        modelBuilder.Entity<Fornecedor>().HasKey(f => f.Id_Fornecedor);
        modelBuilder.Entity<LinhaFatura>().HasKey(l => l.Id_Linha);
    }
}