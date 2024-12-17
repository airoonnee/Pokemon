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

            playerMonster.Health = playerMonster.InitialHealth;
            opponentMonster.Health = opponentMonster.InitialHealth;

            RoundCounter.Text = $"Manche : {currentRound}";

            PlayerMonsterName.Text = playerMonster.Name;
            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
            PlayerMonsterHealthBar.Maximum = playerMonster.InitialHealth;

            PlayerMonsterHealthBar.Value = playerMonster.Health;
            PlayerMonsterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(playerMonster.ImageUrl));

            PlayerMonsterSpells.Children.Clear();
            if (playerMonster.Spell != null)
            {
                foreach (var spell in playerMonster.Spell)
                {
                    var spellButton = new Button
                    {
                        Content = $"{spell.Name} - Damage: {spell.Damage}",
                        FontSize = 14,
                        Margin = new Thickness(5)
                    };
                    spellButtons.Add(spellButton);


                    spellButton.Click += (sender, e) =>
                    {
                        ToggleSpellButtons(false);
                        if (spell.Damage == 0 && playerMonster.Health <= playerMonster.InitialHealth - 20)
                        {
                            MessageBox.Show("+20 HP Pour toi");
                            playerMonster.Health += 20;
                            PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
                            PlayerMonsterHealthBar.Value = playerMonster.Health;
                        }


                        opponentMonster.Health -= spell.Damage;
                        MessageBox.Show($"-{spell.Damage} HP Pour l'adverser");

                        if (opponentMonster.Health <= 0)
                        {
                            MessageBox.Show($"Manche suivante !");
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
                    };
                    PlayerMonsterSpells.Children.Add(spellButton);
                }
            }
            InitializeOpponent(opponentMonster);
        }
        private Monster GenerateImprovedMonster(Monster baseMonster)
        {
            var improvedMonster = new Monster
            {
                Name = baseMonster.Name,
                InitialHealth = (int)(baseMonster.InitialHealth * hpMultiplier),
                Health = (int)(baseMonster.Health * hpMultiplier),
                ImageUrl = baseMonster.ImageUrl,
                Spell = baseMonster.Spell.Select(spell => new Spell
                {
                    Name = spell.Name,
                    Damage = (int)(spell.Damage * damageMultiplier)
                }).ToList()
            };
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
            OpponentMonsterName.Text = opponentMonster.Name;
            OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
            OpponentMonsterHealthBar.Maximum = opponentMonster.InitialHealth;
            OpponentMonsterHealthBar.Value = opponentMonster.Health;
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
                opponentAttackTimer.Stop();
            };
        }
        private void OpponentAttack()
        {
            if (opponentMonster?.Spell != null && opponentMonster.Spell.Any())
            {
                var random = new Random();
                var randomSpell = opponentMonster.Spell.ElementAt(random.Next(opponentMonster.Spell.Count));
                if (randomSpell.Damage == 0 && opponentMonster.Health <= opponentMonster.InitialHealth - 20)
                {
                    MessageBox.Show("+20 HP Pour l'adverser");

                    opponentMonster.Health += 20;
                    OpponentMonsterHealth.Text = $"Health: {opponentMonster.Health}";
                    OpponentMonsterHealthBar.Value = opponentMonster.Health;
                }

                playerMonster.Health -= randomSpell.Damage;
                MessageBox.Show($"-{randomSpell.Damage} HP Pour toi");

                if (playerMonster.Health <= 0)
                {
                    MessageBox.Show($"BRAVO\nTu es arriver a la Manche : {currentRound}");
                    this.Close();
                }
                PlayerMonsterHealth.Text = $"Health: {playerMonster.Health}";
                PlayerMonsterHealthBar.Value = playerMonster.Health;
            }
            else
            {
                MessageBox.Show("No valid opponent or spells available.");
            }
            ToggleSpellButtons(true);

        }
        private void StartOpponentAttackTimer(Monster playerMonster, Monster opponentMonster)
        {
            opponentAttackTimer?.Stop();
            opponentAttackTimer?.Start();
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
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


            MessageBox.Show("Parti Relance");
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Parti Quitter");
            this.Close();
        }
    }
}