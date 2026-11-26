using EAPPP_Aldas_Jostin_Correccion;
using System;
using System.Windows.Forms;

namespace EAPPP_Aldas_Jostin_Correccion
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPedidoCafeteria());
        }
    }
}
