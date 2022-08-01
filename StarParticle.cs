using System.Collections;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    private void Start()
    {
        if (this.gameObject.name.Contains("Slime"))
        {
            Destroy(this.gameObject, 10f);
        }
        else 
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
