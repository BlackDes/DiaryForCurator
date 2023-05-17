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
	public partial class Passport : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtPassport = new DataTable();
		string sql;

		public Passport()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select PassportId, Number, Series, DateIssued, IssuedBy
					 from Passport
					 where StudentId = '{IdVariables.StudentId}'";

			conDB.SqliteReader(sql, dtPassport);

			if (dtPassport.Rows.Count > 0)
			{
				IdVariables.PassportId = Convert.ToInt32(dtPassport.Rows[0][0]);
				tbxNumber.Text = dtPassport.Rows[0][1].ToString();
				tbxSeries.Text = dtPassport.Rows[0][2].ToString();
				dpDateIssue.Text = dtPassport.Rows[0][3].ToString();
				tbxIssuedBy.Text = dtPassport.Rows[0][4].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxNumber.Text) ||
				string.IsNullOrWhiteSpace(tbxSeries.Text) ||
				dpDateIssue.SelectedDate == null ||
				string.IsNullOrWhiteSpace(tbxIssuedBy.Text))
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private bool AreNumberAndSeriesValid()
		{
			if (!int.TryParse(tbxNumber.Text, out int number) || !int.TryParse(tbxSeries.Text, out int series))
			{
				MessageBox.Show("Поле 'Номер' и 'Серия' должны содержать только числа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (!AreFieldsFilled() || !AreNumberAndSeriesValid())
			{
				return;
			}

			Checkers.DataChanged = false;

			if (Checkers.NewStudents == false)
			{
				sql = $@"update Passport
						 set Number = '{tbxNumber.Text}', Series = '{tbxSeries.Text}', DateIssued = '{dpDateIssue.Text}', IssuedBy = '{tbxIssuedBy.Text}'
						 where PassportId = '{IdVariables.HomeAddressId}'";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into Passport (StudentId, Number, Series, DateIssued, IssuedBy)
						 values ('{IdVariables.StudentId}', '{tbxNumber.Text}', '{tbxSeries.Text}', '{dpDateIssue.Text}', '{tbxIssuedBy.Text}')";

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
