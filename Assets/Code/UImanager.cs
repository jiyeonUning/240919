using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public enum GameState { Ready, Runnig, GameOver, GameClear }

    [SerializeField] GameState curState;
    [SerializeField] PlayerMoveController player;

    [Header("UI")]
    [SerializeField] GameObject readyText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;

    private void Start()
    {
        curState = GameState.Ready;
        player = FindObjectOfType<PlayerMoveController>();
        player.OnDied += GameOver;
        player.OnDied += GameClear;

        readyText.SetActive(true);
        gameOverText.SetActive(false);
        gameClearText.SetActive(false);
    }

    private void Update()
    {
        if (curState == GameState.Ready && Input.anyKeyDown) { GameStart(); }
        else if (curState == GameState.GameOver && Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("SampleScene"); }
        else if (curState == GameState.GameClear && Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("SampleScene"); }
    }




    public void GameStart()
    {
        curState = GameState.Runnig;

        readyText.SetActive(false);
        gameOverText.SetActive(false);
        gameClearText.SetActive(false);
    }

    public void GameOver()
    {
        curState = GameState.GameOver;

        readyText.SetActive(false);
        gameOverText.SetActive(true);
        gameClearText.SetActive(false);
    }

    public void GameClear()
    {
        curState = GameState.GameClear;

        readyText.SetActive(false);
        gameOverText.SetActive(false);
        gameClearText.SetActive(true);
    }
}
