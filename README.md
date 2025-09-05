# Ideapad Toolkit

**Ideapad Toolkit is a utility that allows you to adjust your Ideapad's power settings without installing Lenovo Vantage**

![Screenshot 2023-04-24 174110](https://user-images.githubusercontent.com/62750643/234048210-32a98b8a-7e24-4ba3-8990-a25ecdd8a2f9.png)

**Power and charging profiles can be quickly adjusted from the tray icon**

![Screenshot 2023-04-24 174631](https://user-images.githubusercontent.com/62750643/234049214-1324dc57-b3fd-4f8f-9fb0-8a419e2e9d32.png)

**After running, the app windows can be accessed by double clicking the tray icon**
## Supported laptops
| Laptop | Power Mode | Rapid charging | Conservation mode | Always on USB |
| --- | --- | --- | --- | --- |
| Ideapad Flex 5 14ALC05/15ALC05 (Windows 11) | ✅ | ✅ | ✅ | ✅ |
| Ideapad 5 Pro 16ACH6 82L5 (Windows 10 22H2) - credits [@KatayR](https://github.com/KatayR) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad 5 Pro 16ARH7 - credits [@minbcrafter](https://github.com/minbcrafter) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad S540-14IML 81INF - thanks [@minbcrafter](https://github.com/minbcrafter) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad Pro 5 14APH8 - thanks [@Kondix10](https://github.com/Kondix10) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad Pro 5 14APH9 - thanks [@Che0063](https://github.com/Che0063) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad S340-15API - thanks [@ajayfaul](https://github.com/ajayfaul) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad Slim 7 15ITL05 - thanks [@Vitamoon](https://github.com/Vitamoon) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad Pro 5 16APH8 - thanks [@mooms06](https://github.com/mooms06) | ✅ | ✅ | ✅ | ✅ |
| ThinkBook 16 G6 IRL - thanks [@PrizeWhipser](https://github.com/PrizeWhipser) | ✅ | ✅ | ✅ | ✅ |
| ThinkBook 15 G3 ACL 21A4 - credits [@Flicker372](https://github.com/Flicker372) | ✅ | ✅ | ✅ | ✅ |
| V14 G4 AMN - thanks [@blckbearx](https://github.com/blckbearx) | ✅ | ✅ | ✅ | ✅ |
| Yoga C640-13IML - thanks [@Clancione](https://github.com/Clancione) | ✅ | ✅ | ✅ | ✅ |
| IdeaPad S145-14IIL 81W6 - credits [@Antonomasia3](https://github.com/Antonomasia3) | ✅ | ❌ | ✅ | ❌ |
| IdeaPad S145-14IWL - thanks [@Matee001](https://github.com/Matee001) |  ❌ | ❌ | ✅ | ❌ |
| IdeaPad Z50-70 - credits [@Ozzypozzy](https://github.com/Ozzypozzy) | ❌ | ❌ | ✅ | ❌ |
| Xiaoxin Pro-13 2020 - thanks [@BackMountainDevil](https://github.com/BackMountainDevil) | ✅ | ❌ | ✅ | ✅ |
| IdeaPad Slim 3 16IRH10 - thanks [@ta2edh](https://github.com/ta2edh) | ✅ | ✅ | ✅ | ✅ |

If you find your model is compatible and not on the list, please sumbit an issue

To test, try changing the settings and verify if the settings change accordingly in Lenovo Vantage or the UEFI

## Prerequisites
- [.NET 8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-8.0.10-windows-x64-installer)

- [Lenovo Intelligent Thermal Solution Driver](https://www.google.com/search?q=lenovo+<YOUR+MODEL>+intelligent+thermal+solution+driver+download)

- Visual C++ 2015 Runtime
  - This comes packed with most Windows software, so you most likely already have this.
  - If not, install from here: [X64](https://aka.ms/vs/17/release/vc_redist.x64.exe) [X86](https://aka.ms/vs/17/release/vc_redist.x86.exe)

- PowerBattery.dll
  - This dll needs to be placed in the same directory as the executable
  
### How to get PowerBattery.dll
- Method A
  1. Install Lenovo Vantage from the Microsoft Store
  2. Copy it from C:\ProgramData\Lenovo\Vantage\Addins\IdeaNotebookAddin\ to the Ideapad Toolkit directory
  3. Lenovo Vantage can now be uninstalled
- Method B (Without Microsoft Store)
  1.  Go to https://store.rg-adguard.net/
  2.  Enter the link to Lenovo Vantage (https://apps.microsoft.com/store/detail/lenovo-vantage/9WZDNCRFJ4MV)
  3.  Download the newest version in the **.msixbundle** format (The file should be called "something LevovoCompanion something **.msixbundle**)
  4.  Open the .msixbundle file using 7Zip or similar software
  5.  Inside 7Zip, navigate to LenovoVantagePackage\[Version\]x64.msix/DeployAssistant/ImController/Plugins\[Version\].cab/plugins.7z/Normal/IdeaNotebookPlugin/x64
  6.  PowerBattery.dll should be in there

## Languages
- ⚠️ Restart the app to apply the changes
- English
- Czech
- French (by [@mooms06](https://github.com/mooms06))
- Chinese (Simplified) (by [@Flicker372](https://github.com/Flicker372))

## Third party licenses
[Hardcodet.NotifyIcon.Wpf](https://github.com/hardcodet/wpf-notifyicon/blob/develop/LICENSE)

[ModernWpf](https://github.com/Kinnara/ModernWpf/blob/master/LICENSE)
 
[SimpleInjector](https://github.com/simpleinjector/SimpleInjector/blob/master/LICENSE)
 
[.NET Community Toolkit](https://github.com/CommunityToolkit/dotnet/blob/main/License.md)
 
[Serilog](https://github.com/serilog/serilog/blob/dev/LICENSE)

