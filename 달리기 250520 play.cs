using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 씨샵수업
{
    public partial class play: Form
    {
        #region 전역함수
        public delegate int delMessage(object sender, string strResult);//delegate 선언
        public event delMessage eventDelmsg;//이벤트 선언

        string sPlayerName = string.Empty;//플레이어 이름
        public string SPlayerName { get => sPlayerName; set => sPlayerName = value; }
        Thread thread = null;//스레드 선언
        bool bThreadStop = false; //스레스 스탑을 위한 flag
        #endregion

        public play()
        {
            InitializeComponent();
        }
        public play(string strPName)
        {
            InitializeComponent();
            lbl_name.Text = SPlayerName = strPName;
        }

        #region 메서드(함수)
        //void 메서드명() > 반환 없고 매개변수 없음
        public void ThreadStart()
        {
            thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            // UI Control이 자신이 만들어진 스레드가 아닌 다른 스레드에서 접근할 경우 Cross Thread Exception 발생
            try
            {
                int iVar = 0;  //랜덤으로 만들어지는 수를 받는 변수
                Random rd = new Random(); //랜덤으로 난수를 만들어줄 객체 
                
                while (pgb_player.Value < 100 && !bThreadStop)
                {// 프로그레스 값이 100보다 작고 bThreadStop이 false일 때까지 반복
                    if (this.InvokeRequired)
                    {//요청한 스레드가 현재 main Thread가 있는 Control을 엑세스 할 수 있는지 확인
                        this.Invoke(new Action(delegate ()
                        {
                            iVar = rd.Next(1, 11);// 1~10까지의 난수 생성
                            if (pgb_player.Value + iVar > 100)// 현재 프로그래스바 숫자+ 받아온 숫자가 100보다 크면
                            {
                                pgb_player.Value = 100;
                            }
                            else
                            {
                                pgb_player.Value = pgb_player.Value + iVar;
                            }

                            lbl_process.Text = $"진행 상황 표시 : {pgb_player.Value}%";
                            
                            
                            this.Refresh();
                        }));
                        Thread.Sleep(50);
                    }

                }

                if(bThreadStop)//스레드가 멈추었을 때 포기버튼
                {
                    eventDelmsg(this, "중도포기...(Thread Stop)"); 
                }
                else
                {
                    eventDelmsg(this, "완주!!(Thread Complete!");
                }
            }
            catch (ThreadInterruptedException exinterrupt)
            {
                exinterrupt.ToString();
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        public void ThreadAbort()
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }
        public void ThreadJoin()
        {
            if (thread.IsAlive)
            {
                bool bThreadEnd = thread.Join(3000);//스레드 호출 3초간 차단
            }
        }
        #endregion

        #region 이벤트
        private void btn_stop_Click(object sender, EventArgs e)
        {
            if(thread.IsAlive)
            {
                bThreadStop = true;
            }
        }
        #endregion
    }
}
