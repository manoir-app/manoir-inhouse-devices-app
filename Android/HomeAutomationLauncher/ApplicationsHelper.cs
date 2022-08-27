using Android.Content;
using Android.Content.PM;
using System;
using System.Collections.Generic;


namespace HomeAutomationLauncher
{
    class ApplicationsHelper
    {

        public class InstalledApps
        {
            public string appName { get; set; }
            public string packageName { get; set; }
            public string icon { get; set; }

        }

        public static List<InstalledApps> GetInstalledApp()
        {
            List<InstalledApps> installedApps = new List<InstalledApps>();

            try
            {
                var ctx = Android.App.Application.Context;
                var pckMgr = ctx.PackageManager;

                Intent intent = new Intent(Intent.ActionMain, null);
                intent.AddCategory(Intent.CategoryHome);

                var apps = pckMgr.QueryIntentActivities(intent, PackageInfoFlags.MetaData);
                //var apps = pckMgr.GetInstalledApplications(PackageInfoFlags.MetaData);
                if (apps != null)
                {
                    if (apps.Count > 0)
                    {
                        for (int i = 0; i < apps.Count; i++)
                        {
                            InstalledApps installapps = new InstalledApps()
                            {
                                appName = apps[i].LoadLabel(Android.App.Application.Context.PackageManager),
                                packageName = apps[i].ResolvePackageName,
                                icon = apps[i].LoadIcon(Android.App.Application.Context.PackageManager).ToString()
                            };
                            installedApps.Add(installapps);
                        }
                        return installedApps;
                    }
                }
            }
            catch (Exception ex)
            {

                //do something with error

            }
            return installedApps;
        }
    }
}