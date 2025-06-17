using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelSettings : MonoBehaviour
{
	// the Id of this panel
	public string Id;

	// Enums
	public enum ObjectPosStates { UseObjectSettings, DroppedPosition, PanelPosition };
	public enum ObjectLockStates { UseObjectSettings, LockObject, DoNotLockObject };
	public enum ObjectReplace { Allowed, NotAllowed, MultiObjectMode };
	//

	// Default Panel
	public Sprite DefaultSprite;

	// Customization Tools
	[Header("Object Position")]
	[Tooltip("Customize the position of the object when it gets dropped on this panel")]
	public ObjectPosStates ObjectPosition;

	[Header("Lock Object")]
	[Tooltip("Customize Object Locking")]
	public ObjectLockStates LockObject;

	[Header("Replacement & Multi Object")]
	[Tooltip("Allow Object to Replace & Switch or use Multi Object Mode")]
	public ObjectReplace ObjectReplacement;
	//

	[Header("Events Management")]
	[Tooltip("When any object gets dropped on the panel, the functions that you added to this event trigger will be called")]
	public UnityEvent OnObjectDropped;

	[Tooltip("When the object of this panel, gets dropped on another panel")]
	public UnityEvent OnObjectExit;

	[HideInInspector]
	// used for Multi Object tool
	public List<string> PanelIdManager;

	public void SetupPanelEvents()
	{
		// Events Management
		if (OnObjectDropped != null)
		{
			OnObjectDropped.Invoke();
		}
	}

	public void SetMultiObject(string ObjectId)
	{
		// Adding new object to the list of dropped objects
		PanelIdManager.Add(ObjectId);
		if (DragAndDropManager.DDM.SaveStates)
		{
			SaveObjectsList();
		}
	}

	public void RemoveMultiObject(string ObjectId)
	{
		// Removing an object from list of dropped objects
		if (DragAndDropManager.DDM.SaveStates)
		{
			PlayerPrefs.DeleteKey(Id + "&&" + (PanelIdManager.Count - 1).ToString());
		}
		PanelIdManager.Remove(ObjectId);
	}

	void SaveObjectsList()
	{
		for (int i = 0; i < PanelIdManager.Count; i++)
		{
			PlayerPrefs.SetString(Id + "&&" + i.ToString(), PanelIdManager[i]);
		}
	}

	public void LoadObjectsList()
	{
		PanelIdManager.Clear();
		// Loading the list of dropped objects
		for (int i = 0; i < DragAndDropManager.DDM.AllObjects.Count; i++)
		{
			if (PlayerPrefs.HasKey(Id + "&&" + i.ToString()))
			{
				PanelIdManager.Add(PlayerPrefs.GetString(Id + "&&" + i.ToString()));
			}
		}
		for (int i = 0; i < PanelIdManager.Count; i++)
		{
			for (int j = 0; j < DragAndDropManager.DDM.AllObjects.Count; j++)
			{
				if (DragAndDropManager.DDM.AllObjects[j].Id == PanelIdManager[i])
				{
					DragAndDropManager.DDM.AllObjects[j].GetComponent<RectTransform>().SetAsLastSibling();

					for (int k = 0; k < DragAndDropManager.DDM.AllPanels.Length; k++)
					{
						if (DragAndDropManager.DDM.AllPanels[k].Id == Id)
						{
							DragAndDropManager.DDM.SetPanelObject(k, DragAndDropManager.DDM.AllObjects[j].Id);
						}
					}
				}
			}
		}
	}

	public void ToggleObjectReplacement(int choice)
    {
        switch (choice)
        {
			case 0: ObjectReplacement = ObjectReplace.NotAllowed;
				break;
			case 1: ObjectReplacement = ObjectReplace.Allowed;
				break;
			case 2: ObjectReplacement = ObjectReplace.MultiObjectMode;
				break;
			default:
                break;
        }
    }
}
