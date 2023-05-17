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
	public partial class MedicalPolicy : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtMedicalPolicy = new DataTable();
		string sql;

		public MedicalPolicy()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select MedicalPolicyId, Number, DateIssued
					 from MedicalPolicy
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtMedicalPolicy);

			if (dtMedicalPolicy.Rows.Count > 0)
			{
				IdVariables.MedicalPolicyId = Convert.ToInt32(dtMedicalPolicy.Rows[0][0]);
				tbxNumberMed.Text = dtMedicalPolicy.Rows[0][1].ToString();
				dpDateIssueMed.Text = dtMedicalPolicy.Rows[0][2].ToString();
			}
			else
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxNumberMed.Text) || dpDateIssueMed.SelectedDate == null)
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
			if (!IsValidNumber(tbxNumberMed.Text))
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
				sql = $@"update MedicalPolicy
						 set Number = '{tbxNumberMed.Text}', DateIssued = '{dpDateIssueMed.Text}'
						 where MedicalPolicyId = {IdVariables.MedicalPolicyId}";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into MedicalPolicy (StudentId, Number, DateIssued)
						 values ('{IdVariables.StudentId}', '{tbxNumberMed.Text}', '{dpDateIssueMed.Text}')";

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
