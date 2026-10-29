using SQLite;

namespace EasyShop.Models;

/// <summary>
/// Entidade relacional armazenada no SQLite. O schema é fixo e as restrições
/// (NOT NULL, UNIQUE, tamanho máximo) são aplicadas pelo próprio banco.
/// </summary>
[Table("customers")]
public class Customer
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [NotNull, Unique, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [NotNull]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
