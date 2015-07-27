/*----------------------------------------------------*/
//Автор - Кутаєв О.В.
//Програма-клієнт
/*----------------------------------------------------*/

using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Client_opityvannya
{
    public partial class Form1 : Form
    {
        TcpClient client;
        public Form1()
        {
            InitializeComponent();
        }
        int iter = 0;//вкладки

        /*Збереження даних, інформація респондента та його відповіді у соц опитуванні*/
        int[] arr = new int [5];//обрані варіанти відповідей на питання 1-5
        string[] txt_msg = new string[2];//особисті дані: ПІБ{0} та місто{1}
        /******************************************************************************/

        private void Connect(Int32 port, string server)
        {
            try
            {
                client = new TcpClient(server, port);//conect to server
                
                Byte[] data1 = System.Text.Encoding.Unicode.GetBytes(txt_msg[0]);//ПІБ
                Byte[] data2 = System.Text.Encoding.Unicode.GetBytes(txt_msg[1]);//Місто
                Byte[] data3 = new byte[arr.Length];//Варіанти відповідей
                data3 = (from i in arr select (byte)i).ToArray();
                NetworkStream stream = client.GetStream();

                stream.Write(data1, 0, data1.Length);
                Thread.Sleep(100);
                stream.Write(data2, 0, data2.Length);
                Thread.Sleep(100);
                stream.Write(data3, 0, data3.Length);
            }
            catch (SocketException)
            {
                MessageBox.Show("Неможливо підключитись до сервера, щоб завантажити результати опитування. Спробуйте пізніше.");
                this.Close();
            }
            finally
            {
                client.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)//Далі
        {
            iter++;
            tabControl1.SelectedIndex = iter;//вкладки

            if (iter == 2) //введення особистих даних
            {
                txt_msg[0] = textBox1.Text + " " + textBox2.Text + " " + textBox3.Text;
                txt_msg[1] = textBox4.Text;
            }
            else if (iter == 3)//питання1
            {
                if (radioButton1.Checked)
                {
                    arr[0] = 1;
                }
                else if (radioButton2.Checked)
                {
                    arr[0] = 2;
                }
            }

            else if (iter == 4)//питання2
            {
                if (radioButton3.Checked)
                {
                    arr[1] = 1;
                }
                else if (radioButton4.Checked)
                {
                    arr[1] = 2;
                }
                else if (radioButton5.Checked)
                {
                    arr[1] = 3;
                }
            }

            else if (iter == 5)//питання3
            {
                if (radioButton6.Checked)
                {
                    arr[2] = 1;
                }
                else if (radioButton7.Checked)
                {
                    arr[2] = 2;
                }
                else if (radioButton8.Checked)
                {
                    arr[2] = 3;
                }
            }

            else if (iter == 6)//питання4
            {
                if (radioButton9.Checked)
                {
                    arr[3] = 1;
                }
                else if (radioButton10.Checked)
                {
                    arr[3] = 2;
                }
                else if (radioButton11.Checked)
                {
                    arr[3] = 3;
                }
                else if (radioButton12.Checked)
                {
                    arr[3] = 4;
                }
                else if (radioButton13.Checked)
                {
                    arr[3] = 5;
                }
            }

            else if (iter == 7)//питання5
            {
                button1.Visible = false;
                button2.Visible = true;//включення кнопки "Завершити"

                if (radioButton14.Checked)
                {
                    arr[4] = 1;
                }
                else if (radioButton15.Checked)
                {
                    arr[4] = 2;
                }
                else if (radioButton16.Checked)
                {
                    arr[4] = 3;
                }

                Connect(9595, "127.0.0.1");
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)// Завершити
        {
            this.Close();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
