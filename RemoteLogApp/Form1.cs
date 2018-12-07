using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RemoteLogApp
{
    public partial class Form1 : Form
    {
        private readonly WebServer _webServer = new WebServer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _webServer.Start();
                labelStatus.Text = $"Started";
                linkLabelUrl.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "服务启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要关闭服务么？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            try
            {
                notifyIcon1.Dispose();
                _webServer.Stop();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("服务停止失败" + ex.Message);
            }
        }

        private void 控制台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(_webServer.Url);
        }

        private void 复制脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject($"<script src='{_webServer.Url}/Scripts/client.js'></script>");
            MessageBox.Show("复制成功，请将内容粘贴到需要调试的页面！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/cxwl3sxl/remoteconsole");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/cxwl3sxl/remoteconsole/blob/master/README.md");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Show();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized) return;
            ShowInTaskbar = false;
            Hide();
        }

        private void linkLabelUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_webServer.Url);
        }
    }
}
