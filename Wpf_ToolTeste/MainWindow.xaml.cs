using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.InteropServices;
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
using System.IO;
using System.Data.SQLite;
using Microsoft.Win32;

namespace Wpf_ToolTeste
{

    public partial class MainWindow : Window
    {
        string[] Domain = new string[] {
                                            "C",
                                            "C++",
                                            "Embedded",
                                            "Logic",
                                            "Multithreading"
                                            };
        string[] Difficulty = new string[] {
                                                "Easy",
                                                "Medium",
                                                "Hard"
                                                };
        private int counter = 1;
        Int32 selectedListBox1Index;
        Int32 selectedListBox2Index;

        public MainWindow()
        {
            InitializeComponent();

            // get reference to MainWindow from other windows
            // in order to invoke MainWindow methods from outside
            Application.Current.MainWindow = this;

            selectedListBox1Index = ListBox1.SelectedIndex;
            selectedListBox2Index = ListBox2.SelectedIndex;
            foreach (string obj in Domain)
                ListBox1.Items.Add(obj);
            foreach(string obj in Difficulty)
                ListBox2.Items.Add(obj);
            if (!File.Exists("ToolTeste.sqlite"))
            {
                SQLiteConnection.CreateFile("ToolTeste.sqlite");
                using (SQLiteConnection m_dbConnection1 = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;"))
                {
                    m_dbConnection1.Open();
                    string create = "CREATE TABLE Interview (domain VARCHAR(20), difficulty VARCHAR(20), text VARCHAR(255))";
                    SQLiteCommand commandCreate = new SQLiteCommand(create, m_dbConnection1);
                    commandCreate.ExecuteNonQuery();
                    m_dbConnection1.Close();
                }
            }
        }

        bool CheckDomainDifficulty()
        {
            if (selectedListBox1Index == -1 || selectedListBox2Index == -1)
            {
                MessageBox.Show("Domain or Difficulty not selected!", "Querying database", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if(CheckDomainDifficulty())
            {
                UInt32 randomPicks;
                if (string.IsNullOrWhiteSpace(Items.Text))
                    randomPicks = 5;
                else
                    randomPicks = Convert.ToUInt32(Items.Text);

                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = @"SELECT  * FROM Interview WHERE domain = """ +
                             Domain[selectedListBox1Index] + @""" and difficulty = """ +
                             Difficulty[selectedListBox2Index] +
                             @""" ORDER BY RANDOM() LIMIT " +
                             Convert.ToString(randomPicks) + @" ;";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock1.Text += Convert.ToString(counter++) +")\n" +
                                       Convert.ToString(reader["text"]) + "\n";
                }
                m_dbConnection.Close();
            }
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedListBox1Index = ListBox1.SelectedIndex;
        }

        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedListBox2Index = ListBox2.SelectedIndex;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1();
            w1.Show();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { 
                                                    FileName = "TestInterview",
                                                    Filter = "Text (*.txt)|*.txt"
                                                    };
            if (sfd.ShowDialog() == true)
                File.WriteAllLines(sfd.FileName, TextBlock1.Text.Split('\n'));
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDomainDifficulty())
            { 
                string message = Domain[selectedListBox1Index] + "\n" + Difficulty[selectedListBox2Index];
                string query_data = "";

                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                {
                    message += "\n";
                    query_data = Clipboard.GetText(TextDataFormat.Text);
                }

                Window3 w3 = Window3.GetInstance();
                if (w3 != null)
                {
                    w3.PersonalizedDialogBox(ref Domain[selectedListBox1Index], ref Difficulty[selectedListBox2Index], ref query_data);
                    w3.Show();
                }
                else
                    Insert();
            }
        }

        public void Insert()
        {
            string query_data = "";
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                query_data = Clipboard.GetText(TextDataFormat.Text);

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;");
            string insert_db = "insert into Interview (domain, difficulty, text) values ( " +
                        "\"" + Domain[selectedListBox1Index] + "\", " +
                        "\"" + Difficulty[selectedListBox2Index] + "\", " +
                        "\"" + query_data + "\"" +
                        ")";
            m_dbConnection.Open();
            SQLiteCommand insert_db_command = new SQLiteCommand(insert_db, m_dbConnection);
            try
            {
                insert_db_command.ExecuteNonQuery();
            }
            catch (SQLiteException exc)
            {
                MessageBox.Show(exc.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            m_dbConnection.Close();
        }

        private void ShowDbButton_Click(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            //best practice order of calls
            w2.DisplayDatabase();
            w2.Show();
            
        }
    }
}
