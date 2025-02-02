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
    public partial class ConfirmDialog : Form
    {
        public event System.EventHandler OnConfirm;
        public event System.EventHandler OnCancel;

        public ConfirmDialog(string headerText, string mainText)
        {
            InitializeComponent();
            this.HeaderLabel.Text = headerText;
            this.MainTextBox.Text = mainText;
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.OnConfirm?.Invoke(null, EventArgs.Empty);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.OnCancel?.Invoke(null, EventArgs.Empty);
        }
    }
}
