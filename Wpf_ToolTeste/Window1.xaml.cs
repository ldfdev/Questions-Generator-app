using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Microsoft.Win32;
using System.Data.SQLite;

namespace Wpf_ToolTeste
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    
   
    public partial class Window1 : Window
    {
        OpenFileDialog sfd;
        //private class ParseFunctions
        //{
        //    string[] Domain;
        //    string[] Difficulty;
        //    FileStream reader;

        //    public ParseFunctions()
        //    {
        //        Domain = new string[] {
        //                                "C",
        //                                "C++",
        //                                "Embedded",
        //                                "Logic",
        //                                "Multithreading"
        //                                };
        //        Difficulty = new string[] {
        //                                    "Easy",
        //                                    "Medium",
        //                                    "Hard"
        //                                    };
        //        OpenFileDialog sfd = new OpenFileDialog();
        //        reader = new FileStream(sfd.FileName, FileMode.Open, FileAccess.Read);
        //    }

        //    private long Extract_object(long start_offset = 0)
        //    {
        //        char open_bracket, close_bracket;
        //        open_bracket = '{';
        //        close_bracket = '}';
        //        Stack st = new Stack();
        //        long end_offset;

        //        reader.Seek(start_offset, SeekOrigin.Begin); //set read position
        //        end_offset = start_offset; //case when a new json doesnt exist
        //        byte[] c = new byte[1];
        //        reader.Read(c, (int)start_offset, 1);
        //        while ((reader.Read(c, (int)start_offset, 1)) != 0)
        //        {
        //            start_offset += 1;
        //            if (c[0] == open_bracket)
        //            {
        //                //cout << "\"{\" encountered.\n";
        //                if (st.Count == 0)
        //                    start_offset = reader.Seek(0, SeekOrigin.Current); // an object starts here
        //                st.Push(c); // when "{" comes from text property
        //            }
        //            else if (c[0] == close_bracket)
        //            {
        //                //cout << "\"}\" encountered.\n";
        //                if (st.Count != 0)
        //                { //ignore if starts with "}"
        //                    st.Pop();
        //                    if (st.Count == 0)
        //                    {
        //                        end_offset = reader.Seek(0, SeekOrigin.Current); //an object ends here
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        reader.Close();
        //        reader.Dispose();
        //        return end_offset;
        //    }

        //    private bool Read_database()
        //    {
        //        if (!File.Exists("ToolTeste.sqlite"))
        //        {
        //            SQLiteConnection.CreateFile("ToolTeste.sqlite");
        //            using (SQLiteConnection m_dbConnection1 = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;"))
        //            {
        //                m_dbConnection1.Open();
        //                string create = "CREATE TABLE Interview (domain VARCHAR(20), difficulty VARCHAR(20), text VARCHAR(255))";
        //                SQLiteCommand commandCreate = new SQLiteCommand(create, m_dbConnection1);
        //                commandCreate.ExecuteNonQuery();
        //                m_dbConnection1.Close();
        //            }
        //        }
        //        SQLiteConnection database = new SQLiteConnection("Data Source=ToolTeste.sqlite;Version=3;"))
        //        database.Open();

        //        long start_offset, end_offset;
        //        start_offset = 0;
        //        end_offset = Extract_object(start_offset);

        //        while (end_offset != start_offset) //exists next json obj
        //        {
        //            //cout << start_offset << " -> " << end_offset << endl;
        //            int size = (int)(end_offset - start_offset + 1);
        //            //set pointer pos in file relative to begining
        //            reader.Seek(start_offset, SeekOrigin.Begin);
        //            // allocate memory to contain file data
        //            byte[] contents = new byte[size];
        //            //contents[size - 1] = 0; //null terninated
        //            // get file data
        //            if (reader.Read(contents, (int)start_offset, size -1) != 0); // exclude trailing "}" from json obj
        //                                                                         //cout << contents;

        //            //parse contents for domain, difficulty, text
                    

        //            //update offsets
        //            Extract_object(end_offset);
        //        }
        //        //delete contents;

        //        database.Close();
        //        return false;
        //    }
        //}

        public class Interview
        {
            public string domain { get; set; }
            public string difficulty { get; set; }
            public string text { get; set; }
        }

        public ObservableCollection<Interview> Elements { get; set; }
        
        public Window1()
        {
            InitializeComponent();
            sfd = new OpenFileDialog();
            Elements = new ObservableCollection<Interview>()
           {
             new Interview(){ domain = "domain1", difficulty = "difficulty1", text = "This is Cat1 Desc"},
             new Interview(){ domain = "domain2", difficulty = "difficulty2", text = "This is Cat2 Desc"},
             new Interview(){ domain = "domain3", difficulty = "difficulty3", text = "This is Cat3 Desc"},
             new Interview(){ domain = "domain4", difficulty = "difficulty4", text = "This is Cat4 Desc"},
             new Interview(){ domain = "domain5", difficulty = "difficulty5", text = "This is Cat5 Desc"}
           };

            Elements.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PartsDataGrid_SelectionChanged);
            this.DataContext = Elements;
        }

        private void PartsDataGrid_SelectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                MessageBox.Show("New Row Added");
            }
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            
            if (sfd.ShowDialog() == true)
                Elements.Add(new Interview());
        }

        
        
    }
}