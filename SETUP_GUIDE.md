# 🎮 Setup Guide - AR Game Ular Tangga

Panduan lengkap untuk setup aplikasi di Unity setelah download semua script dan data.

---

## 📋 Checklist Setup

- [ ] Import Vuforia ke Unity
- [ ] Setup 20 Marker di Vuforia
- [ ] Setup Folder Structure
- [ ] Copy Semua Script
- [ ] Copy JSON Data
- [ ] Setup Scenes
- [ ] Configure GameObject

---

## 1️⃣ Import Vuforia

### Download & Install
1. Download Vuforia Engine dari [developer.vuforia.com](https://developer.vuforia.com)
2. Pilih Unity Package
3. Extract dan import ke project Unity Anda

### Setup License Key
1. Login ke Vuforia Developer Portal
2. Buat License Key baru
3. Copy License Key
4. Di Unity: `Vuforia > Open Vuforia Engine` 
5. Paste License Key di inspector

### Create Target Database
1. Di Vuforia portal: Create Database
2. Nama: "IndonesianAnimals" (atau pilihan Anda)
3. Type: "Device"
4. Tambah 20 Image Target dengan format:
   ```
   Marker_01.jpg (gambar marker)
   Marker_02.jpg
   ...
   Marker_20.jpg
   ```
5. Download Unity Editor package
6. Import ke project

---

## 2️⃣ Folder Structure Setup

Buat struktur folder di `Assets/`:

```
Assets/
├── Resources/
│   ├── animals_data.json          ✅ (sudah ada)
│   └── Images/                    
│       ├── komodo.jpg             (letakkan 20 file gambar di sini)
│       ├── orangutan.jpg
│       └── ... (18 lainnya)
│
├── Scripts/
│   ├── Data/
│   │   └── AnimalData.cs          ✅ (sudah ada)
│   ├── AR/
│   │   └── ARMarkerDetector.cs    ✅ (sudah ada)
│   ├── UI/
│   │   ├── MenuManager.cs         ✅ (sudah ada)
│   │   ├── ResultUIManager.cs     ✅ (sudah ada)
│   │   └── QuizUIManager.cs       ✅ (sudah ada)
│   └── Core/
│       └── GameManager.cs         ✅ (sudah ada)
│
└── Scenes/
    ├── HomeScene.unity            (create baru)
    ├── MenuScene.unity            (create baru)
    └── ARScene.unity              (create baru)
```

---

## 3️⃣ Scene Setup Detail

### HomeScene
```
Hierarchy:
├── Canvas
│   ├── Background (Image)
│   ├── AppTitle (Text) - "Game Ular Tangga"
│   └── StartButton (Button) - "MULAI"
│
└── Scripts:
    └── (None, hanya navigate ke MenuScene)
```

**Setup Button:**
1. Select StartButton
2. Add `On Click()` event
3. Drag scene name atau use SceneManager.LoadScene("MenuScene")

---

### MenuScene
```
Hierarchy:
├── Canvas - MenuCanvas (active by default)
│   ├── Title (Text)
│   ├── StartGameButton (Button)
│   ├── InstructionButton (Button)
│   ├── AboutButton (Button)
│   └── BackButton (Button) - di setiap submenu
│
├── Canvas - InstructionCanvas (disabled)
│   ├── ScrollView
│   │   └── Text (instruksi permainan)
│   └── BackButton
│
├── Canvas - AboutCanvas (disabled)
│   ├── ScrollView
│   │   └── Text (tentang aplikasi)
│   └── BackButton
│
└── MenuManager (Script) ✅
    - Assign ketiga Canvas di inspector
```

**Setup Buttons:**
1. Select StartGameButton → On Click → Load Scene "ARScene"
2. Select InstructionButton → On Click → MenuManager.OnInstructionPressed()
3. Select AboutButton → On Click → MenuManager.OnAboutPressed()
4. Semua BackButton → On Click → MenuManager.OnBackPressed()

---

### ARScene (Paling Penting!)
```
Hierarchy:
├── ARCamera (dari Vuforia)
│   └── (Vuforia components)
│
├── ImageTarget (dari Vuforia Database)
│   ├── Marker_01
│   ├── Marker_02
│   ├── ... (20 marker total)
│   └── (setiap ImageTarget punya script ARMarkerDetector)
│
├── Canvas - QuizCanvas (disabled by default)
│   ├── AnimalNameText (Text)
│   ├── QuestionText (Text)
│   ├── AnimalImage (Image)
│   ├── AnswerButton_1 (Button + Text child)
│   ├── AnswerButton_2 (Button + Text child)
│   ├── AnswerButton_3 (Button + Text child)
│   └── BackButton (Button)
│
├── Canvas - ResultCanvas (disabled)
│   ├── ResultTitleText (Text)
│   ├── ResultImage (Image)
│   ├── DescriptionText (Text)
│   ├── FunFactText (Text)
│   ├── RescanButton (Button)
│   └── ExitButton (Button)
│
└── Scripts/Managers:
    ├── GameManager (GameObject) ✅
    │   - Singleton
    │   - DontDestroyOnLoad
    │
    ├── QuizUIManager ✅
    │   - Assign semua UI elements
    │
    └── ResultUIManager ✅
        - Assign semua UI elements
```

---

## 4️⃣ GameObject Setup Detail

### ARScene - ImageTarget Setup

Untuk setiap 20 ImageTarget (Marker_01 s/d Marker_20):

1. **Select ImageTarget** (dari Vuforia)
2. **Add Component → ARMarkerDetector.cs**
3. **Di Inspector:**
   ```
   Quiz Canvas: (drag dari scene)
   Back Button: (dari QuizCanvas → BackButton)
   ```

4. **Vuforia Component:**
   - Target Name harus sesuai JSON: `Marker_01`, `Marker_02`, dst
   - Extract dari database yang di-download

### Canvas - QuizCanvas Setup

```
Text Elements:
├── Animal Name Text
├── Question Text
└── Button Answer Texts (3 pieces)

Image:
└── Animal Image (Aspect Ratio: Fit Inside)

Buttons (3 pieces):
├── AnswerButton_1
│   └── Text (child)
├── AnswerButton_2
│   └── Text (child)
└── AnswerButton_3
    └── Text (child)
```

**Setup QuizUIManager:**
1. Create empty GameObject: "QuizUIManager"
2. Add Component → QuizUIManager.cs
3. Di Inspector assign:
   ```
   Animal Name Text: (drag Text)
   Question Text: (drag Text)
   Animal Image: (drag Image)
   Answer Buttons: (drag 3 button ke array)
   Answer Texts: (drag 3 text ke array)
   Quiz Canvas: (drag Canvas)
   ```

### Canvas - ResultCanvas Setup

**Setup ResultUIManager:**
1. Create empty GameObject: "ResultUIManager"
2. Add Component → ResultUIManager.cs
3. Di Inspector assign:
   ```
   Result Image: (drag Image)
   Result Title: (drag Text)
   Description Text: (drag Text)
   Fun Fact Text: (drag Text)
   Rescan Button: (drag Button)
   Exit Button: (drag Button)
   Result Canvas: (drag Canvas)
   ```

### GameManager Setup

1. Create empty GameObject: "GameManager"
2. Add Component → GameManager.cs
3. Make it Singleton:
   ```csharp
   // Script sudah implement singleton
   // Pastikan hanya 1 GameManager di scene
   ```
4. Di Inspector:
   ```
   Result UI Manager: (drag ResultUIManager GameObject)
   ```

---

## 5️⃣ Konfigurasi Script

### AnimalDataManager
- **Otomatis load** `animals_data.json` dari Resources
- **Tidak perlu setup manual**
- Cek Console untuk konfirmasi

### ARMarkerDetector
- Attach ke setiap ImageTarget
- Otomatis detect marker sesuai nama
- Trigger event ke GameManager

### Event Flow
```
Marker Detected
    ↓ (ARMarkerDetector.OnMarkerDetected)
QuizUIManager.OnMarkerDetected (via event)
    ↓
Display Quiz
    ↓
User Click Answer
    ↓
QuizUIManager.OnAnswerButtonClicked
    ↓
GameManager.OnAnswerSubmitted
    ↓
ResultUIManager.ShowCorrectAnswer / ShowWrongAnswer
    ↓
Tampilkan Result Canvas
```

---

## 6️⃣ Testing Checklist

- [ ] Load HomeScene - click StartButton ke MenuScene
- [ ] MenuScene - test semua button navigation
- [ ] Click "Mulai Permainan" → load ARScene
- [ ] ARScene - Vuforia ARCamera render dengan baik
- [ ] Show marker ke kamera → marker detect & quiz tampil
- [ ] Click jawaban → result tampil dengan benar
- [ ] Click Rescan → kembali ke marker scan
- [ ] Click Exit → keluar aplikasi
- [ ] Test 20 marker - semua bisa terdeteksi

---

## 7️⃣ Build Settings

```
Build Scenes (drag ke Build Settings):
1. HomeScene
2. MenuScene
3. ARScene

Player Settings:
- Minimum API Level: 21 (Android)
- Orientation: Portrait
- Camera Permission: Yes
```

---

## ⚠️ Troubleshooting

### Marker tidak terdeteksi
- ✓ Marker quality di Vuforia: minimal 3 bintang
- ✓ Lighting: cukup terang
- ✓ Target name di ImageTarget sesuai JSON
- ✓ Camera permission Android

### JSON data tidak load
- ✓ File path: `Assets/Resources/animals_data.json`
- ✓ Nama file: `animals_data.json` (exact)
- ✓ Format JSON: valid (test di jsonlint.com)

### Gambar tidak tampil
- ✓ Path: `Assets/Resources/Images/nama_file.jpg`
- ✓ Filename di JSON = actual filename
- ✓ Format: JPG atau PNG

### Event tidak trigger
- ✓ GameManager ada di scene
- ✓ Script subscribe ke event di Start()
- ✓ Check Console untuk error message

---

## 📱 Build & Deploy

### Android APK
```
File > Build Settings
- Scenes: (add 3 scenes)
- Platform: Android
- Build
```

### iOS
```
File > Build Settings
- Platform: iOS
- Build to Xcode
- Open di Xcode → Build & Run
```

---

## 🎯 Final Checklist

- [ ] Semua 20 marker terdeteksi dengan baik
- [ ] Quiz tampil sesuai marker yang di-scan
- [ ] Jawaban benar/salah tercatat di GameManager
- [ ] Result UI tampil sesuai jawaban
- [ ] Navigation bekerja di semua scene
- [ ] Tidak ada error di Console
- [ ] APK/IPA bisa di-build
- [ ] Game bisa di-play di device

---

**Selamat! Aplikasi Anda siap untuk dimainkan!** 🎉

Jika ada yang kurang jelas atau error, check console log dan dokumentasi kode di setiap script.
