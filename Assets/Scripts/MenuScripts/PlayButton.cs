using TMPro;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public TMP_Text text;

    [SerializeField] private float maxSize = 0.25f;
    // Update is called once per frame
    void Update()
    {
        text.transform.localScale = Vector3.one + Vector3.one * (maxSize * Mathf.Sin(Time.time));
    }

}
