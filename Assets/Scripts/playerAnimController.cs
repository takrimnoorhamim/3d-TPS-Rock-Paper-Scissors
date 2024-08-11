// playerAnimController.cs
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerAnimController : MonoBehaviour
{
    public Animator anim;
    public float playerYPos;
    private ThirdPersonMovement playerMovement;
    public float runSpeed, speed;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (SceneManager.GetActiveScene().name != "Character Sel")
        {
            gameObject.transform.position = new Vector3(0, playerYPos, 0);
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerMovement = playerObject.GetComponent<ThirdPersonMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("ThirdPersonMovement component not found on the Player object.");
            }
        }
    }

    public Animator GetAnimator()
    {
        return anim;
    }

    void Update()
    {
        if (playerMovement == null) return;
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Run", 1.0f, 0.1f, Time.deltaTime);
            playerMovement.SetSpeed(runSpeed);
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Run", 0.5f, 0.1f, Time.deltaTime);
            playerMovement.SetSpeed(speed);
        }
        else
        {
            anim.SetFloat("Run", 0.0f, 0.1f, Time.deltaTime);
            playerMovement.SetSpeed(speed);
        }
    }

}