﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : StageObject 
{

	public GameObject ShotStart;
	public GameObject Bullet;
	public GameObject Dot;
	public float BulletForce;
	public float RotationSpeed = 100;

	public bool Controlled = false;

	private bool _hasBall;
	private PlayerUserController _player;
	private Vector2 _shootLocation;

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerUserController> ();
	}

	// Update is called once per frame
	void Update () 
	{
		_shootLocation = new Vector2(ShotStart.transform.position.x, ShotStart.transform.position.y);

		if (Controlled == true) 
		{

			if (Input.GetKey (KeyCode.W)) 
			{
				transform.Rotate (Vector3.forward * RotationSpeed * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.S)) 
			{
				transform.Rotate (Vector3.back * RotationSpeed * Time.deltaTime);
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
	