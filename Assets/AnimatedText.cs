
using System.Collections;
using TMPro;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{

    private TMP_Text m_TextComponent;

    public float FadeSpeed = 1.0f;
    public float StepTime = 1.0f;
    public int RolloverCharacterSpread = 10;
    private Color ColorTint;
    public Color ColorA;
    public Color ColorB;

    void OnEnable()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void Start()
    {
        AnimateVertexColors();
    }

    private void Update()
    {
        // float i = Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time));
        // float l = Mathf.Lerp(0, 1, i);

        // ColorTint = Color.Lerp(ColorA, ColorB, l);
    }

    /// <summary>
    /// Method to animate vertex colors of a TMP Text object.
    /// </summary>
    /// <returns></returns>
    void AnimateVertexColors()
    {
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        m_TextComponent.ForceMeshUpdate();


        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        Color32[] newVertexColors;

        int currentCharacter = 0;
        int startingCharacterRange = currentCharacter;
        bool isRangeMax = false;


        int characterCount = textInfo.characterCount;

        // // Spread should not exceed the number of characters.
        // byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);


        for (int i = 0; i < characterCount; i++)
        {
            // Skip characters that are not visible
            //if (textInfo.characterInfo[i].isVisible) continue;

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;


            LeanTween.value(gameObject, ColorA, ColorB, 0.5f).setLoopPingPong(100).setOnUpdate((Color val) =>
            {
                newVertexColors[vertexIndex + 0] = val;
                newVertexColors[vertexIndex + 1] = val;
                newVertexColors[vertexIndex + 2] = val;
                newVertexColors[vertexIndex + 3] = val;

                // Upload the changed vertex colors to the Mesh.
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            });



            // // Get the current character's alpha value.
            // //byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);


            // // // Set new alpha values.
            // // newVertexColors[vertexIndex + 0].a = alpha;
            // // newVertexColors[vertexIndex + 1].a = alpha;
            // // newVertexColors[vertexIndex + 2].a = alpha;
            // // newVertexColors[vertexIndex + 3].a = alpha;

            // // Tint vertex colors
            // // Note: Vertex colors are Color32 so we need to cast to Color to multiply with tint which is Color.
            // newVertexColors[vertexIndex + 0] = ColorTint;
            // newVertexColors[vertexIndex + 1] = ColorTint;
            // newVertexColors[vertexIndex + 2] = ColorTint;
            // newVertexColors[vertexIndex + 3] = ColorTint;

            // // Upload the changed vertex colors to the Mesh.
            // m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);


            // // if (alpha == 255)
            // // {
            // //     startingCharacterRange += 1;

            // //     if (startingCharacterRange == characterCount)
            // //     {
            // //         // Update mesh vertex data one last time.
            // //         m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // //         yield return new WaitForSeconds(1.0f);

            // //         // Reset the text object back to original state.
            // //         m_TextComponent.ForceMeshUpdate();

            // //         yield return new WaitForSeconds(1.0f);

            // //         // Reset our counters.
            // //         currentCharacter = 0;
            // //         startingCharacterRange = 0;
            // //         //isRangeMax = true; // Would end the coroutine.
            // //     }
            // // }



            // // if (currentCharacter + 1 < characterCount) currentCharacter += 1;

            // yield return new WaitForSeconds(StepTime);
        }
    }
}