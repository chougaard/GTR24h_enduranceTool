using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SQLite;

namespace Server
{
    interface IDbConnection
    {
        bool CreateTable(string tableName);
        bool WriteDb(int id, string teamName, string ipAddress, int engineWear);
        void ReadDb(string ip);
    }


    class DbConnection : IDbConnection
    {
        public const bool DEBUG = false;

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


        public void ReadDb(string ip)
        {

            try
            {
                _con.Open();

                var cmd = new SQLiteCommand(_con);
                cmd.CommandText = "SELECT * FROM Cars WHERE ip = '" + ip + "'";

                SQLiteDataReader reader = cmd.ExecuteReader();

                reader.Read();

                int idout = reader.GetInt32(0);
                string teamout = reader.GetString(1);
                string ipout = reader.GetString(2);
                int wearout = reader.GetInt32(3);

                
            }
            catch (Exception e)
            {
                if (DEBUG)
                    MessageBox.Show("Error: " + e.Message);
                _con.Close();
            }
        }


    }
}
