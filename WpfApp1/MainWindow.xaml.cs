using System.Text;
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


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVM mainWindowVM { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            //create mainwindow view model
            mainWindowVM = new MainWindowVM();

            //Assign VM to datacontext
            //=> View can acces to variables to VM;
            DataContext = mainWindowVM;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}