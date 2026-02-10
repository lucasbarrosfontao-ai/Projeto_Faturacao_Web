using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProjetoFaturacao.Models;
using System.Globalization;
public class PdfGeneratorService
{
    // O QuestPDF precisa de uma licença comunitária (gratuita para estudantes)
    static PdfGeneratorService() => QuestPDF.Settings.License = LicenseType.Community;

    public byte[] GerarFaturaPdf(Fatura fatura)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                // CABEÇALHO
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("FaturaFlow").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("O fluxo inteligente da sua gestão").FontSize(10).Italic();
                        col.Item().Text($"NIF: 123456789");
                    });

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text($"FATURA Nº: {fatura.Numero_Fatura}").FontSize(14).SemiBold();
                        col.Item().Text($"Data: {fatura.Data_Emissao:dd/MM/yyyy}");
                    });
                });

                // CONTEÚDO (Dados do Cliente e Tabela)
                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Item().PaddingBottom(10).Column(c => {
                        c.Item().Text("CLIENTE:").SemiBold();
                        c.Item().Text(fatura.Cliente?.Nome ?? "Consumidor Final");
                        c.Item().Text($"NIF: {fatura.Cliente?.NIF}");
                    });

                    // TABELA DE PRODUTOS
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Produto
                            columns.RelativeColumn();  // Quant
                            columns.RelativeColumn();  // Preço
                            columns.RelativeColumn();  // Total
                            columns.RelativeColumn();  // IVA
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Produto");
                            header.Cell().Element(CellStyle).Text("Qtd");
                            header.Cell().Element(CellStyle).Text("Preço Liquido");
                            header.Cell().Element(CellStyle).Text("Total");
                            header.Cell().Element(CellStyle).Text("IVA");

                            static IContainer CellStyle(IContainer container) => 
                                container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        });

                        foreach (var linha in fatura.LinhasFatura)
                        {
                            table.Cell().Element(CellStyle).Text(linha.Produto!.Nome ?? "Produto");
                            table.Cell().Element(CellStyle).Text(linha.Quantidade.ToString());
                            table.Cell().Element(CellStyle).Text($"{linha.Preco_Unitario.ToString("C", new CultureInfo("pt-PT"))}");
                            table.Cell().Element(CellStyle).Text($"{linha.Subtotal.ToString("C", new CultureInfo("pt-PT"))}");
                            table.Cell().Element(CellStyle).Text(linha.Taxa_IVA.ToString());

                            static IContainer CellStyle(IContainer container) => container.PaddingVertical(5);
                        }
                    });

                    col.Item().AlignRight().PaddingTop(10).Text(text =>
                    {
                        text.Span("TOTAL A PAGAR: ").FontSize(14).SemiBold();
                        text.Span($"{fatura.Valor_Total_Pagar.ToString("C", new CultureInfo("pt-PT"))}").FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                });
            });
        });

        return documento.GeneratePdf();
    }
}