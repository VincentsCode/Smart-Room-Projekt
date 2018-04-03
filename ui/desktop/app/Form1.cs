using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app {
    public partial class Window : Form {

        Color[] graphColors = { // 14 colors
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

        public Window() {
            InitializeComponent();
        }

        // BEGIN: Moveable Window
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
        // END: Moveable Window

        // BEGIN: Closeable Window
        private void closeBtn_MouseClick(object sender, MouseEventArgs e) {
            this.Close();
        }
        // END: Closeable Window

        // BEGIN: Sidebar-Elements
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
        // END: Sidebar-Elements


        // BEGIN: Marker
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
        // END: Marker


        // BEGIN: Preset-Loader
        private void clearPanel() {
            contentPanel.Controls.Clear();
        }

        private void loadÜbersicht() {
            clearPanel();
            // TODO ACQUIRE REAL LOG
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
            var l = devices.OrderBy(key => key.Key);
            devices = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

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
                p.Height = (int) (devices.Values.ToArray()[i] * oneMinuteSize);
                p.Dock = DockStyle.Bottom;
                p.Margin = new Padding(4, 0, 4, 0);
                p.Paint += new PaintEventHandler(panel1_Paint);
                p.Name = devices.Keys.ToArray()[i];
                p.Show();
                frame.Controls.Add(p, i, 0);
            }
            frame.BackColor = Color.FromArgb(235, 236, 237);
            frame.Show();
            contentPanel.Controls.Add(frame);
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            var p = sender as Panel;
            if (p.Height > 50) {
                var g = e.Graphics; FontFamily fontFamily = new FontFamily("Segoe UI");
                Font font = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
                g.TranslateTransform(p.Width / 2, p.Height / 2);
                g.RotateTransform(-90);
                string mText = p.Name;
                SizeF textSize = g.MeasureString(mText, font);
                g.DrawString(mText, font, Brushes.White, -(textSize.Width / 2), -(textSize.Height / 2));
            }
        }


        private void loadMontag() {
            clearPanel();
        }
        private void loadDienstag() {
            clearPanel();
        }
        private void loadMittwoch() {
            clearPanel();
        }
        private void loadDonnerstag() {
            clearPanel();
        }
        private void loadFreitag() {
            clearPanel();
        }
        private void loadSamstag() {
            clearPanel();
        }
        private void loadSonntag() {
            clearPanel();
        }

        private void loadLive_Kamera() {
            clearPanel();
        }

        private void loadEinstellungen() {
            clearPanel();
        }
        // END: Preset-Loader

        // BEGIN: COMMUNICATION
        // TODO
        private string getDeviceLogToday() {
            return "";
        }
        // END: COMMUNICATION
    }
}
