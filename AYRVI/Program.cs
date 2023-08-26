using Microsoft.Win32;
using System.Windows.Forms;

namespace AYRVI;

internal static class Program
{
    private static readonly AyrviContext AyrviContext = new();

    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(AyrviContext);

        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
    }

    private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
        if (e.Mode == PowerModes.Suspend)
        {
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
            AyrviContext.TrayIcon.Visible = false;
            Application.Exit();
        }
    }
}
