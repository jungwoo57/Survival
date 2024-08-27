using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AchiveManagement : MonoBehaviour
{
    public GameObject[] lockCharater;
    public GameObject[] unlockCharater;
    public GameObject uiNotice;

    enum Achive { UnlockM, UnlockW}
    Achive[] achives;
    WaitForSecondsRealtime wait;


    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("MyData")){ 
            Init();
        }
        wait = new WaitForSecondsRealtime(5);
    }

    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives) {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {
        UnlockCharacter();
    }
    private void LateUpdate()
    {
        foreach(Achive achive in achives) {
            CheckAchive(achive);
        }
    }
    void UnlockCharacter() 
    {
        for (int index = 0; index < lockCharater.Length; index++) {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName)==1;
            lockCharater[index].SetActive(!isUnlock);
            unlockCharater[index].SetActive(isUnlock);
        }

    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive) {
            case Achive.UnlockM:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockW:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index = 0; index < uiNotice.transform.childCount; index++) {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine("NoticeRoutine");
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        yield return wait;
        uiNotice.SetActive(false);
    }

}
