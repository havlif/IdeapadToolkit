# Analisis Awal Repository

## Status audit

Audit dilakukan sebelum perubahan source. Working tree bersih pada awal audit (`main...origin/main`). Tidak ada file source yang diubah pada fase ini.

## Struktur repository

- `IdeapadToolkit.sln`: solution utama.
- `IdeapadToolkit/`: aplikasi WPF lama, entry point `App.xaml.cs`.
- `IdeapadToolkit.Core/`: model, helper Win32, dan service hardware bersama.
- `IdeapadToolkit.WinUI/`: aplikasi WinUI 3 utama, entry point `App.xaml.cs`.
- `IdeapadToolkitTests/`: test MSTest.
- `README.md`, `LICENSE`, `.editorconfig`: dokumentasi dan konfigurasi repository.

Direktori `bin/` dan `obj/` ada secara lokal, tetapi di-ignore Git.

## Arsitektur proyek

Solution memuat empat project:

| Project | Target | Peran |
|---|---|---|
| `IdeapadToolkit` | `net8.0-windows` | UI WPF lama dan tray |
| `IdeapadToolkit.Core` | `net8.0` | model dan abstraksi/integrasi hardware |
| `IdeapadToolkit.WinUI3` | `net8.0-windows10.0.22621.0`, x64 | UI WinUI 3 utama |
| `IdeapadToolkitTests` | `net8.0-windows10.0.20348.0` | MSTest |

WinUI 3 menggunakan Windows App SDK 1.6, `CommunityToolkit.Mvvm`, `Pure.DI`, `WinUIEx`, dan `H.NotifyIcon.WinUI`. Project WPF menggunakan Simple Injector. Keduanya mereferensikan `IdeapadToolkit.Core`.

## Entry point

Startup utama adalah `IdeapadToolkit.WinUI/App.xaml.cs`, khususnya `OnLaunched`. Startup melakukan konfigurasi DI, pengecekan instance ganda, pengecekan `PowerBattery.dll`, pembuatan window, dan tray icon. `IdeapadToolkit/App.xaml.cs` adalah entry point aplikasi WPF lama.

## Integrasi hardware

`IdeapadToolkit.Core/Services/LenovoPowerSettingsService.cs` memakai `LibraryImport` ke entry point C++ ter-mangle pada `PowerBattery.dll` untuk:

- ITS / Power Profile melalui `CIntelligentCooling`.
- Battery Charging Mode melalui `CChargingMode`.
- Always On USB melalui `CUSBCharger`.
- Always On USB on battery melalui `CUSBBatteryCharger`.

`UEFISettingsService` memakai Windows firmware environment API di `Win32.cs` untuk Flip to Boot. `AdministratorPermissionService` menangani kebutuhan hak administrator. Belum ada device profile atau capability-based detection khusus S340-14API.

## Mekanisme PowerBattery.dll

File lokal berada di `IdeapadToolkit/PowerBattery.dll` dan `IdeapadToolkit.WinUI/PowerBattery.dll`, tetapi keduanya di-ignore oleh aturan `*PowerBattery.dll`; binary tidak boleh dianggap artefak source-controlled. Project WPF dan WinUI mendeklarasikan DLL sebagai `None` dengan `CopyToOutputDirectory=Always`. Startup WinUI dan WPF memeriksa keberadaan DLL pada `AppDomain.CurrentDomain.BaseDirectory` sebelum menampilkan UI.

## Fitur yang saat ini terlihat didukung

- Power Profile: Battery Saving (`EfficiencyMode`), Intelligent Cooling, Extreme Performance.
- Battery Charging Mode: Conservation, Normal, Rapid.
- Always On USB.
- Always On USB on battery.
- Flip to Boot pada UI WPF dan WinUI.
- Tray icon, startup setting, localization, dan settings persistence.

Klaim dukungan hardware belum tervalidasi oleh audit ini.

## Limitasi yang ditemukan

- Flip to Boot masih terekspos, padahal target S340-14API tidak mendukungnya.
- Belum ada deteksi manufacturer, model, machine type, Lenovo ITS, atau capability profile.
- Tidak ada abstraksi mock untuk hardware service yang terlihat pada implementasi saat ini.
- Operasi Power Profile dan Charging Mode melakukan `Refresh`, tetapi service tidak memvalidasi state hasil operasi secara eksplisit.
- Setter radio button pada `MainPageViewModel` kosong; command dipasang melalui XAML, sehingga investigasi interaksi Debug/Release masih diperlukan.
- Thermal profile dan manual fan control belum memiliki implementasi terverifikasi.
- Dokumentasi fase proyek belum tersedia sebelum audit ini.

## Baseline build dan test

Perintah yang dijalankan:

```text
dotnet restore IdeapadToolkit.sln
dotnet build IdeapadToolkit.sln -c Debug --no-restore
dotnet test IdeapadToolkitTests\IdeapadToolkitTests.csproj -c Debug --no-restore
```

Hasil restore: berhasil, semua project up-to-date.

Hasil build Debug: berhasil, **0 error, 59 warning**. Angka 59 adalah jumlah warning MSBuild yang dilaporkan; beberapa warning muncul dua kali karena WPF menghasilkan temporary project (`*_wpftmp.csproj`) selain project utama.

Hasil test baseline: **7 total, 5 passed, 2 failed**. Dua kegagalan adalah `UEFISettingsServiceTests.IsFlipToBootOnTest` dan `UEFISettingsServiceTests.FlipToBootSettingTest`; keduanya gagal saat membaca firmware Flip to Boot pada mesin audit dengan `System.Exception` dari `UEFISettingsService.GetFlipToBootStatus()`.

## Ringkasan warning awal

| Kategori | Kode | Area |
|---|---|---|
| Nullability | CS8618, CS8602, CS8622, CS8625 | App, tray, navigation, ViewModel |
| Dead/unused code | CS0168 | App startup |
| MVVM analyzer | MVVMTK0032 | WPF ViewModel |
| WinUI analyzer | DIW000 | WinUI generated/DI types |
| Code analysis | CA1001, CA1051, CA1305, CA1725, CA1822, CA1824, CA1859 | WPF dan helper |

Warning paling relevan pada WinUI: nullability di `App.xaml.cs`, `MainWindow.xaml.cs`, `NavigationService.cs`, `SettingsViewModel.cs`, dan `MainPageViewModel.cs`; serta `DIW000` dari source generation/override dependency. Warning lengkap tersimpan pada output console baseline fase ini.

## Kesimpulan fase

Struktur repository dan jalur hardware utama telah dipahami. Tidak ada implementasi hardware yang diubah. Fase berikutnya dapat melindungi source, menyiapkan branch bila diizinkan kondisi Git, lalu menghapus Flip to Boot secara device-specific.
