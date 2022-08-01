using System.Collections;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public GameObject playerObject;

    private Animator anim;
    public bool moving = false;
    public GameObject slimeSquishEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword")) 
        {
            Die();
        }
    }

    void Die() 
    {
        Instantiate(slimeSquishEffect, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (playerObject)
        {
            float dist = Vector3.Distance(transform.position, playerObject.transform.position);

            if (dist < 50)
            {
                transform.LookAt(new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z));
                moving = true;
            }
            else
            {
                moving = false;
            }
        }

        anim.SetBool("moving", moving);
    }
}
