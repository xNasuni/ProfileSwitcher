# ProfileSwitcher

<img src="/assets/splash.png" width="250px"/>

**ProfileSwitcher** is a utility for the browser **Opera GX** that allows you to change between the profiles you want links to automatically open, as in when you click on a link, it opens **Opera GX** as the profile you choose.

> This utilizes the registry in order to change the **one (1)** registry key responsible for opening Opera GX and simple file reading to create a list and obtain the data and state of your created profiles.

>
# Download
> **[`Latest Version` `f69a0aea7229c3c541f0502c19941c76b3481fcee36fc39ef68ca577be21118c`](https://github.com/xNasuni/ProfileSwitcher/releases/tag/1.0.0)**

## Extra Information

> **ProfileSwitcher** only changes a single registry key and only reads the **Opera GX profile data** thats **neccessary** to make this program work.
> The registry key and the directories it reads is listed below.
> ```
> Folders    %LocalAppData%\Programs\Opera GX
> Folders    %AppData%\Opera Software\Opera GX Stable\_side_profiles
> Registry   HKEY_CURRENT_USER\Software\Classes\Opera GXStable\shell\open\command
> ```
