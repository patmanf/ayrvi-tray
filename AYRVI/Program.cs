using Microsoft.Win32;
using System.Windows.Forms;

namespace AYRVI;

internal static class Program
{
    private static readonly AyrviContext AyrviContext = new();

    private static void Main()
    {
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(AyrviContext);

        AyrviContext.TrayIcon.Visible = false;
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
