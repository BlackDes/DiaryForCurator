using Diary4CuratorFullEdition.Auxiliary.Classes;
using Diary4CuratorFullEdition.Pages;
using System;
using System.Collections.Generic;
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

namespace Diary4CuratorFullEdition
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			BorderVariables.win = this;
			BorderVariables.frm = fMain;

			fMain.Navigate(new Authorization());
		}
    }
}
