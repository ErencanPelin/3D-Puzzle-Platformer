using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "settings", menuName = "Settings")]
public class GameSettings : ScriptableObject
{
    [Range(0, 100)]
    public float musicVolume;
    [Range(0, 10)]
    public float camSensitivity;
}
