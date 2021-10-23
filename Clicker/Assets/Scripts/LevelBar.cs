using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    private Image levelBar;
    public float currentExp;
    private float maxExp = 10;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelBar = GetComponent<Image>();
    }

    
    void Update()
    {
        currentExp = gameManager.exp;
        levelBar.fillAmount = currentExp / maxExp;
    }
}
