using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    public GameObject boosterFlame;
    private float elapsedTime = 0f;
    public float scoreMultiplier = 10f;
    private float score = 0f;
    public UIDocument uIDocument;
    //score lable
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Restart button
        restartButton = uIDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;

        restartButton.clicked += ReloadScene;

        rb = GetComponent<Rigidbody2D>();

        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
    }

    // Update is called once per frame
    void Update()
    {
        //score updating function
        UpdateScore();
        //Mover player function
        MovePlayer();

        //make the flame 
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }
    //Game over function
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
    }


    void UpdateScore()
    {
        //increase the elapsed time
        elapsedTime = elapsedTime + Time.deltaTime;

        //score calculator
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        Debug.Log("Score: " + score);
        //Debug.Log("Elapsed time: " + elapsedTime);

        //Disply the score
        scoreText.text = "Score: " + score;
    }
    
    

    void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            //Debug.Log("Mouse position : " + mousePos);

            // Move player in direction of mouse
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
                boosterFlame.SetActive(true);

            }
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}