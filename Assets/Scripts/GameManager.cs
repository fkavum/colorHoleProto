using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public PlayerControler playerControler;
	public CameraController cameraController;
	public MenuManager menuUIManager;
	public LevelManager levelManager;

	public bool levelStarted = false;
	public bool stepStarted = true;
	public bool gameOver;

    public float gravity = 10f;

    public int currentLevel = 1;
    public int currentStep = 0;
    private GameObject currentLevelObj;

    private bool levelJustInitialized = false;
    
    public override void Awake()
    {
	    base.Awake();
    }

    private void Start()
    {
	    LoadLevel(currentLevel.ToString());
    }

    public void Play()
	{
		levelStarted = true;
	}

	public void RestartScene()
	{
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

	public void GoNextLevel()
	{
		if(levelJustInitialized) return;

		StartCoroutine(LevelInitializedJustYet());
		
		currentLevel++;

		if (currentLevel > 5)
		{
			currentLevel = 1;
		}
		LoadLevel(currentLevel.ToString());
	}
	private void LoadLevel(string level)
	{
		if (currentLevelObj != null)
		{
			stepStarted = false;
			Destroy(currentLevelObj);
			menuUIManager.StartLevel();
		}
		
		GameObject levelPrefab = Resources.Load<GameObject>(level);
		currentLevelObj = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
	}

	public void Restart()
	{
		LoadLevel(currentLevel.ToString());
	}

	IEnumerator LevelInitializedJustYet()
	{
		levelJustInitialized = true;
		yield return new WaitForSeconds(5f);
		levelJustInitialized = false;
	}

}