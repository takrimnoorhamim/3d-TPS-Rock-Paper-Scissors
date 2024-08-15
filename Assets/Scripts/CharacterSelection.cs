using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    public Transform spot;
    public Image characterIconImage;
    public Text characterNameText;
    public Text characterPriceText;
    public CharacterPurchaseManager purchaseManager;
    public Button buyButton;
    public Button playButton;
    private List<GameObject> chars;
    private int currentChar;
    [SerializeField]
    private string nextSceneName = "GameScene";

    public AudioSource src;
    public AudioClip buy;

    private void Start()
    {
        if (characterDatabase == null)
        {
            Debug.LogError("CharacterDatabase is not assigned!");
            return;
        }
        chars = new List<GameObject>();
        foreach (var characterData in characterDatabase.characters)
        {
            if (characterData.prefab == null)
            {
                Debug.LogWarning($"Null character prefab found for {characterData.name}!");
                continue;
            }
            GameObject go = Instantiate(characterData.prefab, spot.position, Quaternion.identity);
            go.SetActive(false);
            go.transform.SetParent(spot);
            chars.Add(go);

            // Auto-purchase characters with price 0
            if (characterData.price == 0)
            {
                AutoPurchaseCharacter(characterData);
            }
        }
        if (chars.Count == 0)
        {
            Debug.LogError("No characters were instantiated!");
            return;
        }
        purchaseManager.LoadPurchasedStatus();
        ShowCharFromList();
    }

    private void AutoPurchaseCharacter(CharacterData characterData)
    {
        if (!characterData.isPurchased)
        {
            characterData.isPurchased = true;
            int index = characterDatabase.characters.IndexOf(characterData);
            PlayerPrefs.SetInt("Character_" + index + "_Purchased", 1);
            PlayerPrefs.Save();
            Debug.Log($"Character {characterData.name} auto-purchased (free).");
        }
    }

    void ShowCharFromList()
    {
        if (chars == null || chars.Count == 0 || currentChar < 0 || currentChar >= chars.Count)
        {
            Debug.LogError("Invalid character list or index!");
            return;
        }
        chars[currentChar].SetActive(true);
        CharacterData currentCharacterData = characterDatabase.characters[currentChar];
        if (characterIconImage == null)
        {
            Debug.LogError("Character Icon Image is not assigned!");
        }
        else
        {
            characterIconImage.sprite = currentCharacterData.icon;
        }
        if (characterNameText == null)
        {
            Debug.LogError("Character Name Text is not assigned!");
        }
        else
        {
            characterNameText.text = currentCharacterData.name;
        }
        if (characterPriceText == null)
        {
            Debug.LogError("Character Price Text is not assigned!");
        }
        else
        {
            characterPriceText.text = currentCharacterData.price.ToString();
        }
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        CharacterData currentCharacterData = characterDatabase.characters[currentChar];
        buyButton.gameObject.SetActive(!currentCharacterData.isPurchased);
        playButton.gameObject.SetActive(currentCharacterData.isPurchased);
        buyButton.interactable = CreditsManager.Instance.GetScore() >= currentCharacterData.price;
    }

    public void OnclickNext()
    {
        if (chars == null || chars.Count == 0)
        {
            Debug.LogError("No characters available!");
            return;
        }
        chars[currentChar].SetActive(false);
        currentChar = (currentChar + 1) % chars.Count;
        ShowCharFromList();
    }

    public void OnClickPrev()
    {
        if (chars == null || chars.Count == 0)
        {
            Debug.LogError("No characters available!");
            return;
        }
        chars[currentChar].SetActive(false);
        currentChar = (currentChar - 1 + chars.Count) % chars.Count;
        ShowCharFromList();
    }

    public void OnClickPlay()
    {
        if (characterDatabase.characters[currentChar].isPurchased)
        {
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentChar);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("Character not purchased!");
        }
    }

    public void OnClickBuy()
    {
        CharacterData currentCharacterData = characterDatabase.characters[currentChar];

        if (CreditsManager.Instance.GetScore() >= currentCharacterData.price && !currentCharacterData.isPurchased)
        {
            CreditsManager.Instance.DeductCredits(currentCharacterData.price);
            currentCharacterData.isPurchased = true;

            // Save the purchased status
            PlayerPrefs.SetInt("Character_" + currentChar + "_Purchased", 1);
            PlayerPrefs.Save();

            Debug.Log($"Character {currentCharacterData.name} purchased successfully!");

            src.PlayOneShot(buy);
        }
        else if (currentCharacterData.isPurchased)
        {
            Debug.Log("Character already purchased!");
        }
        else
        {
            Debug.Log("Not enough credits!");
        }
        UpdateButtonStates();
    }

    public int GetCurrentCharacterIndex()
    {
        return currentChar;
    }
}
