using Microsoft.Win32;
using System.Windows.Forms;

namespace SVRPT;

internal static class Program
{
    private static readonly SvrptContext SvrptContext = new();

    private static void Main()
    {
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(SvrptContext);

        SvrptContext.TrayIcon.Visible = false;
        SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
    }

    private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
        if (e.Mode == PowerModes.Suspend)
        {
            Application.Exit();
        }
    }
}
