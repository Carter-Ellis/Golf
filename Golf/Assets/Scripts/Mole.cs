using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private float idleTime;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem popupParticles;

    private CapsuleCollider2D cc;
    private float timer = 0f;
    private bool playedAudio = false;
    private Inventory inv;

    private void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
        cc.enabled = false;
        inv = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > idleTime)
        {
            if (!playedAudio)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.moleCrawl, transform.position);
                playedAudio = true;
                PlayPopupParticles();
            }

            anim.SetBool("IsPopup", true);

            Vector3 pos = transform.position;
            Ball ball = FindObjectOfType<Ball>();
            if (ball == null) return;

            bool ballAbove = ball.transform.position.y > transform.position.y;

            // Adjust Z-depth based on ball position
            transform.position = new Vector3(pos.x, pos.y, ballAbove ? -5 : 5);

            // Flip particle direction

            cc.enabled = true;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("MolePopup") && stateInfo.normalizedTime > 1f)
        {
            playedAudio = false;
            anim.SetBool("IsPopup", false);
            timer = 0;
            cc.enabled = false;

            // Reset Z-depth
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, 5);
        }
    }

    void PlayPopupParticles()
    {
        if (popupParticles == null) return;

        popupParticles.Play(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameMode.current != GameMode.TYPE.CLUBLESS)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.moleHit, transform.position);
        }
        
    }

}
