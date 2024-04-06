using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	public partial class SignupPage : Page
	{
		public SignupPage()
		{
			InitializeComponent();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(!string.IsNullOrEmpty(NameTextBox.Text) && !string.IsNullOrEmpty(EmailTextBox.Text)
			&& !string.IsNullOrEmpty(PasswordTextBox.Text) && !string.IsNullOrEmpty(RePasswordTextBox.Text))
			{
				LoginButton.IsEnabled = true;
			}
		}

		private bool ValidateFields()
		{
			bool isEmailValid = IsValidEmail();
			bool isPasswordValid = IsValidPassword();
			bool isConfirmPasswordValid = PasswordTextBox.Text == RePasswordTextBox.Text && !string.IsNullOrEmpty(PasswordTextBox.Text);

			SetFieldValidation(EmailTextBox, EmailSubTextBlock, isEmailValid);
			SetFieldValidation(PasswordTextBox, PasswordSubTextBlock, isPasswordValid);
			SetFieldValidation(RePasswordTextBox, RePasswordSubTextBlock, isConfirmPasswordValid);

			return isEmailValid && isConfirmPasswordValid && isPasswordValid;
		}

		private void SetFieldValidation(TextBox textBox, TextBlock subBlock, bool isValid)
		{
			if (isValid)
			{
				textBox.Style = (Style)FindResource("TextBox.Rectangle.small");
				subBlock.Visibility = Visibility.Collapsed;
			}
			else
			{
				textBox.Style = (Style)FindResource("TextBox.Rectangle.small-error");
				subBlock.Visibility = Visibility.Visible;
			}
		}

		private bool IsValidEmail()
		{
			Regex emailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
			return emailRegex.IsMatch(EmailTextBox.Text);
		}

		private bool IsValidPassword()
		{
			Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
			return passwordRegex.IsMatch(PasswordTextBox.Text);
		}

		//Дописать чтобы работало на всем экране
		private void SendButton_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && LoginButton.IsEnabled)
			{
				SubmitApplication();
			}
		}
		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			SubmitApplication();
		}
		private void HyperLink_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new LoginPage());
		}

		private void SubmitApplication()
		{
			if(ValidateFields())
			{
				NavigationService.Navigate(new SignupConfirmPage(NameTextBox.Text, EmailTextBox.Text, PasswordTextBox.Text));
			}
		}
	}
}
