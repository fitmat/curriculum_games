using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("from ontrigger Enter : " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            print("Go To Battle Zone");
        }
    }
}
