using AYRVI.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AYRVI;

internal class AyrviContext : ApplicationContext
{
    private readonly string SteamExe;
    private readonly string VanillaPath;

    internal readonly NotifyIcon TrayIcon = new()
    {
        Icon = Resources.ayrvi,
        Text = "Are you ready for Valve Index?",
        Visible = true,
        ContextMenuStrip = new ContextMenuStrip()
    };

    public AyrviContext()
    {
        var reg = Registry.GetValue(
            keyName: @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam",
            valueName: "SteamExe",
            defaultValue: null);

        SteamExe = reg != null
            ? reg.ToString()
            : @"C:\Program Files (x86)\Steam\steam.exe";

        VanillaPath = Environment.GetCommandLineArgs().FirstOrDefault(
            (string arg) => arg.EndsWith("compatapp.exe"),
            @"C:\Program Files (x86)\Steam\steamapps\common\AreYouReadyForValveIndex\compatapp.exe"
        );


        if (File.Exists(SteamExe))
            TrayIcon.DoubleClick += OpenLibrary;

        if (File.Exists(VanillaPath))
            TrayIcon.ContextMenuStrip.Items.Add("vanilla", Resources.ayrvi.ToBitmap(), OpenVanilla);

        TrayIcon.ContextMenuStrip.Items.Add("kill", Resources.dreamed, Exit);
    }

    private void OpenLibrary(object sender, EventArgs e)
    {
        Process.Start(SteamExe, "steam://nav/games/details/1070910");
    }

    private void OpenVanilla(object sender, EventArgs e)
    {
        ProcessStartInfo ProcessInfo = new()
        {
            FileName = VanillaPath,
            WorkingDirectory = Path.GetDirectoryName(VanillaPath)
        };
        Process.Start(ProcessInfo);
    }

    private void Exit(object sender, EventArgs e)
    {
        TrayIcon.Visible = false;
        Application.Exit();
    }
}
