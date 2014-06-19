using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CR_SetPluginVersion
{
    public partial class GetVersion : Form
    {
        public GetVersion()
        {
            InitializeComponent();
        }


        public static string GetNewVersion(string OldVersion)
        {
            var Form = new GetVersion();
            Form.txtNewVersion.Text = OldVersion;
            if (Form.ShowDialog() == DialogResult.OK)
            {
                return Form.txtNewVersion.Text;
            }
            else
            {
                return OldVersion;
            }
        }
    }
}
