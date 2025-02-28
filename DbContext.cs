using System.Reflection;
using Garrison.Lib.DataAnnotations;
using Garrison.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MySql.EntityFrameworkCore.Extensions;

namespace Garrison.Lib;

public partial class GarrisonContext : DbContext
{
    public DbSet<User>      Users       { get; set; }
    public DbSet<Character> Characters  { get; set; }

    public DbSet<Adventure> Adventures  { get; set; }

    public DbSet<ApiKey>    ApiKeys     { get; set; }

    private string? _connectionString = default;

    /// <summary>
    /// This is only invoked by the dotnet ef migrations tool.
    /// It pulls in a connection string from the environment.
    /// </summary>
    public GarrisonContext()
    {
        _connectionString = Environment.GetEnvironmentVariable("GarrisonConnectionString");
    }

    public GarrisonContext(DbContextOptions<GarrisonContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Conventions.Remove<TableNameFromDbSetConvention>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder
            .UseSnakeCaseNamingConvention();

        if (_connectionString is string str)
            builder.UseMySQL(str);
    }
                
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // new List<Type> { typeof(Application), typeof(Authorization), typeof(Scope), typeof(Token) }
        //     .ForEach(t => builder.Model.FindEntityType(t)?.SetTableName($"_auth_{t.Name.ToLower()}"));

        var allProperties = builder.Model.GetEntityTypes().SelectMany(e => e.GetProperties()).ToList();

        allProperties
            .Where(p => p.ClrType == typeof(string))
            .Select(p => new PropertyAttributeInfo<CaseSensitivityAttribute>(p))
            .Where(p => p.HasAttribute)
            .ToList()
            .ForEach(p =>
            {
                var charset = "utf8mb4";
                var collation = p.Attribute!.CaseSensitive ? "utf8mb4_0900_as_cs" : "utf8mb4_0900_ai_ci";

                p.Property.SetCharSet(charset);
                p.Property.SetCollation(collation);
                p.Property.SetAnnotation("MySQL:Charset", charset);
                p.Property.SetAnnotation("MySQL:Collation", collation);
            });

        allProperties
            .Select(p => new PropertyAttributeInfo<FixedLengthAttribute>(p))
            .Where(p => p.HasAttribute)
            .ToList()
            .ForEach(p =>
            {
                p.Property.SetIsFixedLength(true);
                p.Property.SetMaxLength(p.Attribute?.Length);
            });

    }

    private class PropertyAttributeInfo<T>(IMutableProperty property) where T: Attribute
    {
        public IMutableProperty Property { get; } = property;

        public T? Attribute { get; } = property.PropertyInfo?.GetCustomAttribute<T>();

        public bool HasAttribute => Attribute is not null;
    }
}
