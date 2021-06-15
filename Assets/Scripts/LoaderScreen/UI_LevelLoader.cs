using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelLoader : MonoBehaviourSingleton<UI_LevelLoader>
{
    public Image loadingImage;

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        float loadingVal = LoaderManager.Get().loadingProgress;
        loadingImage.transform.Rotate(new Vector3(0, 0, loadingVal)); // ratacion de la imagen de carga

        if (LoaderManager.Get().loadingProgress >= 1)
            SetVisible(false);
    }
}
