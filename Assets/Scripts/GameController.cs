using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI finalScoreText;  // 胜利界面中的分数

    [Header("分数界面")]
    public GameObject gameScorePanelPrefab;  // 游戏分数界面的预制件
    [Header("失败界面")]
    public GameObject gameOverPanelPrefab;  // 游戏失败界面的预制件
    [Header("胜利界面")]
    public GameObject gameVictoryPanelPrefab;  // 游戏胜利界面的预制件

    public static GameController Instance;

    private GameObject gameScorePanel;  // 游戏分数界面
    private GameObject gameOverPanel;  // 游戏失败界面
    private GameObject gameVictoryPanel;  // 游戏胜利界面

    public GameObject GameScorePanel  // 游戏分数界面的访问器
    {
        get
        {
            if (gameScorePanel == null)
            {
                gameScorePanel = Instantiate(gameScorePanelPrefab, Instance.transform);
            }
            return gameScorePanel;
        }
    }
    public GameObject GameOverPanel  // 游戏失败界面的访问器
    {
        get
        {
            if (gameOverPanel == null)
            {
                gameOverPanel = Instantiate(gameOverPanelPrefab, Instance.transform);
            }
            return gameOverPanel;
        }
    }
    public GameObject GameVictoryPanel  // 游戏胜利界面的访问器
    {
        get
        {
            if (gameVictoryPanel == null)
            {
                gameVictoryPanel = Instantiate(gameVictoryPanelPrefab, Instance.transform);
            }
            return gameVictoryPanel;
        }
    }

    private int score = 0;  // 分数
    private int finalScore = 0;  // 最终分数

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(GameController.Instance);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level3") && Instance.GameScorePanel.activeSelf)
        {
            Instance.GameScorePanel.SetActive(false);
        }
    }

    // 获得分数
    public void GainScore(int value)
    {
        score += value;
        Instance.GameScorePanel.GetComponentInChildren<Text>().text = Instance.score.ToString();
    }

    // 显示游戏失败界面
    public void ShowGameOverPanel()
    {
        Instance.GameOverPanel.SetActive(true);
    }

    // 显示游戏胜利界面
    public void ShowGameVictoryPanel()
    {
        Instance.GameVictoryPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Score: " + score;
        //finalScoreText.text += score;
        Instance.GameVictoryPanel.SetActive(true);
    }

    // 进入关卡，或回到主界面
    public void StartLevel(string levelName)
    {
        if (levelName.Equals("Welcome"))
        {
            // 如果是主界面，分数清零，隐藏分数界面
            Instance.finalScore = 0;
            Instance.score = 0;
            Instance.GainScore(0);
            Instance.GameScorePanel.SetActive(false);
        }
        else
        {
            // 如果是关卡，分数刷新，显示分数界面
            Instance.finalScore = Instance.score;
            Instance.GainScore(0);
            Instance.GameScorePanel.SetActive(true);
        }
        // 隐藏胜利界面和失败界面
        Instance.GameOverPanel.SetActive(false);
        Instance.GameVictoryPanel.SetActive(false);

        SceneManager.LoadScene(levelName);
    }

    // 重启当前关卡
    public void RestartLevel()
    {
        // 隐藏胜利界面和失败界面
        Instance.GameOverPanel.SetActive(false);
        Instance.GameVictoryPanel.SetActive(false);
        // 重置分数
        Instance.score = Instance.finalScore;
        Instance.GainScore(0);

        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
    }

    // 结束游戏
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}