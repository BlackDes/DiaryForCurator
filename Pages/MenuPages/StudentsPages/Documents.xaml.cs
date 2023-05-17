using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages.DocumentsPages;
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

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class Documents : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtDocumentsCheck = new DataTable();
		string sql;

		public Documents()
		{
			InitializeComponent();

			miKDNiZP.IsEnabled = false;
			miStateAid.IsEnabled = false;
			miITU.IsEnabled = false;

			// Стоит на учёте 
			sql = $@"select * 
					 from StatusesStudent
					 where StatusId = 6";

			conDB.SqliteReader(sql, dtDocumentsCheck);

			if (dtDocumentsCheck.Rows.Count > 0)
			{
				miKDNiZP.IsEnabled = true;
			}


			// Многодетная семья
			sql = $@"select * 
					 from StatusesStudent
					 where StatusId = 10";

			conDB.SqliteReader(sql, dtDocumentsCheck);

			if (dtDocumentsCheck.Rows.Count > 0)
			{
				miStateAid.IsEnabled = true;
			}

			// Инвалид
			sql = $@"select * 
					 from StatusesStudent
					 where StatusId = 19";

			conDB.SqliteReader(sql, dtDocumentsCheck);

			if (dtDocumentsCheck.Rows.Count > 0)
			{
				miITU.IsEnabled = true;
			}
		}

		private void miPassport_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new Passport());
        }
		private void miPlaceRegistration_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new PlaceRegistration());
		}

		private void miMedicalPolicy_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new MedicalPolicy());
		}

		private void miKDNiZP_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new KDNiZP());
		}

		private void miStateAid_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new StateAid());
		}

		private void miITU_Click(object sender, RoutedEventArgs e)
		{
			fDocuments.Navigate(new ITU());
		}
	}
}
