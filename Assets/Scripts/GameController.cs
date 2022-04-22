using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int totalScore = 0;  // 分数
    public Text scoreText;  // 分数界面

    public GameObject gameOverPanel;  // 游戏结束界面

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

    // 重启关卡
    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        levelName = "Level1";
        Debug.Log("重启关卡:" + levelName);
        SceneManager.LoadScene(levelName);
    }

}