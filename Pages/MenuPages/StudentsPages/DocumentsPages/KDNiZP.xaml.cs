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
	public partial class KDNiZP : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtKDNiZP = new DataTable();
		string sql;

		public KDNiZP()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select KDNiZPId, CauseSupply, DisciplinaryPenalties
					 from KDNiZP
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtKDNiZP);


			if (dtKDNiZP.Rows.Count > 0)
			{
				IdVariables.KDNiZPId = Convert.ToInt32(dtKDNiZP.Rows[0][0]);
				tbxCauseSupply.Text = dtKDNiZP.Rows[0][1].ToString();
				tbxDisciplinaryPenalties.Text = dtKDNiZP.Rows[0][2].ToString();
			}
			else 
			{
				Checkers.NewStudents = true;
			}
		}

		private bool AreFieldsFilled()
		{
			if (string.IsNullOrWhiteSpace(tbxCauseSupply.Text) || string.IsNullOrWhiteSpace(tbxDisciplinaryPenalties.Text))
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (!AreFieldsFilled())
			{
				return;
			}

			Checkers.DataChanged = false;

			if (Checkers.NewStudents == false)
			{
				sql = $@"update KDNiZP
						 set CauseSupply = '{tbxCauseSupply.Text}', DisciplinaryPenalties = '{tbxDisciplinaryPenalties.Text}'
						 where KDNiZPId = {IdVariables.KDNiZPId}";

				conDB.SqliteModification(sql);
			}
			else
			{
				sql = $@"insert into KDNiZP (StudentId, CauseSupply, DisciplinaryPenalties)
						 values ('{IdVariables.StudentId}', '{tbxCauseSupply.Text}', '{tbxDisciplinaryPenalties.Text}')";

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
