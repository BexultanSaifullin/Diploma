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
    public Image ProgressBar;
    public GameObject PressAnyKey;

    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("closing");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false;
    }
    void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            componentAnimator.SetTrigger("opening");
            instance.ProgressBar.fillAmount = 1;
        }
    }

    void Update()
    {
        if (loadingSceneOperation != null)
        {
            float loading_progress = Mathf.Clamp01(loadingSceneOperation.progress / 0.9f);
            LoadingPercentage.text = $"{(loading_progress * 100).ToString("0")}%";
            // ProgressBar.fillAmount = loading_progress;
            if (loading_progress >= 0.8) ProgressBar.fillAmount = 1;
            else ProgressBar.fillAmount = Mathf.Lerp(loading_progress, loadingSceneOperation.progress, Time.deltaTime * 5);
        }
        if (PressAnyKey.activeSelf)
        {
            if (Input.anyKey)
            {
                loadingSceneOperation.allowSceneActivation = true;
            }
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;
        if (loadingSceneOperation.progress >= 0.9f && !loadingSceneOperation.allowSceneActivation)
        {
            PressAnyKey.SetActive(true);
            PressAnyKey.GetComponent<Animator>().Play("PressAnyKey");
        }
        // loadingSceneOperation.allowSceneActivation = true;
        // StartCoroutine(wait3Sec());
    }
    IEnumerator wait3Sec()
    {
        yield return new WaitForSeconds(3f);
        loadingSceneOperation.allowSceneActivation = true;
    }
}
