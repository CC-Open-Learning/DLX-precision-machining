using NodeCanvas.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrecisionMachiningGUI : MonoBehaviour
{
    public static PrecisionMachiningGUI Instance { get; private set; }
    public static Color CorrectColor { get; private set; }
    public static Sprite CorrectSprite { get; private set; }
    public static Color IncorrectColor { get; private set; }
    public static Sprite IncorrectSprite { get; private set; }
    public static Color DefaultColor { get; private set; }
    public static Sprite DefaultSprite { get; private set; }
    public static GameObject Feedback { get; private set; }

    [Header("Window Settings")]
    [SerializeField] private float fadeDuration = 0.4f;

    [SerializeField, Tooltip("The color used for correct answer windows.")]
    private Color correctColor;
    [SerializeField, Tooltip("The sprite used for correct answer windows.")]
    private Sprite correctSprite;

    [SerializeField, Tooltip("The color used for incorrect answer windows.")]
    private Color incorrectColor;
    [SerializeField, Tooltip("The sprite used for incorrect answer windows.")]
    private Sprite incorrectSprite;

    [SerializeField, Tooltip("The default color used for windows.")]
    private Color defaultColor;
    [SerializeField, Tooltip("The default sprite used for windows.")]
    private Sprite defaultSprite;

    [Header("Feedback")]
    [SerializeField, Tooltip("Feedback gameobject")] 
    private GameObject feedback;
    private ModalWindow modalWindow;

    [SerializeField, Tooltip("Decision Window game object")]
    private GameObject decisionWindow;
    private DecisionWindow decisionScript;

    [Header("Popup Window")]
    [SerializeField, Tooltip("Popup Window game object")] private PopUpWindow popupWindow;
    [SerializeField, Tooltip("Popup Close Button")] private Button popupCloseButton;


    [Header("Audio")]

    [SerializeField, Tooltip("Audio effect played when an answer is correct")] private AudioClip correctAudioClip;
    [SerializeField, Tooltip("Audio effect played when an answer is incorrect")] private AudioClip incorrectAudioClip;
    [SerializeField, Tooltip("Default audio clips for the decision window.")] private List<AudioClip> defaultDescisionAudio;

    public bool FeedbackDismissed = false; //Variable that tracks if the feedback modal has been dismissed
    public bool ModalActive = false; //Variable that tracks if the feedback modal is open 
    public bool PopupActive = false; // Tracks if the pop up window is active

    /// <summary>
    /// Main audio source
    /// </summary>
    public AudioSource audioSource;

    public enum WindowState
    {
        Default,
        Correct,
        Incorrect
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CorrectColor = correctColor;
            CorrectSprite = correctSprite;
            IncorrectColor = incorrectColor;
            IncorrectSprite = incorrectSprite;
            DefaultColor = defaultColor;
            DefaultSprite = DefaultSprite;
            Feedback = feedback;
        }
        else
        {
            Destroy(gameObject);
        }

        feedback.SetActive(false);
        modalWindow = feedback.GetComponent<ModalWindow>();

        decisionScript = decisionWindow.GetComponent<DecisionWindow>();
        decisionWindow.SetActive(false);

        //Popup window setup
        popupWindow.gameObject.SetActive(false);
        popupCloseButton.onClick.AddListener(ClosePopup);
    }

    #region Feedback

    /// <summary>
    /// Shows feedback to the user based on the selected answer
    /// </summary>
    /// <param name="answer">Answer that contains feedback text and audio</param>public static void ShowFeedback(Bubble bubble, bool correct)
    public void ShowFeedBack(Answer answer)
    {
        VoiceOverManager.Instance.PlayAudioClip(answer.AudioClip);
        modalWindow.ChangeWindow(answer);
        FeedbackDismissed = false;
        ModalActive = true;
        FadeIn(feedback);
    }


    /// <summary>
    /// Shows feedback to the user based on the selected procedure step
    /// </summary>
    /// <param name="answer">Answer that contains feedback text and audio</param>public static void ShowFeedback(Bubble bubble, bool correct)
    public void ShowFeedBack(ProcedureStep step)
    {
        VoiceOverManager.Instance.PlayAudioClip(step.AudioClip);
        modalWindow.ChangeWindow(step);
        FeedbackDismissed = false;
        ModalActive = true;
        FadeIn(feedback);
    }

    /// <summary>
    /// Shows feedback to the user based on the selected answer, used for
    /// bubble drag and drop minigame
    /// </summary>
    /// <param name="bubble">Bubble answer type containing feedback text and audio</param>
    /// <param name="correct">Whether the users answer was correct or not</param>
    public static void ShowFeedback(Bubble bubble, bool correct)
    {
        AudioClip clip = correct ? bubble.CorrectAudioClip : bubble.IncorrectAudioClip;
        Instance.modalWindow.ChangeWindow(bubble, correct);
        Instance.FeedbackDismissed = false;

        Instance.FadeIn(Feedback);
        VoiceOverManager.Instance.PlayAudioClip(clip);
        Instance.ModalActive = true;
    }

    /// <summary>
    /// Show modal can be called to update the content and display the modal. Audio clip for the modal are optional
    /// </summary>
    /// <param name="windowState">The window state will determine the appearnce of the modal window. Default, Correct, Incorrect</param>
    /// <param name="title">Title that will be displayed in the modal.</param>
    /// <param name="content">Information that is displayed inside the modal.</param>
    /// <param name="buttonText">Text displayed in the modal button.</param>
    /// <param name="audioClips">audio clip or clips that are played when the modal is displayed.</param>
    public void ShowModal(WindowState windowState, string title, string content, string buttonText, List<AudioClip> audioClips = null)
    {
        if (audioClips != null)
        {
            VoiceOverManager.Instance.PlayAudioClips(audioClips);
        }

        modalWindow.ChangeWindow(windowState, title, content, buttonText);
        FeedbackDismissed = false;
        ModalActive = true;
        FadeIn(feedback);
    }

    /// <summary>
    /// Shows the decision window.
    /// </summary>
    /// <param name="audioClips">The audio clips to be played. Can be left null.</param>
    /// <param name="content">The content to be displayed on the decision window. Can be left null.</param>
    public void ShowDecision(List<AudioClip> audioClips = null, string content = null)
    {
        if (content != null)
            decisionScript.ChangeForMinigame(content);
        else
            decisionScript.ChangeForHomeButton();

        if (audioClips != null)
            VoiceOverManager.Instance.PlayAudioClips(audioClips);
        else
            VoiceOverManager.Instance.PlayAudioClips(defaultDescisionAudio);

        FeedbackDismissed = false;
        ModalActive = true;
        FadeIn(decisionWindow);
    }

    /// <summary>
    /// The pop up windows are larger and they can display image and text.
    /// </summary>
    /// <param name="popupObject">The pop up scripatable object is used to 
    /// set the content of the pop up window</param>
    public void ShowPopup(PopUpWindowScriptableObject popupObject)
    {
        popupWindow.SetPopUpWindowContent(popupObject);
        VoiceOverManager.Instance.PlayAudioClip(popupObject.audioClip);
        FadeIn(popupWindow.gameObject);
        SubtitlesManager.Instance.ClearSubtitle();
        PopupActive = true;
    }


    /// <summary>
    /// Closes the feedback window with a fade out
    /// </summary>
    public void CloseFeedBack()
    {
        FadeOut(feedback);
        VoiceOverManager.Instance.StopAudio();

        //When the feedback modal is closed it set the feedback dismissed to true. This variable is checked by the Drag and Drop task.
        FeedbackDismissed = true;

        // Set the ModalActive to false so the navigation keys on the keyboard are active
        ModalActive = false;
    }

    /// <summary>
    /// Closes the decision window with a fade out.
    /// </summary>
    public void CloseDecision()
    {
        FadeOut(decisionWindow);
        VoiceOverManager.Instance.StopAudio();

        //When the feedback modal is closed it set the feedback dismissed to true. This variable is checked by the Drag and Drop task.
        FeedbackDismissed = true;

        // Set the ModalActive to false so the navigation keys on the keyboard are active
        ModalActive = false;
    }

    public void ClosePopup()
    {
        FadeOut(popupWindow.gameObject);
        VoiceOverManager.Instance.StopAudio();
        PopupActive = false;
    }
    #endregion

    #region Fade

    /// <summary>
    /// Fades canvas group in
    /// </summary>
    /// <param name="window"></param>
    public void FadeIn(GameObject window)
    {
        StartCoroutine(FadeCanvasGroup(window,0,1));
    }

    /// <summary>
    /// Fades canvas group out
    /// </summary>
    /// <param name="window"></param>
    public void FadeOut(GameObject window)
    {
        StartCoroutine(FadeCanvasGroup(window, 1, 0));
    }

    /// <summary>
    /// Interpolates between alphas for canvas groups
    /// </summary>
    /// <param name="window"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private IEnumerator FadeCanvasGroup(GameObject window, float start, float end)
    {
        float counter = 0;

        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();

        if (start == 0)
        {
            window.SetActive(true);
        }

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeDuration);
            yield return null;
        }

        if(end == 0)
        {
            window.SetActive(false);
        }
    }

    #endregion
}
