using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreditsManager : MonoBehaviour
{
    private static CreditsManager _instance;
    public static CreditsManager Instance
    {
        get
        {
            // If the instance is null, try to find an existing one in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<CreditsManager>();

                // If still null, log an error because there should be one CreditsManager in the scene
                if (_instance == null)
                {
                    Debug.LogError("No instance of CreditsManager found in the scene!");
                }
            }
            return _instance;
        }
    }

    public Text creditsText;
    private int score;
    private string saveFilePath;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        LoadScore();
        DisplayCredits();
    }

    private void LoadScore()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            score = data.score;
            Debug.Log("Score loaded from " + saveFilePath);
        }
        else
        {
            Debug.Log("Save file not found, defaulting to 0 score.");
            score = 0;
        }
    }

    private void DisplayCredits()
    {
        string creditsString = $"{score}";
        creditsText.text = creditsString;
    }

    public void DeductCredits(int amount)
    {
        score -= amount;
        SaveScore();
        DisplayCredits();
    }

    private void SaveScore()
    {
        SaveData data = new SaveData { score = this.score };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Score saved to " + saveFilePath);
    }

    public int GetScore()
    {
        return score;
    }

    [System.Serializable]
    private class SaveData
    {
        public int score;
    }
}
