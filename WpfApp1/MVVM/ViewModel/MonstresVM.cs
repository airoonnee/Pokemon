using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Windows;
using WpfApp1.MVVM.Model;
using WpfApp1.Service;
using System.Threading;

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

        public static List<(int Id, string Name, int Health, string ImageUrl, List<int> Spells)> DisplayMonsterImages()
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
                    .Select(m => new 
                    { 
                        m.Id,
                        m.Name, 
                        m.Health, 
                        m.ImageUrl,
                        Spells = m.Spell.Select( s => s.Id).ToList() // Récupérer les noms des sorts

                    })
                    .ToList();

                if (!monsters.Any())
                {
                    MessageBox.Show("Aucun monstre trouvé.");
                    return null;
                }

                // Afficher les noms et URLs des images pour vérification (peut être retiré en production)
                //foreach (var monster in monsters)
                //{
                //    MessageBox.Show($"Name: {monster.Name}, ImageURL: {monster.ImageUrl}");
                //}

                // Retourner une liste de tuples contenant le nom et l'URL de l'image
                return monsters.Select(m => (m.Id, m.Name, m.Health, m.ImageUrl, m.Spells)).ToList();
                //return monsters.Select(m => (m.Name, m.Health, m.ImageUrl, m.Spells.Select(s => s.Damage).ToList())).ToList();

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
        // Fonction pour obtenir un monstre aléatoire
        public static Monster GetRandomMonster()
        {
            var monsters = DisplayMonsterImages(); // Récupérer la liste des monstres
            if (monsters == null || !monsters.Any())
            {
                throw new InvalidOperationException("Aucun monstre disponible pour la sélection.");
            }

            // Sélectionner un monstre au hasard
            var random = new Random();
            var randomMonster = monsters[random.Next(monsters.Count)];

            // Convertir le tuple en un objet Monster
            var monsterSpells = DataSpell.DisplaySpell()
                .Where(s => randomMonster.Spells.Contains(s.Id))
                .Select(s => new Spell
                {
                    Id = s.Id,
                    Name = s.Name,
                    Damage = s.Damage,
                    Description = s.Description
                })
                .ToList();

            // Retourner un objet Monster
            return new Monster
            {
                Id = randomMonster.Id,
                Name = randomMonster.Name,
                Health = randomMonster.Health,
                ImageUrl = randomMonster.ImageUrl,
                Spell = monsterSpells
            };
        }


    }
}
