using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectEvents : EventTrigger
{
	// Object Setting
	ObjectSettings OS;

	// DDM Game Object
	DragAndDropManager DDM;

	void Awake()
	{
		// Getting the Object Setting that assigned to this GameObject
		OS = GetComponent<ObjectSettings>();
		// Getting DDM GameObject
		// Replacing the call to GameObject.Find("DDM") with a FindObjectOfType,
		// so that the GameObject isn't coupled to the name 'DDM'
		DDM = FindObjectOfType<DragAndDropManager>();
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!OS.OnReturning)
		{
			if (DDM.TargetPlatform == DragAndDropManager.Platforms.PC)
			{
				// for PC
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					OS.PointerDown("User", "");
				}
			}
			else
			{
				// for Mobile
				if (eventData.pointerId == 0)
				{
					OS.PointerDown("User", "");
				}
			}
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (!OS.OnReturning)
		{
			if (DDM.TargetPlatform == DragAndDropManager.Platforms.PC)
			{
				// for PC
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					OS.PointerUp("User");
				}
			}
			else
			{
				// for Mobile
				if (eventData.pointerId == 0)
				{
					OS.PointerUp("User");
				}
			}
		}
	}
}
