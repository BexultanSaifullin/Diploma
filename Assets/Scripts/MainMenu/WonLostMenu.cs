using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WonLostMenu : MonoBehaviour
{
    public GameObject wonMenu, lostMenu, lostText, lostBtn, wonText, wonBtn;
    private GameEntryMenu gameEntryMenu;

    public CinemachineVirtualCamera[] VirtualCameras;
    void Start()
    {
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
    }
    public void WonMenu()
    {
        StartCoroutine(wait1sec());
        wonMenu.SetActive(true);
        wonText.GetComponent<Animator>().Play("TextAnimation");
        wonBtn.GetComponent<Animator>().Play("ButtonAnimation");
    }
    public void LostMenu()
    {
        StartCoroutine(wait1sec());
        lostMenu.SetActive(true);
        lostText.GetComponent<Animator>().Play("TextAnimation");
        lostBtn.GetComponent<Animator>().Play("ButtonAnimation");
    }
    IEnumerator wait1sec()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
    }
    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        foreach (var cam in VirtualCameras)
        {
            cam.Priority = 0;
        }
        VirtualCameras[4].Priority = 1;
        gameEntryMenu.DeletePlane();
        if (lostMenu.activeSelf)
        {
            lostMenu.SetActive(false);
        }
        else
        {
            wonMenu.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}
