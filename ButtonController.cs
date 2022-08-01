using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool isOn = false;
    public Light lightFx;
    public Animator activates;
    private void OnTriggerStay(Collider col)
    {
        isOn = true;
    }

    public void OnTriggerExit(Collider col)
    {
        isOn = false;
    }

    private void Update()
    {
        lightFx.enabled = isOn;
        activates.SetBool("open", isOn);
    }
}
