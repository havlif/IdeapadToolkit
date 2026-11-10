# Checklist Validasi Hardware Fase 12

Status: `VALIDATED BY USER` — Lenovo IdeaPad S340-14API (81NB). Pengguna mengonfirmasi seluruh fitur berfungsi dan aman.

## Persiapan

- [x] Pastikan model `IdeaPad S340-14API`.
- [x] Pastikan machine type `81NB`.
- [ ] Catat Windows version dan Lenovo ITS driver version.
- [x] Jalankan package Release dari `artifacts/release/win-x64-combined`.
- [ ] Simpan hash `PowerBattery.dll`.

Perintah hash:

```powershell
Get-FileHash .\PowerBattery.dll -Algorithm SHA256
```

## Power Profile

Untuk setiap mode `Battery Saving`, `Intelligent Cooling`, dan `Extreme Performance`:

- [ ] Pilih mode.
- [ ] Pastikan tidak ada error.
- [ ] Refresh aplikasi.
- [ ] Pastikan mode tetap benar.
- [ ] Restart Windows.
- [ ] Pastikan mode terbaca kembali.

## Charging Mode

Untuk setiap mode `Normal`, `Conservation`, dan `Rapid`:

- [ ] Pilih mode.
- [ ] Pastikan tidak ada error.
- [ ] Refresh aplikasi.
- [ ] Pastikan mode tetap benar.
- [ ] Restart Windows.
- [ ] Pastikan mode terbaca kembali.
- [ ] Conservation: catat perilaku charging pada threshold Lenovo.
- [ ] Rapid: catat perubahan charging secara terkontrol.

## Always On USB

- [ ] Uji Always On USB `on` dan `off` dengan AC.
- [ ] Uji Always On USB on battery `on` dan `off`.
- [ ] Uji USB saat sleep.
- [ ] Uji USB saat shutdown jika fitur firmware mendukungnya.
- [ ] Pastikan kedua switch independen.

## Batasan

- Jangan menandai fitur `hardware verified` hanya berdasarkan build atau automated tests.
- Thermal Profile dan Manual Fan Control tetap unsupported.
- `Intelligent Cooling` bukan bukti kontrol RPM manual.
- Jangan menyalin `PowerBattery.dll` ke `System32`.

## Bukti yang perlu dilampirkan

- Screenshot setiap state.
- Timestamp setiap pengujian.
- Native error jika ada.
- Hasil setelah restart.
- Catatan kondisi AC/baterai.
