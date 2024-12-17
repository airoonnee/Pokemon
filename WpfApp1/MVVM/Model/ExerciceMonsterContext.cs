using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text.Json;
using System.Windows;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Service;
using WpfApp1.MVVM.View;

namespace WpfApp1.MVVM.Model;

public partial class ExerciceMonsterContext : DbContext
{
    private readonly string _connectionString;

    public ExerciceMonsterContext(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("La chaîne de connexion ne peut pas être vide ou nulle.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }



    public virtual DbSet<Login> Login { get; set; }
    public virtual DbSet<Monster> Monster { get; set; }
    public virtual DbSet<Player> Player { get; set; }
    public virtual DbSet<Spell> Spell { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
