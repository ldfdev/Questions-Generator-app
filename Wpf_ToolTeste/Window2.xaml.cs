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
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace Wpf_ToolTeste
{

    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        public void DisplayDatabase()
        {
            string query = "select * from Interview";
            DataTable table = new DataTable();
            SQLiteConnection m_dbConnection1 = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;");
            SQLiteCommand commandDisplay = new SQLiteCommand(query, m_dbConnection1);
            m_dbConnection1.Open();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter.SelectCommand = commandDisplay;
            adapter.Fill(table);
            BazaDeDate.DataContext = table.DefaultView;
            m_dbConnection1.Close();
        }

        private void BazaDeDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    public class Window3 : Window2
    {
        static bool doNotShow = false;
        public bool yesNoInsertDB { get; }
        MainWindow parent;

        public static Window3 GetInstance()
        {
            if (doNotShow == false)
                return new Window3();
            return null;
        }

        private Window3()
        {
            yesNoInsertDB = false;
            this.Loaded += Window3_Loaded;
        }

        private void Window3_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            parent = Window.GetWindow(this) as MainWindow;

        }

        public void PersonalizedDialogBox(ref string message1, ref string message2, ref string message3)
        {
            // create a DoNotShow checkBox
            CheckBox cb1 = new CheckBox { Content = "Do not show this DialogBox again!", Height = 30, VerticalContentAlignment = VerticalAlignment.Center };

            // create YesNo button

            Button[] b = new Button[2] { new Button { Content = "Confirm" }, new Button { Content = "Cancel" } };
            StackPanel wp = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                //VerticalAlignment = VerticalAlignment.Top,
                Name = "wp"
            };
            wp.Children.Add(cb1);
            foreach (Button btn in b) { btn.Height = 30; wp.Children.Add(btn); };
            cb1.Checked += new RoutedEventHandler(DoNotShow);

            //raise event when b[0] is clicked
            //if (parent != null)
            //{
            b[0].Click += new RoutedEventHandler(Insert);
            b[0].Click += new RoutedEventHandler(Close);
            //}

            b[1].Click += new RoutedEventHandler(Close);
            DataTable dt = new DataTable();
            dt.Columns.Add("Domain", typeof(string));
            dt.Columns.Add("Difficulty", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            dt.Rows.Add(message1, message2, message3);
            BazaDeDate.DataContext = dt.DefaultView;

            //add wp as a new row in grid
            GridPanel.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(wp, 1);
            Grid.SetColumn(wp, 0);
            GridPanel.Children.Add(wp);
        }

        private void Insert(object s, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow ;
            main.Insert();
        }

        private void DoNotShow(object s, RoutedEventArgs e)
        {
            doNotShow = true;
        }

        private void Close(object s, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
