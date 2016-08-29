using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Serial
{
    public partial class Form1 : Form
    {
        private SerialPort McuPort = new SerialPort();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();//获取本机串口数组
            Array.Sort(ports);//对串口排序
            comboBoxPort.Items.AddRange(ports);//添加到下拉列表
            comboBoxPort.SelectedIndex = comboBoxPort.Items.Count > 0 ? 0 : -1;
            comboBoxBaud.SelectedIndex = comboBoxBaud.Items.IndexOf("9600");
            textBox2.Text = "串口" + comboBoxPort.Text + "初始化完成" + "\r\n等待打开";
            McuPort.DataReceived += Port_DataReceived;
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int n = McuPort.BytesToRead;
            byte[] buf = new byte[n];
            McuPort.Read(buf, 0, n);
            textBox2.AppendText("\r\n"+Encoding.ASCII.GetString(buf));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (McuPort.IsOpen)
            {
                McuPort.Close();
            }

            else
            {
                McuPort.PortName = comboBoxPort.Text;
                McuPort.BaudRate = int.Parse(comboBoxBaud.Text);
                try
                {
                    McuPort.Open();
                    textBox2.Text = "串口已打开";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
            btnOpen.Text = McuPort.IsOpen ? "关闭" : "打开";
            
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            McuPort.Write(textBox1.Text);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e) 
        {   
            if (e.KeyCode == Keys.Enter)   
            {
                this.BtnSend_Click(sender, e);//触发按钮事件
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = null;
        }
    }
}
