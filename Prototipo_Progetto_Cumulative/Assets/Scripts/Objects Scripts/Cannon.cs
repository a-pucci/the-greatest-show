﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : StageObject 
{
	public GameObject Bullet;
	public float BulletForce;
	public float RotationSpeed = 100;

	private bool _controlled = false;
	private bool _hasBall;
	private PlayerUserController _player;
	private Vector2 _shootLocation;
	private Transform _shotStart;
	private GameObject _aim;

	// Use this for initialization
	void Start () 
	{
		base.ID = (int)IDList.ID.Cannon;
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerUserController> ();
		_shotStart = this.gameObject.transform.FindChild ("BarrelEnd");
		_aim = this.gameObject.transform.FindChild ("Aim").gameObject;
		_aim.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{
		_shootLocation = new Vector2(_shotStart.position.x, _shotStart.position.y);

		if (_controlled == true) 
		{
			_aim.SetActive (true);
			if (Input.GetKey (KeyCode.W)) 
			{
				if(transform.rotation.z < 0.5)
				{
					transform.Rotate (Vector3.forward * RotationSpeed * Time.deltaTime);
				}
			}

			if (Input.GetKey (KeyCode.S)) 
			{
				if(transform.rotation.z > 0)
				{
					transform.Rotate (Vector3.back * RotationSpeed * Time.deltaTime);
				}
			}
				

			if (Input.GetKeyDown (KeyCode.Mouse1)) 
			{
				Shoot ();
				_controlled = false;
				_aim.SetActive (false);
				_player.CanMove = true;
			}
		}

	}

	void Shoot()
	{
		if(_hasBall)
		{
			GameObject clone = Instantiate (Bullet, _shootLocation, Quaternion.identity) as GameObject;
			Rigidbody2D cloneRB = clone.GetComponent<Rigidbody2D> ();
			SpriteRenderer cloneSR = clone.GetComponent <SpriteRenderer> ();
			cloneSR.sortingLayerName = "Interactive";
			clone.GetComponent<CannonBall> ().shooted = true;

			cloneRB.AddRelativeForce (transform.TransformDirection (new Vector2( (Mathf.Cos (transform.rotation.z * Mathf.Deg2Rad) * BulletForce),
				(Mathf.Sin (transform.rotation.z * Mathf.Deg2Rad) * BulletForce) )), ForceMode2D.Impulse);

			_hasBall = false;
		}
	}

	public bool InsertBall()
	{
		_hasBall = true;
		return _hasBall;
	}

	public override void Interact (GameObject other)
	{
		if(CanInteract (other))
		{
			if(other.GetComponent<StageObject>().ID == (int)IDList.ID.Torch && _hasBall)
			{
				_controlled = true;
				_player.CanMove = false;
			}
			else if(other.GetComponent<StageObject>().ID == (int)IDList.ID.CannonBall)
			{
				_hasBall = true;
			}
		}
	}

	public override bool CanInteract (GameObject other)
	{
		bool canInteract = false;
		if(other.GetComponent<StageObject> () != null)
		{
			int otherID = other.GetComponent<StageObject> ().ID;
			if(otherID == (int)IDList.ID.CannonBall || otherID == (int)IDList.ID.Torch)
			{
				canInteract = true;
			}		
		}
		return canInteract;
	}
} 
	