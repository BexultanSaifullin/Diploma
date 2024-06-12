using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WonLostMenu : MonoBehaviour
{
    public GameObject wonMenu, lostMenu, lostImg, lostBtn, wonImg, wonBtn, confetti1, confetti2;
    private GameEntryMenu gameEntryMenu;

    public CinemachineVirtualCamera[] VirtualCameras;
    void Start()
    {
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
    }
    public void WonMenu()
    {
        StartCoroutine(wait2sec());
        Instantiate(confetti1);
        Instantiate(confetti2);
        wonMenu.SetActive(true);
        wonImg.GetComponent<Animator>().Play("win_img");
        wonBtn.GetComponent<Animator>().Play("win_btn");
    }
    public void LostMenu()
    {
        StartCoroutine(wait2sec());
        lostMenu.SetActive(true);
        lostImg.GetComponent<Animator>().Play("win_img");
        lostBtn.GetComponent<Animator>().Play("win_btn");
    }
    IEnumerator wait2sec()
    {
        yield return new WaitForSeconds(4f);
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
