# Kompatibilitas Hardware

## Profile target

`Lenovo IdeaPad S340-14API (81NB)` dikenali melalui `SystemManufacturer`, `SystemProductName`, dan `SystemSKU` pada Registry Windows.

## Capability S340-14API

| Capability | Status profile |
|---|---|
| Power Profile | Hardware verified pada S340-14API |
| Conservation Mode | Hardware verified pada S340-14API |
| Normal Charging Mode | Hardware verified pada S340-14API |
| Rapid Charge | Hardware verified pada S340-14API |
| Always On USB gabungan | Hardware verified pada S340-14API |
| Thermal Profile | Tidak diaktifkan |
| Manual Fan Control | Tidak diaktifkan |
| Flip to Boot | Selalu tidak didukung |

Profile hanya menentukan capability berdasarkan identifier perangkat. Status `didukung oleh profile` bukan bukti operasi hardware berhasil.

Validasi hardware langsung pada S340-14API telah dikonfirmasi pengguna: fitur berfungsi dan aman, termasuk switch Always On USB gabungan.

Power Profile `Intelligent Cooling` tidak dihitung sebagai Thermal Profile atau Manual Fan Control. Keduanya tetap tidak tersedia karena interface terverifikasi tidak ditemukan.

Perangkat tidak dikenal menerima seluruh capability `false`.
