﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerPickup : MonoBehaviour 
{
	public Text PickupText;
	public Image InventoryIcon;
	public float YDropOffset = 0.3f;

	private GameObject _triggerObject;
	private GameObject _storedItem;

	private bool _pickedUp = false;


	// Use this for initialization
	void Start () 
	{
		PickupText.enabled = false;
		InventoryIcon.enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if(_triggerObject != null && CrossPlatformInputManager.GetButtonDown("Pickup"))
		{
			if (_triggerObject.CompareTag ("Pickup"))
			{
				Pickup ();
			}
			else if (_triggerObject.CompareTag("Interactive")  && _pickedUp)
			{
				Interact (_triggerObject);
			}
		}

		if(_storedItem != null && CrossPlatformInputManager.GetButtonDown("Drop"))
		{
			DropItem ();
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		_triggerObject = collision.gameObject;
		PickupText.enabled = true;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		_triggerObject = null;
		PickupText.enabled = false;
	}

	private void Pickup()
	{
		_pickedUp = true;
		InventoryIcon.enabled = true;
	
		InventoryIcon.sprite =  _triggerObject.GetComponent<SpriteRenderer> ().sprite;

		_storedItem = _triggerObject;
		_storedItem.gameObject.SetActive (false);
	}

	private void DropItem ()
	{
		_storedItem.gameObject.SetActive (true);

		_storedItem.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + YDropOffset, this.transform.position.z);
		_storedItem = null;
		_pickedUp = false;
		InventoryIcon.enabled = false;

	}

	private void Interact(GameObject trigger)
	{
		int IDstored = _storedItem.gameObject.GetComponent<Identifier> ().ID;
		int IDcollide = trigger.GetComponent<Identifier> ().ID;

		switch (IDcollide)
		{
		case (int)IDList.ID.Dummy:
			
			Dummy dummy = trigger.GetComponent<Dummy> ();
			if(dummy.canInteract(IDstored))
			{
				_storedItem.gameObject.SetActive (true);
				dummy.PutMask (_storedItem);
				_storedItem = null;
				_pickedUp = false;
				InventoryIcon.enabled = false;
			}
			break;

		case (int)IDList.ID.Box:
			
			Box box = trigger.GetComponent<Box> ();
			if(box.canInteract(IDstored))
			{
				box.DropHappyMask ();
				box.DropKey ();
				box.DropSadMask ();
				trigger.SetActive (false);
			}
			break;


		case (int)IDList.ID.Chest:

			Chest chest = trigger.GetComponent<Chest> ();
			if(chest.canInteract(IDstored))
			{
				chest.DropHappyMask ();
				chest.DropHammer ();
				chest.DropSadMask ();
				trigger.SetActive (false);
			}
			break;


		}
	}
}
