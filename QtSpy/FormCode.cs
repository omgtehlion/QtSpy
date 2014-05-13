using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QtSpy
{
    public partial class FormCode : Form
    {
        public FormCode()
        {
            InitializeComponent();
        }

        public string Code { get; private set; }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Code = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
