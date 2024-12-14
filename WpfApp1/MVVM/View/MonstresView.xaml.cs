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

                foreach (var monster in monsters)//monsterImages)
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

                    // Créer un bouton
                    var button = new Button
                    {
                        Margin = new Thickness(5),
                        //HorizontalAlignment = HorizontalAlignment.Stretch,
                        Tag = monster
                    };

                    // Créer un StackPanel pour contenir l'image et le texte
                    var stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };

                    // Ajouter l'image au StackPanel

                    try
                    {
                        var image = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)),
                            Height = 75, // Ajustez la taille selon vos besoins
                            Margin = new Thickness(5)
                        };
                        stackPanel.Children.Add(image);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'affichage du nom {monster.ImageUrl}: {ex.Message}");
                    }
                    // Ajouter le texte au StackPanel
                    var nameTextBlock = new TextBlock
                    {
                        Text = monster.Name,
                        FontSize = 8,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    stackPanel.Children.Add(nameTextBlock);

                    var monsterSpells = spells
                        .Where(spell => monster.Spells.Contains(spell.Id)) // Associez les sorts par ID
                        .ToList();

                    // Ajouter un TextBlock pour chaque sort
                    if (monsterSpells.Any())
                    {
                        foreach (var spell in monsterSpells)
                        {
                            var spellTextBlock = new TextBlock
                            {
                                Text = $"Spell : {spell.Name}, Damage : {spell.Damage}",
                                FontSize = 7,
                                Margin = new Thickness(5, 0, 5, 0),
                                Foreground = Brushes.DarkSlateBlue
                            };
                            stackPanel.Children.Add(spellTextBlock);
                        }
                    }
                    else
                    {
                        var noSpellsTextBlock = new TextBlock
                        {
                            Text = "Aucun sort disponible",
                            FontSize = 7,
                            Margin = new Thickness(5, 0, 5, 0),
                            Foreground = Brushes.Gray
                        };
                        stackPanel.Children.Add(noSpellsTextBlock);
                    }
                    button.Content = stackPanel;
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
