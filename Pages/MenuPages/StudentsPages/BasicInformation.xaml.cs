using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.Main;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class BasicInformation : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtBasic = new DataTable();
		DataTable dtGender = new DataTable();

		string sql;

		public BasicInformation()
		{
			InitializeComponent();

			sql = $@"select GenderId as [Id], Name
					 from Genders";

			conDB.FillComboBox(sql, dtGender, cbxGender);

			if (Checkers.NewStudents == false)
			{
				Reload();
			}
		}

		#region Интрументы
		private void Reload()
		{
			sql = $@"select S.GroupId, S.Surname, S.Name, S.Patronymic, S.GenderId, S.DateBirth, S.PhoneNumber, S.Email
					 from Students S, Genders G
					 where S.StudentId = {IdVariables.StudentId} and S.GenderId = G.GenderId and S.GroupId = {GroupVariables.GroupSelected}";

			conDB.SqliteReader(sql, dtBasic);

			tbxSurname.Text = dtBasic.Rows[0][1].ToString();
			tbxName.Text = dtBasic.Rows[0][2].ToString();
			tbxPatronymic.Text = dtBasic.Rows[0][3].ToString();
			cbxGender.SelectedValue = Convert.ToInt32(dtBasic.Rows[0][4]);
			dpDOB.Text = dtBasic.Rows[0][5].ToString();
			tbxNumber.Text = dtBasic.Rows[0][6].ToString();
			tbxEmail.Text = dtBasic.Rows[0][7].ToString();
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(tbxSurname.Text) ||
				   string.IsNullOrWhiteSpace(tbxName.Text) ||
				   string.IsNullOrWhiteSpace(tbxPatronymic.Text) ||
				   string.IsNullOrWhiteSpace(cbxGender.Text) ||
				   dpDOB.SelectedDate == null ||
				   string.IsNullOrWhiteSpace(tbxNumber.Text) ||
				   string.IsNullOrWhiteSpace(tbxEmail.Text);
		}

		private bool IsValidName(string input)
		{
			return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[A-Za-zА-Яа-я]+$");
		}

		private bool IsValidNumber(string input)
		{
			return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[0-9]+$");
		}

		private bool IsValidEmail(string input)
		{
			return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
		}
		#endregion

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (!IsValidName(tbxSurname.Text) || !IsValidName(tbxName.Text) || !IsValidName(tbxPatronymic.Text))
			{
				MessageBox.Show("Фамилия, имя и отчество должны содержать только буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!IsValidNumber(tbxNumber.Text))
			{
				MessageBox.Show("Номер телефона должен содержать только цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!IsValidEmail(tbxEmail.Text))
			{
				MessageBox.Show("Пожалуйста, введите действительный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (Checkers.NewStudents == false)
			{
				sql = $@"update Students 
						 set GroupId = '{GroupVariables.GroupSelected}', Surname = '{tbxSurname.Text}', Name = '{tbxName.Text}', Patronymic = '{tbxPatronymic.Text}', GenderId = '{cbxGender.SelectedValue}', DateBirth = '{dpDOB.Text}', PhoneNumber = '{tbxNumber.Text}', Email = '{tbxEmail.Text}'
						 where StudentId = {IdVariables.StudentId}";
			}
			else
			{
				sql = $@"insert into Students (GroupId, Surname, Name, Patronymic, GenderId, DateBirth, PhoneNumber, Email)
						 values ('{GroupVariables.GroupSelected}', '{tbxSurname.Text}', '{tbxName.Text}', '{tbxPatronymic.Text}', '{cbxGender.SelectedValue}', '{dpDOB.Text}', '{tbxNumber.Text}', '{tbxEmail.Text}')";

				Checkers.NewStudents = false;
			}

			Checkers.DataChanged = false;

			conDB.SqliteModification(sql);

			BorderVariables.frm.Navigate(new Students());
		}

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}
	}
}
