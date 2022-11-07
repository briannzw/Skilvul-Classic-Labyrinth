using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public Hole hole;

    float timer;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (hole == null) return;

        if(!hole.entered) timer += Time.deltaTime;

        if (hole.entered && !gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "Finished in " + Math.Round(timer, 2).ToString() + "s!";
        }
    }

    public void Replay()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
