// PlayerSetup.cs
using UnityEngine;
public class PlayerSetup : MonoBehaviour
{
    public CharacterDatabase characterDatabase; // Reference to the CharacterDatabase asset
    void Start()
    {
        SetupSelectedCharacter();
    }

    void SetupSelectedCharacter()
    {
        int selectedCharIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        if (characterDatabase != null && selectedCharIndex >= 0 && selectedCharIndex < characterDatabase.characters.Count)
        {
            CharacterData selectedCharData = characterDatabase.characters[selectedCharIndex];
            if (selectedCharData.prefab != null)
            {
                GameObject selectedChar = Instantiate(selectedCharData.prefab, transform);
                selectedChar.transform.localPosition = Vector3.zero;
                selectedChar.transform.localRotation = Quaternion.identity;
                // Ensure the character has the correct tag
                selectedChar.tag = "Player";
                Debug.Log($"Spawned character: {selectedCharData.name}");
            }
            else
            {
                Debug.LogError($"Prefab for character '{selectedCharData.name}' is not set.");
            }
        }
        else
        {
            Debug.LogError("Invalid character index or CharacterDatabase not set up correctly.");
        }
    }
}