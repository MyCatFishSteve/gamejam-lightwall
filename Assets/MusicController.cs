using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    static private MusicController s_Instance;

    public GameObject menuMusicObj;
    public GameObject levelMusicObj;

    public string sceneName;
    public bool inMenu = true;

    private void Awake()
    {
        SceneManager.sceneLoaded += CheckScene;
        DontDestroyOnLoad(this.gameObject);

        if (s_Instance != null){
            Destroy(s_Instance.gameObject);
        }
        s_Instance = this;

        //get scene name
        sceneName = SceneManager.GetActiveScene().name;

        SetMusic();
    }

    private void CheckScene(Scene scene, LoadSceneMode mode)
    {
        sceneName = scene.name;
        if(sceneName == "Menu"){
            inMenu = true;
        }else{
            inMenu = false;
        }
        SetMusic();
    }

    private void SetMusic()
    {
        menuMusicObj.SetActive(inMenu);
        levelMusicObj.SetActive(!inMenu);
    }
}
