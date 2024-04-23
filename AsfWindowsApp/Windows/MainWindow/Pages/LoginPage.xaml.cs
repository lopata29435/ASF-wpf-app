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

	public partial class LoginPage : Page
	{
		private const string ApiUrl = "your_api_url_here";

		public LoginPage()
		{
			InitializeComponent();
			CheckFields();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CheckFields();
			ResetErrorStyles();
		}

		private void CheckFields()
		{
			if (!string.IsNullOrWhiteSpace(EmailTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordTextBox.Text))
			{
				LoginButton.IsEnabled = true;
			}
			else
			{
				LoginButton.IsEnabled = false;
			}
		}

		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			await LoginAsync();
		}

		private async Task LoginAsync()
		{
			string email = EmailTextBox.Text;
			string password = PasswordTextBox.Text;

			using (HttpClient client = new HttpClient())
			{
				var content = new StringContent($"{{\"email\": \"{email}\", \"password\": \"{password}\"}}", Encoding.UTF8, "application/json");
				string ApiUrl = $"{Values.Route.ENDPOINT}login/";
				try
				{
					var response = await client.PostAsync(ApiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						HandleSuccessfulLogin();
					}
					else
					{
						HandleErrorResponse();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
		}

		private void HandleErrorResponse()
		{
			EmailTextBox.Style = (Style)FindResource("TextBox.Rectangle.small-error");
			PasswordTextBox.Style = (Style)FindResource("TextBox.Rectangle.small-error");
		}

		private void HandleSuccessfulLogin()
		{
			NavigationService.Navigate(new ProjectsPage());
		}
		private async void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && LoginButton.IsEnabled)
			{
				await LoginAsync();
			}
		}
		private void ForgotPasswordHyperlink_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new RecoveryPage());
		}
		private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SignupPage());
		}

		private void ResetErrorStyles()
		{
			if(EmailTextBox.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				EmailTextBox.Style = (Style)FindResource("TextBox.Rectangle.small");
				PasswordTextBox.Style = (Style)FindResource("TextBox.Rectangle.small");
			}
		}

	}
}
