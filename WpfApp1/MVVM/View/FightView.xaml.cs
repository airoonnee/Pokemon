using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.MVVM.Model;

namespace WpfApp1.MVVM.View
{
    public partial class FightView : Window
    {
        public FightView(Monster playerMonster, Monster opponentMonster)
        {
            InitializeComponent();

            // Initialiser les données pour le monstre de gauche
            PlayerMonsterName.Text = playerMonster.Name;
            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
            PlayerMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(playerMonster.ImageUrl));

            // Ajouter les sorts du monstre sélectionné
            if (playerMonster.Spell != null)
            {
                foreach (var spell in playerMonster.Spell)
                {
                    PlayerMonsterSpells.Children.Add(new TextBlock
                    {
                        Text = $"{spell.Name} - Damage: {spell.Damage}",
                        FontSize = 14,
                        Margin = new Thickness(5)
                    });
                }
            }

            // Initialiser les données pour le monstre de droite
            OpponentMonsterName.Text = opponentMonster.Name;
            OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
            OpponentMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(opponentMonster.ImageUrl));
        }
    }
}
