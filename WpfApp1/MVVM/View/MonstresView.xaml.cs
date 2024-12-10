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
            //LoadMonsterImages();
        }
        //private void LoadMonsterImages()
        //{
        //    // Appel de la fonction pour récupérer toutes les URLs des images des monstres
        //    var monsterImages = DataMonster.DisplayMonsterImages();

        //    if (monsterImages != null && monsterImages.Any())
        //    {
        //        foreach (var imageUrl in monsterImages)
        //        {
        //            Console.WriteLine($"Image URL: {imageUrl}");
        //            // Vous pouvez également afficher ces images dans l'interface utilisateur, si nécessaire.
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Aucune image trouvée.");
        //    }

        //    //var monster = context.Monster.FirstOrDefault(m => m.Monster == monster);

        //}
    }
}
