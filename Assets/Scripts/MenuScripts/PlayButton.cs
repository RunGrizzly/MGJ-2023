using TMPro;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public TMP_Text text;

    private float m_minSize;

    private float m_maxSize;

    private bool m_isIncreasing = false;
    // Start is called before the first frame update
    void Start()
    {
        m_minSize = text.fontSize - 30;
        m_maxSize = text.fontSize + 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isIncreasing)
        {
            text.fontSize += 50 * Time.deltaTime;
            if(text.fontSize >= m_maxSize)
            {
                m_isIncreasing = !m_isIncreasing;
            }
        }
        else
        {
            text.fontSize -= 50 * Time.deltaTime;
            if(text.fontSize <= m_minSize)
            {
                m_isIncreasing = !m_isIncreasing;
            }
        }
    }
}
