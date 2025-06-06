using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private float idleTime;
    private float timer = 0;
    [SerializeField] private Animator anim;
    private CapsuleCollider2D cc;
    private bool playedAudio;
    private void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
        cc.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Set idle to MolePopup
        if (timer > idleTime )
        {
            if (!playedAudio)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.moleCrawl, transform.position);
                playedAudio = true;
            }

            anim.SetBool("IsPopup", true);
            Vector3 pos = transform.position;
            if (FindObjectOfType<Ball>() == null)
            {
                return;
            }
            if (FindObjectOfType<Ball>().transform.position.y > transform.position.y)
            {

                transform.position = new Vector3(pos.x, pos.y, -5);
            }
            else
            {
                transform.position = new Vector3(pos.x, pos.y, 5);
            }
            cc.enabled = true;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        //Set MolePopup to idle
        if (stateInfo.IsName("MolePopup") && stateInfo.normalizedTime > 1f)
        {
            playedAudio = false;
            anim.SetBool("IsPopup", false);
            timer = 0;
            cc.enabled = false;
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, 5);
            

        }
    }
}
