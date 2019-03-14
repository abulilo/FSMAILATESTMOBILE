using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class statusdetector : MonoBehaviour {

    public string status;
    AudioSource ping;
	// Use this for initialization
	void Start () {
        ping = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            FindObjectOfType<GameManager>().OnScriptChange(status);
            ping.Play();
        } 
    }

}
