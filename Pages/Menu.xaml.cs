using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.Main;
using Diary4CuratorFullEdition.Pages.MenuPages;
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

namespace Diary4CuratorFullEdition.Pages
{
	public partial class Menu : Page
	{
		DataTable dtGroupSelection = new DataTable();

		public Menu()
		{
			InitializeComponent();

			ConnectionDB conDB = new ConnectionDB();

			btnStudents.IsEnabled = false;
			btnCuratedHours.IsEnabled = false;

			string sql = $@"select G.GroupId as [Id], G.Name
							from Groups G, Specialties S
							where AuthorizationId = {AutorizationVariables.AutorizationId} and G.SpecialtyId = S.SpecialtyId";

			conDB.FillComboBox(sql, dtGroupSelection, cbxGroupSelection);
		}

		private void NavigateToPage<T>() where T : Page, new()
		{
			NavigationService.Navigate(new T());
		}

		private void btnEditingGroups_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage<EditingGroups>();
		}

		private void btnStudents_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage<Students>();
		}

		private void btnCuratedHours_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage<CuratedHours>();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage<Authorization>();
		}

		private void cbxGroupSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GroupVariables.GroupSelected = Convert.ToInt32(dtGroupSelection.Rows[cbxGroupSelection.SelectedIndex]["Id"]);
			if (cbxGroupSelection.SelectedIndex != -1)
			{
				btnStudents.IsEnabled = true;
				btnCuratedHours.IsEnabled = true;
			}
		}
	}
}
