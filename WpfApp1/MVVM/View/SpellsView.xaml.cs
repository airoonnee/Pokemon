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
    /// Logique d'interaction pour SpellsView.xaml
    /// </summary>
    public partial class SpellsView : UserControl
    {
        public SpellsView()
        {
            InitializeComponent();
            LoadSpell();
        }
        private void LoadSpell()
        {
            var spells = DataSpell.DisplaySpell();
            if (spells != null && spells.Any())
            {
                foreach (var spell in spells)
                {
                    // Créer un conteneur pour chaque sort
                    var spellPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(10)
                    };

                    // Ajouter le nom du sort
                    var nameTextBlock = new TextBlock
                    {
                        Text = spell.Name,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 5, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(nameTextBlock);
                    // Ajouter les dégâts du sort
                    var damageTextBlock = new TextBlock
                    {
                        Text = $"Damage: {spell.Damage}",
                        FontSize = 14,
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(damageTextBlock);

                    // Ajouter la description du sort
                    var descriptionTextBlock = new TextBlock
                    {
                        Text = spell.Description,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(descriptionTextBlock);

                    // Ajouter le conteneur au ItemsControl
                    SpellsItemsControl.Items.Add(spellPanel);
                }
            }
            else
            {
                // Afficher un message si aucun sort n'est trouvé
                var noSpellsTextBlock = new TextBlock
                {
                    Text = "Aucun sort disponible.",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
                SpellsItemsControl.Items.Add(noSpellsTextBlock);
            }

            //var monster = context.Monster.FirstOrDefault(m => m.Monster == monster);

        }

    }
}
