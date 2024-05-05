using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MyCollection : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI cardDescriptionText;
    // public GameObject backToMenuBtn;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToHide;
    private Vector3 initialPos;
    private GameObject currentCard;
    private string cardName;

    private Dictionary<string, string> cardDescriptions = new Dictionary<string, string>
    {
        {"Card1", "Description of Card 1"},
        {"Card2", "Description of Card 2"},
        {"Card3", "Description of Card 3"},
        {"Card4", "Description of Card 4"},
        {"Card5", "Description of Card 5"},
        {"Card6", "Description of Card 6"},
        {"Card7", "Description of Card 7"},
        {"Card8", "Description of Card 8"},
        {"Card9", "Description of Card 9"},
        {"Card10", "Description of Card 10"}
    };

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("CardColl"))
                {
                    currentCard = clickedObject;
                    initialPos = clickedObject.transform.position;
                    cardName = clickedObject.name;
                    clickedObject.name = "Current_Card";

                    Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 4f + mainCamera.transform.right * -1.1f;
                    clickedObject.transform.position = newPosition;

                    foreach (var obj in objectsToActivate)
                    {
                        obj.SetActive(true);
                    }
                    foreach (var obj in objectsToHide)
                    {
                        Debug.Log(clickedObject.name);
                        if (obj.name != "Current_Card")
                        {
                            obj.gameObject.SetActive(false);
                        }
                    }

                    if (cardDescriptions.TryGetValue(clickedObject.name, out string description))
                    {
                        cardDescriptionText.text = description;
                    }
                }
                else if (clickedObject.CompareTag("BackFromDescrioptionBtn") && currentCard != null)
                {
                    currentCard.transform.position = initialPos;
                    currentCard.name = cardName;
                    foreach (var obj in objectsToActivate)
                    {
                        obj.SetActive(false);
                    }
                    objectsToHide[0].SetActive(true);
                    objectsToHide[1].SetActive(true);
                    objectsToHide[2].SetActive(true);
                    for (int i = 5; i < 13; i++)
                    {
                        objectsToHide[i].SetActive(true);
                    }
                }
                else if (clickedObject.CompareTag("NextBtn"))
                {
                    Debug.Log("Next btn clicked!");
                    objectsToHide[13].transform.parent.gameObject.SetActive(true);
                    objectsToHide[13].SetActive(true);
                    objectsToHide[14].SetActive(true);
                    objectsToHide[3].SetActive(true);
                    objectsToHide[2].SetActive(false);
                    for (int i = 5; i < 13; i++)
                    {
                        objectsToHide[i].SetActive(false);
                    }
                }
                else if (clickedObject.CompareTag("PrevBtn"))
                {
                    Debug.Log("Prev btn clicked!");
                    objectsToHide[2].SetActive(true);
                    objectsToHide[3].SetActive(false);
                    objectsToHide[13].SetActive(false);
                    objectsToHide[14].SetActive(false);
                    for (int i = 5; i < 13; i++)
                    {
                        objectsToHide[i].SetActive(true);
                    }
                }
                // else if (clickedObject.CompareTag("BtnBackColl"))
                // {
                //     for (int i = 5; i < 15; i++)
                //     {
                //         objectsToHide[i].SetActive(true);
                //     }
                // }
            }
        }
    }
}