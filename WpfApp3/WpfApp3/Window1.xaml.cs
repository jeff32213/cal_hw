using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp3
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Btn_Show(object sender, RoutedEventArgs e)
        {
            string connStr = "data source=localhost;database=calc;user id=root;password='';pooling=true;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `calc_data`", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            dg.ItemsSource = dt.DefaultView;
            conn.Close();
        }

        private void Btn_Delete(object sender, RoutedEventArgs e)
        {
            string connStr = "data source=localhost;database=calc;user id=root;password='';pooling=true;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            //MySqlCommand cmd = new MySqlCommand(, conn);
            var cellInfo = dg.SelectedCells[0];
            var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;

            MySqlCommand cmd = new MySqlCommand("Delete From `calc_data` Where `ID` = @id", conn);
    
   
            cmd.Parameters.AddWithValue("@id", content);
            cmd.ExecuteNonQuery();
            conn.Close();

            MySqlConnection conn2 = new MySqlConnection(connStr);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `calc_data`", conn2);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dg.ItemsSource = dt.DefaultView;
            conn.Close();
            
        }
    }
}
