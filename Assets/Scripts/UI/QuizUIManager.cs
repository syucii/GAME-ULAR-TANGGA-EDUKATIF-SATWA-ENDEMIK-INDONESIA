using UnityEngine;
using UnityEngine.UI;
using GameUlarTangga.Data;
using GameUlarTangga.Core;

namespace GameUlarTangga.UI
{
    /// <summary>
    /// Manager untuk quiz UI dan menampilkan pertanyaan + pilihan jawaban
    /// </summary>
    public class QuizUIManager : MonoBehaviour
    {
        [Header("Quiz UI Elements")]
        [SerializeField] private Text animalNameText; // Nama hewan (judul)
        [SerializeField] private Text questionText; // Pertanyaan quiz
        [SerializeField] private Image animalImage; // Gambar hewan

        [Header("Answer Buttons")]
        [SerializeField] private Button[] answerButtons = new Button[3]; // 3 tombol jawaban
        [SerializeField] private Text[] answerTexts = new Text[3]; // Text untuk setiap tombol

        [Header("Canvas")]
        [SerializeField] private Canvas quizCanvas;

        private AnimalQuestion currentAnimal;
        private bool hasAnswered = false;

        private void Start()
        {
            if (quizCanvas != null)
                quizCanvas.enabled = false;

            // Setup button listeners
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                {
                    int index = i;
                    answerButtons[i].onClick.AddListener(() => OnAnswerButtonClicked(index));
                }
            }

            // Subscribe ke event GameManager
            GameManager.MarkerDetectedEvent += OnMarkerDetected;
            GameManager.AnswerSubmittedEvent += OnAnswerSubmitted;
        }

        private void OnDestroy()
        {
            GameManager.MarkerDetectedEvent -= OnMarkerDetected;
            GameManager.AnswerSubmittedEvent -= OnAnswerSubmitted;
        }

        /// <summary>
        /// Callback saat marker terdeteksi
        /// </summary>
        private void OnMarkerDetected(AnimalQuestion animal)
        {
            DisplayQuiz(animal);
        }

        /// <summary>
        /// Tampilkan quiz untuk hewan tertentu
        /// </summary>
        private void DisplayQuiz(AnimalQuestion animal)
        {
            if (animal == null)
            {
                Debug.LogError("[QuizUIManager] Animal is null");
                return;
            }

            currentAnimal = animal;
            hasAnswered = false;

            // Set animal name
            if (animalNameText != null)
                animalNameText.text = animal.animalName;

            // Set question
            if (questionText != null)
                questionText.text = animal.question;

            // Load dan set gambar hewan
            LoadAnimalImage(animal.imageFileName);

            // Setup answer buttons
            SetupAnswerButtons(animal);

            // Tampilkan canvas
            if (quizCanvas != null)
                quizCanvas.enabled = true;

            Debug.Log($"[QuizUIManager] Displaying quiz for: {animal.animalName}");
        }

        /// <summary>
        /// Load gambar hewan dari Resources
        /// </summary>
        private void LoadAnimalImage(string imageFileName)
        {
            if (animalImage == null)
                return;

            string imagePath = $"Images/{System.IO.Path.GetFileNameWithoutExtension(imageFileName)}";
            Texture2D texture = Resources.Load<Texture2D>(imagePath);

            if (texture != null)
            {
                animalImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    Vector2.zero
                );
                Debug.Log($"[QuizUIManager] Image loaded: {imagePath}");
            }
            else
            {
                Debug.LogWarning($"[QuizUIManager] Image not found: {imagePath}");
            }
        }

        /// <summary>
        /// Setup 3 tombol jawaban
        /// </summary>
        private void SetupAnswerButtons(AnimalQuestion animal)
        {
            for (int i = 0; i < Mathf.Min(3, animal.answerOptions.Count); i++)
            {
                if (answerTexts[i] != null)
                    answerTexts[i].text = animal.answerOptions[i];

                if (answerButtons[i] != null)
                {
                    answerButtons[i].interactable = true;
                    
                    // Ubah warna kembali ke normal
                    ColorBlock colors = answerButtons[i].colors;
                    colors.normalColor = Color.white;
                    answerButtons[i].colors = colors;
                }
            }

            Debug.Log($"[QuizUIManager] Answer buttons setup for {animal.animalName}");
        }

        /// <summary>
        /// Callback saat tombol jawaban diklik
        /// </summary>
        private void OnAnswerButtonClicked(int selectedIndex)
        {
            if (hasAnswered || currentAnimal == null)
                return;

            hasAnswered = true;

            // Disable semua button
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                    answerButtons[i].interactable = false;
            }

            // Check jawaban
            bool isCorrect = selectedIndex == currentAnimal.correctAnswerIndex;

            // Ubah warna button yang dipilih
            if (answerButtons[selectedIndex] != null)
            {
                ColorBlock colors = answerButtons[selectedIndex].colors;
                colors.normalColor = isCorrect ? Color.green : Color.red;
                answerButtons[selectedIndex].colors = colors;
            }

            Debug.Log($"[QuizUIManager] Answer selected: {selectedIndex}, Correct: {isCorrect}");

            // Panggil GameManager untuk handle hasil
            GameManager.Instance.OnAnswerSubmitted(isCorrect);
        }

        /// <summary>
        /// Callback saat jawaban sudah di-submit (untuk UI feedback)
        /// </summary>
        private void OnAnswerSubmitted(AnimalQuestion animal, bool isCorrect)
        {
            // Bisa ditambahkan delay sebelum tampil result
            Debug.Log($"[QuizUIManager] Result shown: {(isCorrect ? "Correct" : "Wrong")}");
        }

        /// <summary>
        /// Sembunyikan quiz UI
        /// </summary>
        public void HideQuizUI()
        {
            if (quizCanvas != null)
                quizCanvas.enabled = false;

            hasAnswered = false;
        }

        /// <summary>
        /// Reset quiz untuk soal berikutnya
        /// </summary>
        public void ResetForNextQuiz()
        {
            HideQuizUI();
            
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                {
                    answerButtons[i].interactable = true;
                    ColorBlock colors = answerButtons[i].colors;
                    colors.normalColor = Color.white;
                    answerButtons[i].colors = colors;
                }
            }

            hasAnswered = false;
            currentAnimal = null;
        }
    }
}
