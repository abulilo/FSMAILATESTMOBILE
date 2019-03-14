using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMgmt : MonoBehaviour
{
    public GameObject playbtn, optionsbtn, exitbtn, Optionspanel, loadinslider, selectmapPanel;

    public Slider loader;
    AsyncOperation op;
    public AudioClip UiBtnClick;
    AudioSource Source;
    // Use this for initialization
    void Start()
    {
        Source = GetComponent<AudioSource>();
        Time.timeScale = 1;
        loadinslider.SetActive(false);
        playbtn.SetActive(true);
        optionsbtn.SetActive(true);
        exitbtn.SetActive(true);
        Optionspanel.SetActive(false);
        selectmapPanel.SetActive(false);
        Admanager.Instance.ShowBanner();
        //graphicsdropdown.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Onmap1Click(){
        selectmapPanel.SetActive(false);
        Source.PlayOneShot(UiBtnClick);
        StartCoroutine(Playstart());

    }
    public void Onmap2Click()
    {
        selectmapPanel.SetActive(false);
        Source.PlayOneShot(UiBtnClick);
        StartCoroutine(Playstart2());
    }
    public void Onmap3Click()
    {
        selectmapPanel.SetActive(false);
        Source.PlayOneShot(UiBtnClick);
        StartCoroutine(Playstart3());
    }

    public void OnPlayClick()
    {
        //StartCoroutine(Playstart()); 
        selectmapPanel.SetActive(true);
        playbtn.SetActive(false);
        optionsbtn.SetActive(false);
        exitbtn.SetActive(false);
        Source.PlayOneShot(UiBtnClick);

    }
    public void OnSelectMapMenuBackClick(){
        selectmapPanel.SetActive(false);
        playbtn.SetActive(true);
        optionsbtn.SetActive(true);
        exitbtn.SetActive(true);
        Source.PlayOneShot(UiBtnClick);
    }

    public void OnExitClick()
    {
        Source.PlayOneShot(UiBtnClick);
        Application.Quit();

    }
    public void OnOptionsClick()
    {
        playbtn.SetActive(false);
        optionsbtn.SetActive(false);
        exitbtn.SetActive(false);
        Optionspanel.SetActive(true);
        Source.PlayOneShot(UiBtnClick);
        // graphicsdropdown.SetActive(true);
    }
    public void OnbackClick()
    {
        playbtn.SetActive(true);
        optionsbtn.SetActive(true);
        exitbtn.SetActive(true);
        Optionspanel.SetActive(false);
        Source.PlayOneShot(UiBtnClick);
        //graphicsdropdown.SetActive(false);
    }
    public void setQualtiySettings(int qualityindex)
    {
        QualitySettings.SetQualityLevel(qualityindex);
        Source.PlayOneShot(UiBtnClick);
    }
    IEnumerator Playstart()
    {
        //selectmapPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        op = SceneManager.LoadSceneAsync("base");
        op.allowSceneActivation = false;
        loadinslider.SetActive(true);
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(progress);

            loader.value = progress;
            if (progress == 1.0f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }

    }
    IEnumerator Playstart2()
    {
        yield return new WaitForSeconds(0.5f);
        selectmapPanel.SetActive(false);
        op = SceneManager.LoadSceneAsync("base");
        op.allowSceneActivation = false;
        loadinslider.SetActive(true);
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(progress);

            loader.value = progress;
            if (progress == 1.0f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    IEnumerator Playstart3()
    {
        yield return new WaitForSeconds(0.5f);
        selectmapPanel.SetActive(false);
        op = SceneManager.LoadSceneAsync("base");
        op.allowSceneActivation = false;
        loadinslider.SetActive(true);
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(progress);

            loader.value = progress;
            if (progress == 1.0f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}