# Release Notes

## Lenovo IdeaPad S340-14API (81NB)

- Power Profile melalui Lenovo ITS dan `PowerBattery.dll`.
- Charging Mode: Normal, Conservation, Rapid.
- Always On USB digabung menjadi satu switch.
- Enable: Always On USB, lalu Always On USB on battery.
- Disable: Always On USB on battery, lalu Always On USB.
- Flip to Boot dihapus.
- Thermal Profile dan Manual Fan Control tidak tersedia.
- Unknown device tidak mendapat kontrol hardware otomatis.

## Verification

- Debug x64 build: passed.
- Release x64 build: passed.
- Release tests: 7 passed, 0 failed, 0 skipped.
- Hardware validation: dikonfirmasi pengguna pada S340-14API (81NB).
- Package: `artifacts/release/win-x64-combined`.
- `PowerBattery.dll` tetap lokal; tidak dipindah ke `System32`.

NativeAOT tidak dipakai karena environment tidak memiliki C++ linker NativeAOT.
