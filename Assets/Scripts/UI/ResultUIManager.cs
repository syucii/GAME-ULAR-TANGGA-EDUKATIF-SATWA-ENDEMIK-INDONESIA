using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameUlarTangga.Data;

namespace GameUlarTangga.UI
{
    /// <summary>
    /// Manager untuk UI hasil jawaban (Benar/Salah)
    /// </summary>
    public class ResultUIManager : MonoBehaviour
    {
        [Header("Result UI Elements")]
        [SerializeField] private Image resultImage; // Gambar hewan
        [SerializeField] private Text resultTitle; // Judul (Benar! / Salah!)
        [SerializeField] private Text descriptionText; // Deskripsi/Jawaban benar
        [SerializeField] private Text funFactText; // Fun fact
        
        [Header("Buttons")]
        [SerializeField] private Button rescanButton; // Kembali ke scan AR
        [SerializeField] private Button exitButton; // Keluar dari aplikasi
        
        [Header("Canvas")]
        [SerializeField] private Canvas resultCanvas;
        
        [Header("Colors")]
        [SerializeField] private Color correctColor = Color.green;
        [SerializeField] private Color wrongColor = Color.red;

        private void Start()
        {
            if (rescanButton != null)
                rescanButton.onClick.AddListener(OnRescanPressed);
            
            if (exitButton != null)
                exitButton.onClick.AddListener(OnExitPressed);

            if (resultCanvas != null)
                resultCanvas.enabled = false;
        }

        /// <summary>
        /// Tampilkan hasil jawaban BENAR
        /// </summary>
        public void ShowCorrectAnswer(AnimalQuestion animal)
        {
            if (resultCanvas != null)
                resultCanvas.enabled = true;

            resultTitle.text = "Jawaban Benar! ✓";
            resultTitle.color = correctColor;
            
            descriptionText.text = $"Funfact:\n{animal.funFact}";
            funFactText.text = $"Nama Hewan: {animal.animalName}\nAsal: {animal.region}";

            LoadAnimalImage(animal.imageFileName);
        }

        /// <summary>
        /// Tampilkan hasil jawaban SALAH
        /// </summary>
        public void ShowWrongAnswer(AnimalQuestion animal)
        {
            if (resultCanvas != null)
                resultCanvas.enabled = true;

            resultTitle.text = "Jawaban Salah! ✗";
            resultTitle.color = wrongColor;
            
            descriptionText.text = $"Jawaban Benar: {animal.answerOptions[animal.correctAnswerIndex]}\n\nDeskripsi:\n{animal.correctDescription}";
            funFactText.text = $"Nama Hewan: {animal.animalName}\nAsal: {animal.region}";

            LoadAnimalImage(animal.imageFileName);
        }

        /// <summary>
        /// Load gambar hewan dari Resources
        /// </summary>
        private void LoadAnimalImage(string imageFileName)
        {
            if (resultImage == null)
                return;

            // Coba load dari Resources/Images folder
            var texture = Resources.Load<Texture2D>($"Images/{Path.GetFileNameWithoutExtension(imageFileName)}");
            if (texture != null)
            {
                resultImage.sprite = Sprite.Create(texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    Vector2.zero);
            }
            else
            {
                Debug.LogWarning($"[ResultUIManager] Image not found: Images/{imageFileName}");
            }
        }

        /// <summary>
        /// Callback tombol rescan - kembali ke scene AR
        /// </summary>
        private void OnRescanPressed()
        {
            Debug.Log("[ResultUIManager] Rescan button pressed");
            
            if (resultCanvas != null)
                resultCanvas.enabled = false;

            // Kembali ke AR Scan scene
            // SceneManager.LoadScene("ARScene") atau reload scene saat ini
            Time.timeScale = 1f; // Pastikan time berjalan normal
        }

        /// <summary>
        /// Callback tombol exit - keluar dari aplikasi atau kembali ke menu
        /// </summary>
        private void OnExitPressed()
        {
            Debug.Log("[ResultUIManager] Exit button pressed");
            
            // Option 1: Keluar dari aplikasi
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif

            // Option 2: Kembali ke menu (uncomment jika ada scene menu)
            // SceneManager.LoadScene("MenuScene");
        }

        /// <summary>
        /// Sembunyikan result UI
        /// </summary>
        public void HideResultUI()
        {
            if (resultCanvas != null)
                resultCanvas.enabled = false;
        }
    }
}
