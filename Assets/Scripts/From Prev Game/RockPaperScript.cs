using UnityEngine;

public class RockPaperScript : MonoBehaviour
{
    public GameObject rock;
    public GameObject paper;
    public GameObject scissors;
    public float minActivationInterval = 5f;
    public float maxActivationInterval = 10f;
    private float timer = 0f;

    private void Start()
    {
        // Start the timer with a random interval
        ResetTimer();
    }

    private void Update()
    {
        // Decrement the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Randomly activate one of the child objects
            int randomIndex = Random.Range(0, 3);
            rock.SetActive(randomIndex == 0);
            paper.SetActive(randomIndex == 1);
            scissors.SetActive(randomIndex == 2);

            // Reset the timer with a new random interval
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minActivationInterval, maxActivationInterval);
    }
}