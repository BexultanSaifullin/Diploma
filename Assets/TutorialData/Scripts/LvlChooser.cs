using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlChooser : MonoBehaviour
{
    public GameObject playDesk;
    public GameObject lvlChoosingPlace;
    public GameObject grass;
    public GameObject lvlBtn;
    public GameObject dialogueScreen;
    void Start()
    {
        StartCoroutine(Wait8_5sec());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.isTrigger && hit.collider.CompareTag("LvlBtn"))
                {
                    playDesk.SetActive(true);
                    StartCoroutine(Wait0_5sec());
                }
            }
        }
    }
    IEnumerator Wait0_5sec()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueScreen.SetActive(true);
        lvlChoosingPlace.SetActive(false);
        grass.SetActive(false);
    }
    IEnumerator Wait8_5sec()
    {
        yield return new WaitForSeconds(8.5f);
        lvlBtn.GetComponent<BoxCollider>().isTrigger = true;
    }
}
