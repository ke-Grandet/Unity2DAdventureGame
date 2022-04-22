using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int totalScore = 0;  // ����
    public Text scoreText;  // ��������

    public GameObject gameOverPanel;  // ��Ϸ��������

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

    // ��ʾ��Ϸ��������
    public void ShowGameOverPanel()
    {
        // ��Ϊû�г�ʼ��������Ҫ���зǿ��ж�
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // �����ؿ�
    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        levelName = "Level1";
        Debug.Log("�����ؿ�:" + levelName);
        SceneManager.LoadScene(levelName);
    }

}