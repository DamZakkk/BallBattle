using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip bgm;
    public void StartGame()
    {
        GameManager.instance.ResetGame();
        GameManager.instance.LoadScene("GameScene");
        SoundManager.PlayMusic(bgm, 0.17f, true, true);
    }
}
