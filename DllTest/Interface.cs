using ESRI.ArcGIS.Carto;
using System;
using System.Windows.Forms;

namespace DllTest
{
    public partial class Interface : Form
    {
        public Interface()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
        }

        public bool test(string[] names)
        {
            Map map = new Map();
            Console.WriteLine(map.Name);
            map.ClearSelection();
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("I am DllTest");
            Console.WriteLine("接收到了数组：{0}", string.Join(" ", names));
            return true;
        }

        public bool SubmitTask(string[] fileNmaes)
        {
            return true;
        }

    }
}
