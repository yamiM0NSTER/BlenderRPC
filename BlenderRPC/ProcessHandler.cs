using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlenderRPC
{
    class ProcessHandler
    {
        // Grab currently worked on project
        public static string PullProjectName()
        {
            string text = "";
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (!process.ProcessName.ToLower().Equals("blender"))
                        continue;

                    text = process.MainWindowTitle.Replace("Blender* [", "").Replace("Blender [", "").Replace("]", "");
                    break;
                }
                catch
                {
                }
            }

            if (text != "Blender")
                return text;

            return "Blender - Idle";
        }

        // Grab blender version
        public static string PullBlenderVersion()
        {
            string result = "";
            foreach (Process process in Process.GetProcessesByName("Blender"))
            {
                string fileName = process.MainModule.FileName;
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
                string fileVersion = versionInfo.FileVersion;
                string text = string.Format("{0}.{1}{2}", versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart);
                string text2 = string.Format("{0}.{1}{2}.{3}", new object[]
                {
                        versionInfo.FileMajorPart,
                        versionInfo.FileMinorPart,
                        versionInfo.FileBuildPart,
                        versionInfo.FilePrivatePart
                });
                break;
            }

            return result;
        }

        // Check if blender process is running
        public static bool isBlenderRunning()
        {
            Process[] processesByName = Process.GetProcessesByName("blender");
            return processesByName.Length > 0;
        }

        // Start blender process
        public static void OpenBlender()
        {
            Process.Start("Blender.exe");
        }
    }
}
