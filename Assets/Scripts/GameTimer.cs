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

    [Header("Componentes que se apagan al perder")]
    [SerializeField] private Behaviour[] componentsToDisable;

    private float timeRemaining;
    private bool gameOver;

    private void Start()
    {
        Time.timeScale = 1f;
        timeRemaining = totalTime;
        gameOver = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateTimerText();
    }

    private void Update()
    {
        if (gameOver)
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
        if (gameOver)
        {
            Time.timeScale = 1f;
        }
    }

    private void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0f;

        foreach (Behaviour component in componentsToDisable)
        {
            if (component != null)
            {
                component.enabled = false;
            }
        }

        XRBaseInteractable[] interactables = FindObjectsByType<XRBaseInteractable>(FindObjectsInactive.Include,FindObjectsSortMode.None);

        foreach (XRBaseInteractable interactable in interactables)
        {
           
            if (gameOverPanel != null && interactable.transform.IsChildOf(gameOverPanel.transform))
            {
                continue;
            }

            interactable.enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}