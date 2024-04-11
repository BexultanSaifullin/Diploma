using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryMenu : MonoBehaviour
{
    public GameObject playBtn;
    public GameObject myCollectionBtn;
    public GameObject exitBtn;
    public GameObject spawnPlayDeskBtn;
    public GameObject book;
    public GameObject paper;
    public GameObject playDesk;
    public bool isNewGameClicked = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject == playBtn)
                {
                    //SceneManager.LoadScene("SampleScene");

                    book.GetComponent<Animator>().Play("Book");
                    paper.GetComponent<Animator>().Play("Paper");
                    playBtn.SetActive(false);
                    myCollectionBtn.SetActive(false);
                    exitBtn.SetActive(false);
                    StartCoroutine(SpawnPlayDesk());
                }
                else if (hitInfo.collider.gameObject == myCollectionBtn)
                {
                    Debug.Log("My Collection opened!");
                }
                else if (hitInfo.collider.gameObject == exitBtn)
                {
                    Application.Quit();
                }
                else if (hitInfo.collider.gameObject == spawnPlayDeskBtn)
                {
                    playDesk.SetActive(true);
                    spawnPlayDeskBtn.SetActive(false);
                    book.SetActive(false);
                    paper.SetActive(false);
                    playDesk.GetComponent<Animator>().Play("PlayDeskSpawn");
                    isNewGameClicked = true;
                }
            }
        }
    }

    IEnumerator SpawnPlayDesk()
    {
        yield return new WaitForSeconds(3.5f);
        spawnPlayDeskBtn.SetActive(true);
    }
}
