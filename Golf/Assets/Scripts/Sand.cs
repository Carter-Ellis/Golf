using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Sand : MonoBehaviour
{
    [SerializeField] private float sandDrag = 5f;
    [SerializeField] private float minSpeedForParticles = 1f;
    private float ballDrag;
    private ParticleSystem ps;
    private void Awake()
    {
        Ball ball = FindObjectOfType<Ball>();
        ballDrag = ball.GetComponent<Rigidbody2D>().drag;
        ps = ball.GetComponentInChildren<ParticleSystem>();
        ps.gameObject.SetActive(false);
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (ps != null)
            {
                ps.gameObject.SetActive(true);
                var velocityModule = ps.velocityOverLifetime;
                velocityModule.enabled = true;

                // Set constant velocity based on ball velocity
                velocityModule.x = new ParticleSystem.MinMaxCurve(rb.velocity.x / 2);
                velocityModule.y = new ParticleSystem.MinMaxCurve(rb.velocity.y / 2);
                velocityModule.z = new ParticleSystem.MinMaxCurve(0f);
                ps.Play();
            }

            if (rb != null)
                rb.drag = sandDrag;
                
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            var particles = collision.GetComponentInChildren<ParticleSystem>();

            if (particles != null)
            {
                if (rb.velocity.magnitude > minSpeedForParticles && !particles.isPlaying)
                {
                    
                    var velocityModule = particles.velocityOverLifetime;
                    velocityModule.enabled = true;

                    // Set constant velocity based on ball velocity
                    velocityModule.x = new ParticleSystem.MinMaxCurve(rb.velocity.x / 2);
                    velocityModule.y = new ParticleSystem.MinMaxCurve(rb.velocity.y / 2);
                    velocityModule.z = new ParticleSystem.MinMaxCurve(0f);
                    particles.Play();
                }
                else if (rb.velocity.magnitude <= minSpeedForParticles && particles.isPlaying)
                {
                    particles.Stop();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            
            if (rb != null)
                rb.drag = ballDrag;

            var particles = collision.GetComponentInChildren<ParticleSystem>();
            if (particles != null)
                particles.Stop();
        }
    }
}
