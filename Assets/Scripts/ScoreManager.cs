using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public OxygenSystem oxygenSystem;
    public Transform playerTransform;

    private float score = 0f;
    private bool isCounting = true;

    void Start()
    {
        if (oxygenSystem == null)
        {
            oxygenSystem = FindObjectOfType<OxygenSystem>();
        }

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (isCounting && oxygenSystem != null && playerTransform != null)
        {
            bool isInDangerZone = playerTransform.position.y < oxygenSystem.dangerZoneY;
            bool hasOxygen = oxygenSystem.currentOxygen > 0;

            if (isInDangerZone && hasOxygen)
            {
                score += Time.deltaTime;
                UpdateScoreUI();
            }
            else if (!hasOxygen)
            {
                isCounting = false;
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
        }
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }
}