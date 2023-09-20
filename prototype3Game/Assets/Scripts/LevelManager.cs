using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    [Header("references")]
    [SerializeField] public Text PointsUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Attributes")]
    [SerializeField] private int maxMissed;

    [Header("Combo")]
    public int combo = 0;

    private int missedNotes;
    private int points = 0;
    [HideInInspector] public bool survived;
    // Start is called before the first frame update

    private void Awake() {
        main = this;
    }
    void Start()
    {
        UpdatePointsUI();
    }

    public void win(){
        survived = true;
        levelCompleteUI.SetActive(true);
    }

    public void loser(){
        gameOverUI.SetActive(true);
    }

    public void IncreasePoints(){
        points += combo;
        UpdatePointsUI();
    }

    public void UpdateCombo(bool hit)
    {
        combo = hit ? combo + 1 : 0;

        // Notify the NewBehaviourScript to update speed
        Circularmovement.main.UpdateSpeed(combo);
    }

    // Update is called once per frame
    public void UpdatePointsUI()
    {
        PointsUI.text = "Points: " + points;
    }
}
