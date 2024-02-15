using QuestPDF.Fluent;

namespace iscas_lune.Api.ITextSharp;

public static class FooterDefault
{
    public static void FooterCustom(this PageDescriptor page)
    {
        page.Footer()
                .AlignRight()
                .Height(20)
                .Text(x =>
                {
                    x.CurrentPageNumber().FontSize(8);
                    x.Span(" / ").FontSize(8);
                    x.TotalPages().FontSize(8);
                });
    }
}
