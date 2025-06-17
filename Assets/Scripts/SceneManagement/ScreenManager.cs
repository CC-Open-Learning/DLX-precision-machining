using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] private GameObject FadePanel;
    [SerializeField, Tooltip("Time it takes to fade in and out")] private float fadeTime = 0.25f;

    public delegate void OnLeavingScene();
    public static OnLeavingScene onLeavingScene;

    private Image fadePanelImage;
    private Vector3 lastMousePosition = Vector3.zero;

    /// <summary>
    /// The scene we're currently loading
    /// </summary>
    private AsyncOperation SceneToLoad;

    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        fadePanelImage = FadePanel.GetComponent<Image>();
    }

    private void OnEnable()
    {
        LoadLevelButton.onLoadSceneButtonClicked += LoadScene;
    }

    /// <summary>
    /// return the current loading progress of the scene
    /// </summary>
    public float LoadingProgress()
    {
        return SceneToLoad == null ? 0f : SceneToLoad.isDone ? 1f : SceneToLoad.progress;
    }

    private void OnDisable()
    {
        LoadLevelButton.onLoadSceneButtonClicked -= LoadScene;
    }

    private void Update()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            DescriptiveAudio.HasMouseMoved = true;
        }
    }

    public void LoadScene(string name, bool exitConfirmation)
    {
        if (name == "") return;

        if (exitConfirmation)
        {
            PrecisionMachiningGUI.Instance.ShowDecision();
        }
        else
            StartCoroutine(_LoadScene(name));
    }



    private IEnumerator _LoadScene(string name)
    {
        onLeavingScene?.Invoke();

        //begin loading the scene
        SceneToLoad = SceneManager.LoadSceneAsync(name);

        //stop the scene from loading before the fading animations
        SceneToLoad.allowSceneActivation = false;

        yield return StartCoroutine("FadeOut");

        SceneToLoad.allowSceneActivation = true;

        yield return new WaitUntil(() => SceneToLoad.isDone);

        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine("FadeIn");
    }

    /// <summary>
    /// fade into the screen from a black screen
    /// </summary>
    public IEnumerator FadeIn()
    {
        float currentFadeTime = 0f;
        while (currentFadeTime <= fadeTime)
        {
            currentFadeTime += Time.deltaTime;
            fadePanelImage.color = new Color(fadePanelImage.color.r, fadePanelImage.color.g, fadePanelImage.color.b, 1 - currentFadeTime / fadeTime);
            yield return null;
        }
        FadePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// fade the screen to black
    /// </summary>
    public IEnumerator FadeOut()
    {
        FadePanel.gameObject.SetActive(true);
        float currentFadeTime = 0f;
        while(currentFadeTime <= fadeTime)
        {
            currentFadeTime += Time.deltaTime;
            fadePanelImage.color = new Color(fadePanelImage.color.r, fadePanelImage.color.g, fadePanelImage.color.b, currentFadeTime / fadeTime);
            yield return null;
        }
    }
}
