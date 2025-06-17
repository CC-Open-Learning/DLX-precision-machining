using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropbox : MonoBehaviour
{
    /// <summary>
    /// List of "holder" objects in the dropbox.
    /// </summary>
    private List<GameObject> children;
    public GameObject CurrentObject;

    private void Start()
    {
        children = new List<GameObject>();
        GatherChildren();
    }

    /// <summary>
    /// Adds a draggable answer to the panel while maintaining the horizontal layout.
    /// </summary>
    /// <param name="dropped"></param>
    public void AddAnswerToDropbox(GameObject dropped)
    {
        Answer answer = dropped.GetComponent<Answer>();

        if (answer.CorrectAnswer)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].transform.childCount == 0)
                {
                    //Add a copy of the dropped object to the panel
                    GameObject copy = Instantiate(dropped, children[i].transform);
                    copy.GetComponent<Answer>().FeedbackImage.SetActive(true);

                    //Adjust the transform so it looks right 
                    RectTransform copyTransform = copy.GetComponent<RectTransform>();
                    copyTransform.offsetMin = Vector2.zero;
                    copyTransform.offsetMax = Vector2.zero;

                    //Hide the original object and then rebuild the list
                    dropped.SetActive(false);
                    GatherChildren();
                    break;
                }
                else
                    continue;
            }
        }
        else
            return;
    }

    /// <summary>
    /// Gathers all the children and adds them to the list.
    /// Clears the list so that it can be rebuilt each time an answer is added.
    /// </summary>
    private void GatherChildren()
    {
        children.Clear();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
    }

    public void SetCurrentObject(GameObject dropped)
    {
        CurrentObject = dropped;
    }

    public void ReenableImage()
    {
        if (CurrentObject != null && !CurrentObject.GetComponent<Image>().enabled)
                CurrentObject.GetComponent<Image>().enabled = true;
    }
}
