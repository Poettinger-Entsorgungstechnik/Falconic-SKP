using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogFileViewer
{
    public partial class Form1 : Form
    {
        #region members

        private int _actualFileSize;
        private string _actualPath;
        private FileSystemWatcher _fsWatcher = new FileSystemWatcher();

        #endregion

        #region Methods

        delegate void AddTextCallback(string value, bool bScrollDown);

        public void AddText(string value, bool bScrollDown)
        {
            if (this.InvokeRequired)
            {
                AddTextCallback d = new AddTextCallback(AddText);
                this.Invoke(d, new object[] { value, bScrollDown });
            }
            else
            {
                // first remove some text if we have reached the max length of the textbox
                if (value.Length + txtTrace.TextLength > txtTrace.MaxLength)
                {
                    this.txtTrace.SelectionStart = 0;
                    this.txtTrace.SelectionLength = value.Length;
                    this.txtTrace.SelectedText = "";
                }
                this.txtTrace.AppendText(value);

                if (bScrollDown)
                {
                    txtTrace.SelectionStart = txtTrace.Text.Length;
                    txtTrace.ScrollToCaret();
                }
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (Directory.Exists("c:\\SKP\\Log"))
                dlg.InitialDirectory = "c:\\SKP\\Log";

            dlg.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _actualPath = dlg.FileName;
                try
                {
                    using (FileStream fs = File.Open(_actualPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
 //                   using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        char[] buffer = new char[32767];
                        int offset = 0;
                        int length = 0;

                        while ((length = sr.Read(buffer, 0, 32767)) != 0)
                        {
                            string str = new string(buffer);
                            offset += length;

                            if (txtFilterContainer.Text != "")
                            {
                                string[] lineToks = str.Split(new char[] { '\n' });
                                str = "";
                                foreach (string s in lineToks)
                                {
                                    if (s.IndexOf("ContainerID " + txtFilterContainer.Text) != -1)
                                        str += s + '\n';
                                }
                            }
                            AddText(str, false);
                        }

                        _actualFileSize = offset;
                    }


                    _fsWatcher.Path = Path.GetDirectoryName(_actualPath);
                    _fsWatcher.Filter = Path.GetFileName(_actualPath);
                    _fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    _fsWatcher.Changed += _fsWatcher_Changed;
                    _fsWatcher.EnableRaisingEvents = true;

                }
                catch (IOException)
                {
                }
            }
        }

        private void _fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                using (FileStream fs = File.Open(_actualPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(_actualFileSize, SeekOrigin.Begin);

                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        char[] buffer = new char[32767];
                        int offset = 0;
                        int length = 0;
                        int addedBytes = 0;

                        while ((length = sr.Read(buffer, 0, 32767)) != 0)
                        {
                            string str = new string(buffer);
                            offset += length;

                            if (txtFilterContainer.Text != "")
                            {
                                string[] lineToks = str.Split(new char[] { '\n' });
                                str = "";
                                string strSearch = "ContainerID " + txtFilterContainer.Text.Trim();
                                strSearch += ": ";
                                foreach (string s in lineToks)
                                {
                                    if (s.IndexOf(strSearch) != -1)
                                        str += s + '\n';
                                }
                            }
                            if (str != "")
                            {
                                AddText(str, false);
                                addedBytes += str.Length;
                            }
                        }

                        _actualFileSize += offset;

                        // refresh caret position if something has changed
                        if (addedBytes != 0)
                            AddText("", true);
                    }
                }
            }
            catch (IOException)
            {
            }
        }
    }
}
