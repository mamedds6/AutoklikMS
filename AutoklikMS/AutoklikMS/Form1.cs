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


        //misc

        bool enabled = false;
        private Point pos0;
        private Point pos1;

        List<Point> positions = new List<Point>();


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


        static int sleepPageLoading = 7000;
        static int sleepInput = 1000;
        int sleepBeforeKeyPress = sleepInput;
        int sleepAfterKeyPress = sleepInput;
        int sleepBeforeClick = sleepInput;
        int sleepAfterClick = sleepInput;

        Random rand = new Random(1);
        int licz = 0;

        /// <summary>
        /// ops
        /// </summary>
        private void pressEnd()
        {
            Thread.Sleep(sleepBeforeKeyPress);
            keybd_event(VK_END, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_END, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(sleepAfterKeyPress);
        }

        private void pressPageUp()
        {
            Thread.Sleep(sleepBeforeKeyPress);
            keybd_event(VK_PAGEUP, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_PAGEUP, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(sleepAfterKeyPress);
        }

        private void mouseMoveAndClick(Point p)
        {
            SetCursorPos(p.X, p.Y);
            Thread.Sleep(sleepBeforeClick);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(sleepAfterClick);
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

        private void button5_Click(object sender, EventArgs e)
        {
            if(!enabled)
            {
                int i = 0;
                mouseMoveAndClick(positions[i]);    //licznik jakby bylo wiecej .heh
                i++;
                Thread.Sleep(sleepPageLoading);
                mouseMoveAndClick(positions[i]);    
                i++;
                Thread.Sleep(sleepPageLoading);
                pressEnd();
                pressPageUp();
                pressPageUp();
                while(i<positions.Count-1)
                {
                    mouseMoveAndClick(positions[i]);
                    i++;
                }

                pressEnd();
                mouseMoveAndClick(positions[i]);
                Thread.Sleep(sleepPageLoading); // ????
                label2.BackColor = Color.Red;


            }
            else 
            {
                
            }

        }

        public Form1()
        {
            InitializeComponent();

            listInit();

            timer1.Interval = 1000;
            timer1.Start();
            
            pos1 = new Point(30,200);
            pos0 = new Point(0, 0);

            SetCursorPos(200, 20);            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pos0.Y = Cursor.Position.Y;
            pos0.X = Cursor.Position.X;
            label1.Text = pos0.X.ToString() + " " + pos0.Y.ToString();

            if (!this.Focused) //flaga checkbox
            {
                //this.Activate();
                //this.TopMost = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var action = button2.Location;
            action.X += 3;
            action.Y += 3;

            //SetCursorPos(action.X, action.Y);
            SetCursorPos(pos1.X, pos1.Y);

            for(int i = 100; i<510; i+=100 )
            {
                SetCursorPos(pos1.X+i, pos1.Y+i);
                Thread.Sleep(sleepBeforeClick);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(sleepAfterClick);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //button2.BackColor = Color.Red;
            //SetCursorPos(200, 20);
            if (Control.IsKeyLocked(Keys.CapsLock))
                button1.BackColor = Color.FromArgb(rand.Next());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {

            //button1.BackColor = Color.FromArgb(rand.Next());
            //licz++;
            //label3.Text = licz.ToString();
            //textBox1.AppendText(pos0.X.ToString()+" \n"+pos0.Y.ToString()+"\n\n");

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }


    }
}
