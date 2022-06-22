using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameConstants gameConstants;
    public UnityEvent onApplicationExit;
    public string nextSceneName;
    public FloatVariable loadingProgress;
    private bool audioClear = false;
    void OnApplicationQuit()
    {
        onApplicationExit.Invoke();
    }

    public void levelCompleteResponse()
    {
        StartCoroutine(startAsyncLoad(nextSceneName));
    }

    public void RestartClickedResponse()
    {
        StartCoroutine(startAsyncLoad(gameConstants.firstSceneName));
    }

    IEnumerator startAsyncLoad(string sceneName)
    {
        Debug.Log("Entered Async Stage");
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncLoadScene.allowSceneActivation = false;
        while (!asyncLoadScene.isDone)
        {
            loadingProgress.SetValue(asyncLoadScene.progress);
            if (audioClear && asyncLoadScene.progress >= .9f)
            {
                Debug.Log("Exit load stage");
                yield return new WaitForSeconds(1f);
                asyncLoadScene.allowSceneActivation = true;
            }
            yield return null;
        }
        Debug.Log("Exit AsyncLoad Stage");
    }

    public void audioFinisherResponse()
    {
        audioClear = true;
    }

    void Awake()
    {
        
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
