using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
{
    [SerializeField]
    float minimumTime = 2;

    public float loadingProgress;
    public float timeLoading;

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadAssets(scene));
    }

    IEnumerator LoadAssets(string scene)
    {
        loadingProgress = 0;
        timeLoading = 0;
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = operation.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minimumTime;

            if (loadingProgress >= 1)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }        
    }
}
