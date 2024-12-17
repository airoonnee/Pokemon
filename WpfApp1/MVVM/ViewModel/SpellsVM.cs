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

        public static List<(int Id, string Name, int Damage, string Description, List<int> MonstersID)> DisplaySpell()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée.");
            }

            using var context = new ExerciceMonsterContext(_connectionString);
            try
            {
                var spells = context.Spell
                    .Select(s => new { 
                        s.Id, 
                        s.Name, 
                        s.Damage, 
                        s.Description,
                        MonstersID = s.Monster.Select(m => m.Id).ToList() // Extraire les noms des monstres associés

                    })
                    .ToList();

                if (!spells.Any())
                {
                    MessageBox.Show("Aucun monstre trouvé.");
                    return new List<(int, string, int, string?, List<int>)>(); // Liste vide
                }

                return spells
                .Select(s => (s.Id, s.Name, s.Damage, s.Description, s.MonstersID))
                .ToList();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL lors de la récupération des données : {sqlEx.Message}");
                return new List<(int, string, int, string?, List<int>)>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue : {ex.Message}");
                return new List<(int, string, int, string?, List<int>)>();
            }
        }


    }
}
