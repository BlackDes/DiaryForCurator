using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.Main;
using Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages.DocumentsPages;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Diary4CuratorFullEdition.Pages.MenuPages.StudentsPages
{
	public partial class HomeAddress : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtHomeAddress = new DataTable();
		string sql;

		public HomeAddress()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select HomeAddressId, Region, City, Street, House
					 from HomeAddress
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtHomeAddress);

			if (dtHomeAddress.Rows.Count > 0)
			{
				IdVariables.HomeAddressId = Convert.ToInt32(dtHomeAddress.Rows[0][0]);
				tbxRegion.Text = dtHomeAddress.Rows[0][1].ToString();
				tbxCity.Text = dtHomeAddress.Rows[0][2].ToString();
				tbxStreet.Text = dtHomeAddress.Rows[0][3].ToString();
				tbxHouse.Text = dtHomeAddress.Rows[0][4].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(tbxRegion.Text) ||
				   string.IsNullOrWhiteSpace(tbxCity.Text) ||
				   string.IsNullOrWhiteSpace(tbxStreet.Text) ||
				   string.IsNullOrWhiteSpace(tbxHouse.Text);
		}

		private bool ContainNumbers()
		{
			string region = tbxRegion.Text;
			string city = tbxCity.Text;
			string street = tbxStreet.Text;

			Regex regex = new Regex(@"\d");
			return regex.IsMatch(region) || regex.IsMatch(city) || regex.IsMatch(street);
		}

		private void ClearFields()
		{
			tbxRegion.Text = string.Empty;
			tbxCity.Text = string.Empty;
			tbxStreet.Text = string.Empty;
			tbxHouse.Text = string.Empty;
		}

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (ContainNumbers())
			{
				MessageBox.Show("Поля региона, города и улицы не должны содержать числа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}


			if (Checkers.NewStudents == false)
			{
				sql = $@"update HomeAddress 
						 set Region = '{tbxRegion.Text}', City = '{tbxCity.Text}', Street = '{tbxStreet.Text}', House = '{tbxHouse.Text}'
						 where StudentId = {IdVariables.StudentId}";
			}
			else
			{
				sql = $@"insert into HomeAddress (StudentId, Region, City, Street, House)
						 values ('{IdVariables.StudentId}', '{tbxRegion.Text}', '{tbxCity.Text}', '{tbxStreet.Text}', '{tbxHouse.Text}')";

				Checkers.NewStudents = false;
			}

			conDB.SqliteModification(sql);

			Checkers.DataChanged = false;

			ClearFields();

			BorderVariables.frm.Navigate(new Students());
		}
	}
}
