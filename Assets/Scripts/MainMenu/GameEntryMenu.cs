using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    public GameObject gameStart;
    private AudioManager audioManager;
    public GameObject objectToSpawn;
    public bool IsPlaneDestroyed = false;
    public List<CinemachineVirtualCamera> introCameras;
    public List<CinemachineVirtualCamera> gameCameras;


    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

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
                    if (book.activeSelf == false && paper.activeSelf == false)
                    {
                        book.SetActive(true);
                        paper.SetActive(true);
                    }
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
                    gameStart = Instantiate(objectToSpawn);
                    gameStart.SetActive(true);
                    playDesk.GetComponent<Animator>().Play("PlayDeskSpawn");
                    gameStart.GetComponent<Animator>().Play("PanelSpawn");
                    isNewGameClicked = true;
                    audioManager.CleanUp();
                    audioManager.InitializeMenuMusic(FMODEvents.instance.BackgroundMusic);
                    foreach (var camera in introCameras)
                    {
                        camera.gameObject.SetActive(false);
                    }
                    introCameras[5].Priority = 1;
                    foreach (var camera in gameCameras)
                    {
                        camera.Priority = 0;
                    }
                    gameCameras[0].Priority = 1;
                }
            }
        }
    }

    public void DeletePlane()
    {
        IsPlaneDestroyed = true;
        Destroy(gameStart);
        RestartGame();
    }

    public void RestartGame()
    {
        introCameras[5].gameObject.SetActive(true);
        introCameras[5].Priority = 10;

        playDesk.SetActive(false);

        // book.SetActive(true);
        // paper.SetActive(true);
        // book.GetComponent<Animator>().Play("Book");
        // paper.GetComponent<Animator>().Play("Paper");

        playBtn.SetActive(true);
        myCollectionBtn.SetActive(true);
        exitBtn.SetActive(true);
        audioManager.CleanUp();
        audioManager.InitializeMenuMusic(FMODEvents.instance.MenuMusic);
    }

    IEnumerator SpawnPlayDesk()
    {
        yield return new WaitForSeconds(3.5f);
        spawnPlayDeskBtn.SetActive(true);
    }
}
