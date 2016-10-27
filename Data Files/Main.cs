using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace St.Thomas_Registration
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void upadm_btn_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void newmbr_btn_Click(object sender, EventArgs e)
        {
            New_Member newmem = new New_Member();
            this.Close();
            newmem.Show();
           // Application.Run(newmem);
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            // This opens a form for search
        }

        private void newadm_btn_Click(object sender, EventArgs e)
        {
            // This opens form for new admin
        }

        private void deladm_btn_Click(object sender, EventArgs e)
        {
            // This open form to delete admin
        }

        private void upadm_btn_Click(object sender, EventArgs e)
        {
            // This opens form to update admin
        }
    }
}
