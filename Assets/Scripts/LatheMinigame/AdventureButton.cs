using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureButton : MonoBehaviour
{
    public Image OptionImage;

    private AdventureScreenScriptableObject direction;
    // Start is called before the first frame update

    public void SetDirection(AdventureScreenScriptableObject direction)
    {
        this.direction = direction;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<GameController>().DisplayNewScreen(direction));
    }
}
