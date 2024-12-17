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
using WpfApp1.Service;
using System.IO;
using System.Text.Json;
using static WpfApp1.MVVM.Model.ExerciceMonsterContext;
using WpfApp1.MVVM.ViewModel;


namespace WpfApp1.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour UrlBddView.xaml
    /// </summary>
    public partial class UrlBddView : UserControl
    {
        public UrlBddView()
        {
            InitializeComponent();
        }
        private void ValidateUrlButton_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = UrlTextBox.Text;
            bool isLoginInitialized = DataLogin.Initialize(connectionString);
            bool isSignupInitialized = DataSignup.Initialize(connectionString);
            bool isMonsterInitialized = DataMonster.Initialize(connectionString);
            bool isSpellInitialized = DataSpell.Initialize(connectionString);

            if (isLoginInitialized && isSignupInitialized && isMonsterInitialized && isSpellInitialized)
            {
                MessageBox.Show("Connection Réussi !");
            }
            else
            {
                MessageBox.Show("Erreur de connexion, veuillez vérifier l'URL de la base de données.");
            }

        }
    }
}
