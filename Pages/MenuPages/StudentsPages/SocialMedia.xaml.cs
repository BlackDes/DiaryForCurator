using Diary4CuratorFullEdition.Auxiliary.Classes;
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
	public partial class SocialMedia : Page
	{
		ConnectionDB conDB = new ConnectionDB();
		DataTable dtSocialMedia = new DataTable();

		string sql;

		public SocialMedia()
		{
			InitializeComponent();
			Reload();
		}

		private void Reload()
		{
			sql = $@"select SocialMediaId, Name, Link, DescriptionActivities
					 from SocialMedia
					 where StudentId = {IdVariables.StudentId}";

			conDB.SqliteReader(sql, dtSocialMedia);

			btnAdd.IsEnabled = true;
			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;
		}

		private bool AreFieldsEmpty()
		{
			return string.IsNullOrWhiteSpace(tbxName.Text) ||
				   string.IsNullOrWhiteSpace(tbxLink.Text) ||
				   string.IsNullOrWhiteSpace(tbxDescription.Text);
		}

		private bool ContainsWhiteSpace(string input)
		{
			return input.Contains(" ");
		}

		private void ClearFields()
		{
			tbxName.Text = string.Empty;
			tbxLink.Text = string.Empty;
			tbxDescription.Text = string.Empty;
		}

		private void dgSocialMedia_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			IdVariables.SocialMediaId = Convert.ToInt32(dtSocialMedia.Rows[0][0]);
			tbxName.Text = dtSocialMedia.Rows[dgSocialMedia.SelectedIndex][1].ToString();
			tbxLink.Text = dtSocialMedia.Rows[dgSocialMedia.SelectedIndex][2].ToString();
			tbxDescription.Text = dtSocialMedia.Rows[dgSocialMedia.SelectedIndex][3].ToString();

			btnAdd.IsEnabled = false;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (ContainsWhiteSpace(tbxLink.Text))
			{
				MessageBox.Show("Поле ссылки не должно содержать пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			sql = $@"insert into SocialMedia (StudentId, Name, Link, Name, DescriptionActivities) 
					 values ({AutorizationVariables.AutorizationId}, '{tbxName.Text}', '{tbxLink.Text}', '{tbxDescription.Text}')";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (AreFieldsEmpty())
			{
				MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (ContainsWhiteSpace(tbxLink.Text))
			{
				MessageBox.Show("Поле ссылки не должно содержать пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			sql = $@"update SocialMedia 
					 set Name = '{tbxName.Text}', Link = '{tbxLink.Text}', DescriptionActivities = '{tbxDescription.Text}'
					 where SocialMediaId = {IdVariables.SocialMediaId}";

			conDB.SqliteModification(sql);

			btnAdd.IsEnabled = true;
			btnChange.IsEnabled = false;
			btnDelete.IsEnabled = false;

			ClearFields();
			Reload();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			sql = $@"delete SocialMedia 
					 where SocialMediaId = {IdVariables.SocialMediaId}";

			conDB.SqliteModification(sql);

			ClearFields();
			Reload();
		}

		private void Changed_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Checkers.DataChanged = true;
		}
	}
}
