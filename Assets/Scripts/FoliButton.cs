using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class FoliButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    [SerializeField] private float m_hoverSize;

    int eventTween = 0;

    public Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(eventTween);
        eventTween = LeanTween.scale(gameObject, Vector3.one * m_hoverSize, 0.45f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => transform.localScale = Vector3.one * m_hoverSize).id;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(eventTween);
        eventTween = LeanTween.scale(gameObject, Vector3.one, 0.45f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => transform.localScale = Vector3.one).id;
    }

    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(eventTween);
        eventTween = LeanTween.scale(gameObject, Vector3.one * 1.5f, 1f).setEase(LeanTweenType.punch).id;
    }
}
