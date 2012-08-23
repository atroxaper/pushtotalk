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
        
        private const string VERSION = "Hardings Global Push-To-Talk v0.10";
        private const string MAIN_DEVELOPER = "Harding";
        private const string MAIN_DEVELOPER_EMAIL = "globalpushtotalk@hardingonline.se";
        private const string SECOND_DEVELOPER = "atroxaper";
        private const string SECOND_DEVELOPER_EMAIL = "atroxaper@gmail.com";
        private const string HOMEPAGE_LINK = "http://hardingonline.se/hgptt";
        private const string LAST_UPDATED = "2012-08-23";
        private const string AboutInfo = VERSION + "\r\nWritten by " + MAIN_DEVELOPER + " (" + MAIN_DEVELOPER_EMAIL + ")\r\nand " + SECOND_DEVELOPER + " (" + SECOND_DEVELOPER_EMAIL + ")\r\n" +
                                         "Homepage: " + HOMEPAGE_LINK + "\r\nLast updated: " + LAST_UPDATED;

        private const string MUTE_LITERAL = "Muted";
        private const string UNMUTE_LITERAL = "UnMuted";

        List<string> ConfigPathList;
        string ConfigPathHotkey;
        string ConfigPathLastUsedSoundcard;
        string ConfigPathWorkType;

        uint TheHotKey;
        Boolean WorkType = true; // true - PTT; false - Toggle;
        Boolean ToggleState = false; // true - UnMute; false - Mute;

        List<bool> DefaultStatesOfMics = new List<bool>();

        #region Init

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("dll"))
                {
                    MessageBox.Show("Could not find pushtotalk.dll, please make sure this is in the same directory as the program. Error: " + exc.Message, "Missing DLL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Unknown error, please send an email to " + MAIN_DEVELOPER_EMAIL + " or " + SECOND_DEVELOPER_EMAIL + " and describe this problem. Error: " + exc.Message, "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ConfigPathWorkType = ConfigPathHotkey + "\\worktype.dat";
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

            #region Read worktype config
            // worktype.dat format is 1 bytes worktype as Boolen (1 - PTT; 0 - Toggle)
            if (File.Exists(ConfigPathWorkType))
            {
                using (FileStream s = new FileStream(ConfigPathWorkType, FileMode.Open, FileAccess.Read)) // This path always exists since the FormLoad() makes that sure
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        WorkType = br.ReadBoolean();
                        br.Close();
                        if (WorkType)
                        {
                            toggleButton.Checked = false;
                            pttButton.Checked = true;
                        }
                        else
                        {
                            toggleButton.Checked = true;
                            pttButton.Checked = false;
                        }

                    }
                }

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
            MessageBox.Show(AboutInfo, "About " + VERSION, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            RegisterHotKey(KeyModifiers.None, TheHotKey);
        }
        #endregion
        
        void pttButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                return;
            }

            WorkType = !rb.Checked;
            Mute();
        }
        
        protected override void OnHotKeyPress(object sender, EventArgs e)
        {
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;
            if (WorkType) // ptt
            {
                OnPttKeyPress();
            }
            else
            {
                OnToggleKeyPress();
            }
            
        }

        private void OnToggleKeyPress()
        {
            if (ToggleState) // UnMute
            {
                Mute();
            }
            else // Mute
            {
                UnMute();
            }
            ToggleState = !ToggleState;
        }

        private void OnPttKeyPress()
        {
            try
            {
                UnMute();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.Message);
            }

            Mute_tmr.Stop();
            Mute_tmr.Start();
        }

        private void Mute_tmr_Tick(object sender, EventArgs e)
        {
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;
            try
            {
                Mute();
                Mute_tmr.Stop();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.Message);
            }
        }

        private void CaptureDevice_cBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (-1 == CaptureDevice_cBox.SelectedIndex)
                return;

            SetMicsToDefaultState();

            Mute();

        }

        private void About_btn_Click(object sender, EventArgs e)
        {
            ShowAboutInfo();
        }

        /// <summary>
        /// Restore the mics to their original state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetMicsToDefaultState();
            SaveHotkey();
            SaveSndCard();
            SaveWorkType();
        }

        #region Save state for next start application
        private void SetMicsToDefaultState()
        {
            for (int i = 0; i < CaptureDevice_cBox.Items.Count; i++)
                if (DefaultStatesOfMics[i])
                    pushtotalk_MuteMic(i);
                else
                    pushtotalk_UnMuteMic(i);
        }

        private void SaveHotkey()
        {
            using (FileStream s = new FileStream(ConfigPathHotkey, FileMode.Create, FileAccess.Write)) // This path always exists since the FormLoad() makes that sure
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    bw.Write(TheHotKey);
                    bw.Write(HotKey_btn.Text);
                    bw.Close();
                }
            }
        }

        private void SaveSndCard()
        {
            using (FileStream s = new FileStream(ConfigPathLastUsedSoundcard, FileMode.Create, FileAccess.Write)) // This path always exists since the FormLoad() makes that sure
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    bw.Write(CaptureDevice_cBox.Items[CaptureDevice_cBox.SelectedIndex].ToString());
                    bw.Close();
                }
            }
        }

        private void SaveWorkType()
        {
            using (FileStream s = new FileStream(ConfigPathWorkType, FileMode.Create, FileAccess.Write)) // This path always exists since the FormLoad() makes that sure
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    bw.Write(WorkType);
                    bw.Close();
                }
            }
        }
        #endregion

        private void Mute()
        {
            pushtotalk_MuteMic(CaptureDevice_cBox.SelectedIndex);
            this.DeviceStateValue_lbl.Text = MUTE_LITERAL;
        }

        private void UnMute()
        {
            pushtotalk_UnMuteMic(CaptureDevice_cBox.SelectedIndex);
            this.DeviceStateValue_lbl.Text = UNMUTE_LITERAL;
        }
    }
}
