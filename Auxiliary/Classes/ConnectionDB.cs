using System.Data.SQLite;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Diary4CuratorFullEdition.Auxiliary.Classes
{
	class ConnectionDB
	{
		static string startupPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
		SQLiteConnection con = new SQLiteConnection($"Data Source={startupPath}\\Diary4Curator; Version=3;");

		public void SqliteReader(string command, DataTable dt)
		{
			try
			{
				dt.Clear();
				con.Open();

				using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command, con))
				{
					dataAdapter.Fill(dt);
				}
				con.Close();
			}
			catch
			{
				MessageBox.Show("Не удалось задействовать SQLite.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void SqliteModification(string command)
		{
			try
			{
				con.Open();
				SQLiteCommand cmd = new SQLiteCommand(command, con);
				cmd.ExecuteReader();
				con.Close();
			}
			catch
			{
				MessageBox.Show("Не удалось задействовать SQLite.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void FillComboBox(string command, DataTable dt, ComboBox cbx)
		{
			SqliteReader(command, dt);

			dt.DefaultView.Sort = "Name";
			cbx.ItemsSource = dt.DefaultView;
			cbx.SelectedValuePath = "Id";
			cbx.DisplayMemberPath = "Name";
		}
	}
}
