using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app {
    public partial class Window : Form {

        // Server-Data
        const string IP = "192.168.2.109";
        const int PORT = 2222;

        // REQUEST-CODES
        const string UI_CLIENT_DATA_REQUEST =           "0000000000000000000000000000000000000000000000000000000000000097";
        const string UI_CLIENT_DEVICES_LOG_REQUEST =    "0000000000000000000000000000000000000000000000000000000000000087";
        const string UI_CLIENT_MOVEMENT_LOG_REQUEST =   "0000000000000000000000000000000000000000000000000000000000000077";
        const string UI_CLIENT_SENSOR_DATA_REQUEST =    "0000000000000000000000000000000000000000000000000000000000000067";
        const string UI_CLIENT_RNN_HABITS_REQUEST =     "0000000000000000000000000000000000000000000000000000000000000057";

        // COMMAND-IDENTIFIER
        const string UI_CLIENT_COMMAND_IDENTIFIER = "CMD_";
        const string UI_CLIENT_SYSTEM_COMMAND_IDENTIFIER = "SYS_";
        const string UI_CLIENT_ADD_DEVICE_IDENTIFIER = "ADD_";

        // UPDATER
        string lastAnswer = "";
        bool skipOne = false;

        // CURRENT VIEW
        int viewSelected = -1;

        // 14 COLORS
        Color[] graphColors = {
            Color.FromArgb(52, 189, 157),
            Color.FromArgb(68, 204, 118),
            Color.FromArgb(57, 154, 216),
            Color.FromArgb(152, 89, 178),
            Color.FromArgb(240, 194, 48),
            Color.FromArgb(228, 123, 44),
            Color.FromArgb(228, 70, 62),
            Color.FromArgb(43, 161, 135),
            Color.FromArgb(58, 174, 102),
            Color.FromArgb(45, 130, 183),
            Color.FromArgb(139, 69, 170),
            Color.FromArgb(241, 153, 39),
            Color.FromArgb(207, 79, 13),
            Color.FromArgb(190, 51, 44)
        };
        Random rnd = new Random();

        // PROMPT FOR ADDING DEVICES
        public static class CreateDevicePrompt {
            public static void ShowDialog(Window w) {
                Form prompt = new Form();
                prompt.Width = 220;
                prompt.Height = 200;
                prompt.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                prompt.Icon = Window.ActiveForm.Icon;
                prompt.Text = "Gerät hinzufügen";

                // NAME
                Label lName = new Label() {
                    Location = new Point(15, 15),
                    Size = new Size(65, 15),
                    Text = "Gerätename"
                };
                lName.Show();
                prompt.Controls.Add(lName);

                TextBox tName = new TextBox() {
                    Location = new Point(83, 13),
                };
                tName.Show();
                prompt.Controls.Add(tName);

                // IP
                Label lIP = new Label() {
                    Location = new Point(15, 38),
                    Size = new Size(65, 15),
                    Text = "IP-Addresse"
                };
                lIP.Show();
                prompt.Controls.Add(lIP);

                TextBox tIP = new TextBox() {
                    Location = new Point(83, 36),
                };
                tIP.Show();
                prompt.Controls.Add(tIP);

                // PORT
                Label lPort = new Label() {
                    Location = new Point(15, 61),
                    Size = new Size(65, 15),
                    Text = "Port"
                };
                lPort.Show();
                prompt.Controls.Add(lPort);

                TextBox tPort = new TextBox() {
                    Location = new Point(83, 59),
                };
                tPort.Show();
                prompt.Controls.Add(tPort);

                // STATES
                Label lStates = new Label() {
                    Location = new Point(15, 61+23),
                    Size = new Size(65, 15),
                    Text = "Stadien"
                };
                lStates.Show();
                prompt.Controls.Add(lStates);

                TextBox tStates = new TextBox() {
                    Location = new Point(83, 59+23),
                };
                tStates.Show();
                prompt.Controls.Add(tStates);

                // SEND REQUEST
                Button confirmBtn = new Button() {
                    Text = "Ok", Width = 100, Top = 120, Left = 52
                };
                confirmBtn.Click += (sender, e) => {
                    if (tName.Text != "" && tIP.Text.Length > 5 && tPort.Text != "" && tStates.Text.Length > 3) {
                        String msg = UI_CLIENT_ADD_DEVICE_IDENTIFIER + tName.Text + "_" + tIP.Text + "_" + tPort.Text + "_";
                        String[] states = tStates.Text.Trim().Split(',');
                        String stateCount = states.Length.ToString();
                        msg += stateCount;
                        foreach(String s in states) {
                            msg += "_" + s.ToUpper();
                        }
                        w.send(msg);
                        prompt.Close();
                    } else {
                        MessageBox.Show("Bitte Daten einfügen.");
                    }
                };
                prompt.Controls.Add(confirmBtn);


                prompt.ShowDialog();
            }
        }

        // INIT
        public Window() {
            InitializeComponent();
            loadÜbersicht();
            markPanel(übersichtBtn);
        }

        // MOVEABLE WINDOW
        private bool mouseDown;
        private Point lastLocation;
        private void grabPanel_MouseDown(object sender, MouseEventArgs e) {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void grabPanel_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }
        private void grabPanel_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
        }

        // CLOSEABLE WINDOW
        private void closeBtn_MouseClick(object sender, MouseEventArgs e) {
            this.Close();
        }

        // SIDEBAR-ELEMENTS
        private void übersichtBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(übersichtBtn);
            loadÜbersicht();
        }
        private void montagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(montagBtn);
            loadMontag();
        }
        private void dienstagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(dienstagBtn);
            loadDienstag();
        }
        private void mittwochBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(mittwochBtn);
            loadMittwoch();
        }
        private void donnerstagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(donnerstagBtn);
            loadDonnerstag();
        }
        private void freitagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(freitagBtn);
            loadFreitag();
        }
        private void samstagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(samstagBtn);
            loadSamstag();
        }
        private void sonntagBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(sonntagBtn);
            loadSonntag();
        }
        private void live_kameraBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(live_kameraBtn);
            loadLive_Kamera();
        }
        private void einstellungenBtn_MouseClick(object sender, MouseEventArgs e) {
            markPanel(einstellungenBtn);
            loadEinstellungen();
        }

        // MARKER
        private void markPanel(Panel mPanel) {
            unmarkAll();
            mPanel.BackColor = Color.FromArgb(243, 227, 221);
            Panel p = new Panel();
            p.BackColor = Color.FromArgb(255, 94, 58);
            p.Dock = DockStyle.Left;
            p.Width = 2;
            p.Visible = true;
            p.Show();
            p.Name = mPanel.Name + "_MARKER";
            mPanel.Controls.Add(p);
        }
        private void unmarkPanel(Panel mPanel) {
            mPanel.BackColor = Color.FromArgb(235, 236, 237);
            foreach (Control child in mPanel.Controls) {
                if (child.Name == mPanel.Name + "_MARKER") {
                    int idx = mPanel.Controls.GetChildIndex(child);
                    mPanel.Controls.RemoveAt(idx);
                }
            }
        }
        private void unmarkAll() {
            unmarkPanel(übersichtBtn);
            unmarkPanel(montagBtn);
            unmarkPanel(dienstagBtn);
            unmarkPanel(mittwochBtn);
            unmarkPanel(donnerstagBtn);
            unmarkPanel(freitagBtn);
            unmarkPanel(samstagBtn);
            unmarkPanel(sonntagBtn);
            unmarkPanel(live_kameraBtn);
            unmarkPanel(einstellungenBtn);
        }

        // PRESET-LOADER
        private void clearPanel() {
            contentPanel.Controls.Clear();
        }

        private void loadÜbersicht() {
            if (viewSelected != 0) {
                clearPanel();
                viewSelected = 0;

                Panel panel0 = new Panel();
                panel0.Margin = new Padding(0, 0, 0, 0);
                panel0.Size = new Size(551, 350);
                panel0.Location = new Point(0, 0);
                panel0.BackColor = Color.FromArgb(235, 236, 237);
                panel0.Show();
                contentPanel.Controls.Add(panel0);

                Panel panel1 = new Panel();
                panel1.Name = "übersicht_mainPanel";
                panel1.Margin = new Padding(0, 0, 0, 0);
                panel1.Size = new Size(410, 550);
                panel1.Location = new Point(565, 0);
                panel1.BackColor = Color.FromArgb(235, 236, 237);
                panel1.Show();
                contentPanel.Controls.Add(panel1);

                Panel panel2 = new Panel();
                panel2.Margin = new Padding(0, 0, 0, 0);
                panel2.Size = new Size(551, 185);
                panel2.Location = new Point(0, 365);
                panel2.BackColor = Color.FromArgb(235, 236, 237);
                panel2.Show();
                contentPanel.Controls.Add(panel2);

                string d_string = send(UI_CLIENT_DATA_REQUEST);
                string[] d_i_string = d_string.Split('+');

                TableLayoutPanel tablePanel = new TableLayoutPanel();
                tablePanel.Name = "übersicht_devicePanel";
                tablePanel.RowCount = d_i_string.Length + 1;
                tablePanel.Height = (d_i_string.Length + 1) * 50;
                tablePanel.Width = panel1.Width;
                tablePanel.ColumnCount = 1;
                tablePanel.Location = new Point(13, 7);
                tablePanel.Show();
                panel1.Controls.Add(tablePanel);

                for (int i = 0; i < d_i_string.Length; i++) {
                    string[] s_properties = d_i_string[i].Split('_');

                    Panel p = new Panel();
                    p.Name = s_properties[0];
                    p.Height = 45;
                    p.Width = panel1.Width - 6;
                    p.BackColor = Color.Transparent; // Color.FromArgb(200, 201, 202); ;
                    p.Show();
                    tablePanel.Controls.Add(p, 0, i);

                    Panel p_c = new Panel();
                    p_c.BackColor = graphColors[i];
                    p_c.Width = 8;
                    p_c.Height = 31;
                    p_c.Location = new Point(0, 7);
                    p_c.Show();
                    p.Controls.Add(p_c);

                    Label nL = new Label();
                    nL.Text = string.Join("", s_properties[0]);
                    FontFamily fontFamily = new FontFamily("Segoe UI");
                    Font font = new Font(fontFamily, 15, FontStyle.Regular, GraphicsUnit.Point);
                    nL.Font = font;

                    Size textSize = TextRenderer.MeasureText(nL.Text, nL.Font);
                    nL.Size = textSize;
                    int temp = (int)(p.Height - nL.Height) / 2 - 1;
                    nL.Location = new Point(12, temp);
                    nL.Show();
                    p.Controls.Add(nL);

                    ComboBox cBox = new ComboBox();
                    cBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    cBox.Show();
                    temp = (int)(p.Height - cBox.Height) / 2;
                    cBox.Location = new Point(250, temp);

                    string[] s_s_p = s_properties[5].Split(',');
                    foreach (string str in s_s_p) {
                        string n_str = str.Replace("'", "");
                        n_str = n_str.Replace("[", "");
                        n_str = n_str.Replace("]", "");
                        n_str = n_str.Trim();
                        cBox.Items.Add(n_str.ToUpper());
                    }

                    cBox.SelectedIndex = int.Parse(s_properties[3]);
                    cBox.Name = string.Join("", s_properties[0]);
                    cBox.MouseClick += (sender, e) => { skipOne = true; };
                    cBox.SelectedIndexChanged += (sender, e) => {
                        string n = ((ComboBox)sender).Name;
                        int idx = ((ComboBox)sender).SelectedIndex;
                        if (idx != -1) {
                            send(UI_CLIENT_COMMAND_IDENTIFIER + n + "_" + idx);
                        }
                    };

                    if (!bool.Parse(s_properties[6])) {
                    nL.Text = nL.Text + " (Offline)";
                    textSize = TextRenderer.MeasureText(nL.Text, nL.Font);
                    nL.Size = textSize;
                    cBox.Enabled = false;
                    cBox.SelectedIndex = -1;
                    }

                    p.Controls.Add(cBox);
                }

                Panel addBtn = new Panel();
                addBtn.Height = 45;
                addBtn.Width = panel1.Width - 38;
                addBtn.BackColor = Color.FromArgb(90, 166, 89);
                addBtn.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };

                Panel addIcon = new Panel();
                addIcon.Height = addBtn.Height;
                addIcon.Width = addIcon.Height;
                addIcon.Location = new Point(4, 0);
                addIcon.BackgroundImageLayout = ImageLayout.Zoom;
                addIcon.BackgroundImage = global::app.Properties.Resources.plus;
                addIcon.Show();
                addIcon.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };
                addBtn.Controls.Add(addIcon);

                Label addText = new Label();
                addText.Text = "Gerät hinzufügen";
                FontFamily fF = new FontFamily("Segoe UI");
                Font f = new Font(fF, 16, FontStyle.Regular, GraphicsUnit.Point);
                addText.Font = f;
                addText.ForeColor = Color.FromArgb(214, 233, 213);
                Size textSize1 = TextRenderer.MeasureText(addText.Text, addText.Font);
                addText.Size = textSize1;
                int temp1 = (int)(addBtn.Height - addText.Height) / 2 - 2;
                addText.Location = new Point(addIcon.Width, temp1);
                addText.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };
                addBtn.Controls.Add(addText);

                tablePanel.Controls.Add(addBtn, 0, tablePanel.RowCount);

                // TODO: ACQUIRE REAL LOG
                Dictionary<string, int> devices = new Dictionary<string, int>();
                //          "NAME", time in min
                devices.Add("Computer0", 200);
                devices.Add("Heater0", 30);
                devices.Add("Light0", 420);
                devices.Add("Computer1", 30);
                devices.Add("Heater1", 32);
                devices.Add("Light1", 42);
                devices.Add("Computer2", 20);
                devices.Add("Heater2", 330);
                devices.Add("Light2", 590);

                TableLayoutPanel frame = new TableLayoutPanel();
                frame.Name = "chart";
                frame.RowCount = 1;
                frame.ColumnCount = devices.Count;
                frame.Size = new Size(devices.Count * 40, 350);
                double oneMinuteSize = 0.2430555555555556;
                for (int i = 0; i < devices.Count; i++) {
                    Panel p = new Panel();
                    p.BackColor = graphColors[i];
                    p.Width = 32;
                    p.Height = (int)(devices.Values.ToArray()[i] * oneMinuteSize);
                    p.Dock = DockStyle.Bottom;
                    p.Margin = new Padding(4, 0, 4, 0);
                    p.Paint += (sender, e) => {
                        var pSD = sender as Panel;
                        if (pSD.Height > 50) {
                            var g = e.Graphics;
                            FontFamily fontFamily = new FontFamily("Segoe UI");
                            Font font = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
                            g.TranslateTransform(pSD.Width / 2, pSD.Height / 2);
                            g.RotateTransform(-90);
                            string mText = pSD.Name;
                            SizeF textSize = g.MeasureString(mText, font);
                            g.DrawString(mText, font, Brushes.White, -(textSize.Width / 2), -(textSize.Height / 2));
                        }
                    };
                    p.Name = devices.Keys.ToArray()[i];
                    p.Show();
                    frame.Controls.Add(p, i, 0);
                }
                frame.BackColor = Color.FromArgb(235, 236, 237);
                frame.Show();
                panel0.Controls.Add(frame);
                
                // TODO: ADD WEATHER AND SENSOR-DATA
            }
            else {
                TableLayoutPanel tablePanel = (TableLayoutPanel) contentPanel.Controls.Find("übersicht_devicePanel", true)[0];
                tablePanel.Controls.Clear();
                string d_string = send(UI_CLIENT_DATA_REQUEST);
                string[] d_i_string = d_string.Split('+');
                for (int i = 0; i < d_i_string.Length; i++) {
                    string[] s_properties = d_i_string[i].Split('_');

                    Panel p = new Panel();
                    p.Name = s_properties[0];
                    p.Height = 45;
                    p.Width = tablePanel.Width - 6;
                    p.BackColor = Color.Transparent; // Color.FromArgb(200, 201, 202); ;
                    p.Show();
                    tablePanel.Controls.Add(p, 0, i);

                    Panel p_c = new Panel();
                    p_c.BackColor = graphColors[i];
                    p_c.Width = 8;
                    p_c.Height = 31;
                    p_c.Location = new Point(0, 7);
                    p_c.Show();
                    p.Controls.Add(p_c);

                    Label nL = new Label();
                    nL.Text = string.Join("", s_properties[0]);
                    FontFamily fontFamily = new FontFamily("Segoe UI");
                    Font font = new Font(fontFamily, 15, FontStyle.Regular, GraphicsUnit.Point);
                    nL.Font = font;

                    Size textSize = TextRenderer.MeasureText(nL.Text, nL.Font);
                    nL.Size = textSize;
                    int temp = (int)(p.Height - nL.Height) / 2 - 1;
                    nL.Location = new Point(12, temp);
                    nL.Show();
                    p.Controls.Add(nL);

                    ComboBox cBox = new ComboBox();
                    cBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    cBox.Show();
                    temp = (int)(p.Height - cBox.Height) / 2;
                    cBox.Location = new Point(250, temp);

                    string[] s_s_p = s_properties[5].Split(',');
                    foreach (string str in s_s_p) {
                        string n_str = str.Replace("'", "");
                        n_str = n_str.Replace("[", "");
                        n_str = n_str.Replace("]", "");
                        n_str = n_str.Trim();
                        cBox.Items.Add(n_str.ToUpper());
                    }

                    cBox.SelectedIndex = int.Parse(s_properties[3]);
                    cBox.Name = string.Join("", s_properties[0]);
                    cBox.MouseClick += (sender, e) => { skipOne = true; };
                    cBox.SelectedIndexChanged += (sender, e) => {
                        string n = ((ComboBox)sender).Name;
                        int idx = ((ComboBox)sender).SelectedIndex;
                        if (idx != -1)
                        {
                            send(UI_CLIENT_COMMAND_IDENTIFIER + n + "_" + idx);
                        }
                    };

                    if (!bool.Parse(s_properties[6]))
                    {
                        nL.Text = nL.Text + " (Offline)";
                        textSize = TextRenderer.MeasureText(nL.Text, nL.Font);
                        nL.Size = textSize;
                        cBox.Enabled = false;
                        cBox.SelectedIndex = -1;
                    }

                    p.Controls.Add(cBox);
                }

                Panel addBtn = new Panel();
                addBtn.Height = 45;
                addBtn.Width = tablePanel.Width - 38;
                addBtn.BackColor = Color.FromArgb(90, 166, 89);
                addBtn.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };

                Panel addIcon = new Panel();
                addIcon.Height = addBtn.Height;
                addIcon.Width = addIcon.Height;
                addIcon.Location = new Point(4, 0);
                addIcon.BackgroundImageLayout = ImageLayout.Zoom;
                addIcon.BackgroundImage = global::app.Properties.Resources.plus;
                addIcon.Show();
                addIcon.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };
                addBtn.Controls.Add(addIcon);

                Label addText = new Label();
                addText.Text = "Gerät hinzufügen";
                FontFamily fF = new FontFamily("Segoe UI");
                Font f = new Font(fF, 16, FontStyle.Regular, GraphicsUnit.Point);
                addText.Font = f;
                addText.ForeColor = Color.FromArgb(214, 233, 213);
                Size textSize1 = TextRenderer.MeasureText(addText.Text, addText.Font);
                addText.Size = textSize1;
                int temp1 = (int)(addBtn.Height - addText.Height) / 2 - 2;
                addText.Location = new Point(addIcon.Width, temp1);
                addText.MouseClick += (sender, e) => {
                    CreateDevicePrompt.ShowDialog(this);
                };
                addBtn.Controls.Add(addText);

                tablePanel.Controls.Add(addBtn, 0, tablePanel.RowCount);
            }
        }

        private void loadMontag() {
            clearPanel();
            viewSelected = 1;
        }
        private void loadDienstag() {
            clearPanel();
            viewSelected = 2;
        }
        private void loadMittwoch() {
            clearPanel();
            viewSelected = 3;
        }
        private void loadDonnerstag() {
            clearPanel();
            viewSelected = 4;
        }
        private void loadFreitag() {
            clearPanel();
            viewSelected = 5;
        }
        private void loadSamstag() {
            clearPanel();
            viewSelected = 6;
        }
        private void loadSonntag() {
            clearPanel();
            viewSelected = 7;
        }

        private void loadLive_Kamera() {
            clearPanel();
            viewSelected = 8;
        }

        private void loadEinstellungen() {
            clearPanel();
            viewSelected = 9;
        }

        private void loadViewById(int id) {
            switch (id) {
                case 0:
                    loadÜbersicht();
                    break;
                case 1:
                    loadMontag();
                    break;
                case 2:
                    loadDienstag();
                    break;
                case 3:
                    loadMittwoch();
                    break;
                case 4:
                    loadDonnerstag();
                    break;
                case 5:
                    loadFreitag();
                    break;
                case 6:
                    loadSamstag();
                    break;
                case 7:
                    loadSonntag();
                    break;
                case 8:
                    loadLive_Kamera();
                    break;
                case 9:
                    loadEinstellungen();
                    break;

            }
        }

        // COMMUNICATION
        private string send(string txt) {
            string msg = txt;
            string message = "";
            try {
                TcpClient client = new TcpClient(IP, PORT);
                NetworkStream nwStream = client.GetStream();

                // SEND
                byte[] bytesToSend = UTF8Encoding.UTF8.GetBytes(msg);
                if (bytesToSend.Length < 64) {
                    msg += "#";
                    bytesToSend = UTF8Encoding.UTF8.GetBytes(msg);
                }
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                // RECV
                byte[] answer = new byte[2048];
                nwStream.Read(answer, 0, 2048);
                message = Encoding.UTF8.GetString(answer);
                message = message.Replace("#", "");

                // CLOSE
                nwStream.Close();
                client.Close();
                nwStream.Dispose();
                client.Dispose();

                updateTimer.Interval = 1000;
            } catch (Exception) {
                MessageBox.Show("Es konnte keine Verbindung zum Server hergestellt werden.");
            }
            if (message.Equals("")) {
                MessageBox.Show("Es konnte keine Verbindung zum Server hergestellt werden.");
            }
            return message;
        }

        // UPDATER
        private void updateTimer_Tick(object sender, EventArgs e) {
            string s = send(UI_CLIENT_DATA_REQUEST);
            if (s != lastAnswer) {
                if (!skipOne) {
                    loadViewById(viewSelected);
                } else {
                    skipOne = false;
                }
            }
            lastAnswer = s;
        }
    }
}