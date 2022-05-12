using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CanvasGroup loadingCanvas;

    public int player1Score, player2Score;
    public bool gameStarted;
    public int round;
    bool isLoading;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadingCanvas.gameObject.SetActive(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetFadeCor(false));
        isLoading = false;
    }

    public void LoadScene(string targetScene)
    {
        if (isLoading) return;
        isLoading = true;
        StartCoroutine(LoadSceneCoroutine(targetScene));
    }

    private IEnumerator LoadSceneCoroutine(string targetScene)
    {

        loadingCanvas.gameObject.SetActive(true);
        StartCoroutine(SetFadeCor(true));
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(targetScene);
    }

    public void SetFade(bool value)
    {
        StartCoroutine(SetFadeCor(value));
    }
    public IEnumerator SetFadeCor(bool value)
    {
        if (true)
        {
            loadingCanvas.DOFade(0, 0f);
            loadingCanvas.gameObject.SetActive(true);
        }

        loadingCanvas.DOFade(value ? 1 : 0, 0.35f);
        if (!value)
        {
            yield return new WaitForSeconds(0.35f);
            loadingCanvas.gameObject.SetActive(false);
        }
    }

    public void ResetGame()
    {
        player1Score = 0;
        player2Score = 0;
        round = 1;
        gameStarted = false;
    }

    public void NextMatch()
    {
        StartCoroutine(NextMatchCoroutine());
    }

    public IEnumerator NextMatchCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        LoadScene("GameScene");
        yield break;
    }
    public void MazeMode()
    {
        StartCoroutine(MazeModeCoroutine());
    }

    public IEnumerator MazeModeCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        LoadScene("GameSceneMaze");
        yield break;
    }
}
