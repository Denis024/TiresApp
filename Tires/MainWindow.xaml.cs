using System.Windows;
using Tires.Class;
using Tires.Pages;

namespace Tires
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Manager.GetFrame = MainFrame;
            MainFrame.Navigate(new ProductPage());
        }
    }
}
