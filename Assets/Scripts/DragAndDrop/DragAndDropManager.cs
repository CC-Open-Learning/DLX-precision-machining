using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDropManager : MonoBehaviour
{
	// platforms
	public enum Platforms { PC, Mobile };
	public Platforms TargetPlatform;

	// dragging modes
	public enum DragModes { ChangeToMousePos, DoNotChange };

	public static DragAndDropManager DDM;

	// panels
	public PanelSettings[] AllPanels;

	// objects
	public List<ObjectSettings> AllObjects;

	// canvases
	public Canvas FirstCanvas;
	public Canvas SecondCanvas;

	[Header("Save scene states")]
	public bool SaveStates = false;

	[Header("Object dragging modes")]
	public DragModes DraggingModes;

	[HideInInspector]
	// used for Panel Object detection
	public List<string> IdManager;

	[Header("Events Management")]
	public UnityEvent BeforeSetup;
	public UnityEvent AfterSetup;
	//

	void Start()
	{
		if (BeforeSetup != null)
		{
			BeforeSetup.Invoke();
		}

		// Getting current DDM GameObject
		DDM = this;

		// Order Objects by sibling index
		AllObjects.Sort((objA, objB) =>
		objA.GetComponent<RectTransform>().GetSiblingIndex().CompareTo(objB.GetComponent<RectTransform>().GetSiblingIndex()));

		// Initialize Drag & Drop system
		Init();

		if (AfterSetup != null)
		{
			AfterSetup.Invoke();
		}
	}

	public void Init()
	{
		// setup IdManager List
		for (int i = 0; i < AllPanels.Length; i++)
		{
			IdManager.Add("");
		}

		for (int i = 0; i < AllObjects.Count; i++)
		{
			// Setting the first position of objects
			AllObjects[i].FirstPos = AllObjects[i].GetComponent<RectTransform>().position;
			// Setting the first scale of objects
			AllObjects[i].FirstScale = AllObjects[i].GetComponent<RectTransform>().localScale;

			SetupDefaultPanel(i);
			if (SaveStates && PlayerPrefs.HasKey(AllObjects[i].Id + "X"))
			{
				LoadSavedPositions(i);
			}
		}
		if (SaveStates)
		{
			// Load Positions of Multi Objects
			for (int j = 0; j < AllPanels.Length; j++)
			{
				if (AllPanels[j].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
				{
					AllPanels[j].LoadObjectsList();
				}
			}
		}
	}

	void SetupDefaultPanel(int i)
	{
		for (int j = 0; j < AllPanels.Length; j++)
		{
			// Check if there is any object on any panel (Default Panel)
			if (RectTransformUtility.RectangleContainsScreenPoint(AllPanels[j].GetComponent<RectTransform>(), AllObjects[i].GetComponent<RectTransform>().position))
			{
				AllObjects[i].DefaultPanel = true;
				if (SaveStates && PlayerPrefs.HasKey(AllObjects[i].Id + "X"))
				{
					// Object detection must be done through LoadSavedPositions method
					return;
				}

				if (AllObjects[i].ScaleOnDropped)
				{
					AllObjects[i].GetComponent<RectTransform>().localScale = AllObjects[i].DropScale;
				}
				// Setting the Id of object for Panel Object detection
				if (AllPanels[j].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
				{
					AllPanels[j].SetMultiObject(AllObjects[i].Id);
				}
				AllObjects[i].Dropped = true;
				SetPanelObject(j, AllObjects[i].Id);
			}
		}
	}

	void LoadSavedPositions(int i)
	{
		if (AllObjects[i].Id != "")
		{
			// Setting the position of object to last saved position
			AllObjects[i].GetComponent<RectTransform>().position = new Vector3(PlayerPrefs.GetFloat(AllObjects[i].Id + "X"), PlayerPrefs.GetFloat(AllObjects[i].Id + "Y"), AllObjects[i].GetComponent<RectTransform>().position.z);
			for (int j = 0; j < AllPanels.Length; j++)
			{
				// Check if the object is on any panel
				if (RectTransformUtility.RectangleContainsScreenPoint(AllPanels[j].GetComponent<RectTransform>(), AllObjects[i].GetComponent<RectTransform>().position))
				{
					if (AllObjects[i].ScaleOnDropped)
					{
						AllObjects[i].GetComponent<RectTransform>().localScale = AllObjects[i].DropScale;
					}
					// Setting the Id of object for Panel Object detection
					SetPanelObject(j, AllObjects[i].Id);
					AllObjects[i].Dropped = true;
				}
			}
		}
		else
		{
			Debug.LogError("Set the Id of <" + AllObjects[i].gameObject.name + "> Object to use save system with it!");
		}
	}

	public void SetPanelObject(int PanelIndex, string ObjectId)
	{
		IdManager[PanelIndex] = ObjectId;
	}

	public static void ResetScene()
	{
		// Reset Objects
		for (int i = 0; i < DDM.AllObjects.Count; i++)
		{
			DDM.AllObjects[i].Dropped = false;
			DDM.AllObjects[i].GetComponent<RectTransform>().SetAsLastSibling();
			DDM.AllObjects[i].GetComponent<RectTransform>().position = DDM.AllObjects[i].FirstPos;
			if (DDM.SaveStates)
			{
				PlayerPrefs.SetFloat(DDM.AllObjects[i].Id + "X", DDM.AllObjects[i].FirstPos.x);
				PlayerPrefs.SetFloat(DDM.AllObjects[i].Id + "Y", DDM.AllObjects[i].FirstPos.y);
			}
			if (DDM.AllObjects[i].ScaleOnDropped)
				DDM.AllObjects[i].GetComponent<RectTransform>().localScale = DDM.AllObjects[i].FirstScale;
		}
		// Reset Panels
		for (int i = 0; i < DDM.AllPanels.Length; i++)
		{
			if (DDM.AllPanels[i].ObjectReplacement == PanelSettings.ObjectReplace.MultiObjectMode)
			{
				DDM.AllPanels[i].PanelIdManager.Clear();
			}
		}

		DDM.IdManager.Clear();
		DDM.Init();
	}

	public static string GetPanelObject(string PanelId)
	{
		string IdStatus = "";

		for (int i = 0; i < DDM.AllPanels.Length; i++)
		{
			if (PanelId == DDM.AllPanels[i].Id)
			{
				IdStatus = DDM.IdManager[i];
			}
		}
		return IdStatus;
	}

	public static string GetObjectPanel(string ObjectId)
	{
		string IdStatus = "";

		for (int i = 0; i < DDM.AllPanels.Length; i++)
		{
			if (DDM.AllPanels[i].ObjectReplacement != PanelSettings.ObjectReplace.MultiObjectMode)
			{
				if (ObjectId == DDM.IdManager[i])
				{
					IdStatus = DDM.AllPanels[i].Id;
				}
			}
			else
			{
				for (int j = 0; j < DDM.AllPanels[i].PanelIdManager.Count; j++)
				{
					if (ObjectId == DDM.AllPanels[i].PanelIdManager[j])
					{
						IdStatus = DDM.AllPanels[i].Id;
					}
				}
			}
		}
		return IdStatus;
	}

	public static string[] GetPanelObjects(string PanelId)
	{
		List<string> IdStatus = new List<string>(1);

		for (int i = 0; i < DDM.AllPanels.Length; i++)
		{
			if (PanelId == DDM.AllPanels[i].Id)
			{
				IdStatus = new List<string>(DDM.AllPanels[i].PanelIdManager.Count);

				IdStatus = DDM.AllPanels[i].PanelIdManager;
			}
		}
		return IdStatus.ToArray();
	}

	public void SmoothMoveStarter(string state, RectTransform Target, Vector3 TargetPos, float Speed)
	{
		StartCoroutine(SmoothMove(state, Target, TargetPos, Speed));
	}

	bool Approximately(float valueA, float valueB)
	{
		return Mathf.Abs(valueA - valueB) < 0.04f;
	}

	// Smooth Movement tool
	IEnumerator SmoothMove(string state, RectTransform Target, Vector3 TargetPos, float Speed)
	{
		Target.GetComponent<ObjectSettings>().OnReturning = true;
		float t = 0.0f;
		TargetPos.z = Target.position.z;

		// Save last position of target object
		if (SaveStates)
		{
			PlayerPrefs.SetFloat(Target.GetComponent<ObjectSettings>().Id + "X", TargetPos.x);
			PlayerPrefs.SetFloat(Target.GetComponent<ObjectSettings>().Id + "Y", TargetPos.y);
		}

		while (!Approximately(Target.position.x, TargetPos.x) || !Approximately(Target.position.y, TargetPos.y))
		{
			t += Time.deltaTime * Speed;
			Target.position = Vector3.Lerp(Target.position, TargetPos, Mathf.SmoothStep(0.0f, 1.0f, t));
			yield return null;
		}
		Target.position = TargetPos;
		Target.GetComponent<ObjectSettings>().OnReturning = false;

		if (state == "AI")
		{
			Target.GetComponent<ObjectSettings>().PointerUp(state);
		}
	}
}
