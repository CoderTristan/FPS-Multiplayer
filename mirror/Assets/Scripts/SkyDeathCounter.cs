using UnityEngine;
using TMPro;

public class SkyDeathCounter : MonoBehaviour
{
    public static SkyDeathCounter instance;

    private TextMeshPro text;

    void Awake()
    {
        instance = this;
        text = GetComponent<TextMeshPro>();
    }

    public void UpdateText()
    {
        text.text = "ATTEMPT " + PlayerControl.deathCount;
    }
}
