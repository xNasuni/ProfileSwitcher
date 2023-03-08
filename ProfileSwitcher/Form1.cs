using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ProfileSwitcher.Utilities;

namespace ProfileSwitcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Button1_Click(null, null);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Panel.Controls.Clear();

            List<Button> Buttons = new List<Button>();
            foreach (string ProfileID in Functions.OperaGX.GetProfiles(false, true))
            {
                Image ProfileIconData;
                ProfileIconData = Functions.OperaGX.GetProfileIcon(ProfileID);
                Bitmap ProfileIcon = new Bitmap(ProfileIconData, new Size(16, 16));
                string ProfileName = Functions.OperaGX.GetProfileName(ProfileID);

                Button Button = new Button
                {
                    Size = new Size((ProfileName.Length * 10) + ProfileIcon.Width * 2, 30),
                    Image = ProfileIcon,
                    ImageAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(29, 29, 29),
                    Tag = ProfileID, // use for later
                    Font = new Font("Segoe UI", 11.25F), // segoe ui comes with every windows 7+ i think so im gonna use that just incase (correct me if im wrong)
                    Text = ProfileName,
                    TextAlign = ContentAlignment.MiddleRight
                };

                if (Functions.OperaGX.GetCurrentStatus().Equals(ProfileID))
                {
                    Button.FlatAppearance.BorderSize = 1;
                    Button.FlatAppearance.BorderColor = Color.White;
                }
                else
                {
                    Button.FlatAppearance.BorderSize = 0;
                }

                Button.Click += delegate
                {
                    Functions.OperaGX.SetDefaultProfile(ProfileID);
                    foreach (Button Button2 in Buttons)
                    {
                        if (Functions.OperaGX.GetCurrentStatus().Equals(Button2.Tag)) // here is the "use for later"
                        {
                            Button2.FlatAppearance.BorderSize = 1;
                            Button2.FlatAppearance.BorderColor = Color.White;
                        }
                        else
                        {
                            Button2.FlatAppearance.BorderSize = 0;
                        }
                    }
                };

                Buttons.Add(Button); // add em to an array so we can loop through em later
            }


            foreach (Button Button in Buttons)
            {
                Panel.Controls.Add(Button); // this is the later
                /*
                 * as you can tell i suck at writing docs or anything like that
                 * i don't really know, this is just a program that i wrote
                 */
            }
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // the code below has probably been pasted millions of times
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
