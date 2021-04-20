using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger Instance;
    public Animator animator;
    public Canvas canvas;
    private int levelToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += FindNewSceneMainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        // FindNewSceneMainCamera();
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
        StartCoroutine(DelayFade());
    }

    private void FindNewSceneMainCamera(Scene scene, LoadSceneMode mode)
    {
        canvas = GameObject.Find("CrossFade").GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("FadeOut");
    }
}
