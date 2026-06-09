using System.Collections.Generic;
using UnityEngine;

namespace GameUlarTangga.Data
{
    /// <summary>
    /// Data struktur untuk satu hewan/soal
    /// </summary>
    [System.Serializable]
    public class AnimalQuestion
    {
        public int id; // ID marker 1-20
        public string animalName; // Nama hewan (Komodo, Orangutan, dll)
        public string region; // Asal region (NTT, Kalimantan, dll)
        public string question; // Soal (Komodo dikenal dari wilayah mana?)
        public List<string> answerOptions; // Opsi jawaban [Jawa, Sumatra, NTT]
        public int correctAnswerIndex; // Index jawaban benar (0-2)
        public string correctDescription; // Deskripsi jawaban benar
        public string funFact; // Fun fact hewan tersebut
        public string imageFileName; // Nama file gambar (Komodo.jpg)
        public string markerName; // Nama marker Vuforia (Marker_01, dll)
    }

    /// <summary>
    /// Container untuk semua data hewan
    /// </summary>
    [System.Serializable]
    public class AnimalDataContainer
    {
        public List<AnimalQuestion> animals = new List<AnimalQuestion>();
    }

    /// <summary>
    /// Manager untuk mengelola semua data hewan
    /// </summary>
    public class AnimalDataManager : MonoBehaviour
    {
        public static AnimalDataManager Instance { get; private set; }

        private AnimalDataContainer animalData;
        private Dictionary<int, AnimalQuestion> animalCache = new Dictionary<int, AnimalQuestion>();

        private const string DATA_PATH = "Data/animals"; // Resources/Data/animals.json
        private const string RESOURCES_PATH = "animals_data"; // Resources/animals_data.json

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAnimalData();
        }

        /// <summary>
        /// Load data dari JSON file di Resources
        /// </summary>
        public void LoadAnimalData()
        {
            try
            {
                TextAsset jsonFile = Resources.Load<TextAsset>(RESOURCES_PATH);
                if (jsonFile != null)
                {
                    animalData = JsonUtility.FromJson<AnimalDataContainer>(jsonFile.text);
                    BuildCache();
                    Debug.Log($"[AnimalDataManager] Loaded {animalData.animals.Count} animals");
                }
                else
                {
                    Debug.LogError($"[AnimalDataManager] File tidak ditemukan di Resources: {RESOURCES_PATH}");
                    CreateDefaultData();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AnimalDataManager] Error loading animal data: {ex.Message}");
                CreateDefaultData();
            }
        }

        /// <summary>
        /// Build cache untuk akses cepat berdasarkan ID
        /// </summary>
        private void BuildCache()
        {
            animalCache.Clear();
            foreach (var animal in animalData.animals)
            {
                animalCache[animal.id] = animal;
            }
        }

        /// <summary>
        /// Dapatkan data hewan berdasarkan ID marker
        /// </summary>
        public AnimalQuestion GetAnimalByMarkerId(int markerId)
        {
            if (animalCache.TryGetValue(markerId, out var animal))
            {
                return animal;
            }
            Debug.LogWarning($"[AnimalDataManager] Animal dengan ID {markerId} tidak ditemukan");
            return null;
        }

        /// <summary>
        /// Dapatkan data hewan berdasarkan nama marker Vuforia
        /// </summary>
        public AnimalQuestion GetAnimalByMarkerName(string markerName)
        {
            foreach (var animal in animalData.animals)
            {
                if (animal.markerName.Equals(markerName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return animal;
                }
            }
            Debug.LogWarning($"[AnimalDataManager] Marker dengan nama '{markerName}' tidak ditemukan");
            return null;
        }

        /// <summary>
        /// Dapatkan semua data hewan
        /// </summary>
        public List<AnimalQuestion> GetAllAnimals()
        {
            return new List<AnimalQuestion>(animalData.animals);
        }

        /// <summary>
        /// Tambah hewan baru
        /// </summary>
        public void AddAnimal(AnimalQuestion animal)
        {
            if (animal == null) return;

            animalData.animals.Add(animal);
            animalCache[animal.id] = animal;
            SaveAnimalData();
            Debug.Log($"[AnimalDataManager] Animal '{animal.animalName}' ditambahkan");
        }

        /// <summary>
        /// Update data hewan
        /// </summary>
        public void UpdateAnimal(int animalId, AnimalQuestion updatedAnimal)
        {
            if (animalCache.TryGetValue(animalId, out var animal))
            {
                int index = animalData.animals.IndexOf(animal);
                if (index >= 0)
                {
                    animalData.animals[index] = updatedAnimal;
                    animalCache[animalId] = updatedAnimal;
                    SaveAnimalData();
                    Debug.Log($"[AnimalDataManager] Animal dengan ID {animalId} diupdate");
                }
            }
        }

        /// <summary>
        /// Hapus hewan berdasarkan ID
        /// </summary>
        public void DeleteAnimal(int animalId)
        {
            if (animalCache.TryGetValue(animalId, out var animal))
            {
                animalData.animals.Remove(animal);
                animalCache.Remove(animalId);
                SaveAnimalData();
                Debug.Log($"[AnimalDataManager] Animal dengan ID {animalId} dihapus");
            }
        }

        /// <summary>
        /// Simpan data ke file JSON (untuk editor/debug)
        /// </summary>
        public void SaveAnimalData()
        {
            try
            {
                string jsonData = JsonUtility.ToJson(animalData, true);
                Debug.Log($"[AnimalDataManager] Data disimpan:\n{jsonData}");
                // Implementasi penyimpanan actual bisa menggunakan PlayerPrefs atau StreamingAssets
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AnimalDataManager] Error saving animal data: {ex.Message}");
            }
        }

        /// <summary>
        /// Buat data default untuk testing
        /// </summary>
        private void CreateDefaultData()
        {
            animalData = new AnimalDataContainer();
            Debug.Log("[AnimalDataManager] Default data created");
        }

        public int GetTotalAnimalCount()
        {
            return animalData.animals.Count;
        }
    }
}
