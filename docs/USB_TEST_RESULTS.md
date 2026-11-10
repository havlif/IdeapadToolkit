# Hasil Implementasi Always On USB

## Implementasi

Implementasi existing `CUSBCharger` dan `CUSBBatteryCharger` tetap digunakan melalui `PowerBattery.dll`.

Setiap perubahan sekarang:

1. Menjalankan native operation.
2. Memeriksa return code yang dikenal oleh implementasi lokal.
3. Membaca state aktual kembali.
4. Melempar error jika read-back berbeda.

UI kini memakai satu switch gabungan. Saat enable, Always On USB diaktifkan lebih dulu, lalu Always On USB on battery. Saat disable, Always On USB on battery dinonaktifkan lebih dulu, lalu Always On USB.

API service tetap mempertahankan operasi terpisah untuk menjaga kompatibilitas hardware dan read-back masing-masing state.

## Status hardware

Belum diverifikasi pada Lenovo IdeaPad S340-14API pada fase ini. Build dan automated test tidak membuktikan daya USB tetap aktif saat laptop sleep atau shutdown.
