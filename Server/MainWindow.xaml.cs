using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;


namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IDbConnection _database;

        public MainWindow()
        {
            InitializeComponent();

            _database = new DbConnection("myDb");
        }

        private void ButGen_Click(object sender, RoutedEventArgs e)
        {
            _database.CreateTable("Cars");
        }

        private void ButRead_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButSend_Click(object sender, RoutedEventArgs e)
        {
            string tempip = TbIp1.Text + "." + TbIp2.Text + "." + TbIp3.Text + "." + TbIp4.Text;

            _database.WriteDb(Convert.ToInt32(TbId.Text), TbTeam.Text, tempip, Convert.ToInt32(TbWear.Text));
        }
    }
}
