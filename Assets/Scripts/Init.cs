using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{

    [Header("ÓÎÏ·¿ØÖÆÆ÷")]
    public GameController gameControllerPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameController.Instance == null)
        {
            Instantiate(gameControllerPrefab);
        }
    }

}
