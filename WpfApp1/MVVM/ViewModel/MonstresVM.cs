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

        public static List<string> DisplayMonsterImages()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                // Récupérer toutes les URL d'images des monstres
                var monsterImages = context.Monsters.Select(m => m.ImageUrl).ToList();

                if (!monsterImages.Any())
                {
                    MessageBox.Show("Aucune image de monstre trouvée.");
                    return null;
                }

                // Afficher les URLs des images pour vérification (peut être retiré en production)
                foreach (var imageUrl in monsterImages)
                {
                    MessageBox.Show($"ImageURL: {imageUrl}");
                }

                return monsterImages;
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
