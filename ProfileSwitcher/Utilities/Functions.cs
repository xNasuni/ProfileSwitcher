using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using ProfileSwitcher.Types;
using System.Text.RegularExpressions;

namespace ProfileSwitcher.Utilities
{
    public class Functions
    {
        public class OperaGX
        {
            private static readonly string OperaGXDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Programs\\Opera GX";
            private static readonly string SideProfilesDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Opera Software\\Opera GX Stable\\_side_profiles\\";
            private static readonly string RegistryKey = $"HKEY_CURRENT_USER\\Software\\Classes\\Opera GXStable\\shell\\open\\command";
            //                                 ^^ i wasnt lying about only using those
            // honestly im not even gonna write any docs for this below, report an issue if you actually want i know nobody does tho
            private static string GenerateProfileLink(string ProfileID)
            {
                if (ProfileID.Equals("Main"))
                {
                    return $"\"{OperaGXDirectory}\\Launcher.exe\" -noautoupdate -- \"%1\"";
                }

                string Link = $"\"{OperaGXDirectory}\\Launcher.exe\" --side-profile-name={ProfileID}";

                ProfileConfiguration ProfileData = GetProfileData<ProfileConfiguration>(ProfileID);

                foreach (string Feature in ProfileData.Features)
                {
                    Link += $" --{Feature}";
                }

                Link += " --with-features:side-profiles --no-default-browser-check -noautoupdate -- \"%1\"";

                return Link;
            }
            public static List<string> GetProfiles(bool GetNames = false, bool GetMain = false)
            {
                if (!Directory.Exists(SideProfilesDirectory))
                {
                    MessageBox.Show("The Opera GX side profiles directory wasn't found.\nThis usually means that you don't have any side profiles.", SideProfilesDirectory, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }

                List<string> ProfileList = new List<string>();
                
                foreach (string FilePath in Directory.GetDirectories(SideProfilesDirectory))
                {
                    string ProfileID = FilePath.Substring(SideProfilesDirectory.Length);
                    if (GetNames)
                    {
                        ProfileList.Add(GetProfileName(ProfileID));
                    }
                    else
                    {
                        ProfileList.Add(ProfileID);
                    }
                }

                if (GetMain)
                {
                    ProfileList.Add("Main");
                }

                return ProfileList;
            }
            public static Image GetProfileIcon(string ProfileID)
            {
                if (ProfileID.Equals("Main"))
                {
                    return GetDefaultIcon();
                }
                string ProfileDirectory = $"{SideProfilesDirectory}\\{ProfileID}";

                if (!Directory.Exists(ProfileDirectory))
                {
                    MessageBox.Show($"A Opera GX side profile directory wasn't found.\nThis might be due to a programming error. Please report it in github issues to https://github.com/xNasuni/ProfileSwitcher", ProfileDirectory, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

                return Image.FromFile($"{ProfileDirectory}\\Opera On The Side.ico");
            }
            public static Image GetDefaultIcon()
            {
                if (!Directory.Exists($"{OperaGXDirectory}"))
                {
                    MessageBox.Show("The Opera GX directory wasn't found.", OperaGXDirectory, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }

                return Icon.ExtractAssociatedIcon($"{OperaGXDirectory}\\launcher.exe").ToBitmap();
            }
            public static void SetDefaultProfile(string ProfileID)
            {
                Registry.SetValue(RegistryKey, "ProfileID", ProfileID);
                Registry.SetValue(RegistryKey, String.Empty, GenerateProfileLink(ProfileID));
            }
            public static string GetCurrentStatus()
            {
                string KeyData = (string) Registry.GetValue(RegistryKey, "ProfileID", null);
                string DefaultData = (string)Registry.GetValue(RegistryKey, "", null);

                if (KeyData == null)
                {
                    Regex ProfileIDRegex = new Regex("--side-profile-name=[A-Za-z0-9]+");
                    Match ProfileIDMatch = ProfileIDRegex.Match(DefaultData);

                    if (!ProfileIDMatch.Success) {
                        MessageBox.Show("Program couldn't find the ProfileID key and resorted to finding it with regex, and that failed, so please report it in github issues to https://github.com/xNasuni/ProfileSwitcher", DefaultData, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }

                    string ProfileID = ProfileIDMatch.Value.Split('=')[1];

                    return ProfileID;
                }

                var Data = Registry.GetValue(RegistryKey, "ProfileID", null);
                return (string) Data;
            }
            public static T GetProfileData<T>(string ProfileID)
            {
                string ProfileDirectory = $"{SideProfilesDirectory}\\{ProfileID}";
                string ProfileData = "";

                if (!Directory.Exists(ProfileDirectory))
                {
                    MessageBox.Show("A profile directory doesn't seem to exist.\nThis is probably due to a programming error so please report it in github issues to https://github.com/xNasuni/ProfileSwitcher", $"Directory @ {ProfileDirectory}", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }

                foreach (string FilePath in Directory.GetFiles(ProfileDirectory))
                {
                    string FileName = FilePath.Substring(ProfileDirectory.Length + 1);
                    if (FileName.Contains("sideprofile.json"))
                    {
                        ProfileData = File.ReadAllText(FilePath);
                    }
                }

                return JsonConvert.DeserializeObject<T>(ProfileData); ;
            }
            public static string GetProfileName(string ProfileID)
            {
                if (ProfileID.Equals("Main"))
                {
                    return "Main";
                }

                if (!Directory.Exists(SideProfilesDirectory))
                {
                    MessageBox.Show("The Opera GX side profiles directory wasn't found.\nThis usually means that you don't have any side profiles.", SideProfilesDirectory, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }

                ProfileConfiguration ProfileData = GetProfileData<ProfileConfiguration>(ProfileID);
                string ProfileName = ProfileData.Name;

                return ProfileName;
            }
        }
    }
}
