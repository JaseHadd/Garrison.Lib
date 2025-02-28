using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Garrison.Lib.Models;

public class Adventure
{
    [Key]
    public          uint                    Id          { get; set; }

    [Required]
    public required User                    GameMaster  { get; set; }

    [Required, MaxLength(256)]
    public required string                  Name        { get; set; }

    public          List<AdventurePlayer>   Players     { get; } = [];

    public          List<Session>           Sessions    { get; } = [];
}

[Owned, PrimaryKey(nameof(AdventureId), nameof(PlayerId))]
public class AdventurePlayer
{
    [Required]
    public required User                    Player      { get; init; }

    [Required]
    public required Adventure               Adventure   { get; init; }

    public          Character?              Character   { get; set; }

    private         uint                    PlayerId    { get; set; }

    private         uint                    AdventureId { get; set; }
}

[Owned, PrimaryKey(nameof(AdventureId), nameof(SessionNumber))]
[EntityTypeConfiguration(typeof(Configuration))]
public class Session
{
    [Required]
    public required Adventure               Adventure       { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required ushort                  SessionNumber   { get; set; }

    private         uint                    AdventureId     { get; set; }

    private class Configuration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(s => s.SessionNumber)
                .HasValueGenerator<SessionNumberGenerator>();
        }
    }

    private class SessionNumberGenerator : ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            if (entry.Entity is Session session)
                return session.Adventure.Sessions.Count;
            else
                throw new Exception("SessionNumberGenerator can only be used for sessions");
        }
    }
}