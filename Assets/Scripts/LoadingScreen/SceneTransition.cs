using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public TextMeshProUGUI LoadingPercentage;
    public Slider LoadingSlider;

    private static SceneTransition instance;
    private static bool shouldPlayAnimation = false;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("closing");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false;
        instance.LoadingSlider.value = 0;
    }
    void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayAnimation) componentAnimator.SetTrigger("opening");
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            LoadingSlider.value = Mathf.Lerp(LoadingSlider.value, loadingSceneOperation.progress, Time.deltaTime * 5);
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
        //StartCoroutine(wait5Sec());
    }
    IEnumerator wait5Sec()
    {
        yield return new WaitForSeconds(5f);
        loadingSceneOperation.allowSceneActivation = true;
    }
}
