using AsfWindowsApp.Windows.MainWindow.Pages;
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

namespace AsfWindowsApp
{
	public partial class MainWindow : Window
	{
		int n = 0;
		public MainWindow()
		{
			InitializeComponent();
			MainFrame.Navigate(new StartPage());
		}
    }
}
