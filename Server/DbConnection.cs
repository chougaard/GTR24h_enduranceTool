using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SQLite;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Server
{
    interface IDbConnection
    {
        bool CreateTable(string tableName);
        bool WriteDb(int id, string teamName, string ipAddress, int engineWear);
        void ReadDb(string ip, ClientData cli);
    }


    class DbConnection : IDbConnection
    {
        public const bool DEBUG = true;

        private static int _NumOfCars { get; set; }
        private static string _cs = "URI=file:noname.db";

        private static SQLiteConnection _con;

        
        public DbConnection(string dbname)
        {
            _cs = "URI=file:" + dbname + ".db";
            _con = new SQLiteConnection(_cs);
        }


        public bool CreateTable(string tableName)
        {
            bool err = false;
            
            try
            {
                _con.Open(); //Open connection

                if (DEBUG)
                    MessageBox.Show("Creating table!");

                //Creates a SQL statement:
                var createTableCmd = new SQLiteCommand(_con);

                createTableCmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
                createTableCmd.ExecuteNonQuery();
                _NumOfCars = 0;

                //Parameters:
                createTableCmd.CommandText = @"CREATE TABLE " + tableName + "(Id INTEGER PRIMARY KEY," +
                                             "TeamName TEXT," +
                                             "IPAddress TEXT," +
                                             "EngineWear INT)";
                if (DEBUG)
                    MessageBox.Show(Convert.ToString(createTableCmd.CommandText));

                //Execute command:
                createTableCmd.ExecuteNonQuery();

                _con.Close();
                return true; //Return true if success
            }
            catch (Exception e)
            {
                if (DEBUG)
                    MessageBox.Show("Error: " + e.Message);
                _con.Close();
                return false;
            }

        }


        public bool WriteDb(int id, string teamName, string ipAddress, int engineWear)
        {
            try
            {
                _con.Open();

                if(DEBUG)
                    MessageBox.Show("Creating car: " + id);

                //Creating Query:
                var cmd = new SQLiteCommand(_con);
                cmd.CommandText = "INSERT INTO Cars VALUES(" + id + ",'" +
                    teamName + "','" +
                    ipAddress + "'," +
                    engineWear + ")";

                //Console.WriteLine (Convert.ToString (cmd.CommandText));
                cmd.ExecuteNonQuery();
                ++_NumOfCars;

                _con.Close();
                return true;
            }
            catch (Exception e)
            {
                if (DEBUG)
                    MessageBox.Show("Error: " + e.Message);
                _con.Close();
                return false;
            }
        }


        public void ReadDb(string ip, ClientData cli)
        {
            //ClientData cli = new ClientData();
            
            try
            {
                _con.Open();

                var cmd = new SQLiteCommand(_con);
                cmd.CommandText = "SELECT * FROM Cars WHERE IPAddress = '" + ip + "'";
                SQLiteDataReader reader = cmd.ExecuteReader();
                
                reader.Read();

                cli.ID = reader.GetInt32(0);
                cli.TEAM = reader.GetString(1);
                cli.IP = reader.GetString(2);
                cli.WEAR = reader.GetInt32(3);
            }
            catch (Exception e)
            {
                if (DEBUG)
                    MessageBox.Show("Error: " + e.Message);
                _con.Close();
            }
            _con.Close();
        }

    }

    //public class Datas : ObservableCollection<ClientData> { };  // Just to reference it from xaml

    //[Serializable]
    public class ClientData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id;
        public string Team;
        string Ip;
        int Wear;        

        public int ID
        {
            get { return Id; }
            set
            {
                Id = value;
                OnPropertyChanged("ID");
            }
        }
        
        public string TEAM
        {
            get { return Team; }
            set 
            {
                Team = value;
                OnPropertyChanged("TEAM");
            }
        }

        public string IP
        {
            get { return Ip; }
            set
            {
                Ip = value;
                OnPropertyChanged("IP");
            }
        }

        public int WEAR
        {
            get { return Wear; }
            set
            {
                Wear = value;
                OnPropertyChanged("WEAR");
            }
        }

        public void Updated()
        {
            OnPropertyChanged("ID");
            OnPropertyChanged("TEAM");
            OnPropertyChanged("IP");
            OnPropertyChanged("WEAR");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handl = PropertyChanged;
            if (handl != null)
            {
                handl(this, new PropertyChangedEventArgs(name));
            }

        }
    }
}
