using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : Singleton<MenuManager>
{
    public int score;
    public int[] scoreGoals;
    public Slider[] sliders;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelText2;

    public GameObject mainMenu;
	public GameObject gameplay;
    public GameObject gameOver;
    public GameObject levelCompleted;
	public TextMeshProUGUI scoreText;

	private void Awake()
	{
		GameManager.Instance.menuUIManager = this;
	}

	public void Start()
	{
		StartLevel();
	}
	
	public void StartLevel()
	{
		mainMenu.SetActive(true);
		
		foreach (var slider in sliders)
		{
			slider.DOValue(0f,1f);
		}
	}

	public void GameOver()
	{
		GameManager.Instance.gameOver = true;
		GameManager.Instance.stepStarted = false;
		gameOver.SetActive(true);
	}

	public void StartGame()
	{
		mainMenu.SetActive(false);
		gameplay.SetActive(true);
		GameManager.Instance.Play();
	}

	public void Continue()
	{
		GameManager.Instance.levelStarted = false;
		GameManager.Instance.gameOver = false;
		GameManager.Instance.RestartScene();
	}
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
	    sliders[GameManager.Instance.currentStep].DOValue((float)score / (float)scoreGoals[GameManager.Instance.currentStep],0.5f);
    }

    public void CheckWinCondition(int goalIndex)
    {
	    if(GameManager.Instance.gameOver) return;
	    if (score >= scoreGoals[goalIndex])
	    {
		    switch (goalIndex)
		    {
			    case 0:
				    Debug.Log("Going Next Step");
				    GameManager.Instance.levelManager.InitStep2();
				    score = 0;
				    break;
			    case 1:
				    Debug.Log("Going Next Level");
				    GameManager.Instance.playerControler.playerParticle.SetActive(true);
				    StartCoroutine(GoNextLevelIE());
				    break;
		    }
	    }
    }

    public void RestartGame()
    {
	    gameOver.SetActive(false);
	    GameManager.Instance.Restart();
    }

    IEnumerator GoNextLevelIE()
    {
	    GameManager.Instance.stepStarted = false;
	    yield return new WaitForSeconds(1.5f);
	    GameManager.Instance.GoNextLevel();
    }
}
