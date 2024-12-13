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
                    var nameTextBlock = new TextBlock
                    {
                        Text = monster.Name,
                        FontSize = 16,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    try
                    {
                        // Créer un contrôle Image
                        var image = new Image
                        {
                            Source = new BitmapImage(new Uri(monster.ImageUrl)), // Charger l'image depuis l'URL
                            Height = 150, // Ajustez la taille selon vos besoins
                            Margin = new System.Windows.Thickness(5)
                        };

                        // Ajouter l'image au StackPanel
                        ImageStackPanel.Children.Add(nameTextBlock);
                        ImageStackPanel.Children.Add(image);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'affichage du nom {monster.ImageUrl}: {ex.Message}");
                    }
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
