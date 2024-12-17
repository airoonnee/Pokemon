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
        private Monster MonsterId;

        public SpellsView()
        {
            InitializeComponent();
            LoadMonstersInFilter(); 
            LoadSpells(DataSpell.DisplaySpell()); 
        }
        private void LoadSpells(IEnumerable<(int Id, string Name, int Damage, string? Description, List<int> Monster)> spells)
        {
            SpellsWrapPanel.Children.Clear(); 
            if (spells != null && spells.Any())
            {
                foreach (var spell in spells)
                {
                    var border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1), 
                        Margin = new Thickness(10)
                    };
                    var spellPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };

                    var nameTextBlock = new TextBlock
                    {
                        Text = spell.Name,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 5, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(nameTextBlock);

                    var damageTextBlock = new TextBlock
                    {
                        Text = $"Damage: {spell.Damage}",
                        FontSize = 10,
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(damageTextBlock);

                    var descriptionTextBlock = new TextBlock
                    {
                        Text = spell.Description,
                        FontSize = 10,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    spellPanel.Children.Add(descriptionTextBlock);
                    border.Child = spellPanel;

                    SpellsWrapPanel.Children.Add(border);
                }
            }
            else
            {
                var noSpellsTextBlock = new TextBlock
                {
                    Text = "Aucun sort disponible.",
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
                SpellsWrapPanel.Children.Add(noSpellsTextBlock);
            }
        }
        private void LoadMonstersInFilter()
        {
            var monsters = DataMonster.DisplayMonsterImages(); 
            MonsterFilterComboBox.Items.Clear();
            MonsterFilterComboBox.Items.Add(new ComboBoxItem { Content = "Tous", IsSelected = true });

            foreach (var monster in monsters)
            {
                MonsterFilterComboBox.Items.Add(new ComboBoxItem
                {
                    Content = monster.Name,
                    Tag = monster 
                });
            }
        }
        //public void CompareSpellWithMonster(IEnumerable<Spell> spells, IEnumerable<Monster> monsters)
        //{
        //    foreach (var spell in spells)
        //    {
        //        // Parcourir chaque sort
        //        foreach (var monster in monsters)
        //        {
        //            // Comparer les IDs de sort et de monstre
        //            if (monster.Spell.Any(s => s.Id == spell.Id))
        //            {
        //                // Le monstre a ce sort, faire quelque chose avec cette association
        //                Console.WriteLine($"Le sort '{spell.Name}' est associé au monstre '{monster.Name}'");
        //            }
        //        }
        //    }
        //}

        private void OnMonsterFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonsterFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedMonster = selectedItem.Tag;
                
            }
        }
    }
}
