using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RemoteTool
{
    public partial class EditAlertingUserDlg : Form
    {
        #region members

        public string _firstname = "";
        public string _lastname = "";
        public string _gsmnumber = "";
        public string _email = "";

        #endregion

        public EditAlertingUserDlg()
        {
            InitializeComponent();
        }

        private void bttnOk_Click(object sender, EventArgs e)
        {
            _firstname = txtFirstName.Text;
            _lastname = txtLastName.Text;
            _email = txtEmail.Text;
            _gsmnumber = txtGSMNumber.Text;

            if (txtFirstName.Text == "" || txtLastName.Text == "")
            {
                MessageBox.Show("Bitte geben Sie einen gültigen Namen ein!");
                this.DialogResult = DialogResult.Cancel;
            }

            if (txtGSMNumber.Text == "" && txtEmail.Text == "")
            {
                MessageBox.Show("Bitte geben Sie entweder eine gültige GSM Nummer oder eine Emailadresse ein!");
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void EditAlertingUserDlg_Load(object sender, EventArgs e)
        {
            txtEmail.Text = _email;
            txtFirstName.Text = _firstname;
            txtLastName.Text = _lastname;
            txtGSMNumber.Text = _gsmnumber;
        }
    }
}
