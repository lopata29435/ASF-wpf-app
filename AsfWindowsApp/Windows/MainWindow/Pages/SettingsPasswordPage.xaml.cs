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
		string token = "394f657232f931de37e3c26626b9ef57da0262f38a046f501bb2ddf6e7dbb192";
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
				Save_Button.IsEnabled = false;
				HttpResponseMessage response = await client.SendAsync(request);
				string body = await response.Content.ReadAsStringAsync();
				Text1.Text = response.StatusCode.ToString();
				Text2.Text = body;
				if (response.IsSuccessStatusCode)
				{
					NavigationService.Navigate(new SettingsPage());
				}
				else
				{
					if (body == "{\"non_field_errors\":[\"Incorrect password\"]}")
					{
						Password.Style = (Style)FindResource("TextBox.Rectangle.small-error");
						ErrorText.Visibility = Visibility.Visible;
					} else if (body == "{\"non_field_errors\":[\"Passwords do not match\"]}")
					{
						ConfirmPassword.Style = (Style)FindResource("TextBox.Rectangle.small-error");
						ConfirmErrorText.Visibility = Visibility.Visible;
					} else
					{
						NewPassword.Style = (Style)FindResource("TextBox.Rectangle.small-error");
						NewErrorText.Visibility = Visibility.Visible;
					}
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
			if (((TextBox)sender).Name == "Password" && Password.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				Password.Style = (Style)FindResource("TextBox.Rectangle.small");
				ErrorText.Visibility = Visibility.Hidden;
			} else if (((TextBox)sender).Name == "ConfirmPassword" && ConfirmPassword.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				ConfirmPassword.Style = (Style)FindResource("TextBox.Rectangle.small");
				ConfirmErrorText.Visibility = Visibility.Hidden;
			} else if (((TextBox)sender).Name == "NewPassword" && NewPassword.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				NewPassword.Style = (Style)FindResource("TextBox.Rectangle.small");
				NewErrorText.Visibility = Visibility.Hidden;
			}
			Save_Button.IsEnabled = Password.Text != "" && NewPassword.Text != "" && ConfirmPassword.Text != ""
				&& Password.Style == (Style)FindResource("TextBox.Rectangle.small")
				&& NewPassword.Style == (Style)FindResource("TextBox.Rectangle.small")
				&& ConfirmPassword.Style == (Style)FindResource("TextBox.Rectangle.small");
		}
	}
}
