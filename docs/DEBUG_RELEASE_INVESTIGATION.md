# Investigasi Debug vs Release

## Temuan

- Tidak ada directive conditional `Debug`/`Release` pada ViewModel atau XAML hardware.
- Tidak ada `IsEnabled`, `CanExecute`, atau permission gate pada Power Profile dan Battery Charging Mode.
- `PowerBattery.dll` dipanggil melalui `LibraryImport` dari `LenovoPowerSettingsService`.
- Startup WinUI memeriksa `PowerBattery.dll` sebelum window dibuat.
- `Page_Loaded` memanggil `ViewModel.Refresh()` untuk membaca Power Profile dan Charging Mode.
- Command `SetPlanCommand` dan `SetChargingModeCommand` memanggil API lalu membaca ulang state melalui `Refresh()`.
- Release mengaktifkan `PublishReadyToRun` dan `PublishTrimmed`; Debug menonaktifkan keduanya.

## Akar masalah UI

`MainPage.xaml` sebelumnya memiliki:

```xml
<RadioButtons SelectedItem="{x:Bind ViewModel.Plan}">
```

`ViewModel.Plan` bertipe `PowerPlan`, sedangkan item `RadioButtons` adalah objek `RadioButton`. Binding tersebut bertentangan dengan `IsChecked` one-way yang memakai enum yang sama. Binding `SelectedItem` tidak diperlukan karena command memakai `CommandParameter` eksplisit.

Binding `SelectedItem` dihapus. Seleksi sekarang dikendalikan oleh `IsChecked` hasil read-back dan command eksplisit, sama pada Debug dan Release.

## Perubahan

- `IdeapadToolkit.WinUI/Views/MainPage.xaml`: hapus `SelectedItem` yang salah tipe.
- `docs/DEBUG_RELEASE_INVESTIGATION.md`: dokumentasi investigasi.

## Verifikasi build

Perintah:

```text
dotnet build IdeapadToolkit.WinUI\IdeapadToolkit.WinUI3.csproj -c Debug -p:Platform=x64 --no-restore
```

Hasil: berhasil, 0 error, 44 warning.

## Batas verifikasi

Interaksi klik radio button dan perubahan state hardware belum dapat diverifikasi dari proses build. Pengujian tersebut memerlukan aplikasi berjalan pada Lenovo IdeaPad S340-14API dengan `PowerBattery.dll`, Lenovo ITS Service, dan hardware target.

Tidak ada klaim bahwa perilaku hardware Debug/Release telah terverifikasi pada fase ini.
