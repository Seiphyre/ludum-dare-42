using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainGameplayUI : MonoBehaviour
{
    public static MainGameplayUI Instance;

    public TextMeshProUGUI ItemCountText;

    public GameObject GameOverCanvas;
    public Selectable RetryButton;

    public GameObject WinCanvas;
    public Selectable NextLevelButton;

    public GameObject FinalWinCanvas;
    public Selectable FinalWinButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void SetItemCount(int count)
    {
        ItemCountText.text = count.ToString();
    }

    public void ShowGameOverCanvas()
    {
        GameOverCanvas.SetActive(true);
        RetryButton.Select();
    }

    public void ShowWinCanvas()
    {
        if (LevelManager.Instance.HasNextLevel())
        {
            WinCanvas.SetActive(true);
            NextLevelButton.Select();
        }
        else
        {
            FinalWinCanvas.SetActive(true);
            FinalWinButton.Select();
        }
    }

    public void HideWinCanvas()
    {
        WinCanvas.SetActive(false);
    }

    public void HideGameOver()
    {
        GameOverCanvas.SetActive(false);
    }

    public void NextLevel()
    {
        LevelManager.Instance.StartNextLevel();
        HideGameOver();
        HideWinCanvas();
    }

    public void Retry()
    {
        LevelManager.Instance.RetryCurrentLevel();
        HideGameOver();
        HideWinCanvas();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
