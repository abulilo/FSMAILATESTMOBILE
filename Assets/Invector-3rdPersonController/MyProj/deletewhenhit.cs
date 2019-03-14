using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletewhenhit : MonoBehaviour {

    public GameObject[] civilians;
    public GameObject[] detector;
    public GameObject[] enemies;
    public bool SpawnEnemieswhenReturn=false;
	
	void Start () {
      
	}

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.layer == LayerMask.NameToLayer("Player")){
            for (int i = 0; i < civilians.Length; i++)
            {
                civilians[i].SetActive(true);

            }
            StartCoroutine(delete());
        }

        
    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(3);
        SpawnatStarting();
        for (int i = 0; i < detector.Length; i++)
            detector[i].SetActive(false);
    }

    public void SpawnatStarting(){
        if(SpawnEnemieswhenReturn){
            for (int i = 0; i < enemies.Length;i++){
                enemies[i].SetActive(true);
            }
        }
    }
}
