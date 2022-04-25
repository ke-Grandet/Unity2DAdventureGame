using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    public int totalScore = 0;  // 分数
    public Text scoreText;  // 分数界面
    public TextMeshProUGUI finalScoreText;  // 胜利界面中的分数

    public GameObject gameOverPanel;  // 游戏失败界面
    public GameObject gameVictoryPanel;  // 游戏胜利界面

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTotalScore()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }

    // 显示游戏结束界面
    public void ShowGameOverPanel()
    {
        // 因为没有初始化，所以要进行非空判断
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowGameVictoryPanel()
    {
        if (gameVictoryPanel != null)
        {
            finalScoreText.text += totalScore;
            gameVictoryPanel.SetActive(true);
        }
    }

    // 重启关卡
    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        levelName = "Level1";
        Debug.Log("重启关卡:" + levelName);
        SceneManager.LoadScene(levelName);
    }

}