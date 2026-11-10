# Review UI Fase 10

## Perubahan

- Power Profile dinonaktifkan jika capability perangkat tidak tersedia.
- Charging Mode dinonaktifkan jika capability perangkat tidak tersedia.
- Always On USB dan Always On USB on battery memakai capability terpisah.
- Error operasi hardware ditampilkan pada halaman utama.
- Thermal Profile, Manual Fan Control, dan Flip to Boot tidak ditambahkan ke UI.

Perangkat tidak dikenal tidak mendapat kontrol hardware otomatis.

## Verifikasi

- `dotnet test IdeapadToolkitTests\IdeapadToolkitTests.csproj -c Debug --no-restore`: 7 passed, 0 failed, 0 skipped.
- XAML compiler menghasilkan file XAML sementara, tetapi proses `XamlCompiler.exe` gagal dengan `MSB3073` tanpa diagnostic XAML pada environment ini.
- Build WinUI belum berstatus berhasil.
