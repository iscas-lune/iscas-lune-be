using iscas_lune.Api.ITextSharp;
using iscas_lune_api.Application.Interfaces;
using iscas_lune_api.Domain.Entities;
using iscas_lune_api.Infrastructure.Interfaces;
using iscas_lune_api.ITextSharp.Interfaces;
using iscaslune.Api;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;
using static iTextSharp.text.pdf.AcroFields;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace iscas_lune_api.ITextSharp.Classes;

public class PdfPedido : IPdfPedido
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IList<string> _colunsName;
    private readonly IEmailService _emailService;
    private readonly string _imageString = "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";
    private readonly byte[] _imageData;
    private Pedido? _pedido;
    private readonly IList<int> _colunsWidt;

    public PdfPedido(IPedidoRepository pedidoRepository, IEmailService emailService)
    {
        _colunsName = new List<string>()
        {
            "REF",
            "Descrição",
            "Tamanho/Peso",
            "Quantidade",
            "Valor unitário",
            "Total"
        };
        _colunsWidt = new List<int>()
        {
            50,150,80,70,90,50
        };
        _pedidoRepository = pedidoRepository;
        _emailService = emailService;
        _imageData = Convert.FromBase64String(_imageString);
    }



    public async Task<bool> GeneratePdfAsync(Guid pedidoId)
    {

        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
            var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();
            var titleStyleName = TextStyle.Default.FontSize(10);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"#Iscas Lune").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Data de emissão: ").SemiBold().FontSize(14);
                        text.Span(_pedido?.DataCriacao.ToString("dd/MM/yyyy"));
                    });

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Cliente: ").Style(titleStyle2);
                        text.Span(_pedido?.Usuario.Nome).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Email: ").Style(titleStyle2);
                        text.Span(_pedido?.Usuario.Email).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Telefone: ").Style(titleStyle2);
                        text.Span(_pedido?.Usuario.Telefone).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("CNPJ: ").Style(titleStyle2);
                        text.Span(_pedido?.Usuario.Cnpj).Style(titleStyleName);
                    });
                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Número: ").Style(titleStyle2);
                        text.Span(_pedido?.Numero.ToString()).Style(titleStyleName);
                    });
                });
                row.ConstantItem(50).Height(50).Image(_imageData);

            });
        }

        static IContainer CellStyleHeaderTable(IContainer container)
        {
            return container
                .DefaultTextStyle(x => x.SemiBold())
                .PaddingVertical(5)
                .BorderBottom(1)
                .BorderColor(Colors.Black);
        }

        static IContainer CellTableStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5);
        }

        try
        {
            _pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoId);
            if (_pedido == null) return false;

            var pdf = Document
                .Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Configurar();
                        page.Header().Element(HeaderCustom);
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var columnsWidth in _colunsWidt)
                                {
                                    columns.ConstantColumn(columnsWidth);
                                }
                            });

                            table.Header(header =>
                            {
                                foreach (var columnsName in _colunsName)
                                {
                                    header
                                        .Cell()
                                        .Element(CellStyleHeaderTable)
                                        .Text(columnsName)
                                        .FontSize(10);
                                }
                            });

                            _pedido.ItensPedido = [.. _pedido.ItensPedido.OrderBy(x => x.Numero)];
                            var itemsPedidosGroup = _pedido.ItensPedido.GroupBy(x => x.ProdutoId);

                            foreach (var itensGroup in itemsPedidosGroup)
                            {
                                foreach (var item in itensGroup)
                                {
                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Produto.Referencia)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Produto.Descricao)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Tamanho?.Descricao ?? item.Peso?.Descricao)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Quantidade.ToString().Replace(".", ","))
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.ValorUnitario.ToString().Replace(".", ","))
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.ValorTotal.ToString().Replace(".", ","))
                                    .FontSize(8);
                                }
                            }

                            var tamamhosItens = _pedido
                                .ItensPedido
                                .OrderBy(x => x.Tamanho?.Numero)
                                .Select(x => x.Tamanho)
                                .ToList()
                                .GroupBy(x => x?.Id);

                            table
                                .Cell()
                                .Element(CellStyleHeaderTable)
                                .Text($"Quantidades")
                                .FontSize(10);

                            foreach (var tamanhoGroup in tamamhosItens)
                            {
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                var itemPedido = _pedido
                                    .ItensPedido
                                    .FirstOrDefault(x => x.TamanhoId == tamanhoGroup.Key);

                                var totalQuantidade = _pedido
                                    .ItensPedido
                                    .Where(x => x.TamanhoId == tamanhoGroup.Key)
                                    .ToList()
                                    .Sum(x => x.Quantidade);

                                table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text($"{itemPedido?.Tamanho?.Descricao} : {totalQuantidade.ToString().Replace(".", ",")}")
                                    .FontSize(8);
                            }
                        });
                        page.FooterCustom();
                    });
                }).GeneratePdf();

            var emailPedido = EnvironmentVariable.GetVariable("EMAIL_PEDIDO");
            var message = $"Que ótima noticia, mais um pedido!\nN. do pedido : {_pedido?.Numero}";
            var assunto = "Novo pedido";
            return _emailService.SendEmail(emailPedido, message, assunto, pdf, $"pedido-{_pedido?.Numero}", "application/pdf");
        }
        catch (Exception)
        {
            return false;
        }
    }

}
