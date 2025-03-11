using UnityEngine;
using TMPro;

public class GameOverTextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float waveSpeed = 5;
    public float waveHeight = 10;

    void Start()
    {
        text.ForceMeshUpdate();
    }
    void Update()
    {
        AnimateWave();
    }

    void AnimateWave()
    {
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            float waveOffset = Mathf.Sin(Time.time * waveSpeed + i) * waveHeight;

            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] += new Vector3(0, waveOffset, 0);
            }
        }

        text.UpdateVertexData();
    }
}
