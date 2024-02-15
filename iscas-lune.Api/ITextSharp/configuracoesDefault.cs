using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iscas_lune.Api.ITextSharp;

public static class ConfiguracoesDefault
{
    public static void Configurar(this PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(5, Unit.Millimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(14));
    }
}
