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
	public partial class ITU : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtITU = new DataTable();
		string sql;

		public ITU()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select ITUId, GroupDisability, Number, Validity
					 from ITU
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtITU);


			if (dtITU.Rows.Count > 0)
			{
				IdVariables.ITUId = Convert.ToInt32(dtITU.Rows[0][0]);
				tbxGroupDisability.Text = dtITU.Rows[0][1].ToString();
				tbxNumber.Text = dtITU.Rows[0][2].ToString();
				dpValidity.Text = dtITU.Rows[0][3].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxGroupDisability.Text) || string.IsNullOrWhiteSpace(tbxNumber.Text) || dpValidity.SelectedDate == null)
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
			if (!IsValidNumber(tbxGroupDisability.Text) || !IsValidNumber(tbxNumber.Text))
			{
				MessageBox.Show("Поля 'Группа инвалидности' и 'Номер' должны содержать только цифры и не должны содержать пробелов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
				sql = $@"update ITU
						 set GroupDisability = '{tbxGroupDisability.Text}', Number = '{tbxNumber.Text}', Validity = '{dpValidity.Text}'
						 where ITU = {IdVariables.ITUId}";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into ITU (StudentId, GroupDisability, Number, Validity)
						 values ('{IdVariables.StudentId}', '{tbxGroupDisability.Text}', '{tbxNumber.Text}', '{dpValidity.Text}')";

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
