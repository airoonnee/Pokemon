using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using WpfApp1.MVVM.Model;
using WpfApp1.MVVM.View;
using WpfApp1.Service;


namespace WpfApp1.MVVM.ViewModel
{
    public static class DataSignup
    {
        private static string _connectionString;

        public static bool Initialize(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return false;
            }
            _connectionString = connectionString;

            using var context = new ExerciceMonsterContext(connectionString);
            try
            {
                context.Database.EnsureCreated();
                if (context.Database.CanConnect())
                {
                    return true;  
                }
                else
                {
                    return false;  
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Login SetUser(string username, string password)
        {

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                if (context.Login.Any(u => u.Username == username))
                {
                    MessageBox.Show("Le nom d'utilisateur existe déjà. Veuillez en choisir un autre.");
                    return null;
                }
                string hashedPassword = PasswordHelper.HashPassword(password);

                var newUser = new Login
                {
                    Username = username,
                    PasswordHash = hashedPassword 
                };
                context.Login.Add(newUser);
                context.SaveChanges();

                MessageBox.Show("Utilisateur créé avec succès !");
                return newUser;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la création de l'utilisateur : {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue : {ex.Message}");
                throw;
            }
        }
    }
}


