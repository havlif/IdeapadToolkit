# Dukungan Fitur

## Power Profile

Implementasi tetap memakai `PowerBattery.dll` dan Lenovo ITS melalui `LenovoPowerSettingsService`. Mode yang tersedia: Battery Saving, Intelligent Cooling, dan Extreme Performance.

`SetPowerPlan` sekarang:

1. Memvalidasi enum.
2. Memeriksa return code native.
3. Membaca state kembali.
4. Melempar error jika state aktual berbeda dari permintaan.

Hasil ini belum merupakan verifikasi hardware. Uji aktual pada Lenovo IdeaPad S340-14API masih diperlukan.

`Intelligent Cooling` berarti mode ITS/Power Profile. Fitur ini tidak menyediakan pembacaan RPM atau pengaturan fan manual.

## Capability

Profile S340-14API mengaktifkan Power Profile. Perangkat tidak dikenal tidak mendapat kontrol hardware otomatis.
