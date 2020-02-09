using System;
using System.Windows.Forms;
using System.Drawing;

namespace GameQuest
{
    public partial class ReferenceForLevel1 : Form
    {
        public ReferenceForLevel1()
        {
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);

        }

        private void ЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
