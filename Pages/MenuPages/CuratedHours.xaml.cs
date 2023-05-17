using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Diary4CuratorFullEdition.Pages.MenuPages
{
	public partial class CuratedHours : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtEvent = new DataTable();
		string sql;

		public CuratedHours()
		{
			InitializeComponent();
			Reload();
			DisableChangeAndDeleteButtons();
		}

		private void Reload()
		{
			sql = $@"select CuratedHoursId, Name, Date, Description
					 from CuratedHours
					 where GroupId = {GroupVariables.GroupSelected}";

			conDB.SqliteReader(sql, dtEvent);

			dgEvent.ItemsSource = dtEvent.AsDataView();
		}

		#region Изменение данных
		private void dgEvent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			int selectedIndex = dgEvent.SelectedIndex;

			if (selectedIndex < 0 || selectedIndex >= dtEvent.Rows.Count)
				return;

			IdVariables.EventId = Convert.ToInt32(dtEvent.Rows[selectedIndex][0]);
			tbxName.Text = dtEvent.Rows[selectedIndex][1].ToString();
			dpDate.Text = dtEvent.Rows[selectedIndex][2].ToString();
			tbxDescription.Text = dtEvent.Rows[selectedIndex][3].ToString();

			EnableChangeAndDeleteButtons();
			DisableAddButton();
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			string name = tbxName.Text;
			string description = tbxDescription.Text;
			DateTime? date = dpDate.SelectedDate;

			if (string.IsNullOrWhiteSpace(name) || date == null)
			{
				MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			sql = $@"insert into CuratedHours (GroupId, Name, Date, Description)
					 values ({GroupVariables.GroupSelected}, '{tbxName.Text}', '{dpDate.Text}', '{tbxDescription.Text}')";

			conDB.SqliteModification(sql);

			ClearInputFields();
			DisableChangeAndDeleteButtons();
			EnableAddButton();

			Reload();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			string name = tbxName.Text;
			string description = tbxDescription.Text;
			DateTime? date = dpDate.SelectedDate;

			if (string.IsNullOrWhiteSpace(name) || date == null)
			{
				MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			sql = $@"update CuratedHours
					 set Name = '{tbxName.Text}', Date = '{dpDate.Text}', Description = '{tbxDescription.Text}'
					 where CuratedHoursId = {IdVariables.EventId}";

			conDB.SqliteModification(sql);

			ClearInputFields();
			IdVariables.EventId = 0;

			DisableChangeAndDeleteButtons();
			EnableAddButton();

			Reload();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			sql = $@"delete from CuratedHours
					 where CuratedHoursId = {IdVariables.EventId}";

			conDB.SqliteModification(sql);

			ClearInputFields();
			IdVariables.EventId = 0;

			DisableChangeAndDeleteButtons();
			EnableAddButton();

			Reload();
		}
		#endregion

		#region Tools
		private void ClearInputFields()
		{
			tbxName.Clear();
			dpDate.Text = String.Empty;
			tbxDescription.Clear();
		}

		private void DisableChangeAndDeleteButtons()
		{
			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;
		}

		private void EnableChangeAndDeleteButtons()
		{
			btnChange.IsEnabled = true;
			btnDelete.IsEnabled = true;
		}

		private void DisableAddButton()
		{
			btnAdd.IsEnabled = false;
		}

		private void EnableAddButton()
		{
			btnAdd.IsEnabled = true;
		}
		#endregion

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}
	}
}
