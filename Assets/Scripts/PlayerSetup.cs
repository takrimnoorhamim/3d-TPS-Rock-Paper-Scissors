using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public GameObject[] characterPrefabs;

    void Start()
    {
        SetupSelectedCharacter();
    }

    void SetupSelectedCharacter()
    {
        int selectedCharIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        if (selectedCharIndex >= 0 && selectedCharIndex < characterPrefabs.Length)
        {
            GameObject selectedChar = Instantiate(characterPrefabs[selectedCharIndex], transform);
            selectedChar.transform.localPosition = Vector3.zero;
            selectedChar.transform.localRotation = Quaternion.identity;

            // Ensure the character has the correct tag
            selectedChar.tag = "Player";

            // You might want to add components or do other setup here
            // For example:
            // selectedChar.AddComponent<CharacterController>();
        }
        else
        {
            Debug.LogError("Invalid character index or character prefabs not set up correctly.");
        }
    }
}