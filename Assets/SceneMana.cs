using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMana : MonoBehaviour
{


    public void changeSceneToStartScene()
    {
        Debug.Log("boom boom boom");
        SceneManager.LoadScene(1);
    }
    void Awake()
    {
        if(FindObjectsOfType<SceneMana>().Length > 1 )
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
