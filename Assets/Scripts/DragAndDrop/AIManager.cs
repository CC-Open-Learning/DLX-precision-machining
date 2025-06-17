using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	// By using this component, you can simulate the drag and drop system.

	[Range(0.1f, 2.0f)]
	public float MovementSpeed = 1.0f;

	static DragAndDropManager DDM;

	void Awake()
	{
		// Getting DDM GameObject
		// Replacing the call to GameObject.Find("DDM") with a FindObjectOfType,
		// so that the GameObject isn't coupled to the name 'DDM'
		DDM = FindObjectOfType<DragAndDropManager>();
	}

	public static void AIDragDrop(string ObjectId, string PanelId)
	{
		for (int i = 0; i < DDM.AllObjects.Count; i++)
		{
			if (ObjectId == DDM.AllObjects[i].Id)
			{
				if (!DDM.AllObjects[i].OnReturning)
				{
					DDM.AllObjects[i].PointerDown("AI", PanelId);
				}
			}
		}
	}
}
