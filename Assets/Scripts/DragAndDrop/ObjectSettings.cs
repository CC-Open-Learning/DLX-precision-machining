using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSettings : MonoBehaviour
{
	// DDM Game Object
	DragAndDropManager DDM;

	// Rect Transform Component of this object
	RectTransform thisRT;

	// Parent of this object
	Transform thisParent;

	// Vector3 variables
	Vector3 CurrentPos;
	Vector3 MousePos;
	[HideInInspector]
	public Vector3 FirstPos;
	[HideInInspector]
	public Vector3 FirstScale;
	//

	// Bools
	bool PDown = false;
	bool CheckStatus = false;
	[HideInInspector]
	public bool DefaultPanel = false;
	[HideInInspector]
	public bool Dropped = false;
	[HideInInspector]
	public bool OnReturning = false;
	//

	//used for dragging modes
	float differenceX;
	float differenceY;
	//

	// Customization Tools
	public string Id;   // the Id of this object

	[Tooltip("Allow user to control this object")]
	public bool UserControl = true;

	[Header("Return Object Smoothly (DragDrop Failed)")]
	[Tooltip("Return Object to its first Position Smoothly When Drag & Drop Failed")]
	public bool ReturnSmoothly = false;
	[Tooltip("Returning speed")]
	[Range(0.1f, 2.0f)]
	public float ReturnSpeed = 1.0f;

	[Header("Scale Object (Dragging)")]
	[Tooltip("Scale Object When dragging gets begun")]
	public bool ScaleOnDrag = false;
	[Tooltip("Object Scale")]
	public Vector3 DragScale = new Vector3(1.0f, 1.0f, 1.0f);

	[Header("Stay Object on dropped position")]
	[Tooltip("Stay Object on dropped position When the it gets dropped successfully")]
	public bool StayDroppedPos = false;

	[Header("Scale Object (Dropped successfully)")]
	[Tooltip("Scale Object When it gets dropped successfully")]
	public bool ScaleOnDropped = false;
	[Tooltip("Object Scale")]
	public Vector3 DropScale = new Vector3(1.0f, 1.0f, 1.0f);

	[Header("Locking Object")]
	[Tooltip("Lock Object When it gets dropped successfully")]
	public bool LockObject = false;

	[Header("Return Object (Dropped successfully)")]
	[Tooltip("Return Object to its first position when it gets dropped successfully")]
	public bool ReturnObject = false;

	[Header("Smooth Replacement")]
	[Tooltip("Replace Object smoothly when it gets dropped successfully")]
	public bool ReplaceSmoothly = false;
	[Range(0.1f, 2.0f)]
	public float ReplacementSpeed = 1.0f;

	[Header("Allow to switch Objects")]
	[Tooltip("Allow to switch Objects between panels")]
	public bool SwitchObjects = false;
	[Tooltip("Move Object smoothly When it is switching")]
	public bool MoveSmoothly = false;
	[Range(0.1f, 2.0f)]
	public float MovementSpeed = 1.0f;

	[Header("Filter Panels")]
	[Tooltip("Allow using Filter Panels tool")]
	public bool FilterPanels = false;
	[Tooltip("The Ids of the panels that object is allowed to drop on them")]
	public string[] AllowedPanels;

	[Header("Events Management")]
	public UnityEvent OnBeginDragging;
	public UnityEvent OnDragDropFailed;
	public UnityEvent OnDroppedSuccessfully;
	//

	void Awake()
	{
		// Getting Rect Transform component of this object
		thisRT = GetComponent<RectTransform>();
		// Getting the parent of this object
		thisParent = thisRT.parent;
		// Getting DDM GameObject
		// Replacing the call to GameObject.Find("DDM") with a FindObjectOfType,
		// so that the GameObject isn't coupled to the name 'DDM'
		DDM = FindObjectOfType<DragAndDropManager>();
	}

	void Update()
	{
		DragObject();
	}

	void DragObject()
	{
		if (PDown)
		{
			SetMousePos();
			if (DDM.DraggingModes == DragAndDropManager.DragModes.ChangeToMousePos)
			{
				thisRT.position = new Vector3(MousePos.x, MousePos.y, thisRT.position.z);
			}
			else
			{
				thisRT.position = new Vector3(MousePos.x + differenceX, MousePos.y + differenceY, thisRT.position.z);
			}
		}
	}

	public void PointerDown(string state, string AIPanel)
	{
		if ((state == "User" && UserControl) || state == "AI")
		{
			PointerDownActions(state, AIPanel);
		}
	}

	void PointerDownActions(string state, string AIPanel)
	{
		if (Dropped)
		{
			// Setup customization tools of the panel
			int index = DDM.IdManager.IndexOf(Id);
			if (index != -1)
			{
				if (DDM.AllPanels[index].LockObject != PanelSettings.ObjectLockStates.LockObject)
				{
					if (!LockObject || DDM.AllPanels[index].LockObject == PanelSettings.ObjectLockStates.DoNotLockObject)
					{
						BeginDragging(state, AIPanel);
					}
				}
			}
		}
		else
		{
			BeginDragging(state, AIPanel);
		}
	}

	void BeginDragging(string state, string AIPanel)
	{
		CurrentPos = thisRT.position;

		if (ScaleOnDrag)
		{ // Setup ScaleOnDrag tool
			thisRT.localScale = DragScale;
		}

		thisRT.SetParent(DDM.SecondCanvas.GetComponent<RectTransform>());

		if (state == "User")
		{
			if (DDM.DraggingModes == DragAndDropManager.DragModes.DoNotChange)
			{
				SetMousePos();

				differenceX = thisRT.position.x - MousePos.x;
				differenceY = thisRT.position.y - MousePos.y;
			}
			PDown = true;
		}
		else
		{
			// Setup AI system
			float speed = DDM.GetComponent<AIManager>().MovementSpeed;

			for (int i = 0; i < DDM.AllPanels.Length; i++)
			{
				if (AIPanel == DDM.AllPanels[i].Id)
				{
					DDM.SmoothMoveStarter(state, thisRT, DDM.AllPanels[i].GetComponent<RectTransform>().position, speed);
				}
			}
		}

		// Events Management
		if (OnBeginDragging != null)
		{
			OnBeginDragging.Invoke();
		}
	}

	void SetMousePos()
	{
		Vector3 screenPoint;

		if (DDM.TargetPlatform == DragAndDropManager.Platforms.PC)
		{
			// for PC
			if (DDM.FirstCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				// ScreenSpaceCamera & WorldSpace support
				screenPoint = Input.mousePosition;
				screenPoint.z = DDM.FirstCanvas.planeDistance;
				MousePos = Camera.main.ScreenToWorldPoint(screenPoint);
			}
			else
			{
				MousePos = Input.mousePosition;
			}
		}
		else
		{
			// for Mobile
			if (DDM.FirstCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				// ScreenSpaceCamera & WorldSpace support
				screenPoint = Input.touches[0].position;
				screenPoint.z = DDM.FirstCanvas.planeDistance;
				MousePos = Camera.main.ScreenToWorldPoint(screenPoint);
			}
			else
			{
				MousePos = Input.touches[0].position;
			}
		}
	}

	public void PointerUp(string state)
	{
		if ((state == "User" && UserControl) || state == "AI")
		{
			PointerUpActions();
		}
	}

	void PointerUpActions()
	{
		if (Dropped)
		{
			// Setup customization tools of the panel
			int index = DDM.IdManager.IndexOf(Id);
			if (index != -1)
			{
				if (DDM.AllPanels[index].LockObject != PanelSettings.ObjectLockStates.LockObject)
				{
					if (!LockObject || DDM.AllPanels[index].LockObject == PanelSettings.ObjectLockStates.DoNotLockObject)
					{
						CheckObjectPos();
					}
				}
			}
		}
		else
		{
			CheckObjectPos();
		}
	}

	void CheckObjectPos()
	{
		PDown = false;
		CheckStatus = false;

		for (int i = 0; i < DDM.AllPanels.Length; i++)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(DDM.AllPanels[i].GetComponent<RectTransform>(), thisRT.position))
			{
				if (FilterPanels)
				{
					// setup Filter Panels tool
					if (Array.Exists(AllowedPanels, panel => panel == DDM.AllPanels[i].Id))
					{
						PanelDropTools(i);
					}
				}
				else
				{
					PanelDropTools(i);
				}
			}
		}

		if (!CheckStatus)
		{ // check if drag & drop failed
			if (!ReturnSmoothly)
			{
				thisRT.position = CurrentPos;

				// Save last position of this object
				if (DDM.SaveStates)
				{
					PlayerPrefs.SetFloat(Id + "X", CurrentPos.x);
					PlayerPrefs.SetFloat(Id + "Y", CurrentPos.y);
				}
			}
			else
			{
				DDM.SmoothMoveStarter("User", thisRT, CurrentPos, ReturnSpeed);
			}

			thisRT.localScale = FirstScale;

			// Events Management
			if (OnDragDropFailed != null)
			{
				OnDragDropFailed.Invoke();
			}
		}
		thisRT.SetParent(thisParent);
	}

	void PanelDropTools(int i)
	{
		if (DDM.IdManager[i] != "" && DDM.IdManager[i] != Id)
		{
			for (int j = 0; j < DDM.AllObjects.Count; j++)
			{
				if (DDM.AllObjects[j].Id == DDM.IdManager[i])
				{
					if (DDM.AllObjects[j].OnReturning)
					{
						return;
					}
				}
			}
			// Setup customization tools of the panel
			if (DDM.AllPanels[i].ObjectReplacement != PanelSettings.ObjectReplace.NotAllowed)
			{
				CheckStatus = true;
				DropActions(i);
			}
		}
		else
		{
			CheckStatus = true;
			DropActions(i);
		}
	}

	void DropActions(int i)
	{
		bool SwitchStatus = false;
		bool ReplaceDefaultPanel = false;

		// setup ReturnObject tool
		ReturnObjectTool();

		thisRT.localScale = FirstScale;

		if (!StayDroppedPos && !ReturnObject && DDM.AllPanels[i].ObjectPosition != PanelSettings.ObjectPosStates.DroppedPosition)
		{
			thisRT.position = new Vector3(DDM.AllPanels[i].GetComponent<RectTransform>().position.x, DDM.AllPanels[i].GetComponent<RectTransform>().position.y, thisRT.position.z);
		}

		// Save last position of this object
		if (DDM.SaveStates)
		{
			PlayerPrefs.SetFloat(Id + "X", thisRT.position.x);
			PlayerPrefs.SetFloat(Id + "Y", thisRT.position.y);
		}

		// Check if there is another object on target panel
		if (DDM.IdManager[i] != "" && DDM.IdManager[i] != Id)
		{
			if (DDM.AllPanels[i].ObjectReplacement == PanelSettings.ObjectReplace.Allowed)
			{
				for (int j = 0; j < DDM.AllObjects.Count; j++)
				{
					if (DDM.IdManager[i] == DDM.AllObjects[j].Id)
					{
						if (DDM.AllObjects[j].DefaultPanel)
						{
							ReplaceDefaultPanel = true;
						}
					}
				}
				// Check if objects should not switch between their panels. So this object will replace with second object
				if (!SwitchObjects && !DefaultPanel && !ReplaceDefaultPanel)
				{
					ObjectsReplacement(i);
				}
				else
				{
					// Objects will switch between their panels (if this object was on any panel)
					if (ObjectsSwitching(i))
					{
						SwitchStatus = true;
					}
					else
					{
						// This object was not on any panel. So this object will be replaced with second object
						ObjectsReplacement(i);
					}
				}
			}
		}

		// setup Multi Object tool
		int index = DDM.IdManager.IndexOf(Id);

		if (index != -1)
		{
			if (DDM.AllPanels[index].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode && !SwitchStatus)
			{
				DDM.AllPanels[index].RemoveMultiObject(Id);
				SetPrevPanelId();
			}
			else if (DDM.AllPanels[index].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
			{
				SetPrevPanelId();
			}
		}

		if (DDM.AllPanels[i].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
		{
			SetPrevPanelId();
			DDM.AllPanels[i].SetMultiObject(Id);
		}

		// setup ScaleOnDropped tool
		ScaleOnDroppedTool();

		if (!SwitchObjects && !DefaultPanel)
		{
			SetPrevPanelId();
		}
		else
		{
			if (DDM.IdManager[i] == "")
			{
				SetPrevPanelId();
			}
		}

		DDM.SetPanelObject(i, Id);

		// Setup customization tools of the panel
		if (DDM.AllPanels[i].ObjectPosition == PanelSettings.ObjectPosStates.PanelPosition)
		{
			thisRT.position = new Vector3(DDM.AllPanels[i].GetComponent<RectTransform>().position.x, DDM.AllPanels[i].GetComponent<RectTransform>().position.y, thisRT.position.z);
		}

		// Panel Events Management
		DDM.AllPanels[i].SetupPanelEvents();

		// Events Management
		if (OnDroppedSuccessfully != null)
		{
			OnDroppedSuccessfully.Invoke();
		}
	}

	void ObjectsReplacement(int i)
	{
		for (int j = 0; j < DDM.AllObjects.Count; j++)
		{
			if (DDM.IdManager[i] == DDM.AllObjects[j].Id && DDM.AllObjects[j].Dropped)
			{
				DDM.AllObjects[j].Dropped = false;
				DDM.AllObjects[j].GetComponent<RectTransform>().localScale = DDM.AllObjects[j].FirstScale;
				if (!ReplaceSmoothly)
				{
					DDM.AllObjects[j].GetComponent<RectTransform>().position = DDM.AllObjects[j].FirstPos;

					// Save last position of second object
					if (DDM.SaveStates)
					{
						PlayerPrefs.SetFloat(DDM.AllObjects[j].Id + "X", DDM.AllObjects[j].FirstPos.x);
						PlayerPrefs.SetFloat(DDM.AllObjects[j].Id + "Y", DDM.AllObjects[j].FirstPos.y);
					}
				}
				else
				{
					DDM.SmoothMoveStarter("User", DDM.AllObjects[j].GetComponent<RectTransform>(), DDM.AllObjects[j].FirstPos, ReplacementSpeed);
				}
			}
		}
	}

	bool ObjectsSwitching(int i)
	{
		int index = DDM.IdManager.IndexOf(Id);

		if (index != -1)
		{
			for (int j = 0; j < DDM.AllObjects.Count; j++)
			{
				if (DDM.IdManager[i] == DDM.AllObjects[j].Id)
				{
					if (DDM.AllPanels[index].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
					{
						// setup Multi Object tool
						DDM.AllPanels[index].RemoveMultiObject(Id);
						DDM.AllPanels[index].SetMultiObject(DDM.AllObjects[j].Id);
					}

					DDM.SetPanelObject(index, DDM.AllObjects[j].Id);

					DDM.AllObjects[j].GetComponent<RectTransform>().SetAsLastSibling();

					if (!MoveSmoothly)
					{
						DDM.AllObjects[j].GetComponent<RectTransform>().position = new Vector3(CurrentPos.x, CurrentPos.y, DDM.AllObjects[j].GetComponent<RectTransform>().position.z);

						// Save last position of second object
						if (DDM.SaveStates)
						{
							PlayerPrefs.SetFloat(DDM.AllObjects[j].Id + "X", CurrentPos.x);
							PlayerPrefs.SetFloat(DDM.AllObjects[j].Id + "Y", CurrentPos.y);
						}
					}
					else
					{
						DDM.SmoothMoveStarter("User", DDM.AllObjects[j].GetComponent<RectTransform>(), CurrentPos, MovementSpeed);
					}
				}
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	void SetPrevPanelId()
	{
		int index = DDM.IdManager.IndexOf(Id);

		if (index != -1)
		{
			if (DDM.AllPanels[index].ObjectReplacement != PanelSettings.ObjectReplace.MultiObjectMode || DDM.AllPanels[index].PanelIdManager.Count == 0)
			{
				DDM.SetPanelObject(index, "");
			}
			else
			{
				DDM.SetPanelObject(index, DDM.AllPanels[index].PanelIdManager[DDM.AllPanels[index].PanelIdManager.Count - 1]);
			}
			//Implements OnObjectExit event
			if (DDM.AllPanels[index].OnObjectExit != null)
				DDM.AllPanels[index].OnObjectExit.Invoke();
		}
	}

	void ReturnObjectTool()
	{
		if (!ReturnObject)
		{
			Dropped = true;
		}
		else
		{
			thisRT.position = CurrentPos;
		}
	}

	void ScaleOnDroppedTool()
	{
		if (Dropped && ScaleOnDropped)
		{
			thisRT.localScale = DropScale;
		}
	}

	public void ResetParent(Transform parentTransform)
    {
		thisParent = parentTransform;
    }
}
