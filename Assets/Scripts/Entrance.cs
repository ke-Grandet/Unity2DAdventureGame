using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{

    public GameObject[] objects;  // Ҫ������һ�ص�UI

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����Э��
            StartCoroutine(LoadYourAsyncScene()); 
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // ��ȡ��ǰ�������Ա�֮��ж��
        Scene currentScene = SceneManager.GetActiveScene();
        // ���õ�ǰ�������AudioListener
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
        // �ں�̨�����³���
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
        // ֱ���������ǰ�ȷ���null
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // �ƶ����嵽�³���
        foreach (GameObject gameObject in objects)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Level2"));
        }
        // ж�ص�ǰ����
        SceneManager.UnloadSceneAsync(currentScene);
    }

}
