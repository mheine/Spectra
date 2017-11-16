using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoadMainScene : MonoBehaviour {
    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;

    void Start() {
        StartCoroutine(LoadNewScene());    
    }


    // Updates once per frame
    void Update() {
    	loadingText.text = "Loading, please wait";
    	loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene() {

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync("Assets/Scenes/MainScene.unity");

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }

    }

}