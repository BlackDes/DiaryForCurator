using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages.DocumentsPages;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class Family : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtMemberF = new DataTable();
		DataTable dtCheckMemberF = new DataTable();
		DataTable dtRoleMember = new DataTable();
		string sql;
				
		public Family()
		{
			InitializeComponent();
			Reload();

			sql = $@"select RoleId as [Id], Name as [Name]
					 from Roles";

			conDB.FillComboBox(sql, dtRoleMember, cbxRole);
		}

		#region Инструменты
		private void Reload()
		{
			sql = $@"select F.FamilyId, (R.Name || ' | ' || F.Surname || ' ' || F.Name || ' ' || F.Patronymic) as Member, F.RoleId, F.Surname, F.Name, F.Patronymic, F.PhoneNumber, F.PlaceWork, F.PostWork
					 from Family F, Roles R
					 where F.StudentId = {IdVariables.StudentId} and F.RoleId = R.RoleId";

			conDB.SqliteReader(sql, dtMemberF);

			if (dtMemberF.Rows.Count > 0)
			{
				Checkers.NewStudents = false;
			}
			else
			{
				Checkers.NewStudents = true;
			}

			dgMembers.ItemsSource = dtMemberF.DefaultView;
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(tbxSurname.Text) ||
				   string.IsNullOrWhiteSpace(tbxName.Text) ||
				   string.IsNullOrWhiteSpace(tbxPatronymic.Text) ||
				   string.IsNullOrWhiteSpace(cbxRole.Text) ||
				   string.IsNullOrWhiteSpace(tbxNumber.Text) ||
				   string.IsNullOrWhiteSpace(tbxWork.Text) ||
				   string.IsNullOrWhiteSpace(tbxPost.Text);
		}

		private bool IsValidName(string input)
		{
			return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[A-Za-zА-Яа-я]+$");
		}

		private bool IsValidNumber(string input)
		{
			return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[0-9]+$");
		}

		private void ClearFields()
		{
			tbxSurname.Text = string.Empty;
			tbxName.Text = string.Empty;
			tbxPatronymic.Text = string.Empty;
			cbxRole.SelectedIndex = -1;
			tbxNumber.Text = string.Empty;
			tbxWork.Text = string.Empty;
			tbxPost.Text = string.Empty;
		}
		#endregion

		#region Изменение данных
		private void dgMembers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			IdVariables.FamilyId = Convert.ToInt32(dtMemberF.Rows[dgMembers.SelectedIndex][0]);
			tbxSurname.Text = dtMemberF.Rows[dgMembers.SelectedIndex][3].ToString();
			tbxName.Text = dtMemberF.Rows[dgMembers.SelectedIndex][4].ToString();
			tbxPatronymic.Text = dtMemberF.Rows[dgMembers.SelectedIndex][5].ToString();
			cbxRole.SelectedValue = dtMemberF.Rows[dgMembers.SelectedIndex]["RoleId"];
			tbxNumber.Text = dtMemberF.Rows[dgMembers.SelectedIndex][6].ToString();
			tbxWork.Text = dtMemberF.Rows[dgMembers.SelectedIndex][7].ToString();
			tbxPost.Text = dtMemberF.Rows[dgMembers.SelectedIndex][8].ToString();
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
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

			sql = $@"select *
					 from Family 
					 where RoleId = 1 and StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtCheckMemberF);

			if (dtCheckMemberF.Rows.Count > 0 && Convert.ToInt32(cbxRole.SelectedValue) == 1)
			{
				MessageBox.Show("Член семьи с ролью 'Отец' уже записан.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			sql = $@"select *
					 from Family 
					 where RoleId = 2 and StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtCheckMemberF);

			if (dtCheckMemberF.Rows.Count > 0 && Convert.ToInt32(cbxRole.SelectedValue) == 2)
			{
				MessageBox.Show("Член семьи с ролью 'Мать' уже записан.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			sql = $@"insert into Family (StudentId, RoleId, Surname, Name, Patronymic, PhoneNumber, PlaceWork, PostWork) 
					 values ({IdVariables.StudentId}, '{cbxRole.SelectedValue}', '{tbxSurname.Text}', '{tbxName.Text}', '{tbxPatronymic.Text}', '{tbxNumber.Text}', '{tbxWork.Text}', '{tbxPost.Text}')";

			conDB.SqliteModification(sql);

			Checkers.DataChanged = false;

			ClearFields();
			Reload();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
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

			sql = $@"select *
					 from Family 
					 where RoleId = 1 and FamilyId != {IdVariables.FamilyId} and StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtCheckMemberF);

			if (dtCheckMemberF.Rows.Count > 0 && Convert.ToInt32(cbxRole.SelectedValue) == 1)
			{
				MessageBox.Show("Член семьи с ролью 'Отец' уже записан.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			sql = $@"select *
					 from Family 
					 where RoleId = 2 and FamilyId != {IdVariables.FamilyId} and StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtCheckMemberF);

			if (dtCheckMemberF.Rows.Count > 0 && Convert.ToInt32(cbxRole.SelectedValue) == 2)
			{
				MessageBox.Show("Член семьи с ролью 'Мать' уже записан.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			sql = $@"update Family 
					 set RoleId = '{cbxRole.SelectedValue}', Surname = '{tbxSurname.Text}', Name = '{tbxName.Text}', Patronymic = '{tbxPatronymic.Text}', PhoneNumber = '{tbxNumber.Text}', PlaceWork = '{tbxWork.Text}', PostWork = '{tbxPost.Text}' 
					 where FamilyId = {IdVariables.FamilyId}";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			sql = $@"delete from Family 
					 where FamilyId = {IdVariables.FamilyId}";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}
		#endregion

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}
	}
}
