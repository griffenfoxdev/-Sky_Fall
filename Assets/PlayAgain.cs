using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAgain : MonoBehaviour
{
    public GameObject PlayAgainObject;
    public Button PlayAgainButton;

    void Start()
    {
        PlayAgainObject = GameObject.Find("PlayAgainButton");
        PlayAgainButton = PlayAgainObject.GetComponent<Button>();
        PlayAgainButton.onClick.AddListener(RestartGame);

    }

    void RestartGame()
    {
        Application.LoadLevel(0);
        Instantiate(GameObject.Find("Spawner"));
    }

}
