using System.Windows;
using System.Windows.Controls;
using WpfApp1.Service;
using static WpfApp1.MVVM.Model.ExerciceMonsterContext;
using WpfApp1.MVVM.ViewModel;



namespace WpfApp1.MVVM.View
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;
            var user = DataLogin.GetUser(username, password);
            if (user != null && user.Username == username)
            {
                MessageBox.Show("Identification Réussi.");
                Window parentWindow = Window.GetWindow(this);
                GameView gameview = new GameView();
                gameview.Show();
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
        }
    }
}