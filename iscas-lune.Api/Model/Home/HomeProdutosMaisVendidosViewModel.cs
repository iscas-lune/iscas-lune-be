namespace iscas_lune.Api.Model.Home;

public class HomeProdutosMaisVendidosViewModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public long Numero { get; set; }
    public string Foto { get; set; } = string.Empty;
}
