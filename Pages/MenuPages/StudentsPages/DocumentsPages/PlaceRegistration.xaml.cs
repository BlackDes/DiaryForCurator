using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.Main;
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

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages.DocumentsPages
{
	public partial class PlaceRegistration : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtPalceRegistration = new DataTable();
		string sql;

		public PlaceRegistration()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select PlaceResidenceId, Country, Region, City, Street, House
					 from PlaceResidence
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtPalceRegistration);

			if (dtPalceRegistration.Rows.Count > 0)
			{
				IdVariables.PlaceRegistrationId = Convert.ToInt32(dtPalceRegistration.Rows[0][0]);
				tbxCountry.Text = dtPalceRegistration.Rows[0][1].ToString();
				tbxRegion.Text = dtPalceRegistration.Rows[0][2].ToString();
				tbxCity.Text = dtPalceRegistration.Rows[0][3].ToString();
				tbxStreet.Text = dtPalceRegistration.Rows[0][4].ToString();
				tbxHouse.Text = dtPalceRegistration.Rows[0][5].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxCountry.Text) ||
				string.IsNullOrWhiteSpace(tbxRegion.Text) ||
				string.IsNullOrWhiteSpace(tbxCity.Text) ||
				string.IsNullOrWhiteSpace(tbxStreet.Text) ||
				string.IsNullOrWhiteSpace(tbxHouse.Text))
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private bool IsNumeric(string input)
		{
			int number;
			return !int.TryParse(input, out number);
		}

		private bool AreFieldsValid()
		{
			if (!IsNumeric(tbxCountry.Text) ||
				!IsNumeric(tbxRegion.Text) ||
				!IsNumeric(tbxCity.Text) ||
				!IsNumeric(tbxStreet.Text))
			{
				MessageBox.Show("Поле 'Страна', 'Регион', 'Город' и 'Улица' не должны содержать числа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (!AreFieldsFilled() || !AreFieldsValid())
			{
				return;
			}

			Checkers.DataChanged = false;

			if (Checkers.NewStudents == false)
			{
				sql = $@"update PlaceResidence
						 set Country = '{tbxCountry.Text}', Region = '{tbxRegion.Text}', City = '{tbxCity.Text}', Street = '{tbxStreet.Text}', House = '{tbxHouse.Text}'
						 where PlaceResidenceId = {IdVariables.PlaceRegistrationId}";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into PlaceResidence (StudentId, Country, Region, City, Street, House)
						 values ('{IdVariables.StudentId}', '{tbxCountry.Text}', '{tbxRegion.Text}', '{tbxCity.Text}', '{tbxStreet.Text}', '{tbxHouse.Text}')";

				conDB.SqliteModification(sql);

				Checkers.NewStudents = false;
			}

			conDB.SqliteModification(sql);

			BorderVariables.frm.Navigate(new Students());
		}

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}
	}
}
