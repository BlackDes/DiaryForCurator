using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
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

namespace Diary4CuratorFullEdition.Pages
{
	public partial class SetPassword : Page
	{
		public SetPassword()
		{
			InitializeComponent();
			tbxCode.IsEnabled = false;
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(tbxNewPassword.Text) ||
				   string.IsNullOrWhiteSpace(tbxOldPassword.Text) ||
				   string.IsNullOrWhiteSpace(tbxEmail.Text) ||
				   string.IsNullOrWhiteSpace(tbxCode.Text);
		}

		private bool ContainsWhiteSpace(string input)
		{
			return input.Contains(" ");
		}

		private bool IsPasswordValid(string password)
		{
			string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
			return Regex.IsMatch(password, pattern);
		}

		private bool IsEmailValid(string email)
		{
			string pattern = @"^\S+@\S+\.\S+$";
			return Regex.IsMatch(email, pattern);
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			string sql = $"select Password from Authorization where AuthorizationId = {AutorizationVariables.AutorizationId}";

			DataTable dt = new DataTable();
			ConnectionDB conDB = new ConnectionDB();
			conDB.SqliteReader(sql, dt);

			string oldPassword = dt.Rows[0]["Password"].ToString();
			string newPassword = tbxNewPassword.Text;

			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (ContainsWhiteSpace(tbxNewPassword.Text) || ContainsWhiteSpace(tbxOldPassword.Text))
			{
				MessageBox.Show("Поля пароля не должны содержать пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!IsPasswordValid(tbxNewPassword.Text))
			{
				MessageBox.Show("Пароль должен содержать минимум 8 символов, включая цифры, буквы верхнего и нижнего регистров, и специальные символы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!IsEmailValid(tbxEmail.Text))
			{
				MessageBox.Show("Пожалуйста, введите действительный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (newPassword == oldPassword)
			{
				MessageBox.Show("Данный пароль уже применяется!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrEmpty(newPassword))
			{
				MessageBox.Show("Строка с новым паролем пуста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrEmpty(tbxOldPassword.Text))
			{
				MessageBox.Show("Строка со старым паролем пуста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrEmpty(tbxCode.Text))
			{
				MessageBox.Show("Строка с кодом пуста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (tbxCode.Text != SenderEmailVariables.code.ToString())
			{
				MessageBox.Show("Введён неправильный код!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				tbxCode.IsEnabled = false;
				tbxCode.Clear();
				return;
			}

			sql = $"update Authorization set [Password] = '{newPassword}' where [AuthorizationId] == {AutorizationVariables.AutorizationId}";
			conDB.SqliteModification(sql);

			MessageBox.Show("Пароль изменён!", "Успех", MessageBoxButton.OK);
			NavigationService.GoBack();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
        }

		private void btnSendCode_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(tbxEmail.Text))
			{
				MessageBox.Show("Строка с электронной почтой пуста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			MessageBox.Show($"Код для изменения пароля был выслан на почту {tbxEmail.Text}!\nПроверьте папку 'Спам'", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
			SenderEmail se = new SenderEmail();
			se.SendCode(tbxEmail);
			tbxCode.IsEnabled = true;
		}
	}
}
