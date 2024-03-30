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
using System.Windows.Threading;

namespace AsfWindowsApp.Windows.MainWindow.Pages
{
	public partial class SignupConfirmPage : Page
	{

		private readonly HttpClient httpClient;
		private DispatcherTimer timer;
		private int remainingTimeInSeconds = 59;
		private string userEmail;

		public SignupConfirmPage(string email)
		{
			InitializeComponent();
			httpClient = new HttpClient();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			userEmail = email;

			ShowCodeInput();

		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			remainingTimeInSeconds--;

			if (remainingTimeInSeconds <= 0)
			{
				timer.Stop();

				HLdiv.IsEnabled = true;
				hyperlinkResend.IsEnabled = true;

				textTimer.Text = "Запросить код повторно через 00:00";
			}
			else
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
				textTimer.Text = $"Запросить код повторно через {timeSpan:mm\\:ss}";
			}
		}
		private async void hyperlinkResend_Click(object sender, RoutedEventArgs e)
		{
			remainingTimeInSeconds = 60;
			timer.Start();
			HLdiv.IsEnabled = false;
			hyperlinkResend.IsEnabled = false;
			await SendPasswordRecoveryRequestAsync(userEmail);
		}
		private async Task<bool> SendPasswordRecoveryRequestAsync(string email)
		{
			try
			{
				string json = $"{{\"email\": \"{email}\"}}";
				string t = $"{Values.Route.ENDPOINT}password_recovery/sendemail/";
				var response = await httpClient.PostAsync(t, new StringContent(json, Encoding.UTF8, "application/json"));

				if (response.IsSuccessStatusCode)
				{
					timer.Start();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox currentTextBox = sender as TextBox;
			if (currentTextBox.Text.Length == 1 && !string.IsNullOrEmpty(currentTextBox.Text))
			{
				int nextIndex = Convert.ToInt32(currentTextBox.Name.Substring(7)) + 1;
				if (nextIndex <= 4)
				{
					TextBox nextTextBox = gridCode.FindName($"textBox{nextIndex}") as TextBox;
					nextTextBox?.Focus();
				}
			}

			if (textBox1.Text.Length == 1 && textBox2.Text.Length == 1 && textBox3.Text.Length == 1 && textBox4.Text.Length == 1)
			{
				string code = textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text;
				bool codeVerified = await VerifyCodeAsync(code);

				if (codeVerified)
				{
					ShowSuccesPage();
				}
				else
				{
					ApplyErrorStyleToTextBoxes();
				}
			}
		}
		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			TextBox currentTextBox = sender as TextBox;

			if ((e.Key == Key.Back || e.Key == Key.Delete) && string.IsNullOrEmpty(currentTextBox.Text))
			{
				int previousIndex = Convert.ToInt32(currentTextBox.Name.Substring(7)) - 1;
				if (previousIndex >= 1)
				{
					TextBox previousTextBox = gridCode.FindName($"textBox{previousIndex}") as TextBox;
					previousTextBox?.Focus();
				}
			}
		}
		private void ApplyErrorStyleToTextBoxes()
		{
			textBox1.Style = (Style)FindResource("TextBox.Rectangle.code-red");
			textBox2.Style = (Style)FindResource("TextBox.Rectangle.code-red");
			textBox3.Style = (Style)FindResource("TextBox.Rectangle.code-red");
			textBox4.Style = (Style)FindResource("TextBox.Rectangle.code-red");
		}
		private async Task<bool> VerifyCodeAsync(string code)
		{
			try
			{
				string json = $"{{\"code\": \"{code}\",\n \"email\": \"{userEmail}\"}}";
				string apiUrl = $"{Values.Route.ENDPOINT}password_recovery/checkemail/";
				var response = await httpClient.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));

				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		private void ButtonGoStart_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new StartPage());
		}
		private void ButtonBack_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SignupPage());
		}
		private void ShowCodeInput()
		{
			mainGrid.Visibility = Visibility.Visible;
			subGrid.Visibility = Visibility.Collapsed;
		}
		private void ShowSuccesPage()
		{
			mainGrid.Visibility = Visibility.Collapsed;
			subGrid.Visibility = Visibility.Visible;
			mailblock.Text = userEmail;
		}
	}
}
