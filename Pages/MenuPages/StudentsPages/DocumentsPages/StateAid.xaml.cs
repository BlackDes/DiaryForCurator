using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	public partial class StateAid : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtStaidAid = new DataTable();
		string sql;

		public StateAid()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select StateAidId, Number, Validity
					 from StateAid
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtStaidAid);

			if (dtStaidAid.Rows.Count > 0)
			{
				IdVariables.StateAidId = Convert.ToInt32(dtStaidAid.Rows[0][0]);
				tbxNumber.Text = dtStaidAid.Rows[0][1].ToString();
				dpValidity.Text = dtStaidAid.Rows[0][2].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxNumber.Text) || dpValidity.SelectedDate == null)
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private bool IsValidNumber(string input)
		{
			return Regex.IsMatch(input, @"^\d+$");
		}

		private bool AreFieldsValid()
		{
			if (!IsValidNumber(tbxNumber.Text))
			{
				MessageBox.Show("Поле 'Номер' должно содержать только цифры и не должно содержать пробелов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
				sql = $@"update StateAid
						 set Number = '{tbxNumber.Text}', Validity = '{dpValidity.Text}'
						 where StateAidId = {IdVariables.StateAidId}";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into StateAid (StudentId, Number, Validity)
						 values ('{IdVariables.StudentId}', '{tbxNumber.Text}', '{dpValidity.Text}')";

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
