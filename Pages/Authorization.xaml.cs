using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages.DocumentsPages;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Diary4CuratorFullEdition.Pages
{
	public partial class Authorization : Page
	{
		public Authorization()
		{
			InitializeComponent();
		}

		private void btnAuthorization_Click(object sender, RoutedEventArgs e)
		{
			ConnectionDB conDB = new ConnectionDB();
			DataTable dt = new DataTable();

			string sql = $@"select Password
							from Authorization
							where AuthorizationId == {AutorizationVariables.AutorizationId}";

			conDB.SqliteReader(sql, dt);

			string password = pbxPassword.Password;

			if (string.IsNullOrWhiteSpace(password))
			{
				MessageBox.Show("Пожалуйста, введите пароль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (password != dt.Rows[0][0].ToString())
			{
				MessageBox.Show("Пароль был введён неправильно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			NavigationService.Navigate(new Menu());
		}

		private void btnSetPassword_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SetPassword());
        }
    }
}
