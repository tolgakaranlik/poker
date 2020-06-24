using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RankVerifier
{
    public partial class FormCardSelection : Form
    {
        public string Result = "N/A";

        public FormCardSelection()
        {
            InitializeComponent();
        }

        private void pbS1C1_Click(object sender, EventArgs e)
        {
            Result = (sender as PictureBox).Name;
            DialogResult = DialogResult.OK;
        }
    }
}
