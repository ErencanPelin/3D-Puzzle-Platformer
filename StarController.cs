using System.Collections;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public GameObject collectFX;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject collect = Instantiate(collectFX, this.transform.position, Quaternion.identity);
            col.gameObject.GetComponent<PlayerController>().points += 1;
            Destroy(this.gameObject);
        }
    }
}
