using Microsoft.Win32;
using SVRPT.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SVRPT;

internal class SvrptContext : ApplicationContext
{
    private readonly string SteamExe;
    private readonly string VanillaPath;
    private readonly string VanillaArgs;

    internal readonly NotifyIcon TrayIcon = new()
    {
        Icon = Resources.svrpt,
        Text = "SteamVR Performance Test",
        Visible = true,
        ContextMenuStrip = new ContextMenuStrip()
    };

    public SvrptContext()
    {
        var reg = Registry.GetValue(
            keyName: @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam",
            valueName: "SteamExe",
            defaultValue: null);

        SteamExe = reg != null
            ? reg.ToString()
            : @"C:\Program Files (x86)\Steam\steam.exe";

        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (VanillaPath == null)
            {
                if (arg.EndsWith("vr.exe")) VanillaPath = arg;
                continue;
            }
            VanillaArgs += $"{arg} ";
        }

        if (VanillaPath == null)
        {
            VanillaPath = @"C:\Program Files (x86)\Steam\steamapps\common\SteamVRPerformanceTest\bin\win64\vr.exe";
            VanillaArgs = "-noassert -retail -vrperftest -autofidelity -nosound +map vr_aperture_main";
        }

        if (File.Exists(SteamExe))
            TrayIcon.DoubleClick += OpenLibrary;

        if (File.Exists(VanillaPath))
            TrayIcon.ContextMenuStrip.Items.Add("vanilla", Resources.svrpt.ToBitmap(), OpenVanilla);

        TrayIcon.ContextMenuStrip.Items.Add("kill", Resources.dreamed, Exit);
    }

    private void OpenLibrary(object sender, EventArgs e)
    {
        Process.Start(SteamExe, "steam://nav/games/details/323910");
    }

    private void OpenVanilla(object sender, EventArgs e)
    {
        ProcessStartInfo ProcessInfo = new()
        {
            FileName = VanillaPath,
            Arguments = VanillaArgs,
            WorkingDirectory = Path.GetDirectoryName(VanillaPath)
        };
        Process.Start(ProcessInfo);
    }

    private void Exit(object sender, EventArgs e)
    {
        Application.Exit();
    }
}
