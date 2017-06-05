﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : StageObject 
{
	[Header("-- BOX --")]
	public bool isMetallic;

	[Header("Drop List")]
	public GameObject[] Droppables;

	[Header("Drop Offset")]
	public float xOffset = 0f;
	public float yOffset = 0.8f;

	private GameObject _happyStage;
	private GameObject _sadStage;

	void Start()
	{
		base.ID = (int)IDList.ID.Box;
		_happyStage = GameObject.FindGameObjectWithTag ("HappyStage");
		_sadStage = GameObject.FindGameObjectWithTag ("SadStage");
	}

	public void DropItems ()
	{
		float multiDropOffset = -1f;
		for (int i = 0; i < Droppables.Length; i++)
		{
			GameObject newItem = Instantiate (Droppables[i], new Vector3 (this.transform.position.x + xOffset + multiDropOffset, this.transform.position.y + yOffset, this.transform.position.z), this.transform.rotation);
			if(_happyStage.activeInHierarchy)
			{
				newItem.transform.parent = _happyStage.transform;
			}
			else if (_sadStage.activeInHierarchy)
			{
				newItem.transform.parent = _sadStage.transform;
			}
			multiDropOffset += 1f;
		}
	}
}
