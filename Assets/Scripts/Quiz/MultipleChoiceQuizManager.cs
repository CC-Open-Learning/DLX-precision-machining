using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MultipleChoiceQuizManager : MonoBehaviour
{
    private enum QuizType
    {
        MultipleChoice,
        DragAndDrop,
        FillInTheBlank,
        Procedure
    }

    [SerializeField, Tooltip("Select the type of Quiz Question")] private QuizType quizType;

    [SerializeField, Tooltip("List of answers")] private List<Answer> answers;

    [SerializeField, Tooltip("Reference to the modal window")] private ModalWindow modalWindow;

    public UnityEvent onQuizCompleted;

    public delegate void OnQuizComplete();
    public static OnQuizComplete onQuizComplete;

    private int totalCorrectAnswers = 0;
    private int currentCorrectAnswers = 0;

    private void Awake()
    {
        foreach (Answer a in answers)
        {
            Button button = a.GetComponent<Button>();
            ObjectSettings dragdrop = a.GetComponent<ObjectSettings>();

            if (a.CorrectAnswer)
                totalCorrectAnswers++;

            switch (quizType)
            {
                case QuizType.MultipleChoice:
                    if (button != null)
                        button.onClick.AddListener(() => OnAnswerSelect(a));
                    else
                        dragdrop.OnDroppedSuccessfully.AddListener(() => OnAnswerSelect(a));
                    break;
                case QuizType.DragAndDrop:
                    dragdrop.OnDroppedSuccessfully.AddListener(() => OnAnswerSelect(a));
                    break;
                case QuizType.FillInTheBlank:
                    dragdrop.OnDroppedSuccessfully.AddListener(() => OnAnswerSelect(a));
                    break;
                case QuizType.Procedure:
                    if (button != null)
                        button.onClick.AddListener(() => OnAnswerSelect(a));
                    break;
                default:
                    break;
            }
        }
    }

    private void OnAnswerSelect(Answer a)
    {

        if(SubtitlesManager.Instance != null) SubtitlesManager.Instance.ClearSubtitle();
        modalWindow.GetComponent<CanvasGroup>().alpha = 1;
        PrecisionMachiningGUI.Instance.ShowFeedBack(a);

        if (a.FeedbackImage != null)
        {
            a.FeedbackImage.SetActive(true);
        }
        
        if (a.CorrectAnswer && quizType == QuizType.MultipleChoice)
        {
            foreach (Answer ans in answers)
            {
                Button button = ans.GetComponent<Button>();

                if (button != null)
                    button.onClick.RemoveAllListeners();

                if(ans.FeedbackImage != null)
                    ans.FeedbackImage.SetActive(true);
            }

            onQuizCompleted.Invoke();
            onQuizComplete?.Invoke();
        }
        else if (a.CorrectAnswer && (quizType == QuizType.DragAndDrop || quizType == QuizType.FillInTheBlank))
        {
            currentCorrectAnswers++;

            if (currentCorrectAnswers == totalCorrectAnswers)
            {
                foreach (Answer ans in answers)
                {
                    ObjectSettings dragdrop = ans.GetComponent<ObjectSettings>();
                    dragdrop.UserControl = false;

                    if (ans.FeedbackImage != null && !ans.FeedbackImage.activeSelf)
                        ans.FeedbackImage.SetActive(true);

                   
                }
                onQuizCompleted.Invoke();
                onQuizComplete?.Invoke();
            }
        }
    }
}
