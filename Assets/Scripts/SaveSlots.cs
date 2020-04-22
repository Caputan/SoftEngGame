using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;

public class SaveSlots : MonoBehaviour
{
	public GameObject newSlotPrefab;
	public GameObject createdSlotPrefab;

	private int index;


	private void Start()
	{
		try
		{
			GameObject.Find("SaveSlot_New(Clone)").GetComponent<Button>().onClick.AddListener(CreateNewSaveSlot);
		} catch
		{
			Debug.Log("No new save slots");
		}
	}
	private void Awake()
	{
		for (int i = 0; i < 3; i++)
		{
			index = i;
			string path = Application.persistentDataPath + "/player" + i.ToString() + ".save";
			if (!File.Exists(path))
			{
				Instantiate(newSlotPrefab, this.transform);
				break;
			} else
			{
				GameObject slot = Instantiate(createdSlotPrefab, this.transform);
				Slot slotData = slot.GetComponent<Slot>();
				slotData.SetActiveIndex(index);
				DataToSave data = SaveSystem.LoadPlayer();
				slotData.health = data.playerHealth;
				slotData.nicknameText.readOnly = true;
				slotData.dateText.text = "Date: " + data.date;
				slotData.nicknameText.text = data.nickname;
			}
		}
	}

	public void CreateNewSaveSlot()
	{

		Destroy(GameObject.Find("SaveSlot_New(Clone)"));
		GameObject slot = Instantiate(createdSlotPrefab, this.transform);
		Slot slotData = slot.GetComponent<Slot>();
		slotData.SetActiveIndex(index);
		slotData.health = 100;
		slotData.dateText.text = "Date: " + DateTime.Now.ToString();

		if (index < 2)
		{
			GameObject newSlot = Instantiate(newSlotPrefab, this.transform);
			newSlot.GetComponent<Button>().onClick.AddListener(CreateNewSaveSlot);
		}

	}
}
