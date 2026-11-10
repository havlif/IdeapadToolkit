# Investigasi Fan dan Thermal Management

## Hasil audit

Repository hanya memiliki interface Lenovo ITS melalui `CIntelligentCooling` pada `PowerBattery.dll`. Interface tersebut digunakan untuk memilih Power Profile:

- Battery Saving
- Intelligent Cooling
- Extreme Performance

Tidak ditemukan:

- API thermal profile terpisah.
- API pembacaan fan RPM.
- API manual fan RPM control.
- Penulisan EC register.
- Device interface ACPI fan yang terdokumentasi.
- Driver atau binary thermal/fan tambahan.

## Kesimpulan

`Intelligent Cooling` adalah Power Profile/ITS mode, bukan bukti adanya kontrol RPM manual atau thermal profile terpisah. S340-14API tidak diberi kontrol fan palsu.

Capability berikut tetap `false`:

- `SupportsThermalProfile`
- `SupportsManualFanControl`

Automatic fan management tetap menjadi tanggung jawab firmware/Lenovo ITS. Tidak ada klaim fan RPM atau thermal mode hardware verified.

## Status pengujian

| Fitur | Status |
|---|---|
| Intelligent Cooling Power Profile | Implementasi API existing; hardware belum diverifikasi pada fase ini |
| Thermal Profile terpisah | Unsupported / tidak ditemukan interface |
| Automatic fan management | Dikendalikan firmware/ITS; tidak diukur |
| Manual fan RPM | Unsupported |
