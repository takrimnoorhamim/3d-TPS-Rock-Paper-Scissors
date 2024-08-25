using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerCollisionHandler : MonoBehaviour
{
    public enum RpsType
    {
        Rock,
        Paper,
        Scissors
    }

    [System.Serializable]
    public class RpsChild
    {
        public RpsType type;
        public GameObject gameObject;
    }

    [System.Serializable]
    private class SaveData
    {
        public int score;
    }

    public List<RpsChild> playerChildren = new List<RpsChild>();
    public List<GameObject> bots = new List<GameObject>();
    public Text scoreText;
    public int score;

    private string saveFilePath;
    private Animator anim;
    private playerAnimController playerAnimControll;

    public AudioSource src;
    public AudioClip scoreSound, deadSound;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        LoadJson();
        UpdateScoreText();

        // Find the playerAnimController component on the respawned character
        playerAnimControll = FindObjectOfType<playerAnimController>();
        if (playerAnimControll != null)
        {
            anim = playerAnimControll.GetAnimator();
        }
        else
        {
            Debug.LogError("playerAnimController component not found on the respawned character.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (bots.Contains(collidedObject))
        {
            RpsType? playerActiveType = GetActiveRpsType(playerChildren);
            RpsType? botActiveType = GetBotActiveRpsType(collidedObject);
            Debug.Log($"Player active type: {playerActiveType}");
            Debug.Log($"Bot active type: {botActiveType}");
            if (playerActiveType.HasValue && botActiveType.HasValue)
            {
                int result = DetermineWinner(playerActiveType.Value, botActiveType.Value);
                if (result == 1)
                {
                    // Player wins
                    score++;
                    UpdateScoreText();
                    src.PlayOneShot(scoreSound);
                    SaveJson();
                    Destroy(collidedObject);
                    bots.Remove(collidedObject);
                    Debug.Log("Player wins! Score: " + score);
                }
                else if (result == -1)
                {
                    // Bot wins
                    if (anim != null)
                    {
                        StartCoroutine(TriggerDeathAndLoadScene());
                    }
                    else
                    {
                        Debug.LogError("Animator not found. Loading Scene 0 immediately.");
                        SceneManager.LoadScene(1);
                    }
                }
                else
                {
                    // Tie
                    Debug.Log("It's a tie!");
                }
            }
            else
            {
                Debug.Log("Either player or bot doesn't have an active RPS object.");
            }
        }
    }

    private IEnumerator TriggerDeathAndLoadScene()
    {
        anim.SetTrigger("Death");
        src.PlayOneShot(deadSound);

        // Wait for the death animation to finish
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Load Scene 1
        SceneManager.LoadScene(1);
    }

    private RpsType? GetActiveRpsType(List<RpsChild> children)
    {
        foreach (RpsChild child in children)
        {
            if (child.gameObject != null && child.gameObject.activeSelf)
            {
                return child.type;
            }
        }
        return null;
    }

    private RpsType? GetBotActiveRpsType(GameObject botObject)
    {
        foreach (RpsType type in System.Enum.GetValues(typeof(RpsType)))
        {
            GameObject rpsObject = FindActiveRpsObject(botObject, type.ToString());
            if (rpsObject != null)
            {
                return type;
            }
        }
        return null;
    }

    private GameObject FindActiveRpsObject(GameObject parent, string typeName)
    {
        if (parent.name == typeName && parent.activeSelf)
        {
            return parent;
        }
        foreach (Transform child in parent.transform)
        {
            GameObject found = FindActiveRpsObject(child.gameObject, typeName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    private int DetermineWinner(RpsType playerType, RpsType botType)
    {
        if (playerType == botType) return 0; // Tie
        switch (playerType)
        {
            case RpsType.Rock:
                return (botType == RpsType.Scissors) ? 1 : -1;
            case RpsType.Paper:
                return (botType == RpsType.Rock) ? 1 : -1;
            case RpsType.Scissors:
                return (botType == RpsType.Paper) ? 1 : -1;
            default:
                return 0; // Should never happen
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void SaveJson()
    {
        SaveData data = new SaveData
        {
            score = this.score
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Score saved to " + saveFilePath);
    }

    public void LoadJson()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            this.score = data.score;
            Debug.Log("Score loaded from " + saveFilePath);
        }
        else
        {
            Debug.Log("Save file not found, starting with default score.");
        }
    }
}