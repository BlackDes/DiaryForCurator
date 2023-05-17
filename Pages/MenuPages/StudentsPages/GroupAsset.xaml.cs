using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class GroupAsset : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtGroupAsset = new DataTable();
		DataTable dtGroupAssetCheck = new DataTable();
		DataTable dtAcceptAssets = new DataTable();
		string sql;

		public GroupAsset()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select ActiveGroupId as [Id], Name
					 from ActiveGroup";

			conDB.FillComboBox(sql, dtGroupAsset, cbxRole);

			sql = $@"select ASt.AssetStudentId, AG.Name
					 from ActiveStudent ASt, ActiveGroup AG
					 where ASt.StudentId = {IdVariables.StudentId} and ASt.ActiveGroupId = AG.ActiveGroupId";

			conDB.SqliteReader(sql, dtAcceptAssets);

			dgSelected.ItemsSource = dtAcceptAssets.AsDataView();
		}

		private void cbxRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataRowView selectedRow = cbxRole.SelectedItem as DataRowView;
			if (selectedRow != null)
			{
				int groupId = Convert.ToInt32(selectedRow["Id"]);

				sql = $@"select SS.StatusStudentId
						 from ActiveStudent ASt, ActiveGroup AG
						 where ASt.StudentId = '{IdVariables.StudentId}' and AG.StatusId = '{groupId}' and ASt.ActiveGroupId = AG.ActiveGroupId";

				conDB.SqliteReader(sql, dtGroupAssetCheck);

				if (dtGroupAssetCheck.Rows.Count >= 1)
				{
					MessageBox.Show("Данная активность уже присвоена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				sql = $@"insert into ActiveStudent (StudentId, ActiveGroupId) 
						 values ({AutorizationVariables.AutorizationId}, {groupId})";

				conDB.SqliteModification(sql);

				cbxRole.SelectedIndex = -1;

				Reload();
			}
		}

		private void dgSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (dgSelected.SelectedItem != null)
			{
				DataRowView selectedRow = dgSelected.SelectedItem as DataRowView;
				int assetStudentId = Convert.ToInt32(selectedRow["AssetStudentId"]);

				sql = $@"delete from ActiveStudent 
						 where AssetStudentId = {assetStudentId}";

				conDB.SqliteModification(sql);

				Reload();
			}
		}
	}
}
