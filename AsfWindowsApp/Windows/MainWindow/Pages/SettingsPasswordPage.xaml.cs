using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
	/// Логика взаимодействия для SettingsPasswordPage.xaml
	/// </summary>
	public partial class SettingsPasswordPage : Page
	{
		string token = "2b34d859415eea9e3e7453f7306e3e71e9140487c371818c4ba0d753a8ab9210";
		public SettingsPasswordPage()
		{
			InitializeComponent();
		}
		private async void SavePassword_Click(object sender, RoutedEventArgs e)
		{
			string password = Password.Text;
			string new_password = NewPassword.Text;
			string confirm_password = ConfirmPassword.Text;


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
			var data = new StringContent($"{{\"password\": \"{password}\",\"new_password\": \"{new_password}\",\"confirm_password\": \"{confirm_password}\"}}", Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Values.Route.ENDPOINT}settings/changepassword/")
			{
				Content = data
			};
			try
			{
				HttpResponseMessage response = await client.SendAsync(request);
				Text1.Text = response.StatusCode.ToString();
				Text2.Text = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					NavigationService.Navigate(new SettingsPage());
				}
				else
				{
					Save_Button.IsEnabled = false;
					Password.Style = (Style)FindResource("TextBox.Rectangle.small-error");
					ErrorText.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
		}
		private void BackToMain_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsPage());
		}

		private void Input_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = Password.Text != "";
			if (Password.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				Password.Style = (Style)FindResource("TextBox.Rectangle.small");
				ErrorText.Visibility = Visibility.Hidden;
			}
		}
	}
}
