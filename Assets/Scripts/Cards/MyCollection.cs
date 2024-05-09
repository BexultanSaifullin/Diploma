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
        {"Archer", "Description: \nThe Archer, known for his unparalleled precision and keen eyesight, strikes fear into the hearts of his enemies. From a young age, he honed his skills in the vast wilderness, learning to hit targets unseen by the naked eye."},
        {"Meadow Shepherd", "Description: \nThe Meadow Shepherd, a keeper of the plains, is as steadfast as the land he tends. With a watchful eye and a shepherd’s crook, he defends his flock against any encroaching dangers. Though not trained for battle, his resolve grants him the courage to confront threats up close."},
        {"Warrior", "Description: \nClad in rugged leather armor and wielding a sturdy iron sword, the Warrior thrives in the chaos of close-quarter combat. His training emphasizes endurance and strength, allowing him to sustain blows while delivering powerful, crushing strikes to his foes."},
        {"Zheztyrnak", "Description: \nEnshrouded in the ancient spirits of vengeful warriors, Zheztyrnak embodies the relentless fury of the fallen. Once a revered champion, now a spectral avenger, Zheztyrnak ensures that even in death, justice is delivered swiftly and decisively.\n\nAbility: Final Retribution \nUpon its demise, Zheztyrnak's spirit lashes out, instantly vanquishing the foe standing directly opposite. This retribution is as inevitable as the setting sun, claiming the life of its adversary without fail."},
        {"Ensign", "Description: \nIn the heat of battle, the Ensign is a beacon of hope and strength. Adorned with the emblem of his legion, his presence not only inspires courage but also bolsters the vitality and prowess of his comrades. With the flag held high, he weaves a bond of unyielding strength among the ranks. \n\nAbility: Rallying Cry \nAs long as the Standard Bearer is on the field, he grants all allied units a +1 bonus to both attack and health, symbolizing unity and resilience. His rally cry rejuvenates spirits and fortifies bodies, ensuring that each soldier fights with enhanced vigor and endurance."},
        {"Jute", "Description: \nHarness the ancient art of siegecraft with the Jute, a spell of precision and devastation. Crafted from the whispered secrets of old sappers and the echoes of crumbling fortresses, this spell channels the raw force of nature into a single, destructive pulse aimed at enemy structures."},
        {"Arrow Storm", "Description: \nUnleash the ancient wrath of the skies with Arrowstorm, a spell of formidable power that calls down a relentless barrage of enchanted arrows. Crafted and perfected through centuries, this spell is designed to target and decimate enemy ranks with lethal precision."},
        {"Yurt", "Description: \nRising from the vast, windswept steppes of Kazakhstan, the Urt is a revered structure. This grand Urt serves not just as a dwelling but as a sanctuary for nomadic wisdom and hospitality, its circular form embracing all who seek shelter and knowledge under its expansive roof."},
        {"Barrack", "Description: \nRising from the vast expanses of the steppes, the Barrack stand as a testament to the enduring spirit of the Kazakh warriors. Constructed using traditional methods and adorned with symbols of the eagle and horse, these barracks serve as a rallying point for the nomadic warriors of the plains."},
        {"Arrow Forge", "Description: \nRising from the heart of the battlefield, the Arrow Forge is no ordinary structure. Crafted by ancient master smiths and blessed by the wind spirits, this forge empowers every arrow fired in its presence. As the battle rages, the forge's magic intensifies, honing the skills of the archers and sharpening their resolve."}
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
                    clickedObject.tag = "Untagged";

                    Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 4f + mainCamera.transform.right * -1.3f;
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

                    if (cardDescriptions.TryGetValue(cardName, out string description))
                    {
                        cardDescriptionText.text = description;
                    }
                }
                else if (clickedObject.CompareTag("BackFromDescrioptionBtn") && currentCard != null)
                {
                    currentCard.transform.position = initialPos;
                    currentCard.name = cardName;
                    currentCard.tag = "CardColl";
                    foreach (var obj in objectsToActivate)
                    {
                        obj.SetActive(false);
                    }
                    objectsToHide[0].SetActive(true);
                    objectsToHide[1].SetActive(true);
                    objectsToHide[2].SetActive(true);
                    objectsToHide[13].SetActive(false);
                    objectsToHide[14].SetActive(false);
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