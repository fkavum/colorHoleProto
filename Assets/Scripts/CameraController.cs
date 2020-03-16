using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera camera;
	private Vector3 defaultPos;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		GameManager.Instance.cameraController = this;
	}

	private void Start()
	{
		defaultPos = transform.localPosition;
	}

	
	
}