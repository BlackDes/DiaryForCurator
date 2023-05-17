using Diary4CuratorFullEdition.Auxiliary.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using System.Xml.Linq;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class CallOfParents : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtCallParents = new DataTable();
		DataTable dtMemberF = new DataTable();
		string sql;

		public CallOfParents()
		{
			InitializeComponent();
			Reload();

			sql = $@"select FamilyId as [Id], (Surname || ' ' || Name || ' ' || Patronymic) as [Name]
					 from Family
					 where StudentId = '{IdVariables.StudentId}'";

			conDB.FillComboBox(sql, dtMemberF, cbxFamily);
		}

		private void Reload()
		{
			sql = $@"SELECT P.PhoneConversationId as [Id], F.FamilyId [FamilyId], (F.Surname || ' ' || F.Name || ' ' || F.Patronymic || ' | ' || P.Date) as [Info], P.Date, P.ContentConversations
					 FROM PhoneConversations P
					 JOIN Family F ON F.FamilyId = P.FamilyId
					 WHERE F.StudentId = '{IdVariables.StudentId}'";

			conDB.SqliteReader(sql, dtCallParents);

			dgTelephone.ItemsSource = dtCallParents.DefaultView;

			btnAdd.IsEnabled = true;
			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(cbxFamily.Text) ||
				   string.IsNullOrWhiteSpace(dpDate.Text) ||
				   string.IsNullOrWhiteSpace(tbxDescription.Text);
		}

		private void ClearFields()
		{
			cbxFamily.SelectedIndex = -1;
			dpDate.SelectedDate = null;
			tbxDescription.Text = string.Empty;
		}

		private void dgTelephone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			IdVariables.PhoneConversationsId = Convert.ToInt32(dtCallParents.Rows[dgTelephone.SelectedIndex][0]);
			IdVariables.FamilyId = Convert.ToInt32(dtCallParents.Rows[dgTelephone.SelectedIndex]["FamilyId"]);
			cbxFamily.SelectedValue = dtCallParents.Rows[dgTelephone.SelectedIndex]["FamilyId"];
			dpDate.Text = dtCallParents.Rows[dgTelephone.SelectedIndex][3].ToString();
			tbxDescription.Text = dtCallParents.Rows[dgTelephone.SelectedIndex][4].ToString();

			btnChange.IsEnabled = true;
			btnDelete.IsEnabled = true;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			sql = $@"insert into PhoneConversations (FamilyId, Date, ContentConversations)
					 values ('{IdVariables.FamilyId}', '{dpDate.Text}', {tbxDescription.Text})";

			conDB.SqliteModification(sql);

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

			sql = $@"UPDATE PhoneConversations
					 SET FamilyId = (SELECT FamilyId FROM Family WHERE StudentId = '{IdVariables.StudentId}'),
					 Date = '{dpDate.Text}',
					 ContentConversations = '{tbxDescription.Text}'
					 WHERE FamilyId IN (SELECT FamilyId FROM Family WHERE StudentId = '{IdVariables.StudentId}')";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			sql = $@"delete *
					 from PhoneConversations
					 where PhoneConversationId = '{IdVariables.PhoneConversationsId}'";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}
	}
}
