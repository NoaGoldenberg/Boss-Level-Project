using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class UIManager : MonoBehaviour
{
    public static UIManager UIinstance;
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject BossWonPanel;
    [SerializeField] private GameObject OctopusWonPanel;
    [SerializeField] private GameObject PausePanel; // Reference to the Pause Panel

    private void Awake()
    {
        if (UIinstance == null)
        {
            UIinstance = this;
        }
        else if (UIinstance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 0f;
        BossWonPanel.SetActive(false);
        OctopusWonPanel.SetActive(false);
        PausePanel.SetActive(false); // Ensure the Pause Panel is initially inactive
    }

    public void HideGameMenu()
    {
        StartPanel.SetActive(false);
        Time.timeScale = 1f;
        
    }

    public void BossWon()
    {
        Debug.Log("BossWONui");
        Time.timeScale = 0f;
        BossWonPanel.SetActive(true);
    }

    public void OctopusWon()
    {
        Time.timeScale = 0f;
        OctopusWonPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        PausePanel.SetActive(false); 
        OctopusWonPanel.SetActive(false);
        BossWonPanel.SetActive(false);
        Time.timeScale = 1f;

        Vector3 playerStartPos = new Vector3(-5, 0, 0); 
        Vector3 bossStartPos = new Vector3(0, 0, 0); 

        CoreManager.instance.player.ResetPlayer(playerStartPos);
        CoreManager.instance.boss.ResetBoss(bossStartPos);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame called");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // New Methods for Pausing and Resuming the Game
    public void PauseGame()
    {
        AudioManager.instance.PauseAudio();
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        AudioManager.instance.ResumeAudio();
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MuteGame()
    {
        AudioManager.instance.ToggleMute();
    }
}
