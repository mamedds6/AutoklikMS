using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Timers;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;

namespace AutoklikMS
{
    public partial class Form1 : Form
    {
        #region //mouse and keyboard [DllImport} stuff
        //mouse
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        // [DllImport("user32.dll")]
        // public static extern bool GetCursorPos(out POINT lpPoint);
        //https://stackoverflow.com/questions/1316681/getting-mouse-position-in-c-sharp

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //keys
        // Import the user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        // Declare some keyboard keys as constants with its respective code
        // See Virtual Code Keys: https://msdn.microsoft.com/en-us/library/dd375731(v=vs.85).aspx
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_RCONTROL = 0xA3; //Right Control key code
        public const int VK_END = 0x23; //end
        public const int VK_PAGEUP = 0x21; //page up
        #endregion

        #region //misc        
        static List<Point> positions = new List<Point>();

        static int xInSzablon = 250;
        static int smth = 0;
        Point pWyborPierwszegoKolpaka = new Point(400, 715);
        Point pSzablonKlikNaSrodku = new Point(900, 500);
        Point pSzablonGwarancja = new Point(xInSzablon, 665 + smth);
        Point pSzablonGwarancjaWybor = new Point(xInSzablon, 715 + smth);
        Point pSzablonReklamacje = new Point(xInSzablon, 565 + smth);
        Point pSzablonReklamacjeWybor = new Point(xInSzablon, 615 + smth);
        Point pSzablonZwroty = new Point(xInSzablon, 470 + smth);
        Point pSzablonZwrotyWybor = new Point(xInSzablon, 520 + smth);
        Point pSzablonKlikNaSrodku2 = new Point(900, 500);
        Point pSzablonZakoncz = new Point(678, 569);

        static int sleepPageLoading;
        static int sleepMid;
        static int sleepInput;

        static Random rand = new Random(1);
        static int licz = 0;
        #endregion

        private void pressEnd()
        {
            Thread.Sleep(sleepInput);
            keybd_event(VK_END, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_END, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(sleepInput);
        }

        private void pressPageUp()
        {
            Thread.Sleep(sleepInput);
            keybd_event(VK_PAGEUP, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_PAGEUP, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(sleepInput);
        }

        private void mouseMoveAndClick(Point p)
        {
            SetCursorPos(p.X, p.Y);
            Thread.Sleep(sleepInput);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(sleepInput);
        }

        private void listInit()
        {
            positions.Add(pWyborPierwszegoKolpaka);
            positions.Add(pSzablonKlikNaSrodku);
            positions.Add(pSzablonGwarancja);
            positions.Add(pSzablonGwarancjaWybor);
            positions.Add(pSzablonReklamacje);
            positions.Add(pSzablonReklamacjeWybor);
            positions.Add(pSzablonZwroty);
            positions.Add(pSzablonZwrotyWybor);
            positions.Add(pSzablonKlikNaSrodku2);
            positions.Add(pSzablonZakoncz);
        }

        public Form1()
        {
            InitializeComponent();

            listInit();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerSupportsCancellation = true; // ?
            backgroundWorker2.RunWorkerAsync();

            this.StartPosition = FormStartPosition.Manual;   //position set in form1 design  
            button1.Visible = false;    //not needed atm
        }

        private void button1_Click(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();            
        }

        private void workLoop()
        {
            int i = 0;
            mouseMoveAndClick(positions[i]);
            i++;
            Thread.Sleep(sleepPageLoading);
            mouseMoveAndClick(positions[i]);
            i++;
            pressEnd();
            pressPageUp();
            pressPageUp();
            while (i < positions.Count - 1)
            {
                mouseMoveAndClick(positions[i]);
                i++;
            }
            pressEnd();
            Thread.Sleep(sleepMid);
            mouseMoveAndClick(positions[i]);            
        }

        private void workLoop2()
        {
            mouseMoveAndClick(positions[5]);
            pressPageUp();
            Thread.Sleep(2000);
            mouseMoveAndClick(positions[6]);
        }

        private void workerStarting()
        {
            button2.BackColor = Color.LightGreen;
            button2.Enabled = false;
            numericUpDown1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = false;

            sleepInput = int.Parse(textBox1.Text);
            sleepMid = int.Parse(textBox2.Text);
            sleepPageLoading = int.Parse(textBox3.Text);
        }

        private void workerStoping()
        {
            button2.Enabled = true;
            button2.BackColor = Control.DefaultBackColor;
            numericUpDown1.Enabled = true;
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            label1.Invoke(new Action(() => workerStarting()));

            while (licz < numericUpDown1.Value)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //workLoop2();
                workLoop(); 
                licz++;
                label1.Invoke(new Action(() => label1.Text = licz.ToString()));

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(sleepPageLoading); //sleep needed because of szablon->mainpage
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (backgroundWorker2.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    backgroundWorker1.CancelAsync();
                    button1.Invoke(new Action(() => button1.Enabled = false));
                    button2.Invoke(new Action(() => button2.Enabled = false));
                }
                else
                {
                    button1.Invoke(new Action(() => button1.Enabled = true));
                    button2.Invoke(new Action(() => button2.Enabled = true));
                }

                Thread.Sleep(200);
                //button2.Invoke(new Action(() => button2.Text = sleepMid.ToString()));
                //button2.BackColor = Color.FromArgb(rand.Next());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Invoke(new Action(() => workerStoping()));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            licz = 0;
            label1.Text = licz.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ActiveForm.TopMost = true;
            else
                ActiveForm.TopMost = false;
        }
    }
}