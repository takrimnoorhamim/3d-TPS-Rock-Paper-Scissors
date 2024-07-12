using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharModel[] charModels;
    public Transform spot;
    private List<GameObject> chars;
    private int currentChar;

    [SerializeField]
    private string nextSceneName = "GameScene";

    private void Start()
    {
        chars = new List<GameObject>();
        foreach (var charModel in charModels)
        {
            GameObject go = Instantiate(charModel.character,
                        spot.position, Quaternion.identity);
            go.SetActive(false);
            go.transform.SetParent(spot);
            chars.Add(go);
        }
        ShowCharFromList();
    }

    void ShowCharFromList()
    {
        chars[currentChar].SetActive(true);
    }

    public void OnclickNext()
    {
        chars[currentChar].SetActive(false);
        if (currentChar < chars.Count - 1)
        {
            currentChar = currentChar + 1;
        }
        else
        {
            currentChar = 0;
        }
        ShowCharFromList();
    }

    public void OnClickPrev()
    {
        chars[currentChar].SetActive(false);
        if (currentChar == 0)
        {
            currentChar = chars.Count - 1;
        }
        else
        {
            currentChar = currentChar - 1;
        }
        ShowCharFromList();
    }

    public void OnClickPlay()
    {
        // Store the currently active character index
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentChar);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}