using UnityEngine;
using UnityEngine.UI;

public class CharacterPurchaseManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    public CharacterSelection characterSelection;
    public Button buyButton;
    public Button playButton;

    private void Start()
    {
        LoadPurchasedStatus();
        UpdateButtonStates();
    }

    public void TryPurchaseCharacter()
    {
        int currentCharIndex = characterSelection.GetCurrentCharacterIndex();
        CharacterData currentChar = characterDatabase.characters[currentCharIndex];

        if (CreditsManager.Instance.GetScore() >= currentChar.price && !currentChar.isPurchased)
        {
            CreditsManager.Instance.DeductCredits(currentChar.price);
            currentChar.isPurchased = true;
            UpdateButtonStates();

            SavePurchasedStatus(currentCharIndex);
        }
        else
        {
            Debug.Log("Not enough credits or character already purchased!");
        }
    }

    public void UpdateButtonStates()
    {
        int currentCharIndex = characterSelection.GetCurrentCharacterIndex();
        CharacterData currentChar = characterDatabase.characters[currentCharIndex];

        buyButton.gameObject.SetActive(!currentChar.isPurchased);
        playButton.gameObject.SetActive(currentChar.isPurchased);

        buyButton.interactable = CreditsManager.Instance.GetScore() >= currentChar.price;
    }

    private void SavePurchasedStatus(int characterIndex)
    {
        PlayerPrefs.SetInt("Character_" + characterIndex + "_Purchased", 1);
        PlayerPrefs.Save();
    }

    public bool IsCharacterPurchased(int characterIndex)
    {
        return PlayerPrefs.GetInt("Character_" + characterIndex + "_Purchased", 0) == 1;
    }

    public void LoadPurchasedStatus()
    {
        for (int i = 0; i < characterDatabase.characters.Count; i++)
        {
            characterDatabase.characters[i].isPurchased = IsCharacterPurchased(i);
        }
    }
}
