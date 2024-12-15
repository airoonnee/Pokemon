using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Windows;
using WpfApp1.MVVM.Model;

namespace WpfApp1.MVVM.ViewModel
{
    internal class SpellsVM
    {
    }
    public static class DataSpell
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

        public static List<(int Id, string Name, int Damage, string Description, List<string> Monsters)> DisplaySpell()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                // Récupérer les noms et URL d'images des monstres
                var spells = context.Spell
                    .Select(s => new { 
                        s.Id, 
                        s.Name, 
                        s.Damage, 
                        s.Description,
                        Monsters = s.Monster.Select(m => m.Name).ToList() // Extraire les noms des monstres associés

                    })
                    .ToList();

                if (!spells.Any())
                {
                    MessageBox.Show("Aucun monstre trouvé.");
                    return null;
                }

                // Afficher les noms et URLs des images pour vérification (peut être retiré en production)
                //foreach (var spell in spells)
                //{
                //    MessageBox.Show($"Name: {spell.Name}, Damage: {spell.Damage}");
                //}

                // Retourner une liste de tuples contenant le nom et l'URL de l'image
                return spells.Select(s => (s.Id, s.Name, s.Damage, s.Description, s.Monsters)).ToList();
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
