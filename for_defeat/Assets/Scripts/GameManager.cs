using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public HeroBehaviour hero;
    public AudioSource audioSource;
    public RectTransform EndPanel;
    public Text endText;

    public void GameEnd(string text)
    {
        EndPanel.gameObject.SetActive(true);
        endText.text = text;
        Invoke("Quit", 5f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
