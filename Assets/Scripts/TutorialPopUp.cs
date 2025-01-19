using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Animator>().SetTrigger("TriggerPopUp");
    }
}
