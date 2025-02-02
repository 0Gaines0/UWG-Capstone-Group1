using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ticket_system_winforms.View.Dialogs
{
    public partial class AlertDialog : Form
    {
        public AlertDialog(string labelText, string bodyText)
        {
            InitializeComponent();
            this.HeaderLbl.Text = labelText;
            this.MsgTextBox.Text = bodyText;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
