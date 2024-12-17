using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.MVVM.Model;
using WpfApp1.Service;

namespace WpfApp1.MVVM.ViewModel
{
    public static class DataLogin
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

        public static Login GetUser(string username, string password)
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
                    MessageBox.Show($"Utilisateur non trouvé pour le nom d'utilisateur : {username}");
                    return null; 
                }

                
                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    MessageBox.Show($"mot de passe incorrect, {user.Username} !");
                    return null;
                }
                return user;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la récupération de l'utilisateur : {sqlEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue : {ex.Message}");
                return null;
            }
        }
    }
}
