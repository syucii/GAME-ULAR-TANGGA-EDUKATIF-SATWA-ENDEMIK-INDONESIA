using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using GameUlarTangga.Data;

namespace GameUlarTangga.AR
{
    /// <summary>
    /// Manager untuk menangani deteksi marker Vuforia
    /// </summary>
    public class ARMarkerDetector : MonoBehaviour
    {
        [SerializeField] private Canvas quizCanvas; // Canvas untuk menampilkan soal
        [SerializeField] private Button backButton; // Tombol kembali ke scan
        
        private ObserverBehaviour markerObserver;
        private AnimalQuestion currentAnimal;
        private bool isMarkerDetected = false;

        private void Start()
        {
            markerObserver = GetComponent<ObserverBehaviour>();
            if (markerObserver != null)
            {
                markerObserver.OnTargetStatusChanged += OnMarkerStatusChanged;
            }
            
            if (quizCanvas != null)
                quizCanvas.enabled = false;

            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonPressed);
        }

        private void OnDestroy()
        {
            if (markerObserver != null)
                markerObserver.OnTargetStatusChanged -= OnMarkerStatusChanged;
        }

        /// <summary>
        /// Callback ketika status marker berubah (terdeteksi/hilang)
        /// </summary>
        private void OnMarkerStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
        {
            if (status.Status == TargetStatus.StatusType.TRACKED)
            {
                OnMarkerDetected(behaviour.TargetName);
            }
            else if (status.Status == TargetStatus.StatusType.NO_POSE)
            {
                OnMarkerLost();
            }
        }

        /// <summary>
        /// Dipanggil saat marker terdeteksi
        /// </summary>
        private void OnMarkerDetected(string markerName)
        {
            if (isMarkerDetected)
                return;

            isMarkerDetected = true;
            Debug.Log($"[ARMarkerDetector] Marker detected: {markerName}");

            // Ambil data hewan dari marker name
            currentAnimal = AnimalDataManager.Instance.GetAnimalByMarkerName(markerName);
            
            if (currentAnimal != null)
            {
                DisplayQuiz(currentAnimal);
            }
            else
            {
                Debug.LogWarning($"[ARMarkerDetector] No animal data found for marker: {markerName}");
            }
        }

        /// <summary>
        /// Dipanggil saat marker hilang dari view
        /// </summary>
        private void OnMarkerLost()
        {
            isMarkerDetected = false;
            Debug.Log("[ARMarkerDetector] Marker lost");
            
            if (quizCanvas != null)
                quizCanvas.enabled = false;

            currentAnimal = null;
        }

        /// <summary>
        /// Tampilkan quiz untuk hewan yang terdeteksi
        /// </summary>
        private void DisplayQuiz(AnimalQuestion animal)
        {
            if (quizCanvas == null)
            {
                Debug.LogError("[ARMarkerDetector] Quiz Canvas not assigned");
                return;
            }

            quizCanvas.enabled = true;

            // Set informasi quiz
            var questionText = quizCanvas.GetComponentInChildren<Text>();
            if (questionText != null)
                questionText.text = animal.question;

            // Set gambar hewan (jika ada Image component)
            var animalImage = quizCanvas.GetComponentInChildren<Image>();
            if (animalImage != null)
            {
                var texture = Resources.Load<Texture2D>($"Images/{animal.imageFileName}");
                if (texture != null)
                    animalImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }

            // Setup tombol jawaban
            SetupAnswerButtons(animal);
        }

        /// <summary>
        /// Setup tombol untuk setiap opsi jawaban
        /// </summary>
        private void SetupAnswerButtons(AnimalQuestion animal)
        {
            // Cari semua button di dalam canvas
            Button[] buttons = quizCanvas.GetComponentsInChildren<Button>();
            
            // Pastikan ada cukup button untuk semua opsi
            for (int i = 0; i < animal.answerOptions.Count && i < buttons.Length - 1; i++)
            {
                int answerIndex = i;
                Button btn = buttons[i];
                
                // Set text button
                var btnText = btn.GetComponentInChildren<Text>();
                if (btnText != null)
                    btnText.text = animal.answerOptions[i];

                // Bersihkan listener sebelumnya
                btn.onClick.RemoveAllListeners();
                
                // Add listener untuk button
                btn.onClick.AddListener(() => OnAnswerSelected(animal, answerIndex));
            }
        }

        /// <summary>
        /// Callback saat pemain memilih jawaban
        /// </summary>
        private void OnAnswerSelected(AnimalQuestion animal, int selectedIndex)
        {
            bool isCorrect = selectedIndex == animal.correctAnswerIndex;
            
            Debug.Log($"[ARMarkerDetector] Answer selected: {selectedIndex}, Correct: {isCorrect}");

            // Nonaktifkan semua button jawaban
            Button[] buttons = quizCanvas.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                if (btn != backButton)
                    btn.interactable = false;
            }

            ShowResultUI(animal, isCorrect);
        }

        /// <summary>
        /// Tampilkan hasil jawaban (benar/salah)
        /// </summary>
        private void ShowResultUI(AnimalQuestion animal, bool isCorrect)
        {
            // Implement tampilan hasil sesuai UI design
            Debug.Log($"[ARMarkerDetector] Show result: {(isCorrect ? "Correct" : "Wrong")}");
            Debug.Log($"Description: {animal.correctDescription}");
            Debug.Log($"Fun Fact: {animal.funFact}");
        }

        /// <summary>
        /// Callback tombol kembali
        /// </summary>
        private void OnBackButtonPressed()
        {
            isMarkerDetected = false;
            
            if (quizCanvas != null)
                quizCanvas.enabled = false;

            Debug.Log("[ARMarkerDetector] Back button pressed - returning to marker scan");
        }
    }
}
