using Microsoft.EntityFrameworkCore;
using ProjetoFaturacao.Models;

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
    // Tabelas
    modelBuilder.Entity<Cliente>().ToTable("Clientes");
    modelBuilder.Entity<Fornecedor>().ToTable("Fornecedores");
    modelBuilder.Entity<Produto>().ToTable("Produtos");
    modelBuilder.Entity<Fatura>().ToTable("Faturas");
    modelBuilder.Entity<LinhaFatura>().ToTable("Linhas_fatura");

    // Chaves Primárias
    modelBuilder.Entity<Cliente>().HasKey(c => c.Id_Cliente);
    modelBuilder.Entity<Fornecedor>().HasKey(f => f.Id_Fornecedor);
    modelBuilder.Entity<Produto>().HasKey(p => p.Id_Produto);
    modelBuilder.Entity<Fatura>().HasKey(f => f.Id_Fatura);
    modelBuilder.Entity<LinhaFatura>().HasKey(l => l.Id_Linha);

    // Relacionamento: Fatura -> Cliente
    modelBuilder.Entity<Fatura>()
        .HasOne(f => f.Cliente)
        .WithMany()
        .HasForeignKey(f => f.Id_Cliente)
        .OnDelete(DeleteBehavior.Cascade);

    // Relacionamento: LinhaFatura -> Fatura (ESTE É O PONTO DO ERRO)
    modelBuilder.Entity<LinhaFatura>()
        .HasOne(l => l.Fatura) 
        .WithMany(f => f.LinhasFatura)
        .HasForeignKey(l => l.Id_Fatura) // FORÇA usar apenas Id_Fatura
        .IsRequired();

    // Relacionamento: Produto -> Fornecedor
    modelBuilder.Entity<Produto>()
        .HasOne(p => p.Fornecedor)
        .WithMany()
        .HasForeignKey(p => p.Id_Fornecedor)
        .OnDelete(DeleteBehavior.Restrict);

    // Relacionamento: LinhaFatura -> Produto
    modelBuilder.Entity<LinhaFatura>()
        .HasOne(l => l.Produto)
        .WithMany()
        .HasForeignKey(l => l.Id_Produto)
        .OnDelete(DeleteBehavior.Restrict);
}
}