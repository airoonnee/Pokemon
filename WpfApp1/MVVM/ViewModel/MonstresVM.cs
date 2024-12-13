using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Windows;
using WpfApp1.MVVM.Model;
using WpfApp1.Service;

namespace WpfApp1.MVVM.ViewModel
{
    internal class MonstresVM
    {
    }
    public static class DataMonster
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

        //public static Monster DisplayMonster()
        //{
        //    if (string.IsNullOrEmpty(_connectionString))
        //    {
        //        throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
        //    }

        //    using var context = new ExerciceMonsterContext(_connectionString);
        //    try
        //    {
        //        // Récupérer les informations du monstre associé à l'utilisateur
        //        var monster = context.Monsters.FirstOrDefault(m => m.Id == monster.Id);

        //        if (monster == null)
        //        {
        //            MessageBox.Show("Aucun monstre associé à cet utilisateur.");
        //            return null;
        //        }

        //        // Afficher les détails du monstre
        //        MessageBox.Show($"ID: {monster.Id}\nNom: {monster.Name}\nSanté: {monster.Health}\nImageURL: {monster.ImageUrl}");

        //        return monster;
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        MessageBox.Show($"Erreur SQL lors de la récupération des données : {sqlEx.Message}");
        //        throw; // Ré-élévation pour gestion en amont
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erreur inattendue : {ex.Message}");
        //        throw; // Ré-élévation pour gestion en amont
        //    }
        //}

        public static List<(string Name, string ImageUrl)> DisplayMonsterImages()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                // Récupérer les noms et URL d'images des monstres
                var monsters = context.Monster
                    .Select(m => new { m.Name, m.ImageUrl })
                    .ToList();

                if (!monsters.Any())
                {
                    MessageBox.Show("Aucun monstre trouvé.");
                    return null;
                }

                // Afficher les noms et URLs des images pour vérification (peut être retiré en production)
                foreach (var monster in monsters)
                {
                    MessageBox.Show($"Name: {monster.Name}, ImageURL: {monster.ImageUrl}");
                }

                // Retourner une liste de tuples contenant le nom et l'URL de l'image
                return monsters.Select(m => (m.Name, m.ImageUrl)).ToList();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la récupération des données : {sqlEx.Message}");
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
