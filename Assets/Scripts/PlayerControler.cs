using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
	public GameObject playerParticle;
    //for keyboard play
	public float scale;
	public float radius;

	public Transform visuals;
	public Transform directionRing;

	public SphereCollider detector;
	public BoxCollider[] colliders;
	public List<Rigidbody> victims;
	
	public float maxDragDistance = 40f;
	private float curDragDistance;
	private Vector3 moveDirection;
	public Vector3 dragStartPos;
	private Vector3 playerDragStartPos;
	

	public Transform topRight;
	public Transform bottomLeft;

	private void Awake()
	{
		GameManager.Instance.playerControler = this;
		SetScale();
	}
	private void Update()
	{
		if (GameManager.Instance.gameOver && Input.GetMouseButtonDown(0))
		{
			GameManager.Instance.menuUIManager.RestartGame();
		}
		DetectVictims();

		if (GameManager.Instance.stepStarted == false) return;
		MovePlayer();
    }
	
	private void MovePlayer()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragStartPos = MousePosOnFloor();
			playerDragStartPos = transform.position;
			if (!GameManager.Instance.levelStarted)
			{
				MenuManager.Instance.StartGame();
			}
			return;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 mousePos = MousePosOnFloor();

			if (mousePos.y == -999f || dragStartPos.y == -999f)
			{
				return;
			}
			
			Vector3 newPos = playerDragStartPos + (mousePos - dragStartPos);

			float xmin = bottomLeft.transform.position.x+radius/2f;
			float xmax = topRight.transform.position.x - radius/2f;
			float zmin = bottomLeft.transform.position.z + radius/2f;
			float zmax = topRight.transform.position.z -radius/2f;

			if (newPos.x < xmin)
			{
				float pad = newPos.x - xmin ;
				newPos.x = xmin;
				dragStartPos.x = dragStartPos.x + pad;
			}
			if (newPos.x > xmax)
			{
				float pad = newPos.x - xmax ;
				newPos.x = xmax;
				dragStartPos.x = dragStartPos.x + pad;
			}
			if (newPos.z < zmin)
			{
				float pad = newPos.z - zmin ;
				newPos.z = zmin;
				dragStartPos.z = dragStartPos.z + pad;
			}
			
			if (newPos.z > zmax)
			{
				float pad = newPos.z - zmax ;
				newPos.z = zmax;
				dragStartPos.z = dragStartPos.z + pad;
			}

			transform.position = newPos;
		}
	}

	private void OnDrawGizmos()
	{
		//Gizmos.DrawWireSphere(transform.position,radius);
		Gizmos.DrawWireCube(transform.position, new Vector3(radius*2f,radius*2f,radius*2f));

	}

	private void DetectVictims()
	{
		var nearbyObjects = Physics.OverlapBox(transform.position, new Vector3(radius,radius*3f,radius));
		foreach(var nearbyObject in nearbyObjects)
		{
			if(nearbyObject.gameObject.layer == 10)
			{
				Rigidbody nearbyObjectRb = nearbyObject.GetComponentInParent<Rigidbody>();
				if(!victims.Contains(nearbyObjectRb))
				victims.Add(nearbyObjectRb);
				nearbyObjectRb.isKinematic = false;
				nearbyObjectRb.AddForce(Vector3.down * scale * GameManager.Instance.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (!GameManager.Instance.stepStarted)
		{
			return;
		}
		
		if (other.gameObject.tag == "Enemy" && GameManager.Instance.stepStarted)
		{
			MenuManager.Instance.GameOver();
		}
		
		if (other.gameObject.layer == 10) // Victims fall into the hole
		{
            GameManager.Instance.menuUIManager.AddScore(1);
		}

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.position.y < 0) // Victims falls through the hole
		{
			other.gameObject.SetActive(false);

			Rigidbody victimRb = other.GetComponent<Rigidbody>();
			if (victimRb)
			{
				victims.Remove(victimRb);
			}
			GameManager.Instance.menuUIManager.CheckWinCondition(GameManager.Instance.currentStep);
		}
	}

	public void SetScale()
	{
		visuals.localScale = new Vector3(scale, scale, scale);
		visuals.localPosition = new Vector3(0, -scale / 2f - 0.49f, 0);
		detector.center = new Vector3(0, -1f - scale / 2f, 0);
		detector.radius = scale / 2f;		

		foreach (var coll in colliders)
		{
			var direction = coll.center.normalized * 50.025f;
			coll.center = direction * (1 + scale / 100f);
		}

	}
	
	
	
	
	
	
	
	
	
	
	private Vector3 MousePosOnFloor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
		{
			Vector3 hitPoint = hit.point;
           
			return hitPoint;
		}
		else
		{
			return new Vector3(0f,-999f,0f);
		}
	}
	
	
	
	
	
	

	
}