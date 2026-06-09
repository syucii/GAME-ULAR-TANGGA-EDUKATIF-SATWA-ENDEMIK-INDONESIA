# 🐾 Game Ular Tangga AR - Edukatif Satwa Endemik Indonesia

Aplikasi mobile edukatif berbasis **Augmented Reality (AR)** dengan Unity dan Vuforia untuk mengenalkan satwa endemik Indonesia melalui permainan ular tangga yang interaktif.

---

## 🎮 Konsep Permainan

```
📱 Home View
     ↓ [Tombol MULAI]
📋 Menu View (Mulai Permainan / Instruksi / Tentang)
     ↓ [Mulai Permainan]
📷 AR Scan View (Arahkan kamera ke marker)
     ↓ [Marker Terdeteksi]
❓ Quiz View (Soal hewan dengan 3 opsi jawaban)
     ↓ [Pilih Jawaban]
✅ Result View (Benar: Fun Fact / Salah: Jawaban & Deskripsi)
```

---

## ✨ Fitur Utama

- ✅ **20 Marker AR** - Satu untuk setiap satwa endemik
- ✅ **Quiz Interaktif** - Pertanyaan otomatis saat marker terdeteksi
- ✅ **Edukasi** - Informasi lengkap tentang hewan endemik Indonesia
- ✅ **Fun Facts** - Fakta menarik untuk setiap hewan
- ✅ **Tracking Skor** - Catat jawaban benar/salah
- ✅ **Offline Ready** - Tidak perlu koneksi internet saat bermain

---

## 🏗️ Struktur Proyek

```
Assets/
├── Resources/
│   ├── animals_data.json          # Data 20 hewan & soal
│   └── Images/                    # 20 gambar hewan
│
├── Scripts/
│   ├── Data/
│   │   └── AnimalData.cs          # Manager data hewan
│   ├── AR/
│   │   └── ARMarkerDetector.cs    # Deteksi marker
│   ├── UI/
│   │   ├── MenuManager.cs
│   │   ├── QuizUIManager.cs
│   │   └── ResultUIManager.cs
│   └── Core/
│       └── GameManager.cs         # State & event management
│
└── Scenes/
    ├── HomeScene.unity
    ├── MenuScene.unity
    └── ARScene.unity
```

---

## 📦 Solusi Data Storage

**Format: JSON**

Data disimpan dalam file `animals_data.json` dengan struktur:

```json
{
  "animals": [
    {
      "id": 1,
      "animalName": "Komodo",
      "region": "NTT",
      "question": "Komodo dikenal dari wilayah mana?",
      "answerOptions": ["Jawa", "Sumatra", "NTT"],
      "correctAnswerIndex": 2,
      "correctDescription": "Deskripsi jawaban benar...",
      "funFact": "Fakta menarik...",
      "imageFileName": "komodo.jpg",
      "markerName": "Marker_01"
    }
    // ... 19 hewan lainnya
  ]
}
```

**Keuntungan JSON:**
- 📝 Mudah diedit & ditambah data
- 🔍 Akses cepat dengan caching Dictionary
- 📦 Tidak perlu database eksternal
- 🎵 Human-readable format
- 📱 Kompatibel semua platform

---

## 🚀 Quick Start

### 1. Setup Folder Structure
```bash
# Buat folder di Assets/
Assets/Resources/animals_data.json
Assets/Resources/Images/            # Letakkan 20 gambar hewan di sini
Assets/Scripts/Data/
Assets/Scripts/AR/
Assets/Scripts/UI/
Assets/Scripts/Core/
```

### 2. Copy Script Files
Semua script sudah tersedia di repository ini:
- `AnimalData.cs`
- `GameManager.cs`
- `ARMarkerDetector.cs`
- `MenuManager.cs`
- `QuizUIManager.cs`
- `ResultUIManager.cs`

### 3. Setup Vuforia
1. Download Vuforia Engine
2. Buat 20 Image Target (Marker_01 s/d Marker_20)
3. Import database ke Unity

### 4. Follow SETUP_GUIDE.md
Baca file `SETUP_GUIDE.md` untuk langkah-langkah detail:
- Scene setup
- GameObject configuration
- Event wiring

---

## 📚 Dokumentasi Lengkap

- **SETUP_GUIDE.md** - Panduan setup Unity step-by-step
- **PROJECT_STRUCTURE.md** - Penjelasan struktur dan arsitektur

---

## 🎯 Data 20 Satwa Endemik

| No | Satwa | Region | Marker |
|----|-------|--------|--------|
| 1 | Komodo | NTT | Marker_01 |
| 2 | Orangutan | Kalimantan & Sumatra | Marker_02 |
| 3 | Harimau Sumatera | Sumatra | Marker_03 |
| 4 | Badak Jawa | Jawa | Marker_04 |
| 5 | Gajah Sumatera | Sumatra | Marker_05 |
| 6 | Burung Cendrawasih | Papua | Marker_06 |
| 7 | Anoa | Sulawesi | Marker_07 |
| 8 | Kasuari | Papua | Marker_08 |
| 9 | Owa Jawa | Jawa | Marker_09 |
| 10 | Pesut | Kalimantan | Marker_10 |
| 11 | Buaya Siam | Kalimantan | Marker_11 |
| 12 | Pangolin Jawa | Jawa | Marker_12 |
| 13 | Kuskus | Papua | Marker_13 |
| 14 | Tarsius | Sulawesi | Marker_14 |
| 15 | Burung Rajah Ampas | Papua | Marker_15 |
| 16 | Banteng | Jawa & Kalimantan | Marker_16 |
| 17 | Rafflesia | Sumatra & Kalimantan | Marker_17 |
| 18 | Rangkong | Kalimantan & Sumatra | Marker_18 |
| 19 | Elang Jawa | Jawa | Marker_19 |
| 20 | Jalak Bali | Bali | Marker_20 |

---

## 💻 Requirements

- **Unity** 2020 LTS atau lebih baru
- **Vuforia Engine** plugin
- **C# scripting knowledge** (basic)
- **20 gambar hewan** (JPG format)

---

## 🔧 Teknologi

- **Game Engine:** Unity
- **AR Framework:** Vuforia
- **Data Format:** JSON
- **Language:** C#
- **Target Platform:** Android/iOS

---

## 📱 Platform Support

- ✅ **Android 5.0+**
- ✅ **iOS 11+**
- ✅ **Standalone** (Windows/Mac untuk development)

---

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Marker tidak terdeteksi | Pastikan Vuforia license terpasang, lighting cukup, marker quality > 3 bintang |
| Gambar tidak muncul | Cek path: `Assets/Resources/Images/`, nama file sesuai JSON |
| JSON tidak ter-load | File harus di: `Assets/Resources/animals_data.json`, format valid |
| Event tidak trigger | Pastikan script subscribe di `Start()`, cek console untuk error |

---

## 📞 Support

Jika ada masalah:
1. Check console log untuk error message
2. Baca dokumentasi kode (XML comments) di setiap script
3. Ikuti SETUP_GUIDE.md step-by-step

---

## 📄 Folder Structure

```
Repository/
├── README.md                          # File ini
├── PROJECT_STRUCTURE.md               # Penjelasan struktur
├── SETUP_GUIDE.md                     # Panduan setup detail
│
├── Assets/
│   ├── Scripts/
│   │   ├── Data/
│   │   │   └── AnimalData.cs
│   │   ├── AR/
│   │   │   └── ARMarkerDetector.cs
│   │   ├── UI/
│   │   │   ├── MenuManager.cs
│   │   │   ├── QuizUIManager.cs
│   │   │   └── ResultUIManager.cs
│   │   └── Core/
│   │       └── GameManager.cs
│   │
│   └── Resources/
│       ├── animals_data.json          # Data 20 hewan
│       └── Images/                    # 20 gambar hewan (letakkan di sini)
│
└── Scenes/
    ├── HomeScene.unity                # Create manually
    ├── MenuScene.unity                # Create manually
    └── ARScene.unity                  # Create manually
```

---

## 🎓 Learning Outcomes

Setelah menyelesaikan project ini, akan belajar:
- ✅ Unity AR development dengan Vuforia
- ✅ Event-driven programming pattern
- ✅ Singleton pattern untuk Game Manager
- ✅ JSON data management
- ✅ UI/UX design dalam mobile apps
- ✅ Scene management di Unity

---

## 📈 Fitur Potential untuk Future

- 🎵 Sound effects & background music
- 📊 Leaderboard & statistics tracking
- 🌍 Multi-language support
- 🎨 Custom themes/skins
- 🏆 Achievement system
- 📹 Tutorial video

---

**Status:** ✅ Siap untuk Development  
**Last Updated:** 2026-06-09  
**Version:** 1.0.0

---

**Happy Developing! 🎮🐾**
