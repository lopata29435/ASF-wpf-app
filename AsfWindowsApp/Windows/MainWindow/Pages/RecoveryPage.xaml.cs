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
	public partial class RecoveryPage : Page
	{
		private readonly HttpClient httpClient;
		private DispatcherTimer timer;
		private int remainingTimeInSeconds = 59;
		private enum PageState
		{
			EmailInput,
			CodeInput,
			NewPasswordInput
		}
		private PageState currentState;
		public RecoveryPage()
		{
			InitializeComponent();
			httpClient = new HttpClient();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;

			ShowEmailInput();
			textBoxEmail.KeyDown += TextBoxEmail_KeyDown;

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
			string email = textBoxEmail.Text;
			await SendPasswordRecoveryRequestAsync(email);
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
					ShowCodeInput();
					timer.Start();
					return true;
				}
				else
				{
					textBoxEmail.Style = (Style)FindResource("TextBox.Rectangle.small-error");
					textBoxEmailWarning.Visibility = Visibility.Visible;
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private async void TextBoxEmail_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (!string.IsNullOrWhiteSpace(textBoxEmail.Text))
				{
					string email = textBoxEmail.Text;
					bool requestSent = await SendPasswordRecoveryRequestAsync(email);
				}
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
					ShowNewPasswordInput();
				}
				else
				{
					ApplyErrorStyleToTextBoxes();
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
				string email = textBoxEmail.Text;
				string json = $"{{\"code\": \"{code}\",\n \"email\": \"{email}\"}}";
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

		private void TextBoxPsw_TextChanged(object sender, TextChangedEventArgs e)
		{
			CheckPasswordFields();
		}

		private void CheckPasswordFields()
		{
			if (!string.IsNullOrWhiteSpace(textBoxNewPassword1.Text) && !string.IsNullOrWhiteSpace(textBoxNewPassword2.Text))
			{
				buttonSetPassword.IsEnabled = true;
			}
			else
			{
				buttonSetPassword.IsEnabled = false;
			}
		}

		private void ButtonSetPassword_Click(object sender, RoutedEventArgs e)
		{
			CheckPasswords();
		}

		private async void CheckPasswords()
		{
			string password1 = textBoxNewPassword1.Text;
			string password2 = textBoxNewPassword2.Text;

			bool isPasswordValid = IsPasswordValid(password1);

			if (isPasswordValid)
			{
				if(password1 == password2)
				{
					if (await ChangePswAsync(password1))
					{
						mainGrid.Visibility = Visibility.Collapsed;
						subGrid.Visibility = Visibility.Visible;
					}
				}
				else
				{
					textBoxNewPassword1.Style = (Style)FindResource("TextBox.Rectangle.small-error");
					textBoxNewPassword2.Style = (Style)FindResource("TextBox.Rectangle.small-error");
					gridNewPassword.RowDefinitions[0].Height = (GridLength)FindResource("RowHeight");
					textPasswordMismatch.Text = "Пароли не совпадают";
					textPasswordMismatch.Visibility = Visibility.Visible;
				}
			}
			else
			{
				textBoxNewPassword1.Style = (Style)FindResource("TextBox.Rectangle.small-error");
				textBoxNewPassword2.Style = (Style)FindResource("TextBox.Rectangle.small-error");
				gridNewPassword.RowDefinitions[0].Height = new GridLength(120);
				textPasswordMismatch.Text = "Пароль должен содержать не менее 8 символов,\n включая хотя бы одну заглавную букву,\n одну цифру и один специальный символ";
				textPasswordMismatch.Visibility = Visibility.Visible;
			}
		}
		private async Task<bool> ChangePswAsync(string password)
		{
			try
			{
				string email = textBoxEmail.Text;
				string json = $"{{\"password\": \"{password}\",\n \"confirm_password\": \"{password}\",\n \"email\": \"{email}\"}}";
				string apiUrl = $"{Values.Route.ENDPOINT}password_recovery/";
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

		private void ButtonBack_Click(object sender, RoutedEventArgs e)
		{
			switch (currentState)
			{
				case PageState.CodeInput:
					ShowEmailInput();
					break;
				case PageState.NewPasswordInput:
					ShowCodeInput();
					break;
				default:
					break;
			}
		}
		private bool IsPasswordValid(string password)
		{
			bool hasUpperCase = password.Any(char.IsUpper);
			bool hasDigit = password.Any(char.IsDigit);
			bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

			return password.Length >= 8 && hasUpperCase && hasDigit && hasSpecialChar;
		}
		private void ShowEmailInput()
		{
			currentState = PageState.EmailInput;
			gridEmail.Visibility = Visibility.Visible;
			gridCode.Visibility = Visibility.Collapsed;
			gridCode2.Visibility = Visibility.Collapsed;
			gridNewPassword.Visibility = Visibility.Collapsed;
			gridNewPassword2.Visibility = Visibility.Collapsed;
		}

		private void ShowCodeInput()
		{
			currentState = PageState.CodeInput;
			gridEmail.Visibility = Visibility.Collapsed;
			gridCode.Visibility = Visibility.Visible;
			gridCode2.Visibility = Visibility.Visible;
			gridNewPassword.Visibility = Visibility.Collapsed;
			gridNewPassword2.Visibility = Visibility.Collapsed;
		}

		private void ShowNewPasswordInput()
		{
			currentState = PageState.NewPasswordInput;
			gridEmail.Visibility = Visibility.Collapsed;
			gridCode.Visibility = Visibility.Collapsed;
			gridCode2.Visibility = Visibility.Collapsed;
			gridNewPassword.Visibility = Visibility.Visible;
			gridNewPassword2.Visibility = Visibility.Visible;
		}
	}
}
