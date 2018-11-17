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


        static int sleepPageLoading = 6000;
        static int sleep2000 = 2000;
        static int sleepInput = 200;
        int sleepBeforeKeyPress = sleepInput;
        int sleepAfterKeyPress = sleepInput;
        int sleepBeforeClick = sleepInput;
        int sleepAfterClick = sleepInput;

        Random rand = new Random(1);
        int licz = 0;

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


        public Form1()
        {
            InitializeComponent();
            listInit();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                int i = 0;
                mouseMoveAndClick(positions[i]);    //licznik jakby bylo wiecej .heh
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
                Thread.Sleep(sleep2000);
                mouseMoveAndClick(positions[i]);

                licz += 1;
                button2.Text = licz.ToString();

                if (Control.IsKeyLocked(Keys.CapsLock))
                    this.TopMost = true;
                else
                    Thread.Sleep(sleepPageLoading);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
                button2.BackColor = Color.FromArgb(rand.Next());
        }
    }
}
