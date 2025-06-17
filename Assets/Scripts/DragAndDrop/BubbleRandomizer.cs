using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// A class that controls the behavior for the randomized bubble pool.
/// </summary>
public class BubbleRandomizer : MonoBehaviour
{
    /// <summary>
    /// The total amount of bubble answers in the minigame.
    /// </summary>
    public int TotalBubbles { get; private set; }

    /// <summary>
    /// The current amount of correct answers the user has submitted.
    /// </summary>
    public int CurrentCorrect { get; private set; }

    [SerializeField, Tooltip("Game Object the holds all the child bubbles")] private GameObject bubblePool;

    [SerializeField, Tooltip("Game Object that will parent Bubbles on the Left")] private GameObject leftSpot;

    [SerializeField, Tooltip("Game Object that will parent Bubbles on the Right")] private GameObject rightSpot;

    [SerializeField, Tooltip("Thought bubble Left")] private Sprite leftThought;

    [SerializeField, Tooltip("Thought bubble Right")] private Sprite rightThought;

    [SerializeField, Tooltip("Speech bubble Left")] private Sprite leftSpeech;

    [SerializeField, Tooltip("Speech bubble Right")] private Sprite rightSpeech;

    private List<Bubble> bubbles;
    private const string DO_PANEL = "do";
    private const string DONT_PANEL = "dont";
 
    /// <summary>
    /// All the bubbles are added to the bubbles list and randomized.
    /// The fisrt 2 bubbles in the list are displayed.
    /// </summary>
    void Start()
    {
        bubbles = new List<Bubble>();
        CurrentCorrect = 0; 

        for (int i = 0; i < bubblePool.transform.childCount; i++)
        {
            bubbles.Add(bubblePool.transform.GetChild(i).gameObject.GetComponent<Bubble>());
        }

        TotalBubbles = bubbles.Count;

        Randomize();

        NextBubble(leftSpot.transform);
        NextBubble(rightSpot.transform);
    }

    /// <summary>
    /// Randomizes the bubble pool.
    /// </summary>
    void Randomize()
    {
        Random random = new Random();
        int k = 0;

        for (int i = bubbles.Count - 1; i > 0; i--)
        {
            k = random.Next(i + 1);
            Bubble bubble = bubbles[k];
            bubbles[k] = bubbles[i];
            bubbles[i] = bubble;
        }
    }

    /// <summary>
    /// Loads the next bubble after a successful drop.
    /// </summary>
    /// <param name="parent">The empty parent that the new bubble should be assigned to.</param>
    void NextBubble(Transform parent)
    {
        Bubble bubble = bubbles.First();
        bubble.gameObject.transform.SetParent(parent, false);

        RectTransform bubbleTransform = bubble.GetComponent<RectTransform>();
        bubbleTransform.offsetMax = Vector2.zero;
        bubbleTransform.offsetMin = Vector2.zero;

        SwapBubbleImage(bubble);

        bubble.gameObject.SetActive(true);

        bubbles.RemoveAt(0);
    }

    /// <summary>
    /// Loads the next bubble after a successful drop.
    /// </summary>
    /// <param name="sender">The sender of the event, usually the previously dropped bubble.</param>
    public void NextBubble(GameObject sender)
    {
        StartCoroutine(NextBubbleCoroutine(sender));
    }

    /// <summary>
    /// A coroutine that loads the next bubble after a successful drop.
    /// </summary>
    /// <param name="sender">The sender of the event, usually the previously dropped bubble.</param>
    /// <returns></returns>
    IEnumerator NextBubbleCoroutine(GameObject sender)
    {
        string panel = DragAndDropManager.GetObjectPanel(sender.GetComponent<ObjectSettings>().Id);
        Bubble senderBubble = sender.GetComponent<Bubble>();

        yield return new WaitForEndOfFrame();

        Transform parent = rightSpot.transform;

        if (sender.transform.parent == leftSpot.transform)
            parent = leftSpot.transform;

        sender.SetActive(false);

        // Checks whether the answer was dropped into the correct panel.
        if ((senderBubble.AnswerType == Bubble.BubbleAnswer.Do && panel == DONT_PANEL) 
            || (senderBubble.AnswerType == Bubble.BubbleAnswer.Dont && panel == DO_PANEL))
        {
            PrecisionMachiningGUI.ShowFeedback(senderBubble, false);
            Bubble bub = sender.GetComponent<Bubble>();
            PushToBottom(bub);
        }
        else
        {
            PrecisionMachiningGUI.ShowFeedback(senderBubble, true);
            CurrentCorrect++;
        }

        if (bubbles.Count > 0)
        {
            Bubble newBubble = bubbles.First();
            newBubble.gameObject.transform.SetParent(parent, false);
            newBubble.gameObject.GetComponent<ObjectSettings>().ResetParent(newBubble.gameObject.transform.parent);

            // Resets the transform so that it's displayed properly.
            RectTransform bubbleTransform = newBubble.gameObject.GetComponent<RectTransform>();
            bubbleTransform.offsetMax = Vector2.zero;
            bubbleTransform.offsetMin = Vector2.zero;

            SwapBubbleImage(newBubble);

            newBubble.gameObject.SetActive(true);

            bubbles.RemoveAt(0);
        } 
    }

    /// <summary>
    /// Pushes a bubble to the bottom of the bubble pool.
    /// </summary>
    /// <param name="bubble"></param>
    void PushToBottom(Bubble bubble)
    {
        bubbles.Add(bubble);

        ObjectSettings obj = bubble.gameObject.GetComponent<ObjectSettings>();

        bubble.gameObject.transform.SetParent(bubblePool.transform, false);
        obj.Dropped = false;
    }

    /// <summary>
    /// Because the position the bubbles will appear is randome we replace the 
    /// bubble image according to the position it will appear.
    /// </summary>
    /// <param name="bubble">The bubble that will be displayed</param>
    public void SwapBubbleImage(Bubble bubble)
    {
        Transform parent = bubble.gameObject.transform.parent;
        Image bubbleImage = bubble.gameObject.GetComponent<Image>();

        if (parent == leftSpot.transform)
        {

            bubbleImage.sprite = bubble.bubbleType == Bubble.BubbleType.Thought ? leftThought : leftSpeech;
        }
        else if (parent == rightSpot.transform)
        {
            bubbleImage.sprite = bubble.bubbleType == Bubble.BubbleType.Thought ? rightThought : rightSpeech;
        }
    }
}
