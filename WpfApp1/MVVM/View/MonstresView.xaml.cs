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

            if (monsters != null && monsters.Any())
            {
                foreach (var monster in monsters)//monsterImages)
                {
                    // Créer un bouton
                    var button = new Button
                    {
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Stretch
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
                            Height = 150, // Ajustez la taille selon vos besoins
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
                        FontSize = 16,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    stackPanel.Children.Add(nameTextBlock);

                    // Ajouter le StackPanel au bouton
                    button.Content = stackPanel;

                    // Ajouter le bouton au StackPanel principal
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
