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
{
	/// <summary>
	/// Логика взаимодействия для TestPage.xaml
	/// </summary>
	public partial class TestPage : Page
	{
		public TestPage()
		{
			InitializeComponent();
		}

		private void nameEdit(object sender, RoutedEventArgs e)
		{
			gridMain.Visibility = Visibility.Collapsed;
			gridName.Visibility = Visibility.Visible;
        }

		private void BackFromNameToMane(object sender, RoutedEventArgs e)
		{
			gridName.Visibility = Visibility.Collapsed;
			Input.Text = string.Empty;
			gridMain.Visibility = Visibility.Visible;
		}
	}
}
