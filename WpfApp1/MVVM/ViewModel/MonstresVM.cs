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

        public static List<(int Id, string Name, int Health, int InitialHealth, string ImageUrl, List<int> Spells)> DisplayMonsterImages()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
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

                return monsters.Select(m => (m.Id, m.Name, m.Health, m.Health, m.ImageUrl, m.Spells)).ToList();

            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la récupération des données : {sqlEx.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue : {ex.Message}");
                throw; 
            }
        }
        public static Monster GetRandomMonster()
        {
            var monsters = DisplayMonsterImages(); 
            if (monsters == null || !monsters.Any())
            {
                throw new InvalidOperationException("Aucun monstre disponible pour la sélection.");
            }

            var random = new Random();
            var randomMonster = monsters[random.Next(monsters.Count)];

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

            return new Monster
            {
                Id = randomMonster.Id,
                Name = randomMonster.Name,
                Health = randomMonster.Health,
                InitialHealth = randomMonster.InitialHealth,
                ImageUrl = randomMonster.ImageUrl,
                Spell = monsterSpells
            };
        }


    }
}
