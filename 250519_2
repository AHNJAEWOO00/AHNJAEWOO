using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CIFX_50RE;

namespace _250519_2
{
    public partial class Form1: Form
    {
        int currentTargetCount = 0;
        int currentCount = 0;
        bool prevSensorState = false;
        byte[] Writedata = new byte[8];
        byte[] Readdata = new byte[34];
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           
            bool sensorTriggered = (Readdata[18] & 0x02) != 0;
            label5.Text = sensorTriggered ? "센서 ON" : "센서 OFF";
            btn_connect.Text = "Communication NG";
            btn_connect.BackColor = Color.White;

            uint connect = CIFX.DriveConnect();
            if (connect != 0)
            {
                btn_connect.Text = "Communication OK";
                btn_connect.BackColor = Color.Pink;
                timer1.Interval = 50;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Readdata = CIFX.xChannelRead();
            lbl_18input.Text = Convert.ToString(Readdata[18], 2).PadLeft(8, '0');
            lbl_19input.Text = Convert.ToString(Readdata[19], 2).PadLeft(8, '0');
            lbl_output.Text = Convert.ToString(Writedata[1], 2).PadLeft(8, '0');
            lbl_18input.Text = $"HEX: {Readdata[18]:X2}, BIN: {Convert.ToString(Readdata[18], 2).PadLeft(8, '0')}";
            bool sensorTriggered = (Readdata[18] & 0x40) != 0;
            lbl_output.Text = sensorTriggered ? "센서 ON" : "센서 OFF"; // 입력 0x40 센서
            if (!this.DesignMode)
            {
                if (sensorTriggered && !prevSensorState && (Writedata[0] & 0xC0) != 0)
                {
                    currentCount++;
                    numbercount.Value = currentCount;
                    label7.Text = currentCount.ToString();
                    

                    if ((Writedata[0] & 0x40) != 0 && currentCount >= currentTargetCount)
                        btn_stop_Click(null, null);
                    else if ((Writedata[0] & 0x80) != 0 && currentCount >= currentTargetCount)
                        btn_stop_Click(null, null);
                }
                prevSensorState = sensorTriggered;
            }
            Readdata = CIFX.xChannelRead();
            lbl_18input.Text = Convert.ToString(Readdata[18], 2).PadLeft(8, '0');
            lbl_19input.Text = Convert.ToString(Readdata[19], 2).PadLeft(8, '0');
            lbl_output.Text = Convert.ToString(Writedata[1], 2).PadLeft(8, '0');
        }

        private void btn_manucw_Click(object sender, EventArgs e)
        {
            if ((Writedata[0] & 0x40) == 0 && numbercount.Value < numbercount.Maximum)
            {
                currentTargetCount = (int)numbercount.Value; // 목표 회전 수 저장
                currentCount = 0; // 현재 회전 수 초기화
                Writedata[0] |= 0x40; // CW ON
                Writedata[0] &= unchecked((byte)~0x80); // CCW OFF
                CIFX.xChannelWrite(Writedata);
            }
            label7.Text = currentCount.ToString();
        }

        private void btn_manuccw_Click(object sender, EventArgs e)
        {
            if ((Writedata[0] & 0x80) == 0 && numbercount.Value > numbercount.Minimum)
            {
                currentTargetCount = (int)numbercount.Value;
                currentCount = 0;
                Writedata[0] |= 0x80; // CCW ON
                Writedata[0] &= unchecked((byte)~0x40); // CW OFF
                CIFX.xChannelWrite(Writedata);
            }
            label7.Text = currentCount.ToString();
        }
        private void btn_clear_Click(object sender, EventArgs e)
        {
            Writedata[0] &= unchecked((byte)~0x40); // CW OFF
            Writedata[0] &= unchecked((byte)~0x80); // CCW OFF
            CIFX.xChannelWrite(Writedata);
        }

        private void btn_auto_Click(object sender, EventArgs e)
        {

        }

        private void btn_cclear_Click(object sender, EventArgs e)
        {
            if ((Writedata[0] & 0xC0) != 0) // 모터 회전 중
            {
                MessageBox.Show("모터가 회전 중입니다. 정지 후 클리어 진행하세요.");
                return;
            }

            numbercount.Value = 0;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Writedata[0] &= unchecked((byte)~0x40); // CW OFF
            Writedata[0] &= unchecked((byte)~0x80); // CCW OFF
            CIFX.xChannelWrite(Writedata);
        }

        private void numbercount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
