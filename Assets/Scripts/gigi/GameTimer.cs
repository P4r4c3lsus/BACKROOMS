using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GameTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float totalTime = 300f;
    [SerializeField] private TMP_Text timerText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Demo complete")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Componentes que se apagan al perder")]
    [SerializeField] private Behaviour[] componentsToDisable;

    private float timeRemaining;
    private bool gameOver;
    private bool demoCompleted;

    public bool IsRunning => !gameOver && !demoCompleted;
    public bool DemoCompleted => demoCompleted;

    private void Start()
    {
        Time.timeScale = 1f;
        timeRemaining = totalTime;
        gameOver = false;
        demoCompleted = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        UpdateTimerText();
    }

    private void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            EndGame();
            return;
        }

        UpdateTimerText();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnDestroy()
    {
        if (gameOver || demoCompleted)
        {
            Time.timeScale = 1f;
        }
    }

    private void EndGame()
    {
        if (!IsRunning) return;
        gameOver = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("GameOverMusic");
        }

        StopGameplay(gameOverPanel);
    }

    public void CompleteDemo()
    {
        if (!IsRunning) return;
        demoCompleted = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        StopGameplay(victoryPanel);
    }

    private void StopGameplay(GameObject resultPanel)
    {
        Time.timeScale = 0f;
        if (timerText != null) timerText.gameObject.SetActive(false);

        foreach (Behaviour component in componentsToDisable ?? System.Array.Empty<Behaviour>())
        {
            if (component != null)
            {
                component.enabled = false;
            }
        }

        XRBaseInteractable[] interactables = FindObjectsByType<XRBaseInteractable>(FindObjectsInactive.Include,FindObjectsSortMode.None);

        foreach (XRBaseInteractable interactable in interactables)
        {
            if (interactable.gameObject.scene != gameObject.scene) continue;
            if (resultPanel != null && interactable.transform.IsChildOf(resultPanel.transform))
            {
                continue;
            }

            interactable.enabled = false;
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }
    }
}
