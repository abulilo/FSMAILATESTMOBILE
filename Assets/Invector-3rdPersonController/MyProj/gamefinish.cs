using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamefinish : MonoBehaviour {
    public int civilians=0,numberthreshold=6;

    // Use this for initialization
    private void Start()
    {
        civilians = 0;
    }

    private void Update()
    {
        if (civilians == numberthreshold)
        {
            FindObjectOfType<GameManager>().GameFinish();
            civilians = 0;
        }
        else return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "civilian" && other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            Debug.Log("came in");
            civilians++;

        }
    }
}
