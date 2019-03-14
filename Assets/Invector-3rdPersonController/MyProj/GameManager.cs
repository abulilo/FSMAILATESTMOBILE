using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Invector;

public class GameManager : MonoBehaviour {
    public Text statusstext;
   public  string status;
    float respawnTimer;
    public GameObject gamoverPanel, PausePanel,player;
    public AudioClip Win;
    public AudioClip loose;
    AudioSource source;
	// Use this for initialization
	void Start () {

        gamoverPanel.SetActive(false);
        status = "Cross the Border";
        source = GetComponent<AudioSource>();
        Admanager.Instance.HideBanner();
	}
	
	// Update is called once per frame
	void Update () {
        statusstext.text = status;
        Debug.Log(status);
	}

    public void OnScriptChange(string s){
        status = s;
        Debug.Log("someoneiscalling");
    }
    public void GameOver(){
        //FindObjectOfType<vGameController>().Continuewithgame();
        gamoverPanel.SetActive(true);
        source.PlayOneShot(loose);
        Admanager.Instance.ShowBanner(); 
        Admanager.Instance.ShowVideo();
    }
    public void PauseGame(){
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        Admanager.Instance.ShowBanner();
    }
    public void Exitgame()
    { 

        SceneManager.LoadScene("menu");
    }
    public void GameFinish(){
        Debug.Log("Game is Finished");
        source.PlayOneShot(Win);
        Admanager.Instance.ShowVideo();
    }
    public void OnGameOverRESume(){
        //FindObjectOfType<vGameController>().Continuewithgame();
        // Invoke("ResetScene", respawnTimer);

        vGameController.instance.OnCharacterDead(player);
        var scene = SceneManager.GetActiveScene();
         SceneManager.LoadScene(scene.name);
        Admanager.Instance.ShowRewardedvideo();
        Admanager.Instance.HideBanner();
    }
    public void OnResumeclick(){
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        Admanager.Instance.HideBanner();
    }
    public void OnenemyDie(){
        Debug.Log("Enemydied");
    }
}
