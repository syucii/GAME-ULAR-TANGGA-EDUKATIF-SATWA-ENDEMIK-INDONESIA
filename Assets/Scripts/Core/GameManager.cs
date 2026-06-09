using UnityEngine;
using GameUlarTangga.Data;
using GameUlarTangga.UI;

namespace GameUlarTangga.Core
{
    /// <summary>
    /// Game manager global untuk mengatur state dan event permainan
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [System.Serializable]
        public class GameStats
        {
            public int totalQuestions = 0;
            public int correctAnswers = 0;
            public int wrongAnswers = 0;

            public float GetAccuracy()
            {
                if (totalQuestions == 0)
                    return 0f;
                return (correctAnswers / (float)totalQuestions) * 100f;
            }

            public void Reset()
            {
                totalQuestions = 0;
                correctAnswers = 0;
                wrongAnswers = 0;
            }
        }

        // Delegate untuk event
        public delegate void OnAnswerSubmitted(AnimalQuestion animal, bool isCorrect);
        public delegate void OnMarkerDetected(AnimalQuestion animal);
        public delegate void OnGameSessionEnded();

        // Events
        public static event OnAnswerSubmitted AnswerSubmittedEvent;
        public static event OnMarkerDetected MarkerDetectedEvent;
        public static event OnGameSessionEnded GameSessionEndedEvent;

        [SerializeField] private ResultUIManager resultUIManager;
        
        private GameStats currentStats;
        private AnimalQuestion currentAnimal;
        private bool isQuizActive = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            currentStats = new GameStats();
        }

        private void Start()
        {
            Debug.Log("[GameManager] Game Manager initialized");
        }

        /// <summary>
        /// Dipanggil saat marker terdeteksi dan soal ditampilkan
        /// </summary>
        public void OnMarkerDetectedHandler(AnimalQuestion animal)
        {
            currentAnimal = animal;
            isQuizActive = true;
            
            Debug.Log($"[GameManager] Marker detected for: {animal.animalName}");
            
            // Trigger event
            MarkerDetectedEvent?.Invoke(animal);
        }

        /// <summary>
        /// Dipanggil saat pemain menjawab soal
        /// </summary>
        public void OnAnswerSubmitted(bool isCorrect)
        {
            if (!isQuizActive || currentAnimal == null)
                return;

            isQuizActive = false;
            currentStats.totalQuestions++;

            if (isCorrect)
            {
                currentStats.correctAnswers++;
                if (resultUIManager != null)
                    resultUIManager.ShowCorrectAnswer(currentAnimal);
            }
            else
            {
                currentStats.wrongAnswers++;
                if (resultUIManager != null)
                    resultUIManager.ShowWrongAnswer(currentAnimal);
            }

            Debug.Log($"[GameManager] Answer: {(isCorrect ? "CORRECT" : "WRONG")}");
            Debug.Log($"[GameManager] Stats - Total: {currentStats.totalQuestions}, Correct: {currentStats.correctAnswers}, Accuracy: {currentStats.GetAccuracy():F1}%");

            // Trigger event
            AnswerSubmittedEvent?.Invoke(currentAnimal, isCorrect);
        }

        /// <summary>
        /// Akhiri sesi permainan
        /// </summary>
        public void EndGameSession()
        {
            isQuizActive = false;
            Debug.Log($"[GameManager] Game session ended. Final Stats: {currentStats.totalQuestions} questions, {currentStats.correctAnswers} correct");
            
            GameSessionEndedEvent?.Invoke();
        }

        /// <summary>
        /// Dapatkan statistik permainan saat ini
        /// </summary>
        public GameStats GetCurrentStats()
        {
            return currentStats;
        }

        /// <summary>
        /// Reset statistik permainan
        /// </summary>
        public void ResetStats()
        {
            currentStats.Reset();
            Debug.Log("[GameManager] Game stats reset");
        }

        /// <summary>
        /// Simpan statistik ke PlayerPrefs (optional)
        /// </summary>
        public void SaveStats()
        {
            PlayerPrefs.SetInt("GameStats_Total", currentStats.totalQuestions);
            PlayerPrefs.SetInt("GameStats_Correct", currentStats.correctAnswers);
            PlayerPrefs.SetInt("GameStats_Wrong", currentStats.wrongAnswers);
            PlayerPrefs.Save();
            
            Debug.Log("[GameManager] Game stats saved to PlayerPrefs");
        }

        /// <summary>
        /// Load statistik dari PlayerPrefs (optional)
        /// </summary>
        public void LoadStats()
        {
            if (PlayerPrefs.HasKey("GameStats_Total"))
            {
                currentStats.totalQuestions = PlayerPrefs.GetInt("GameStats_Total");
                currentStats.correctAnswers = PlayerPrefs.GetInt("GameStats_Correct");
                currentStats.wrongAnswers = PlayerPrefs.GetInt("GameStats_Wrong");
                
                Debug.Log("[GameManager] Game stats loaded from PlayerPrefs");
            }
        }

        /// <summary>
        /// Dapatkan hewan yang sedang aktif di quiz
        /// </summary>
        public AnimalQuestion GetCurrentAnimal()
        {
            return currentAnimal;
        }

        /// <summary>
        /// Check apakah quiz sedang aktif
        /// </summary>
        public bool IsQuizActive()
        {
            return isQuizActive;
        }
    }
}
