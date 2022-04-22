using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{

    public GameObject[] objects;  // 要带到下一关的UI

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
            // 开启协程
            StartCoroutine(LoadYourAsyncScene()); 
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // 获取当前场景，以便之后卸载
        Scene currentScene = SceneManager.GetActiveScene();
        // 禁用当前摄像机的AudioListener
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
        // 在后台加载新场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
        // 直到方法完成前先返回null
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // 移动物体到新场景
        foreach (GameObject gameObject in objects)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Level2"));
        }
        // 卸载当前场景
        SceneManager.UnloadSceneAsync(currentScene);
    }

}
