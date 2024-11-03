using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int CurrentLevel;
    public ArrayList ActiveAudioList;
    public GameObject FadeInOverlay;
    private bool isGameFailed;
    public bool isGameEnd;
    public GameObject GameEndTrigger;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        CurrentLevel = 0;
    }



    //Audio source dynamic list helper function
    public bool IsAudioActive(GameObject obj)
    {
        for(int i = 0;i<ActiveAudioList.Count;i++)
        {
            if ((GameObject)ActiveAudioList[i] == obj) return true;
        }
        return false;
    }

    public void ActiveAudio(GameObject obj)
    {
        if(!IsAudioActive(obj))
        {
            ActiveAudioList.Add(obj);
            obj.GetComponent<AudioSource>().Play();
        }
    }

    public void HandleEnableGameEndTrigger()
    {
        GameEndTrigger.SetActive(true);
    }

    public void HandleGameEnd()
    {
        isGameEnd = true;
        isGameFailed = false;
        Color color = FadeInOverlay.GetComponentInChildren<Image>().color;
        color.a = 0.5f;
        StartCoroutine(HandleLoadScene(SceneManager.GetActiveScene().buildIndex+1));
    }
    public void HandleGameFailed()
    {
        isGameFailed = true;
        isGameEnd = true;
        Color color = FadeInOverlay.GetComponentInChildren<Image>().color;
        color.a = 0.5f;
        FadeInOverlay.GetComponentInChildren<Image>().color = color;
        isGameFailed = true;
        StartCoroutine(HandleLoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator HandleLoadScene(int buildIndex)
    {
        yield return new WaitForSeconds(3);
        if(buildIndex<=3)SceneManager.LoadScene(buildIndex);
    }

    public bool GetIsGameFailed()
    {
        return isGameFailed;
    }

    public void DeActiveAudio(GameObject obj)
    {
        if(IsAudioActive(obj))
        {
            ActiveAudioList.Remove(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveAudioList = new ArrayList();
        isGameFailed = false;
        isGameEnd = false;
        FadeInOverlay.SetActive(false);
        GameEndTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameEnd)
        {
            FadeInOverlay.SetActive(true);
            if (!isGameFailed) FadeInOverlay.GetComponentInChildren<Text>().text = "CONGRATS! YOU SAVE A LIFE!";
            else FadeInOverlay.GetComponentInChildren<Text>().text = "YOU GOT CAUGHT";
            Color color = FadeInOverlay.GetComponentInChildren<Image>().color;
            color.a += 0.5f * Time.deltaTime;
            FadeInOverlay.GetComponentInChildren<Image>().color = color;
        }
       
    }
}
