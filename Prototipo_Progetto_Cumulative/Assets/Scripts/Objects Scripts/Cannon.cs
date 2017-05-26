﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : StageObject 
{
	public GameObject Bullet;
	public float BulletForce;
	public float RotationSpeed = 100;

	public bool Controlled = false;

	private bool _hasBall;
	private PlayerUserController _player;
	private Vector2 _shootLocation;
	private Transform _shotStart;

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerUserController> ();
		_shotStart = this.gameObject.transform.FindChild ("BarrelEnd");
	}

	// Update is called once per frame
	void Update () 
	{
		_shootLocation = new Vector2(_shotStart.position.x, _shotStart.position.y);

		if (Controlled == true) 
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
				Controlled = false;
				_player.controllingCannon = false;
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
	public bool hasBall()
	{
		return _hasBall;
	}
} 
	