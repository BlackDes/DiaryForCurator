using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class Statuses : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtStatuses = new DataTable();
		DataTable dtStatusCheck = new DataTable();
		DataTable dtAcceptStatuses = new DataTable();
		string sql;

		public Statuses()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select StatusId as [Id], Name
					 from Statuses";

			conDB.FillComboBox(sql, dtStatuses, cbxStatuses);

			sql = $@"select SS.StatusStudentId, S.Name
					 from StatusesStudent SS, Statuses S
					 where SS.StudentId = {IdVariables.StudentId} and SS.StatusId = S.StatusId";

			conDB.SqliteReader(sql, dtAcceptStatuses);

			dgSelected.ItemsSource = dtAcceptStatuses.AsDataView();
		}

		private void cbxStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataRowView selectedRow = cbxStatuses.SelectedItem as DataRowView;

			if (selectedRow != null)
			{
				int statusId = Convert.ToInt32(selectedRow["Id"]);

				sql = $@"select SS.StatusStudentId
						 from StatusesStudent SS, Statuses S
						 where SS.StudentId = '{IdVariables.StudentId}' and SS.StatusId = '{statusId}' and S.StatusId = SS.StatusId";

				conDB.SqliteReader(sql, dtStatusCheck);

				if (dtStatusCheck.Rows.Count >= 1)
				{
					MessageBox.Show("Данный статус уже присвоен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				sql = $@"insert into StatusesStudent (StudentId, StatusId) 
						 values ({AutorizationVariables.AutorizationId}, {statusId})";

				conDB.SqliteModification(sql);

				cbxStatuses.SelectedIndex = -1;

				Reload();
			}
		}

		private void dgSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			sql = $@"delete from StatusesStudent 
					 where StatusStudentId = '{Convert.ToInt32(dtAcceptStatuses.Rows[dgSelected.SelectedIndex][0])}'";

			conDB.SqliteModification(sql);

			Reload();
		}
	}
}
