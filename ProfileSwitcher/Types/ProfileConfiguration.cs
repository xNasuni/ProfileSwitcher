using System;
using System.Collections.Generic;
namespace ProfileSwitcher.Types
{
    public class ProfileConfiguration
    {
        /* this is all for Newtonsoft.Json to read, basically every gx profile has a file called sideprofile.json which contains some info,
         * like below, color, name, and features which are like the ones that you can choose like "Bare bones", "clear all browsing data on exit",
         * "mute all tabs by default" and such, we know why they put those featuers in lol
        */
        public string Color { get; set; }
        public string Name { get; set; }
        public string[] Features{ get; set; }

    }
}
