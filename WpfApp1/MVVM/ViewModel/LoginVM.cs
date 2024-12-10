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

        public static void Initialize(string connectionString)
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

        public static Login GetUser(string username, string password)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                // Rechercher l'utilisateur par nom d'utilisateur
                var user = context.Login.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    MessageBox.Show($"Utilisateur non trouvé pour le nom d'utilisateur : {username}");
                    return null; // Retourne null si l'utilisateur n'est pas trouvé
                }

                //// Vérifier le mot de passe hashé
                //MessageBox.Show(user.PasswordHash);
                //MessageBox.Show(hashedPassword);
                //if (user.PasswordHash != hashedPassword)
                //{
                //    MessageBox.Show("Mot de passe incorrect.");
                //    return null; // Retourne null si le mot de passe est incorrect
                //}
                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    MessageBox.Show($"mot de passe incorrect, {user.Username} !");
                }

                // Retourner l'utilisateur si tout est valide

                return user;
                
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la récupération de l'utilisateur : {sqlEx.Message}");
                throw; // Ré-élévation pour gestion en amont
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue : {ex.Message}");
                throw; // Ré-élévation pour gestion en amont
            }
        }
    }
}
