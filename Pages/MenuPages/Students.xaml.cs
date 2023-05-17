using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Diary4CuratorFullEdition.Pages.Main
{
	public partial class Students : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtStudent = new DataTable();

		string sql;

		public Students()
		{
			InitializeComponent();
			Reload();
		}

		#region Инструменты
		private void Reload()
		{
			sql = $@"select StudentId, (Surname || ' ' || Name || ' ' || Patronymic) as SNP 
					 from Students
					 where GroupId = {GroupVariables.GroupSelected}";

			conDB.SqliteReader(sql, dtStudent);

			dgStudents.ItemsSource = null;
			dgStudents.ItemsSource = dtStudent.AsDataView();

			mStudent.IsEnabled = false;
			fStudent.IsEnabled = false;

			miHomeAddress.IsEnabled = false;
			miDocuments.IsEnabled = false;
			miFamily.IsEnabled = false;
			miSocialMedia.IsEnabled = false;
			miGroupAsset.IsEnabled = false;
			miStatuses.IsEnabled = false;

			btnDelete.IsEnabled = false;
		}

		private void OpenPages()
		{
			if (Checkers.NewStudents == false)
			{
				mStudent.IsEnabled = true;
				fStudent.IsEnabled = true;

				miHomeAddress.IsEnabled = true;
				miDocuments.IsEnabled = true;
				miFamily.IsEnabled = true;
				miSocialMedia.IsEnabled = true;
				miGroupAsset.IsEnabled = true;
				miStatuses.IsEnabled = true;
				miCallOfParents.IsEnabled = true;
			}
			else
			{
				mStudent.IsEnabled = true;
				fStudent.IsEnabled = true;
			}
		}

		private void NavigateToPage(Page page)
		{
			if (Checkers.DataChanged == true)
			{
				MessageBoxResult result = MessageBox.Show("Остались не сохранённые данные! Продолжить?", "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					fStudent.Navigate(page);
					Checkers.DataChanged = false;
				}
			}
			else
			{
				fStudent.Navigate(page);
			}
		}
		#endregion

		#region Изменение данных студентов в группе
		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			Checkers.NewStudents = true;
			mStudent.IsEnabled = true;
			fStudent.IsEnabled = true;
			OpenPages();
			NavigateToPage(new BasicInformation());
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			conDB.SqliteModification(sql);
			btnDelete.IsEnabled = false;

			Reload();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{

		}

		private void dgStudents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			int selectedIndex = dgStudents.SelectedIndex;

			if (selectedIndex < 0 || selectedIndex >= dtStudent.Rows.Count)
			{
				return;
			}

			IdVariables.StudentId = Convert.ToInt32(dtStudent.Rows[selectedIndex][0]);

			Checkers.DataChanged = false;
			Checkers.DataTableChanged = true;

			mStudent.IsEnabled = true;
			fStudent.IsEnabled = true;

			btnDelete.IsEnabled = true;

			OpenPages();

			NavigateToPage(new BasicInformation());
		}
		#endregion

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			if (Checkers.DataChanged == true)
			{
				MessageBoxResult result = MessageBox.Show("Остались не сохранённые данные! Продолжить?", "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					NavigationService.Navigate(new Menu());
				}
			}
			else
			{
				NavigationService.Navigate(new Menu());
			}
		}

		#region Меню информации студентов
		private void miBasic_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new BasicInformation());
		}

		private void miHomeAddress_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new HomeAddress());
		}

		private void miDocuments_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new Documents());
		}

		private void miFamily_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new Family());
		}

		private void miSocialMedia_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new SocialMedia());
		}

		private void miGroupAsset_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new GroupAsset());
		}

		private void miStatuses_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new Statuses());
		}

		private void miCallOfParents_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(new CallOfParents());
		}
		#endregion
	}
}
