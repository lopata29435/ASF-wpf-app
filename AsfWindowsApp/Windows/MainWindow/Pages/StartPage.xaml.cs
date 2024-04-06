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

namespace AsfWindowsApp.Windows.MainWindow.Pages
{	public partial class StartPage : Page
	{
		public StartPage()
		{
			InitializeComponent();
		}

		private void ButtonLogin_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new LoginPage());
		}
		private void ButtonReg_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SignupPage());
		}
	}
}
