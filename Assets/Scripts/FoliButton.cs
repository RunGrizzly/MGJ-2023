using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FoliButton : MonoBehaviour
{
    public TMP_Text text;
    public float min;
    public float max;
    public float speed;

    public Button button;

    void Update()
    {
        var l = Mathf.InverseLerp(-1, 1, Mathf.Sin(speed * Time.time));
        var f = Mathf.Lerp(min, max, l);

        text.transform.localScale = Vector3.one * f;
    }
}
