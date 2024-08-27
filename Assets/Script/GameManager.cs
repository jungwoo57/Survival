using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("#Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;


    [Header("#Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUpUI levelUpUi;
    public Result resultUI;
    public Transform uiJoy;
    public GameObject enemyCleaner;

    [Header("#Player Data")]
    public int playerId;
    public int level;
    public int kill;
    public int exp;
    public float hp;
    public float maxHp;
    public int[] nextExp;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }
    public void GameStart(int id)
    {
        playerId = id;
        hp = maxHp;
        player.gameObject.SetActive(true);
        levelUpUi.Select(playerId%2);
        Resume();
        //임시 스크립트
        enemyCleaner.SetActive(false);
        isLive = true;
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
    
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        resultUI.gameObject.SetActive(true);
        resultUI.Lose();
        Stop();
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false; 
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        resultUI.gameObject.SetActive(true);
        resultUI.Win();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry() 
    {
        SceneManager.LoadScene(0);
    }

    public void GameExit()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLive)
            return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime){
            gameTime = maxGameTime;
            GameVictory();
        }
    }
    public void GetExp()
    {
        if (!isLive)
            return;
        exp++;
        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) {
            level++;
            exp = 0;
            levelUpUi.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }
    
    public void Resume() 
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
