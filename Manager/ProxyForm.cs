using System;
using System.Windows.Forms;

namespace Manager
{
    public partial class ProxyForm : Form
    {
        private string dllName;
        private string methodName;
        private string[] args;

        public string[] res;


        public ProxyForm(string dllName, string methodName, string[] args)
        {
            InitializeComponent();
            this.dllName = dllName;
            this.methodName = methodName;
            this.args = args;
        }

        private void ProxyForm_Load(object sender, EventArgs e)
        {
            res = Proxy.invoke(args, dllName, methodName);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}
