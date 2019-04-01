using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnchat_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            this.Hide();
            f.Show();
        }

        private void btnfile_Click(object sender, EventArgs e)
        {
            File fi = new File();
            this.Close();
            fi.Show();
        }
    }
}
