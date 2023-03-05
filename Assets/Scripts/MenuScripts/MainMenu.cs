using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void Back(Button button)
    {
        button.transform.parent.gameObject.SetActive(false);
    }

    public void AdjustVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
}
