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
            Label n = new Label();
            n.Text = "HIASD";
            n.Show();
            n.Visible = true;
            contentPanel.Controls.Add(n);
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
    }
}
