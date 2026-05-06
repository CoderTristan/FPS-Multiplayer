using UnityEngine;

public class ScoreboardEntry : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText, killText, deathText;

    public void SetData(string name, int kills, int deaths)
    {
        nameText.text = name;
        killText.text = kills.ToString();
        deathText.text = deaths.ToString();
    }
}
