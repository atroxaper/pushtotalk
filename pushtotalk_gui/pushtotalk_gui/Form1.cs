using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace pushtotalk_gui
{
    public partial class Form1 : HotKeyForm
    {
        [System.Runtime.InteropServices.DllImport("pushtotalk.dll", EntryPoint = "GetMuteStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool pushtotalk_GetMuteStatus(int Index);

        [System.Runtime.InteropServices.DllImport("pushtotalk.dll", EntryPoint = "MuteMic", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pushtotalk_MuteMic(int Index);

        [System.Runtime.InteropServices.DllImport("pushtotalk.dll", EntryPoint = "UnMuteMic", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pushtotalk_UnMuteMic(int Index);

        [System.Runtime.InteropServices.DllImport("pushtotalk.dll", EntryPoint = "FindFirstCaptureDevice", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool pushtotalk_FindFirstCaptureDevice(StringBuilder FirstCaptureDevice, uint FirstCaptureDeviceLen);

        [System.Runtime.InteropServices.DllImport("pushtotalk.dll",  EntryPoint = "FindNextCaptureDevice", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool pushtotalk_FindNextCaptureDevice(StringBuilder CaptureDevice, uint CaptureDeviceLen);
        
        string VERSION = "Hardings Global Push-To-Talk v0.9.3";
        string CODER_EMAIL = "globalpushtotalk@hardingonline.se";
        string HOMEPAGE_LINK = "http://hardingonline.se/hgptt";
        string LAST_UPDATED = "2012-12-06";
        List<string> ConfigPathList;
        uint TheHotKey;
        string ConfigPathHotkey;
        string ConfigPathLastUsedSoundcard;
        List<bool> DefaultStatesOfMics = new List<bool>();

        #region Init

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = VERSION;

            try
            {
                uint MAX_STRING_LENGTH = 255;
                StringBuilder bs = new StringBuilder(Convert.ToInt32(MAX_STRING_LENGTH));
                pushtotalk_FindFirstCaptureDevice(bs, MAX_STRING_LENGTH);
                CaptureDevice_cBox.Items.Clear();
                CaptureDevice_cBox.Items.Add(bs.ToString());
                while (pushtotalk_FindNextCaptureDevice(bs, MAX_STRING_LENGTH))
                    CaptureDevice_cBox.Items.Add(bs.ToString());

                // Save the default state the mics where in before we started to fool around
                for (int i = 0; i < CaptureDevice_cBox.Items.Count; i++)
                    DefaultStatesOfMics.Add(pushtotalk_GetMuteStatus(i));
                
                // Removed as of 2012-01-06 since we save the last used soundcard in the config file /Harding 
                /*
                // Select the first one
                if (CaptureDevice_cBox.Items.Count > 0)
                {
                    CaptureDevice_cBox.SelectedIndex = 0;
                    CaptureDevice_cBox_SelectedIndexChanged(null, null); // Simulate the user chosing the first mic as default
                }
                */

            }
            catch (Exception exc)
            {
                if (exc.Message.Substring(0, 18) == "Unable to load DLL")
                {
                    MessageBox.Show("Could not find pushtotalk.dll, please make sure this is in the same directory as the program", "Missing DLL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Unknown error, please send an email to " + CODER_EMAIL + " and describe this problem.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
            }

            #region Setup the config path

            ConfigPathList = new List<string>();
            ConfigPathList.Add("Harding");
            ConfigPathList.Add("PushToTalk");

            ConfigPathHotkey = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            foreach (string dir in ConfigPathList)
            {
                ConfigPathHotkey += "\\" + dir;
                if (!Directory.Exists(ConfigPathHotkey))
                    Directory.CreateDirectory(ConfigPathHotkey);
            }
            ConfigPathList = null;
            ConfigPathLastUsedSoundcard = ConfigPathHotkey + "\\lastsndcard.dat";
            ConfigPathHotkey += "\\hotkey.dat";

            #region Read hotkey config
            // hotkey.dat format is 4 bytes HotKey as Int, 1 byte length of friendly name, friendly name
            if (File.Exists(ConfigPathHotkey))
            {
                using (FileStream s = new FileStream(ConfigPathHotkey, FileMode.Open, FileAccess.Read)) // This path always exists since the FormLoad() makes that sure
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        TheHotKey = br.ReadUInt32();
                        HotKey_btn.Text = new string(br.ReadChars(100));
                        HotKey_btn.Text = HotKey_btn.Text.Substring(1);
                        br.Close();
                    }
                }
                RegisterHotKey(KeyModifiers.None, TheHotKey);

            }
            #endregion

            #region Read last used soundcard config
            // lastsndcard.dat format is 1 byte length of friendly name, friendly name
            if (File.Exists(ConfigPathLastUsedSoundcard))
            {
                string lastusedsndcard = "";
                using (FileStream s = new FileStream(ConfigPathLastUsedSoundcard, FileMode.Open, FileAccess.Read)) // This path always exists since the FormLoad() makes that sure
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        lastusedsndcard = new string(br.ReadChars(1024)).Substring(1); // The first char is the length of the string
                        br.Close();
                    }
                }
                for (int i = 0; i < CaptureDevice_cBox.Items.Count; i++)
                {
                    string a = CaptureDevice_cBox.Items[i].ToString();
                    if (CaptureDevice_cBox.Items[i].ToString() == lastusedsndcard)
                    {
                        CaptureDevice_cBox.SelectedIndex = i;
                        break;
                    }

                }
            }
            #endregion

            #endregion
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
                this.Hide();
        }
        
        #endregion

        protected override void OnHotKeyPress(object sender, EventArgs e)
        {
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;
            try
            {
                pushtotalk_UnMuteMic(CaptureDevice_cBox.SelectedIndex);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.Message);
            }
            
            Mute_tmr.Stop();
            Mute_tmr.Start();
        }

        #region Notify icon

        private void MyNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNotifyIcon_DoubleClick(null, null);
        }

        /// <summary>
        /// Shows infomation about the program
        /// </summary>
        private void ShowAboutInfo()
        {
            string info = VERSION + " written by Harding (" + CODER_EMAIL + ")\r\n";
            info += "Last updated " + LAST_UPDATED + ".\r\n";
            info += "Homepage: " + HOMEPAGE_LINK;
            
            MessageBox.Show(info, "About " + VERSION, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutInfo();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Set new hotkey

        private void HotKey_btn_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.DialogResult.OK == MessageBox.Show("Click OK and then press the key you want as hotkey", "Set new hotkey", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
            {
                UnregisterHotKey();
                this.HotKey_btn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKey_btn_KeyDown);
            }
            else
                CaptureDevice_cBox.Focus();

        }

        /// <summary>
        /// Used to get hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotKey_btn_KeyDown(object sender, KeyEventArgs e)
        {
            this.HotKey_btn.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.HotKey_btn_KeyDown);
            TheHotKey = Convert.ToUInt32(e.KeyValue);
            HotKey_btn.Text = System.Enum.GetName(typeof(Keys), e.KeyCode);
            CaptureDevice_cBox.Focus();

            using (FileStream s = new FileStream(ConfigPathHotkey, FileMode.Create, FileAccess.Write)) // This path always exists since the FormLoad() makes that sure
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    bw.Write(TheHotKey);
                    bw.Write(HotKey_btn.Text);
                    bw.Close();
                }
            }
            RegisterHotKey(KeyModifiers.None, TheHotKey);
        }
        #endregion

        private void Mute_tmr_Tick(object sender, EventArgs e)
        {
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;
            try
            {
                pushtotalk_MuteMic(CaptureDevice_cBox.SelectedIndex);
                Mute_tmr.Stop();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.Message);
            }
        }

        private void SetMicsToDefaultState()
        {
            for (int i = 0; i < CaptureDevice_cBox.Items.Count; i++)
                if (DefaultStatesOfMics[i])
                    pushtotalk_MuteMic(i);
                else
                    pushtotalk_UnMuteMic(i);
        }
        /// <summary>
        /// Restore the mics to their original state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetMicsToDefaultState();
        }
        
        private void CaptureDevice_cBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMicsToDefaultState();
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;
            
            #region Save the select card as last used for next time we start the program
            using (FileStream s = new FileStream(ConfigPathLastUsedSoundcard, FileMode.Create, FileAccess.Write)) // This path always exists since the FormLoad() makes that sure
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    bw.Write(CaptureDevice_cBox.Items[CaptureDevice_cBox.SelectedIndex].ToString());
                    bw.Close();
                }
            }
            #endregion

            pushtotalk_MuteMic(CaptureDevice_cBox.SelectedIndex);

        }

        private void About_btn_Click(object sender, EventArgs e)
        {
            ShowAboutInfo();
        }
    }
}
