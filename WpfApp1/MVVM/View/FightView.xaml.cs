using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApp1.MVVM.Model;
using System.Linq;
using WpfApp1.MVVM.ViewModel;


namespace WpfApp1.MVVM.View
{
    public partial class FightView : Window
    {
        private DispatcherTimer opponentAttackTimer;
        private Monster opponentMonster;
        private int MaxHealthOpponentMonster;

        public FightView(Monster playerMonster, int MaxHealthPlayerMonster)
        {
            InitializeComponent();
            InitializeFight(playerMonster, MaxHealthPlayerMonster);
            InitializeOpponentAttackTimer();
        }
        public void InitializeFight(Monster playerMonster, int MaxHealthPlayerMonster)
        {
            opponentMonster = DataMonster.GetRandomMonster();
            MaxHealthOpponentMonster = opponentMonster.Health;
            // Réinitialiser les HP à chaque lancement ou relancement
            playerMonster.Health = MaxHealthPlayerMonster;
            opponentMonster.Health = MaxHealthOpponentMonster;

            //RoundCounter.Text = $"Manche : {currentRound}";

            // Initialiser les données pour le monstre de gauche
            PlayerMonsterName.Text = playerMonster.Name;
            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
            PlayerMonsterHealthBar.Maximum = MaxHealthPlayerMonster;

            PlayerMonsterHealthBar.Value = playerMonster.Health; // Mise à jour de la barre de vie
            PlayerMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(playerMonster.ImageUrl));

            // Ajouter les sorts du monstre sélectionné
            PlayerMonsterSpells.Children.Clear(); // Nettoyer les anciens boutons de sorts
            if (playerMonster.Spell != null)
            {
                foreach (var spell in playerMonster.Spell)
                {
                    var spellButton = new Button
                    {
                        Content = $"{spell.Name} - Damage: {spell.Damage}",
                        FontSize = 14,
                        Margin = new Thickness(5)
                        //HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    // Ajoutez un événement clic au bouton
                    spellButton.Click += (sender, e) =>
                    {
                        // Réduire la santé de l'adversaire
                        opponentMonster.Health -= spell.Damage;
                        if (opponentMonster.Health <= 0)
                        {
                            MessageBox.Show($"You defeated {opponentMonster.Name}!");
                            opponentMonster = DataMonster.GetRandomMonster();
                            MaxHealthOpponentMonster = opponentMonster.Health;
                            InitializeOpponent(opponentMonster, MaxHealthOpponentMonster);
                        }
                        else
                        {
                            UpdateOpponentHealth(opponentMonster);
                        }
                        StartOpponentAttackTimer(playerMonster, opponentMonster);

                        //if (opponentMonster.Health < 0) opponentMonster.Health = 0;

                        //// Mettre à jour l'interface utilisateur
                        //OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
                        //OpponentMonsterHealthBar.Value = opponentMonster.Health;

                        //StartOpponentAttackTimer(playerMonster, opponentMonster);

                        // Logique à exécuter lorsqu'un sort est utilisé
                        MessageBox.Show($"You used {spell.Name} dealing {spell.Damage} damage!\nadverse {opponentMonster.Health}");
                    };

                    // Ajouter le bouton à l'interface
                    PlayerMonsterSpells.Children.Add(spellButton);
                }
            }
            InitializeOpponent(opponentMonster, MaxHealthOpponentMonster);
        }
        private void InitializeOpponent(Monster opponentMonster, int MaxHealthOpponentMonster)
        {
            // Initialiser les données pour le monstre de droite
            OpponentMonsterName.Text = opponentMonster.Name;
            OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
            OpponentMonsterHealthBar.Maximum = MaxHealthOpponentMonster;
            OpponentMonsterHealthBar.Value = opponentMonster.Health; // Mise à jour de la barre de vie
            OpponentMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(opponentMonster.ImageUrl));
        }
        private void UpdateOpponentHealth(Monster opponentMonster)
        {
            OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
            OpponentMonsterHealthBar.Value = opponentMonster.Health;
        }

        private void InitializeOpponentAttackTimer()
        {
            opponentAttackTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            opponentAttackTimer.Tick += (s, e) =>
            {
                OpponentAttack();
                opponentAttackTimer.Stop(); // Stopper le timer après l'attaque
            };
        }
        // Initialiser le timer pour les attaques de l'adversaire
        //    opponentAttackTimer = new DispatcherTimer
        //        {
        //            Interval = TimeSpan.FromSeconds(5)
        //        };
        //opponentAttackTimer.Tick += (s, e) =>
        //        {
        //            OpponentAttack(playerMonster, opponentMonster);
        //opponentAttackTimer.Stop(); // Stopper le timer après l'attaque
        //        };


        //    RestartButton.Click += (sender, e) =>
        //    {
        //        // Réinitialiser les HP et relancer le jeu
        //        //playerMonster.Health = initialPlayerHealth;
        //        //opponentMonster.Health = initialOpponentHealth;

        //        PlayerMonsterHealth.Text = $"Health: {MaxHealthPlayerMonster}";
        //        PlayerMonsterHealthBar.Value = MaxHealthPlayerMonster;

        //        OpponentMonsterHealth.Text = $"Health: {MaxHealthOpponentMonster}";
        //        OpponentMonsterHealthBar.Value = MaxHealthOpponentMonster;

        //        MessageBox.Show("La partie a été relancée !");
        //    };

        //    QuitButton.Click += (sender, e) =>
        //    {
        //        // Logique pour quitter le jeu
        //        Close();
        //    };

        //}
        private void OpponentAttack()
        {
            if (opponentMonster?.Spell != null && opponentMonster.Spell.Any())
            {
                var random = new Random();
                var randomSpell = opponentMonster.Spell.ElementAt(random.Next(opponentMonster.Spell.Count));

                // Reduce player's health
                PlayerMonsterHealth.Text = $"Health: {opponentMonster.Health}";
                PlayerMonsterHealthBar.Value = opponentMonster.Health;

                MessageBox.Show($"{opponentMonster.Name} used {randomSpell.Name}, dealing {randomSpell.Damage} damage!");
            }
            else
            {
                MessageBox.Show("No valid opponent or spells available.");
            }
        }
        private void StartOpponentAttackTimer(Monster playerMonster, Monster opponentMonster)
        {
            opponentAttackTimer?.Stop(); // Arrêter un timer précédent (s'il existe)
            opponentAttackTimer?.Start(); // Démarrer un nouveau timer
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game restarted!");
            // Relancer le jeu (logique à ajouter si nécessaire)
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game exited!");
            Close();
        }

        private void OpponentAttack(Monster playerMonster, Monster opponentMonster)
        {
            if (opponentMonster.Spell != null && opponentMonster.Spell.Any())
            {
                // Choisir un sort aléatoire
                var random = new Random();
                var randomSpell = opponentMonster.Spell.ElementAt(random.Next(opponentMonster.Spell.Count));

                // Réduire la santé du joueur
                playerMonster.Health -= randomSpell.Damage;
                if (playerMonster.Health < 0) playerMonster.Health = 0;

                // Mettre à jour la barre de vie et le texte
                PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
                PlayerMonsterHealthBar.Value = playerMonster.Health;

                // Message pour l'attaque de l'adversaire
                MessageBox.Show($"{opponentMonster.Name} used {randomSpell.Name}, dealing {randomSpell.Damage} damage!");
            }
        }
    }

}
