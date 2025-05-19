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



namespace _250519
{
    public partial class Form1 : Form
    {
        byte[] Writedata = new byte[8];
        byte[] Readdata = new byte[34];
        string ReadDataConv = "00000000";
        int mode = 0;
        int step = 0;
        int count = 0;
        int counting = 0;
        int sequence = 0;
        bool isAuto = false;
        bool isStep = false;




        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Value = 4;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_ethercat_okng.Text = "NG";


            uint connect = CIFX.DriveConnect();
            if (connect != 0)
            {
                lbl_ethercat_okng.Text = "OK";


                timer1.Interval = 1000;
                timer1.Start();
                timer2.Interval = 1000;
                timer2.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Readdata = CIFX.xChannelRead();
            ReadDataConv = Convert.ToString(Readdata[18], 2).PadLeft(8, '0');
            lbl_동작횟수_num.Text = isAuto ? count.ToString() : "";
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            if (!isAuto && !isStep)
            {
                timer2.Start();
                return;
            }

            counting = Convert.ToInt32(numericUpDown1.Value);
            lbl_step_num.Text = step.ToString();

            switch (step)
            {
                case 0:
                    Writedata[0] |= 0x01; // A 전진 ON
                    Writedata[0] &= unchecked((byte)~0x02); // A 후진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 1:
                    Writedata[0] |= 0x02; // A 후진 ON
                    Writedata[0] &= unchecked((byte)~0x01); // A 전진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 2:
                    Writedata[0] |= 0x40; // DC 모터 CW
                    Writedata[0] &= unchecked((byte)~0x80); // DC 모터 CCW OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(2000);
                    Writedata[0] &= unchecked((byte)~0x40); // CW OFF
                    CIFX.xChannelWrite(Writedata);
                    step++;
                    timer2.Start();
                    break;
                case 3:
                    // 모터 회전 후에 B 전진
                    await Task.Delay(200);
                    Writedata[0] |= 0x04; // B 전진 ON
                    Writedata[0] &= unchecked((byte)~0x08); // B 후진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 4:
                    // B 전진 후에 C 전진
                    await Task.Delay(200);
                    Writedata[0] |= 0x10; // C 전진 ON
                    Writedata[0] &= unchecked((byte)~0x20); // C 후진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 5:
                    // C 전진 후에 B 후진
                    await Task.Delay(200);
                    Writedata[0] |= 0x08; // B 후진 ON
                    Writedata[0] &= unchecked((byte)~0x04); // B 전진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 6:
                    Writedata[0] |= 0x80; // DC 모터 CCW ON
                    Writedata[0] &= unchecked((byte)~0x40); // CW OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(3000);
                    Writedata[0] &= unchecked((byte)~0x80); // CCW OFF
                    CIFX.xChannelWrite(Writedata);
                    step++;
                    timer2.Start();
                    break;
                case 7:
                    // 모터 역회전 후 C 후진
                    await Task.Delay(200);
                    Writedata[0] |= 0x20; // C 후진 ON
                    Writedata[0] &= unchecked((byte)~0x10); // C 전진 OFF
                    CIFX.xChannelWrite(Writedata);
                    await Task.Delay(1000);
                    step++;
                    timer2.Start();
                    break;
                case 8:
                    count++;
                    if (isStep)
                        isStep = false;
                    else if (count >= counting)
                    {
                        isAuto = false;
                        step = 0;
                        timer2.Start();
                        return;
                    }
                    step = 0;
                    timer2.Start();
                    break;
            }
        }
        
        


        private void btn_auto_Click(object sender, EventArgs e)
        {
            isAuto = true;
            step = 0;
            count = 0;
        }

        private void btn_stepctrl_Click(object sender, EventArgs e)
        {
            isStep = true;
            isAuto = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            isAuto = false;
            isStep = false;
            step = 0;
            count = 0;
            Writedata[0] = 0x00;
            CIFX.xChannelWrite(Writedata);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (!isAuto && !isStep)
            {
                count = 0;
                lbl_동작횟수_num.Text = "0";

                // 모든 실린더 후진: A-, B-, C-
                Writedata[0] = 0x02 | 0x08 | 0x20;
                CIFX.xChannelWrite(Writedata);
            }
        }
    }
}
