using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public Transform t1TopRight;
    public Transform t1BottomLeft;
    public Transform cam1Pos;
    public Transform cam2Pos;
    
    public Transform t2TopRight;
    public Transform t2BottomLeft;
 
    public Transform t2PlayerPos;

    public GameObject theGate;

    public int[] levelScoreGoals;
    private void Start()
    {
        GameManager.Instance.levelManager = this;
        InitStep1();
    }

    public void InitStep1()
    {
        GameManager.Instance.playerControler.bottomLeft = t1BottomLeft;
        GameManager.Instance.playerControler.topRight = t1TopRight;
        GameManager.Instance.playerControler.dragStartPos.y = -999f;
        GameManager.Instance.cameraController.gameObject.transform.position = cam1Pos.position;

        GameManager.Instance.menuUIManager.scoreGoals = levelScoreGoals;
        GameManager.Instance.menuUIManager.score = 0;
        GameManager.Instance.gameOver = false;
        GameManager.Instance.currentStep = 0;
        GameManager.Instance.levelStarted = false;

        foreach (var slider in GameManager.Instance.menuUIManager.sliders)
        {
            slider.value = 0f;
        }

        GameManager.Instance.menuUIManager.levelText.text = GameManager.Instance.currentLevel.ToString();
        GameManager.Instance.menuUIManager.levelText2.text = (GameManager.Instance.currentLevel+1).ToString();
        GameManager.Instance.stepStarted = true;
    }
    
    public void InitStep2()
    {
        GameManager.Instance.stepStarted = false;
        GameManager.Instance.playerControler.bottomLeft = t2BottomLeft;
        GameManager.Instance.playerControler.topRight = t2TopRight;


        theGate.transform.DOMove(new Vector3(0, -5, 0), 1f).SetRelative();
        Vector3 playerPos = new Vector3(t2PlayerPos.position.x,GameManager.Instance.playerControler.transform.position.y,GameManager.Instance.playerControler.transform.position.z);
        GameManager.Instance.playerControler.transform.DOMove(playerPos, 1f).OnComplete(
            () =>
            {
                GameManager.Instance.playerControler.transform.DOMove(t2PlayerPos.position, 5f);
                GameManager.Instance.cameraController.transform.DOMove(cam2Pos.position, 5f).OnComplete(
                    () =>
                    {
                        GameManager.Instance.playerControler.dragStartPos.y = -999f;
                        GameManager.Instance.stepStarted = true;
                        GameManager.Instance.currentStep = 1;
                    }
                    );
            }
            );

    }
}
