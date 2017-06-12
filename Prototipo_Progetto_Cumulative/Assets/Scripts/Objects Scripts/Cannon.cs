﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : StageObject 
{
	public GameObject Bullet;
	public float BulletForce;
	public float RotationSpeed = 100;
	public float CameraWaitAfterShoot;

	private bool _controlled = false;
	private bool _hasBall;
	private PlayerUserController _player;
	private Camera2DFollow _camera;
	private Vector2 _shootLocation;
	private Transform _shotStart;
	private GameObject _aim;
	private PlayerPickup _playerPick;

	// Use this for initialization
	void Start () 
	{
		base.ID = (int)IDList.ID.Cannon;
		_camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera2DFollow> ();
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerUserController> ();
		_playerPick = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerPickup> ();
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
				_camera.LockCamera (false, CameraWaitAfterShoot);
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

	public override void Interact (ref GameObject other)
	{
		if(CanInteract (other))
		{
			if(other.GetComponent<StageObject>().ID == (int)IDList.ID.Torch && _hasBall)
			{
				_controlled = true;
				_player.CanMove = false;
				_aim.SetActive (true);
				_camera.LockCamera (true);
			}
			else if(other.GetComponent<StageObject>().ID == (int)IDList.ID.CannonBall)
			{
				_hasBall = true;
				_playerPick.RemoveItemFromInventory ();
			}
		}
	}

	public override bool CanInteract (GameObject other)
	{
		bool canInteract = false;
		if(other.GetComponent<StageObject> () != null)
		{
			int otherID = other.GetComponent<StageObject> ().ID;
			if(otherID == (int)IDList.ID.CannonBall || (otherID == (int)IDList.ID.Torch && _hasBall))
			{
				canInteract = true;
			}		
		}
		return canInteract;
	}
} 
	