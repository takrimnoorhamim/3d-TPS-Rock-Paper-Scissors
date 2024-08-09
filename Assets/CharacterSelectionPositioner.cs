using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionPositioner : MonoBehaviour
{
    public Vector3 characterSelPosition;
    public Vector3 characterSelRotation;
    public Vector3 characterSelScale = Vector3.one;

    private void Awake()
    {
        // Check if we're in the Character Selection scene
        if (SceneManager.GetActiveScene().name == "Character Sel")
        {
            // Set position, rotation, and scale immediately
            transform.position = characterSelPosition;
            transform.eulerAngles = characterSelRotation;
            transform.localScale = characterSelScale;
        }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we've loaded into the Character Selection scene
        if (scene.name == "Character Sel")
        {
            // Set position, rotation, and scale
            transform.position = characterSelPosition;
            transform.eulerAngles = characterSelRotation;
            transform.localScale = characterSelScale;
        }
    }
}