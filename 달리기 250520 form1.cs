using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 씨샵수업
{
    public partial class Form1: Form
    {
        private enum Player
        {
            루피,
            조로,
            상디,
            나미,
            우솝
        }

        int locationX = 0;
        int locationY = 0;
        
        List<play> lPlay = new List<play>();


        public Form1()
        {
            InitializeComponent();
            locationX = this.Location.X;
            locationY = this.Location.Y;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nud_player.Minimum = 1;
            nud_player.Maximum = 5;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(play py in lPlay)
            {
                py.ThreadAbort(); // 리스트 안에 있는 객체가 가지고 있는 쓰레드를 강제 종료
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseAllPlayers();

            locationX = this.Location.X + this.Size.Width;
            locationY = this.Location.Y;

            for (int i = 0; i < nud_player.Value; i++)
            {
                play py = new play(((Player)i).ToString());
                py.Location = new Point(locationX, locationY + py.Height * i);
                py.eventDelmsg += Py_eventDelMsg;
                py.Show();
                py.ThreadStart();
                lPlay.Add(py);
            }
        }
        

        private int Py_eventDelMsg(object sender, string strResult)
        {
            if(this.InvokeRequired)
            {//요청한 스레드가 현재 메인 스레드에 있는 Control을 엑세스 할 수 있는지 확인
                this.Invoke(new Action(delegate ()
                {
                    play py = sender as play;
                    lb_result.Items.Add($"Player : {py.SPlayerName}, {strResult}");
                }
                ));
            }
            return 0;
        }
        private void CloseAllPlayers()
        {
            foreach (play py in lPlay)
            {
                py.ThreadAbort();
                py.Close();
            }
            lPlay.Clear();
        }
        private void btn_quit_Click(object sender, EventArgs e)
        {
            CloseAllPlayers();
        }
    }
}
