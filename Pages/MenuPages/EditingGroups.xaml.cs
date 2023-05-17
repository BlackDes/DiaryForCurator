using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Diary4CuratorFullEdition.Pages.Main
{
	public partial class EditingGroups : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtSpecialties = new DataTable();
		DataTable dtGroups = new DataTable();
		string sql;

		public EditingGroups()
		{
			InitializeComponent();
			Reload();

			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;
		}

		private void Reload()
		{
			sql = @"select SpecialtyId, Name 
					from Specialties";

			conDB.FillComboBox(sql, dtSpecialties, cbxSpeciality);

			sql = @"select G.GroupId, G.Name as [Name], S.SpecialtyId
					from Groups G, Specialties S where G.SpecialtyId = S.SpecialtyId";

			conDB.SqliteReader(sql, dtGroups);

			dgGroups.ItemsSource = dtGroups.AsDataView();
		}
		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxName.Text) || cbxSpeciality.SelectedItem == null)
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private void dgGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			GroupVariables.GroupId = Convert.ToInt32(dtGroups.Rows[dgGroups.SelectedIndex][0]);
			tbxName.Text = dtGroups.Rows[dgGroups.SelectedIndex][1].ToString();
			cbxSpeciality.SelectedIndex = Convert.ToInt32(dtGroups.Rows[dgGroups.SelectedIndex][2]) - 1;

			btnChange.IsEnabled = true;
			btnDelete.IsEnabled = true;

			btnAdd.IsEnabled = false;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (!AreFieldsFilled())
			{
				return;
			}

			sql = $@"insert into Groups (AuthorizationId, SpecialtyId, Name) 
					 values ({AutorizationVariables.AutorizationId}, '{dtSpecialties.Rows[cbxSpeciality.SelectedIndex][0]}', '{tbxName.Text}')";

			conDB.SqliteModification(sql);

			tbxName.Clear();
			cbxSpeciality.SelectedIndex = -1;

			Reload();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (!AreFieldsFilled())
			{
				return;
			}

			sql = $@"update Groups 
					 set SpecialtyId = '{dtSpecialties.Rows[cbxSpeciality.SelectedIndex][0]}', Name = '{tbxName.Text}' 
					 where GroupId = {GroupVariables.GroupId}";

			conDB.SqliteModification(sql);

			tbxName.Clear();
			cbxSpeciality.SelectedIndex = -1;
			GroupVariables.GroupId = 0;

			btnAdd.IsEnabled = true;

			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;

			Reload();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			sql = $@"delete from Groups 
					 where GroupId = {GroupVariables.GroupId}";

			conDB.SqliteModification(sql);

			tbxName.Clear();
			cbxSpeciality.SelectedIndex = -1;
			GroupVariables.GroupId = 0;

			btnAdd.IsEnabled = true;

			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;

			Reload();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Menu());
		}
	}
}
