using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameQuest
{
    public partial class InventoryForm : Form
    {
        public InventoryForm(Player player)
        {
            InitializeComponent();
            
            label1.Text = "Монет: " + player.Inventory.Money;
            for (int i = 0; i < player.Inventory.Items.Length; i++)
            {
                ListViewItem li = listView1.Items.Add(player.Inventory.Items[i].Count.ToString());
                li.SubItems.Add(player.Inventory.Items[i].Name);
                li.SubItems.Add(player.Inventory.Items[i].Description);
            }
        }
    }
}
