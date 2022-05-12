using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sound;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;
    private void Awake()
    {
        instance = this;
    }

    public BoxCollider ballSpawnField;
    public Transform ball;
    public Transform ballPrefab;
    public Transform ballParent;

    [Header("UI")]
    public GameObject matchCompletePanel;
    public GameObject gameCompletePanel;
    public GameObject pausePanel;
    public TextMeshProUGUI completeText;
    public TextMeshProUGUI timeText, timeText2;
    public TextMeshProUGUI playerText, playerText2;

    [Space]
    public GameObject mainGameObject;
    public GameObject penaltyGameObject;

    [Space]
    public float time;

    public bool isComplete;
    public bool isPaused;

    public Transform camera;
    public bool extraTime;
    public PlayerInfoUI[] playerUIs;
    public AudioClip cheerSound;
    public GameObject arCanvas;
    public GameObject mainCanvas;

    [Header("Cinematic")]
    public GameObject cineCam1, cineCam2;

    private PlayerManager[] playerManagerList;

    void Start()
    {
        playerManagerList = FindObjectsOfType<PlayerManager>();
        StartCoroutine(Cinematic());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (isComplete) return;
        time -= Time.deltaTime;
        timeText.text = time.ToString("F0");
        timeText2.text = time.ToString("F0");
        playerText.text = "Player 1" + (GameManager.instance.round % 2 == 0 ? " (Defender)" : " (Attacker)");
        playerText2.text = "Player 2" + (GameManager.instance.round % 2 != 0 ? " (Defender)" : " (Attacker)");
        if (time <= 0)
        {
            time = 0;
            Complete(null);
        }
    }
    public void StartMatch()
    {
        isComplete = false;
        matchCompletePanel.SetActive(false);
        gameCompletePanel.SetActive(false);
        if (ball) Destroy(ball.gameObject);
        ball = Instantiate(ballPrefab, ballParent);
        ball.position = GetRandomPosition();

        foreach (var pm in playerManagerList)
            pm.ResetGame();

        time = 140;

        playerUIs[1].scoreText.text = "Score " + GameManager.instance.player1Score.ToString();
        playerUIs[0].scoreText.text = "Score " + GameManager.instance.player2Score.ToString();
        mainCanvas.SetActive(true);

        if (GameManager.instance.round % 2 == 0)
        {
            camera.eulerAngles = new Vector3(90, 0, 180);
        }
        else
        {
            camera.eulerAngles = new Vector3(90, 0, 0);
        }
    }

    public IEnumerator Cinematic()
    {
        cineCam1.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        cineCam2.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        cineCam1.SetActive(false);
        cineCam2.SetActive(false);
        camera.gameObject.SetActive(true);
        arCanvas.SetActive(true);
        StartMatch();
        yield break;
    }

    public void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
    }


    private Vector3 GetRandomPosition()
    {
        BoxCollider _bc = ballSpawnField;
        Transform cubeTrans = _bc.GetComponent<Transform>();

        Vector3 cubeSize;
        Vector3 cubeCenter;

        cubeCenter = cubeTrans.position;

        cubeSize.x = cubeTrans.lossyScale.x * _bc.size.x;
        cubeSize.y = cubeTrans.lossyScale.y * _bc.size.y;
        cubeSize.z = cubeTrans.lossyScale.z * _bc.size.z;

        Vector3 randomPosition = new Vector3(Random.Range(-cubeSize.x / 2, cubeSize.x / 2), 0.1855386f, Random.Range(-cubeSize.z / 2, cubeSize.z / 2));

        return cubeCenter + randomPosition;
    }

    public void Complete(PlayerManager pm)
    {
        isComplete = true;
        string name = "";
        if (pm)
        {
            if (pm.side == Side.ATTACKER)
                if (GameManager.instance.round % 2 == 0)
                {
                    name = "Player 2";
                    GameManager.instance.player2Score++;
                }
                else
                {
                    name = "Player 1";
                    GameManager.instance.player1Score++;
                }
            else if (pm.side == Side.DEFENDER)
                if (GameManager.instance.round % 2 == 0)
                {
                    name = "Player 1";
                    GameManager.instance.player1Score++;
                }
                else
                {
                    name = "Player 2";
                    GameManager.instance.player2Score++;
                }
        }
        else
        {
            GameManager.instance.player1Score++;
            GameManager.instance.player2Score++;
        }

        playerUIs[1].scoreText.text = "Score " + GameManager.instance.player1Score.ToString();
        playerUIs[0].scoreText.text = "Score " + GameManager.instance.player2Score.ToString();

        if (pm)
            completeText.text = name + "\n" + (pm.side == Side.ATTACKER ? " ( Attacker )" : " ( Defender )") + "\n" + "Win";
        else completeText.text = "Draw";
        matchCompletePanel.SetActive(true);
        GameManager.instance.round++;

        SoundManager.PlaySound(cheerSound, 0.17f);

        if (GameManager.instance.player1Score >= 3 || GameManager.instance.player2Score >= 3)
        {
            gameCompletePanel.SetActive(true);
        }
        else if (GameManager.instance.player1Score == 2 && GameManager.instance.player2Score == 2)
            StartCoroutine(MazeMode());
        else
            StartCoroutine(NextMatch());
    }

    public IEnumerator MazeMode()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.SetFade(true);
        yield return new WaitForSeconds(0.7f);
        penaltyGameObject.SetActive(true);
        mainGameObject.SetActive(false);
        GameManager.instance.SetFade(false);
    }

    public IEnumerator NextMatch()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.SetFade(true);
        yield return new WaitForSeconds(1f);
        StartMatch();
        GameManager.instance.SetFade(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.LoadScene("MainMenuScene");
    }
}
[System.Serializable]
public class PlayerInfoUI
{
    public Image energyBarFill;
    public GameObject[] energyFillHighLights;
    public TextMeshProUGUI scoreText;
}

