using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int activeLevel = 1;

    public int level1Score;
    public int level2Score;
    public int level3Score;
    public int level4Score;

    public TMP_Text playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveLevel(int level)
    {
        Instance.activeLevel = level;
    }

    public void AddPoints(Card card)
    {
        switch (activeLevel)
        {
            case (int)Enums.Levels.MECANICA:
                level1Score += card.mecanica;
                break;
            case (int)Enums.Levels.NARRATIVA:
                level2Score += card.narrativa;
                break;
            case (int)Enums.Levels.ESTETICA:
                level3Score += card.estetica;
                break;
            case (int)Enums.Levels.TECNOLOGIA:
                level4Score += card.tecnologia;
                break;
        }
    }
   
}
