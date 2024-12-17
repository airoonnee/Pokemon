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
        private int currentRound = 1;
        private Monster playerMonster;
        private List<Button> spellButtons = new List<Button>();
        private double hpMultiplier = 1;
        private double damageMultiplier = 1;



        public FightView(Monster playerMonster)
        {
            InitializeComponent();
            InitializeFight(playerMonster);
            InitializeOpponentAttackTimer();
        }
        public void InitializeFight(Monster PlayerMonster)
        {
            opponentMonster = DataMonster.GetRandomMonster();
            playerMonster = PlayerMonster;
            // Réinitialiser les HP à chaque lancement ou relancement
            playerMonster.Health = playerMonster.InitialHealth;
            opponentMonster.Health = opponentMonster.InitialHealth;

            RoundCounter.Text = $"Manche : {currentRound}";

            // Initialiser les données pour le monstre de gauche
            PlayerMonsterName.Text = playerMonster.Name;
            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
            PlayerMonsterHealthBar.Maximum = playerMonster.InitialHealth;

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
                    spellButtons.Add(spellButton);


                    // Ajoutez un événement clic au bouton
                    spellButton.Click += (sender, e) =>
                    {
                        ToggleSpellButtons(false);
                        MessageBox.Show($"dega : {spell.Damage}\n playerHP : {playerMonster.Health}");
                        // Réduire la santé de l'adversaire
                        if (spell.Damage == 0 && playerMonster.Health <= playerMonster.InitialHealth - 20)
                        {
                            MessageBox.Show("test HP");
                            playerMonster.Health += 20;
                            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
                            PlayerMonsterHealthBar.Value = playerMonster.Health;
                        }
                        

                            opponentMonster.Health -= spell.Damage;
                            if (opponentMonster.Health <= 0)
                            {
                                MessageBox.Show($"You defeated {opponentMonster.Name}!");
                                hpMultiplier += 0.1;
                                damageMultiplier += 0.05;
                                opponentMonster = GenerateImprovedMonster(DataMonster.GetRandomMonster());

                                currentRound++;
                                RoundCounter.Text = $"Manche : {currentRound}";

                                InitializeOpponent(opponentMonster);
                            }
                            else
                            {
                                OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
                                OpponentMonsterHealthBar.Value = opponentMonster.Health;
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
            InitializeOpponent(opponentMonster);
        }
        private Monster GenerateImprovedMonster(Monster baseMonster)
        {
            MessageBox.Show("amelioration");
            // Cloner le monstre de base avec des stats améliorées
            var improvedMonster = new Monster
            {
                Name = baseMonster.Name, // Conserve le nom ou génère un nom aléatoire
                InitialHealth = (int)(baseMonster.InitialHealth * hpMultiplier),
                Health = (int)(baseMonster.Health * hpMultiplier),
                ImageUrl = baseMonster.ImageUrl, // Conserve l'image
                Spell = baseMonster.Spell.Select(spell => new Spell
                {
                    Name = spell.Name,
                    Damage = (int)(spell.Damage * damageMultiplier) // Améliore les dégâts des sorts
                }).ToList()
            };
            MessageBox.Show("amelio fin");
            return improvedMonster;
        }

        private void ToggleSpellButtons(bool isEnabled)
        {
            foreach (var button in spellButtons)
            {
                button.IsEnabled = isEnabled;
            }
        }

        private void InitializeOpponent(Monster opponentMonster)
        {
            // Initialiser les données pour le monstre de droite
            OpponentMonsterName.Text = opponentMonster.Name;
            OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
            OpponentMonsterHealthBar.Maximum = opponentMonster.InitialHealth;
            OpponentMonsterHealthBar.Value = opponentMonster.Health; // Mise à jour de la barre de vie
            OpponentMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(opponentMonster.ImageUrl));
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
                if (randomSpell.Damage == 0 && opponentMonster.Health <= opponentMonster.InitialHealth - 20)
                {
                    opponentMonster.Health += 20;
                    OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
                    OpponentMonsterHealthBar.Value = opponentMonster.Health;
                }
                
                    playerMonster.Health -= randomSpell.Damage;
                    // Reduce player's health
                    if (playerMonster.Health <= 0)
                    {
                        MessageBox.Show($"tu es arriver a la Manche : {currentRound}");
                        this.Close();
                    }
                    PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
                    PlayerMonsterHealthBar.Value = playerMonster.Health;
                    MessageBox.Show($"{opponentMonster.Name} used {randomSpell.Name}, dealing {randomSpell.Damage} damage!");
                
            }
            else
            {
                MessageBox.Show("No valid opponent or spells available.");
            }
            ToggleSpellButtons(true);

        }
        private void StartOpponentAttackTimer(Monster playerMonster, Monster opponentMonster)
        {
            opponentAttackTimer?.Stop(); // Arrêter un timer précédent (s'il existe)
            opponentAttackTimer?.Start(); // Démarrer un nouveau timer
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser les HP et relancer le jeu
            playerMonster.Health = playerMonster.InitialHealth;
            opponentMonster.Health = opponentMonster.InitialHealth;
            currentRound = 1;
            RoundCounter.Text = $"Manche : {currentRound}";


            PlayerMonsterHealth.Text = $"Health: {playerMonster.InitialHealth}";
            PlayerMonsterHealthBar.Value = playerMonster.InitialHealth;

            OpponentMonsterHealth.Text = $"Health: {opponentMonster.InitialHealth}";
            OpponentMonsterHealthBar.Value = opponentMonster.InitialHealth;
            opponentMonster = DataMonster.GetRandomMonster();
            InitializeOpponent(opponentMonster);


            MessageBox.Show("Game restarted!");
            // Relancer le jeu (logique à ajouter si nécessaire)
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game exited!");
            this.Close();
        }

        //private void OpponentAttack(Monster playerMonster, Monster opponentMonster)
        //{
        //    if (opponentMonster.Spell != null && opponentMonster.Spell.Any())
        //    {
        //        // Choisir un sort aléatoire
        //        var random = new Random();
        //        var randomSpell = opponentMonster.Spell.ElementAt(random.Next(opponentMonster.Spell.Count));

        //        // Réduire la santé du joueur
        //        playerMonster.Health -= randomSpell.Damage;
        //        if (playerMonster.Health < 0) playerMonster.Health = 0;

        //        // Mettre à jour la barre de vie et le texte
        //        PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
        //        PlayerMonsterHealthBar.Value = playerMonster.Health;

        //        // Message pour l'attaque de l'adversaire
        //        MessageBox.Show($"{opponentMonster.Name} used {randomSpell.Name}, dealing {randomSpell.Damage} damage!");
        //    }
        //}
    }

}
