using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CIFX_50RE;


namespace 모터돌리기
{
    public partial class Form1: Form
    {
        byte[] Writedata = new byte[8];
        byte[] Readdata = new byte[34];
        private string ReadDataConv = "00000000";
        private string WritedataConv = "00000000";
        int mode = 0;
        int count = 0;

        int counting = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uint connect = CIFX.DriveConnect();
            if (connect != 0)//ECC Module와 연결 성공
            {
                button1.Text = "Communication OK";
                button1.BackColor = Color.Pink;
                timer1.Interval = 20;
                timer1.Start();
                timer2.Interval = 20;
                timer2.Start();
            }
            else//ECC Module와 연결 실패
            {
                button1.Text = "Communication NG";
                button1.BackColor = Color.White;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (button1.Text == "Communication OK")
            {
                Readdata = CIFX.xChannelRead();
                ReadDataConv = Convert.ToString(Readdata[18], 2).PadLeft(8, '0');
                label5.Text = ReadDataConv;//입력 값 표시
                WritedataConv = Convert.ToString(Writedata[0], 2).PadLeft(8, '0');
                label6.Text = WritedataConv;//출력 값 표시
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mode == 0) mode = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mode == 0) mode = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mode = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (mode == 0)
            {
                count = 0;
                label8.Text = count.ToString();
            }
            else
            {
                MessageBox.Show("모터가 회전 중입니다. 정지 후 클리어 진행하세요.");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label8.Text = count.ToString();
            counting = Convert.ToInt32(numericUpDown1.Value);
            if (mode == 1)//정회전
            {
                //Writedata[0] |= 0x40;
                //Writedata[0] &= unchecked((byte)~0x80);
                //CIFX.xChannelWrite(Writedata);
                //if ((Writedata[0] == 0x40 || Writedata[0] == 0x80) && ReadDataConv[1] == '1') count++;
                //if (count > counting) mode = 0;// 카운트가 설정값을 초과하면 정지
                Writedata[0] |= 0x40;
                Writedata[0] &= unchecked((byte)~0x80);
                CIFX.xChannelWrite(Writedata);

                if ((Readdata[18] & 0x02) != 0) count++; // 센서 조건 간단히 체크

                if (count >= counting) mode = 0;

            }
            else if (mode == 2)//역회전
            {
                Writedata[0] |= 0x80;
                Writedata[0] &= unchecked((byte)~0x40);
                CIFX.xChannelWrite(Writedata);

                if ((Readdata[18] & 0x02) != 0) count--;

                if (counting <= 0) mode = 0;

                //Writedata[0] |= 0x80;
                //Writedata[0] &= unchecked((byte)~0x40);
                //CIFX.xChannelWrite(Writedata);
                //if (ReadDataConv[1] == '1') count--;
                //if (count <= 0) mode = 0;// 0보다 작을 경우 정지
            }
            else
            {
                mode = 0;
                Writedata[0] = 0x00;
                CIFX.xChannelWrite(Writedata);
            }
        }
    }
}
