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

    //"Server=DESKTOP-8098RGU\\SQLEXPRESS;Database=ExerciceMonster;Trusted_Connection=True;TrustServerCertificate=True;";


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


    /* public static class DataBb
     {
         private static string _connectionString;

         public static void Initelaze(string connectionString)
         {


             if (string.IsNullOrWhiteSpace(connectionString))
             {
                 MessageBox.Show("Veuillez saisir une URL valide.");
                 return;
             }
             _connectionString = connectionString;

             using var context = new ExerciceMonsterContext(connectionString);
             try
             {
                 context.Database.EnsureCreated();
             }
             catch (Exception ex)
             {
                 MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}");
             }
         }

         public static Login GetUser(string username)
         {
             if (string.IsNullOrEmpty(_connectionString))
             {
                 throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
             }

             using var context = new ExerciceMonsterContext(_connectionString);
             try
             {
                 var user = context.Login.FirstOrDefault(u => u.Username == username);
                 if (user == null)
                 {
                     Console.WriteLine($"Utilisateur non trouvé pour le nom d'utilisateur : {username}");
                 }
                 return user;
             }
             catch (SqlException sqlEx)
             {
                 Console.WriteLine($"Erreur SQL lors de la récupération de l'utilisateur : {sqlEx.Message}");
                 throw; // Ré-élévation pour gestion en amont
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Erreur inattendue : {ex.Message}");
                 throw; // Ré-élévation pour gestion en amont
             }
         }


     }*/

}
