using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.MVVM.Model;
using WpfApp1.MVVM.ViewModel;

namespace WpfApp1.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour MonstresView.xaml
    /// </summary>
    public partial class MonstresView : UserControl
    {
        public MonstresView()
        {
            InitializeComponent();
            LoadMonsterImages();

        }
        private void LoadMonsterImages()
        {
            var monsters = DataMonster.DisplayMonsterImages();
            var spells = DataSpell.DisplaySpell();

            if (monsters != null && monsters.Any())
            {

                foreach (var tupleMonster in monsters)
                {
                    var monsterSpells = spells
                    .Where(s => tupleMonster.Spells.Contains(s.Id)) 
                    .Select(s => new Spell 
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Damage = s.Damage,
                        Description = s.Description
                    })
                    .ToList();

                    var monster = new Monster
                    {
                        Id = tupleMonster.Id,
                        Name = tupleMonster.Name,
                        Health = tupleMonster.Health,
                        InitialHealth = tupleMonster.Health,
                        ImageUrl = tupleMonster.ImageUrl,
                        Spell = monsterSpells  
                    };

                    var button = new Button
                    {
                        Margin = new Thickness(5),
                        Tag = monster
                    };
                    button.Click += (sender, args) =>
                    {
                        if (sender is Button clickedButton && clickedButton.Tag is Monster selectedMonster)
                        {
                            foreach (var child in RightPanelSpells.Children)
                            {
                                if (child is StackPanel panel)
                                {
                                    panel.Visibility = Visibility.Collapsed;
                                }
                            }

                            var matchingPanel = RightPanelSpells.Children
                                .OfType<StackPanel>()
                                .FirstOrDefault(panel => panel.Tag is Monster monster && monster.Id == selectedMonster.Id);

                            if (matchingPanel != null)
                            {
                                matchingPanel.Visibility = Visibility.Visible;
                            }
                        }
                    };

                    var stackPanelLeft = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Background = Brushes.Transparent
                    };
                    var stackPanelRight = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Tag = monster,
                        Visibility = Visibility.Collapsed,
                        Background = Brushes.Transparent
                    };

                    try
                    {
                        var image = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)),
                            Height = 100, 
                            Margin = new Thickness(5)
                        };
                        stackPanelLeft.Children.Add(image);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'affichage du nom {monster.ImageUrl}: {ex.Message}");
                    }

                    var nameTextBlock = new TextBlock
                    {
                        Text = monster.Name,
                        FontSize = 12,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    stackPanelLeft.Children.Add(nameTextBlock);

                    if (monsterSpells.Any())
                    {
                        var namePokemon = new TextBlock
                        {
                            Text = $"{monster.Name}",
                            FontSize = 16,
                            Margin = new Thickness(5),
                            Foreground = Brushes.Black,
                        };
                        stackPanelRight.Children.Add(namePokemon);
                        var imagePokemon = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)),
                            Height = 120, 
                            Margin = new Thickness(5)
                        };
                        stackPanelRight.Children.Add(imagePokemon);
                        var hp = new TextBlock
                        {
                            Text = $"Health : {monster.Health}",
                            FontSize = 16,
                            Margin = new Thickness(5),
                            Foreground = Brushes.Black,
                        };
                        stackPanelRight.Children.Add(hp);
                        foreach (var spell in monsterSpells)
                        {
                            var spellTextBlock = new TextBlock
                            {
                                Text = $"Spell : {spell.Name}, Damage : {spell.Damage}",
                                FontSize = 14,
                                Margin = new Thickness(2),
                                Foreground = Brushes.DarkSlateBlue,
                                Background = Brushes.White
                            };
                            var border = new Border 
                            {
                                BorderBrush = Brushes.Black,
                                BorderThickness = new Thickness(1),
                                Margin = new Thickness(2),
                                Child = spellTextBlock 
                            };
                            stackPanelRight.Children.Add(border);
                        }
                        var playGame = new Button
                        {
                            Content = "PLAY GAME",
                            Margin = new Thickness(5)
                        };
                        playGame.Click += (sender, e) =>
                        {
                            if (monster != null)
                            {
                                var fightView = new FightView(monster);

                                fightView.ShowDialog();
                            }
                        };

                        stackPanelRight.Children.Add(playGame);
                    }
                    else
                    {
                        var noSpellsTextBlock = new TextBlock
                        {
                            Text = "Aucun sort disponible",
                            FontSize = 14,
                            Margin = new Thickness(5),
                            Foreground = Brushes.Gray
                        };
                        stackPanelRight.Children.Add(noSpellsTextBlock);
                    }
                    RightPanelSpells.Children.Add(stackPanelRight);

                    button.Content = stackPanelLeft;
                    ImageStackPanel.Children.Add(button);
                }
            }
            else
            {
                Console.WriteLine("Aucune image trouvée.");
            }
        }
    }
}
