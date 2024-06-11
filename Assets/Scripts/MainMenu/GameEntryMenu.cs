using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameEntryMenu : MonoBehaviour
{
    public GameObject cutsceneCamera;
    public GameObject mainBtnObj;
    public GameObject playBtn;
    public GameObject myCollectionBtn;
    public GameObject exitBtn;
    public GameObject spawnPlayDeskBtn;
    public GameObject backMenuBtn;
    public GameObject tutorialBtn;
    public GameObject book;
    public GameObject paper;
    public GameObject playDesk;
    public bool isNewGameClicked = false;
    private GameObject gameStart;
    private AudioManager audioManager;
    public GameObject objectToSpawn;
    public GameObject cutscene;
    public GameObject buttons3D;
    //public GameObject backBtn;
    public GameObject cameraChangerObj;
    public GameObject jutSpellPlayer;
    public GameObject jutSpellEnemy;
    public GameObject arrowsSpellPlayer;
    public GameObject arrowsSpellEnemy;
    private GameObject cashedCamera;
    public bool gameStarted = false;
    public GameObject[] myCollectionObjects;
    public List<CinemachineVirtualCamera> gameCameras;
    private static int launchCount = 0;
    private const int MaxLaunchCount = 1;


    void Start()
    {
        if (launchCount >= MaxLaunchCount)
        {
            cutscene.GetComponent<PlayableDirector>().Stop();
            // DestroyImmediate(cutscene);
            // isCutsceneDestroyed = true;
            gameCameras[4].Priority = 1;
            buttons3D.SetActive(true);
            mainBtnObj.SetActive(false);
        }
        else
        {
            cutscene.GetComponent<PlayableDirector>().Play();
            cutsceneCamera.SetActive(true);
            launchCount++;
            StartCoroutine(WaitCutscene());
        }
        audioManager = FindObjectOfType<AudioManager>();
        // StartCoroutine(WaitCutscene());
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
                    buttons3D.SetActive(false);
                    gameStarted = true;
                    StartCoroutine(SpawnPlayDesk());
                }
                else if (hitInfo.collider.gameObject == myCollectionBtn)
                {
                    Debug.Log("My Collection opened!");
                    myCollectionObjects[0].SetActive(true);
                    myCollectionObjects[1].SetActive(true);
                    myCollectionObjects[3].SetActive(true);
                    myCollectionObjects[5].SetActive(true);
                    buttons3D.SetActive(false);
                }
                else if (hitInfo.collider.gameObject == exitBtn)
                {
                    Application.Quit();
                }
                else if (hitInfo.collider.gameObject == spawnPlayDeskBtn)
                {
                    cameraChangerObj.SetActive(true);
                    playDesk.SetActive(true);
                    spawnPlayDeskBtn.SetActive(false);
                    backMenuBtn.SetActive(false);
                    tutorialBtn.SetActive(false);
                    book.SetActive(false);
                    paper.SetActive(false);
                    gameStart = Instantiate(objectToSpawn);
                    gameStart.SetActive(true);
                    playDesk.GetComponent<Animator>().Play("PlayDeskSpawn");
                    gameStart.GetComponent<Animator>().Play("PanelSpawn");
                    isNewGameClicked = true;
                    audioManager.CleanUp();
                    audioManager.InitializeMenuMusic(FMODEvents.instance.BackgroundMusic);
                    //Destroy(cutscene);
                    // foreach (var camera in introCameras)
                    // {
                    //     camera.gameObject.SetActive(false);
                    // }
                    gameCameras[4].Priority = 1;
                    foreach (var camera in gameCameras)
                    {
                        camera.Priority = 0;
                    }
                    gameCameras[0].Priority = 1;
                }
                else if (hitInfo.collider.gameObject == myCollectionObjects[5])
                {
                    foreach (var obj in myCollectionObjects)
                    {
                        obj.SetActive(false);
                    }
                    buttons3D.SetActive(true);
                }
                else if (hitInfo.collider.gameObject == backMenuBtn)
                {
                    spawnPlayDeskBtn.SetActive(false);
                    backMenuBtn.SetActive(false);
                    tutorialBtn.SetActive(false);
                    RestartGame();
                }
                else if (hitInfo.collider.gameObject == tutorialBtn)
                {
                    spawnPlayDeskBtn.SetActive(false);
                    backMenuBtn.SetActive(false);
                    tutorialBtn.SetActive(false);
                    SceneTransition.SwitchToScene("Tutorial");
                }
            }
        }
    }

    public void DeletePlane()
    {
        gameStarted = false;
        Destroy(gameStart);
        RestartGame();
    }

    public void RestartGame()
    {
        gameCameras[4].gameObject.SetActive(true);
        gameCameras[4].Priority = 10;

        if (playDesk.activeSelf)
        {
            playDesk.SetActive(false);
        }
        // book.SetActive(true);
        // paper.SetActive(true);
        // book.GetComponent<Animator>().Play("Book");
        // paper.GetComponent<Animator>().Play("Paper");

        if (book.activeSelf == false && paper.activeSelf == false)
        {
            book.SetActive(true);
            paper.SetActive(true);
            audioManager.CleanUp();
            audioManager.InitializeMenuMusic(FMODEvents.instance.MenuMusic);
        }
        book.GetComponent<Animator>().Play("Reverse Book");
        paper.GetComponent<Animator>().Play("Reverse Paper");
        StartCoroutine(Wait5Sec());
    }

    IEnumerator SpawnPlayDesk()
    {
        yield return new WaitForSeconds(3.5f);
        spawnPlayDeskBtn.SetActive(true);
        backMenuBtn.SetActive(true);
        tutorialBtn.SetActive(true);
    }
    IEnumerator Wait5Sec()
    {
        yield return new WaitForSeconds(5.5f);
        buttons3D.SetActive(true);
    }
    IEnumerator WaitCutscene()
    {
        yield return new WaitForSeconds(21f);
        mainBtnObj.SetActive(false);
        // DestroyImmediate(cutscene);
        // isCutsceneDestroyed = true;
        buttons3D.SetActive(true);
        cutsceneCamera.SetActive(false);
    }
}
