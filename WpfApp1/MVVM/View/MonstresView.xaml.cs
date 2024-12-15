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
        //private List<string> selectedMonsterSpells = new List<string>();

        private void LoadMonsterImages()
        {
            var monsters = DataMonster.DisplayMonsterImages();
            var spells = DataSpell.DisplaySpell();

            if (monsters != null && monsters.Any())
            {

                foreach (var tupleMonster in monsters)//monsterImages)
                {
                    //foreach (var spellId in monster.Spells) // ID des sorts du monstre
                    //{
                    //    foreach (var spell in spells) // Liste complète des sorts
                    //    {
                    //        if (spell.Id == spellId)
                    //        {
                    //            // Afficher le nom du sort correspondant
                    //            MessageBox.Show($"Sort trouvé : {spell.Name}, name:{spell.Name}");
                    //        }
                    //    }
                    //}
                    //string spellsText = string.Join(", ", monster.Spells);

                    //MessageBox.Show(spellsText);

                    var monsterSpells = spells
                    .Where(s => tupleMonster.Spells.Contains(s.Id)) // Associer les sorts par Id
                    .Select(s => new Spell // Conversion explicite en objets Spell
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
                        ImageUrl = tupleMonster.ImageUrl,
                        Spell = monsterSpells  // Associer les sorts
                    };
                    //var border = new Border
                    //{
                    //    BorderBrush = Brushes.Black,
                    //    BorderThickness = new Thickness(1), // Correction : utilisation de Thickness pour la largeur de la bordure
                    //    Margin = new Thickness(2)
                    //};
                    // Créer un bouton
                    var button = new Button
                    {
                        Margin = new Thickness(5),
                        //HorizontalAlignment = HorizontalAlignment.Stretch,
                        Tag = monster
                    };
                    button.Click += (sender, args) =>
                    {
                        if (sender is Button clickedButton && clickedButton.Tag is Monster selectedMonster)
                        {
                            // Masquer tous les panels dans RightPanelSpells
                            foreach (var child in RightPanelSpells.Children)
                            {
                                if (child is StackPanel panel)
                                {
                                    panel.Visibility = Visibility.Collapsed;
                                }
                            }

                            // Afficher uniquement le panel correspondant au monstre
                            var matchingPanel = RightPanelSpells.Children
                                .OfType<StackPanel>()
                                .FirstOrDefault(panel => panel.Tag is Monster monster && monster.Id == selectedMonster.Id);

                            if (matchingPanel != null)
                            {
                                matchingPanel.Visibility = Visibility.Visible;
                            }
                        }
                        //if (sender is Button clickedButton && clickedButton.Tag is Monster selectedMonster)
                        //{
                        //    var spellDetails = selectedMonster.Spell
                        //        .Select(spell => $"Nom: {spell.Name}, Dégâts: {spell.Damage}")
                        //        .ToList();

                        //    string message = spellDetails.Any()
                        //        ? string.Join("\n", spellDetails)
                        //        : "Aucun sort disponible.";

                        //    MessageBox.Show($"Sorts du monstre {selectedMonster.Name}, HP : {selectedMonster.Health}:\n{message}");
                        //}
                    };


                    // Créer un StackPanel pour contenir l'image et le texte
                    var stackPanelLeft = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    var stackPanelRight = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Tag = monster,
                        Visibility = Visibility.Collapsed
                    };

                    // Ajouter l'image au StackPanel

                    try
                    {
                        var image = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)),
                            Height = 100, // Ajustez la taille selon vos besoins
                            Margin = new Thickness(5)
                        };
                        stackPanelLeft.Children.Add(image);
                        //stackPanelRight.Children.Add(image);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'affichage du nom {monster.ImageUrl}: {ex.Message}");
                    }
                    // Ajouter le texte au StackPanel
                    var nameTextBlock = new TextBlock
                    {
                        Text = monster.Name,
                        FontSize = 12,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    stackPanelLeft.Children.Add(nameTextBlock);
                    //stackPanelRight.Children.Add(nameTextBlock);


                    //var monsterSpells = spells
                    //    .Where(spell => monster.Spell.Any(ms => ms.Id == spell.Id)) // Association par ID des sorts
                    //    .ToList();


                    // Ajouter un TextBlock pour chaque sort
                    if (monsterSpells.Any())
                    {
                        var namePokemon = new TextBlock
                        {
                            Text = $"{monster.Name}",
                            FontSize = 16,
                            Margin = new Thickness(5),
                            Foreground = Brushes.DarkSlateBlue,
                        };
                        stackPanelRight.Children.Add(namePokemon);
                        var imagePokemon = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)),
                            Height = 120, // Ajustez la taille selon vos besoins
                            Margin = new Thickness(5)
                        };
                        stackPanelRight.Children.Add(imagePokemon);
                        var hp = new TextBlock
                        {
                            Text = $"Health : {monster.Health}",
                            FontSize = 16,
                            Margin = new Thickness(5),
                            Foreground = Brushes.DarkSlateBlue,
                        };
                        stackPanelRight.Children.Add(hp);
                        foreach (var spell in monsterSpells)
                        {
                            var spellTextBlock = new TextBlock
                            {
                                Text = $"Spell : {spell.Name}, Damage : {spell.Damage}",
                                FontSize = 14,
                                Margin = new Thickness(2),
                                Foreground = Brushes.DarkSlateBlue
                            };
                            var border = new Border // Créez une nouvelle instance de Border ici
                            {
                                BorderBrush = Brushes.Black,
                                BorderThickness = new Thickness(1),
                                Margin = new Thickness(2),
                                Child = spellTextBlock // Ajoutez le TextBlock à ce Border
                            };
                            stackPanelRight.Children.Add(border);
                        }
                        var playGame = new Button
                        {
                            Content = "PLAY GAME",
                            Margin = new Thickness(5)
                            //HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        playGame.Click += (sender, e) =>
                        {
                            if (monster != null)
                            {
                                // Sélectionner un monstre aléatoire (remplacez par votre logique)
                                var randomMonster = DataMonster.GetRandomMonster();

                                // Instancier FightView avec les deux monstres
                                var fightView = new FightView(monster, randomMonster);

                                // Afficher FightView
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
                    //RightPanelSpells.Children.Clear();
                    RightPanelSpells.Children.Add(stackPanelRight);

                    button.Content = stackPanelLeft;
                    ImageStackPanel.Children.Add(button);


                }
            }
            else
            {
                Console.WriteLine("Aucune image trouvée.");
            }

            //var monster = context.Monster.FirstOrDefault(m => m.Monster == monster);

        }
       


    }
}
