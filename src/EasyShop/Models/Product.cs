using LiteDB;

namespace EasyShop.Models;

/// <summary>
/// Documento do catálogo armazenado no LiteDB (NoSQL).
/// Os atributos dinâmicos ficam num dicionário e são gravados como sub-documento BSON,
/// então cada produto pode ter um conjunto diferente de características sem alterar schema.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonIgnore]
    public string AttributesSummary =>
        Attributes.Count == 0
            ? "Sem atributos adicionais"
            : string.Join("  •  ", Attributes.Select(a => $"{a.Key}: {a.Value}"));
}
