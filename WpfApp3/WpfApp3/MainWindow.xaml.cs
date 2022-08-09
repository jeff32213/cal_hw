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
using MySql.Data.MySqlClient;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
         

        }


        private void Btn_1(object sender, RoutedEventArgs e)
        {
            Print("1");
        }
        private void Btn_2(object sender, RoutedEventArgs e)
        {
            Print("2");
        }
        private void Btn_3(object sender, RoutedEventArgs e)
        {
            Print("3");
        }
        private void Btn_4(object sender, RoutedEventArgs e)
        {
            Print("4");
        }
        private void Btn_5(object sender, RoutedEventArgs e)
        {
            Print("5");
        }
        private void Btn_6(object sender, RoutedEventArgs e)
        {
            Print("6");
        }
        private void Btn_7(object sender, RoutedEventArgs e)
        {
            Print("7");
        }
        private void Btn_8(object sender, RoutedEventArgs e)
        {
            Print("8");
        }
        private void Btn_9(object sender, RoutedEventArgs e)
        {
            Print("9");
        }
        private void Btn_0(object sender, RoutedEventArgs e)
        {
            Print("0");
        }
        private void Btn_add(object sender, RoutedEventArgs e)
        {
            Print("+");
        }

        private void Btn_sub(object sender, RoutedEventArgs e)
        {
            Print("-");
        }

        private void Btn_mul(object sender, RoutedEventArgs e)
        {
            Print("*");
        }

        private void Btn_div(object sender, RoutedEventArgs e)
        {
            Print("/");
        }
        private void Btn_equ(object sender, RoutedEventArgs e)
        {
            pos_res.Text = InfixToPostfix(result.Text);
            pre_res.Text = InfixToPrefix(result.Text);
            dec_res.Text = Calculate(pos_res.Text);
            bin_res.Text = Convert.ToString(Convert.ToInt32(dec_res.Text), 2);

        }
        private void Btn_clear(object sender, RoutedEventArgs e)
        {
            result.Text = "";
            pos_res.Text = "";
            pre_res.Text = "";
            dec_res.Text = "";
            bin_res.Text = "";
        }
        private void Btn_insert(object sender, RoutedEventArgs e)
        {
            //string sql = @"INSERT INTO `calc_data` (`ID`, `Infix`, `Prefix`, `Postfix`, `Dec`, `Bin`)  VALUES (NULL, '" + result.Text + "', '" + pre_res.Text + "', '" + pos_res.Text + "', '" + dec_res.Text + "', '" + bin_res.Text + "');";
           
            Connect();
        }
        private void Btn_query(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.Show();
        }
        void Connect()
        {
            string connStr = "data source=localhost;database=calc;user id=root;password='';pooling=true;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connStr);

            

            //string sql = @" IF NOT EXISTS (SELECT * FROM `calc_data` WHERE `Infix`='3+3+3') BEGIN INSERT INTO `calc_data`(`ID`, `Infix`, `Prefix`, `Postfix`, `Dec`, `Bin`) VALUES(@v1, @v2, @v3, @v4, @v5, @v6) END;  ";

            string sql_str1 = @" SELECT * FROM `calc_data` WHERE `Infix` = @tmp ";
            string sql_str2 = @"INSERT INTO `calc_data` (`ID`, `Infix`, `Prefix`, `Postfix`, `Dec`, `Bin`)
                           VALUES (NULL, '" + result.Text + "', '" + pre_res.Text + "', '" + pos_res.Text + "', '" + dec_res.Text + "', '" + bin_res.Text + "');";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql_str1, conn);
            
            cmd.Parameters.AddWithValue("@tmp", result.Text);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("資料已存在");
                conn.Close();
            }
            else
            {
                conn.Close();
                conn.Open();
                MySqlCommand cmd2 = new MySqlCommand(sql_str2, conn);
                cmd2.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("insert成功");
            }

        }

        void Print(string str)
        {
            if (result.Text == "")
                result.Text = str;
            else
                result.Text += str;
        }

        String InfixToPostfix(String infix)
        {
            
            Stack<char> operators = new Stack<char>();
          
            String output = "";


            for (int i = 0; i < infix.Length; i++)
            {
               
                if (GetPriority(infix[i]) == 0)
                {
                   
                    output += infix[i];
                }

                else
                {
                    while (operators.Count != 0 &&
                        GetPriority(infix[i]) <=
                            GetPriority(operators.Peek()))
                    {
                        char op = operators.Peek();
                        operators.Pop();

                   
                        output += op;
                    }

                    operators.Push(infix[i]);
                }
            }

          
            while (operators.Count != 0)
            {
                char op = operators.Peek();
                operators.Pop();

                
                output += op;
            }
            
            return output;
        }

        public static int GetPriority(char C)
        {
            if (C == '-' || C == '+')
                return 1;
            else if (C == '*' || C == '/')
                return 2;
            return 0;
        }

        String InfixToPrefix(String infix)
        {
            return Reverse(InfixToPostfix(Reverse(infix)));
        }

        public static String Reverse(String s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new String(charArray);
        }
        String Calculate(string s)
        {
            Stack<int> operands = new Stack<int>();

            int op1, op2;
        
            for(int i=0; i<s.Length; i++)
            {
                switch (s[i])
                {
                    case '+':
                        op1 = operands.Peek();
                        operands.Pop();
                        op2 = operands.Peek();
                        operands.Pop();
                        operands.Push(op2 + op1);
                        break;
                    case '-':
                        op1 = operands.Peek();
                        operands.Pop();
                        op2 = operands.Peek();
                        operands.Pop();
                        operands.Push(op2 - op1);
                        break;
                    case '*':
                        op1 = operands.Peek();
                        operands.Pop();
                        op2 = operands.Peek();
                        operands.Pop();
                        operands.Push(op2 * op1);
                        break;
                    case '/':
                        op1 = operands.Peek();
                        operands.Pop();
                        op2 = operands.Peek();                        
                        operands.Pop();
                        operands.Push(op2 / op1);
                        break;

                    default:
                        operands.Push(Convert.ToInt32(s[i])-'0');
                        break;
                }
            }


            return Convert.ToString(operands.Peek());
        }
    }
}
