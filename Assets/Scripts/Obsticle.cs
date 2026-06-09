using NUnit.Framework.Internal;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Obsticle : MonoBehaviour
{
    public float maxSize = 2.0f;
    public float minSize = 0.5f;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxSpinSpeed = 15f;
    public GameObject bounceEffectPrefab;
    Rigidbody2D rb;
    private float randomSize= 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize,maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 randomDirection = Random.insideUnitCircle;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce((randomDirection * randomSpeed) / randomSize);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Spawn the bounce visual effect at the exact contact point
        Vector2 contactPoint = collision.GetContact(0).point;
        if (bounceEffectPrefab != null)
        {
            GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
            Destroy(bounceEffect, 1f); // Safely destroy effect after 1 second
        }

        // 2. Add an extra physics push away from the impact point (Optional bounce boost)
        // We calculate the direction from the hit point to the center of this obstacle
        Vector2 pushDirection = ((Vector2)transform.position - contactPoint);
        // Push it away! (Adjust the multiplier '10f' to change how violently it reacts)
        rb.AddForce(pushDirection * 10f / randomSize, ForceMode2D.Impulse);
        
    }
}
