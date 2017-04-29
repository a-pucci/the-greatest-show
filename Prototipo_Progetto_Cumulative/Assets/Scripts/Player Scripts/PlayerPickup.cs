﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerPickup : MonoBehaviour 
{
	public Text PickupText;
	public Image InventoryIcon;
	public float xDropOffset = 0.0f;
	public float YDropOffset = 0.3f;

	public GameObject RedStage;
	public GameObject GreenStage;

	private GameObject _triggerObject;
	private GameObject _storedItem;
	private Door _door;

	private bool _pickedUp = false;


	// Use this for initialization
	void Start () 
	{
		_door = GameObject.FindGameObjectWithTag ("Door").GetComponent<Door>();
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
				if (_pickedUp) 
				{
					DropItem ();
				}

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

		if(_storedItem.transform.parent.tag == "Enemy")
		{
			GameObject enemyParent = new GameObject("enemyParent");

			enemyParent = _storedItem.transform.parent.gameObject;
			_storedItem.transform.parent = this.transform.parent;
			Destroy (enemyParent);
		}
		else
		{
			_storedItem.transform.parent = this.transform.parent;
		}

		_storedItem.SetActive (false);
	}

	private void DropItem ()
	{
		_storedItem.SetActive (true);
		_storedItem.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + YDropOffset, this.transform.position.z);

		if(RedStage.activeInHierarchy)
		{
			_storedItem.transform.parent = RedStage.transform;
		}
		else if (GreenStage.activeInHierarchy)
		{
			_storedItem.transform.parent = GreenStage.transform;
		}

		_storedItem = null;
		_pickedUp = false;
		InventoryIcon.enabled = false;

	}

	private void Interact(GameObject trigger)
	{

		if(RedStage.activeInHierarchy)
		{
			_storedItem.transform.parent = RedStage.transform;
		}
		else if (GreenStage.activeInHierarchy)
		{
			_storedItem.transform.parent = GreenStage.transform;
		}

		int IDstored = _storedItem.GetComponent<Identifier> ().ID;
		int IDcollide = trigger.GetComponent<Identifier> ().ID;

		switch (IDcollide)
		{
		case (int)IDList.ID.Dummy:
			
			Dummy dummy = trigger.GetComponent<Dummy> ();
			if(dummy.canInteract(IDstored))
			{
				if (dummy.HasMask ())
				{
					_storedItem.SetActive (true);
					GameObject newMask = _storedItem;
					_storedItem = dummy.LoseMask ();

					_pickedUp = true;
					InventoryIcon.enabled = true;

					InventoryIcon.sprite =  _storedItem.GetComponent<SpriteRenderer> ().sprite;

					_storedItem.SetActive (false);

					dummy.PutMask (newMask);
				}
				else 
				{
					_storedItem.SetActive (true);
					dummy.PutMask (_storedItem);
					_storedItem = null;
					_pickedUp = false;
					InventoryIcon.enabled = false;
				}
				_door.checkOpen ();
			}
			break;

		case (int)IDList.ID.Box:
			
			Box box = trigger.GetComponent<Box> ();
			if(box.canInteract(IDstored))
			{
				box.DropItems ();
				trigger.SetActive (false);
			}
			break;

		case (int)IDList.ID.Chest:

			Chest chest = trigger.GetComponent<Chest> ();
			if(chest.canInteract(IDstored))
			{
				chest.DropItems ();
				trigger.SetActive (false);
			}
			break;
		}
	}
}
