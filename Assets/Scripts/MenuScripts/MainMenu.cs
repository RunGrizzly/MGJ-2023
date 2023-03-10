using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> m_panels = new List<CanvasGroup>();

    //A list to track all active tweens
    private List<int> m_activeTweens = new List<int>();


    private void Start()
    {
        //Pass in the main panel (0) to our panel switcher
        GoToPanel(0);
    }

    public void PrepareBattle()
    {
        Brain.ins.SceneLoader.LoadBattleScene();
    }

    public void Back(Button button)
    {
        button.transform.parent.gameObject.SetActive(false);
    }

    public void AdjustVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }


    public void GoToPanel(int panelIndex)
    {
        //Cancel all currently tracked tweens
        foreach (int id in m_activeTweens)
        {
            LeanTween.cancel(id);
        }
        m_activeTweens.Clear();

        foreach (CanvasGroup panel in m_panels)
        {
            if (m_panels.IndexOf(panel) == panelIndex)
            {
                int newTween = LeanTween.alphaCanvas(panel, 1, 0.45f).setOnComplete(() =>
                  {
                      panel.interactable = true;
                      panel.blocksRaycasts = true;
                  }).id;

                //Track this as an active tween
                m_activeTweens.Add(newTween);
            }
            else
            {
                panel.interactable = false;
                panel.blocksRaycasts = false;

                int newTween = LeanTween.alphaCanvas(panel, 0, 0.6f).id;
                //Track this as an active tween
                m_activeTweens.Add(newTween);
            }
        }
    }
}
