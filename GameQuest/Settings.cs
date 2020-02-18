using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace GameQuest
{
    public partial class Settings : Form
    {
        public Settings(AudioFileReader af)
        {
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);

            Audio = af;
            OldSize = GameField.BlockSize.Width * 30;
            if (af != null)
                OldVolume = Audio.Volume;
            TrackBar.Value = OldSize;
            if (OldVolume < 0)
            {
                OldVolume = 1;
                Audio.Volume = 1;
            }
            trackBar1.Value = Convert.ToInt32(OldVolume * 100);
            label1.Text = String.Format("Разрешение: {0:0}x{1:0}", TrackBar.Value, TrackBar.Value / 1.5);
            label2.Text = String.Format("Громкость музыки: {0:0}%", trackBar1.Value);
        }
        AudioFileReader Audio;

        int OldSize = 900;
        float OldVolume = 1;

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (TrackBar.Value % 30 != 0)
                TrackBar.Value = TrackBar.Value / 30 * 30;
            label1.Text = String.Format("Разрешение: {0:0}x{1:0}", TrackBar.Value, TrackBar.Value / 1.5);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = String.Format("Громкость музыки: {0:0}%", trackBar1.Value);
            if (Audio != null)
                Audio.Volume = trackBar1.Value / 100f;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GameField.BlockSize = new Size(TrackBar.Value / 30, TrackBar.Value / 30);
            DialogResult = DialogResult.OK;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            GameField.BlockSize = new Size(OldSize / 30, OldSize / 30);
            if (Audio != null)
                Audio.Volume = OldVolume;
            DialogResult = DialogResult.Cancel;
        }
    }
}
