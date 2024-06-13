using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public void HideDamageUI()
    {
        gameObject.SetActive(false);
    }
}
