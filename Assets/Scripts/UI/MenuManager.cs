using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameUlarTangga.UI
{
    /// <summary>
    /// Manager untuk navigasi menu utama
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button startButton; // Tombol Mulai
        [SerializeField] private Button instructionButton; // Tombol Instruksi
        [SerializeField] private Button aboutButton; // Tombol Tentang
        [SerializeField] private Button backButton; // Tombol Kembali (dari submenu)

        [Header("Panels")]
        [SerializeField] private Canvas menuCanvas; // Canvas menu utama
        [SerializeField] private Canvas instructionCanvas; // Canvas instruksi
        [SerializeField] private Canvas aboutCanvas; // Canvas tentang aplikasi

        private void Start()
        {
            // Subscribe tombol menu utama
            if (startButton != null)
                startButton.onClick.AddListener(OnStartPressed);
            
            if (instructionButton != null)
                instructionButton.onClick.AddListener(OnInstructionPressed);
            
            if (aboutButton != null)
                aboutButton.onClick.AddListener(OnAboutPressed);

            if (backButton != null)
                backButton.onClick.AddListener(OnBackPressed);

            // Set initial state
            ShowMainMenu();
        }

        /// <summary>
        /// Tombol Mulai Permainan - ke AR Scan Scene
        /// </summary>
        private void OnStartPressed()
        {
            Debug.Log("[MenuManager] Start button pressed");
            
            // Load scene AR
            SceneManager.LoadScene("ARScene");
        }

        /// <summary>
        /// Tombol Instruksi - tampilkan instruksi
        /// </summary>
        private void OnInstructionPressed()
        {
            Debug.Log("[MenuManager] Instruction button pressed");
            
            if (menuCanvas != null)
                menuCanvas.enabled = false;
            
            if (instructionCanvas != null)
                instructionCanvas.enabled = true;
        }

        /// <summary>
        /// Tombol Tentang - tampilkan informasi aplikasi
        /// </summary>
        private void OnAboutPressed()
        {
            Debug.Log("[MenuManager] About button pressed");
            
            if (menuCanvas != null)
                menuCanvas.enabled = false;
            
            if (aboutCanvas != null)
                aboutCanvas.enabled = true;
        }

        /// <summary>
        /// Tombol Kembali - kembali ke menu utama
        /// </summary>
        private void OnBackPressed()
        {
            Debug.Log("[MenuManager] Back button pressed");
            ShowMainMenu();
        }

        /// <summary>
        /// Tampilkan menu utama dan sembunyikan submenu
        /// </summary>
        private void ShowMainMenu()
        {
            if (menuCanvas != null)
                menuCanvas.enabled = true;
            
            if (instructionCanvas != null)
                instructionCanvas.enabled = false;
            
            if (aboutCanvas != null)
                aboutCanvas.enabled = false;
        }
    }
}
