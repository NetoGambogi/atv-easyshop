namespace EasyShop.Data;

public static class DatabaseConstants
{
    public static string ProductsDbPath => Path.Combine(FileSystem.AppDataDirectory, "products.db");
    public static string CustomersDbPath => Path.Combine(FileSystem.AppDataDirectory, "customers.db3");

    public const SQLite.SQLiteOpenFlags SqliteFlags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;
}
