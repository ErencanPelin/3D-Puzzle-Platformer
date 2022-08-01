using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class SettingsController : MonoBehaviour
{
    public Slider VolumeSlider;
    public Slider QualitySlider;

    public RenderPipelineAsset[] renderTiers;
    public int qualityIndex;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (VolumeSlider == null)
        {
            VolumeSlider = null;
            VolumeSlider = GameObject.FindGameObjectWithTag("MusicControl").GetComponent<Slider>();
            VolumeSlider.value = GetComponent<AudioSource>().volume;
        }
        if (QualitySlider == null) 
        {
            QualitySlider = null;
            QualitySlider = GameObject.FindGameObjectWithTag("QualityControl").GetComponent<Slider>();
            QualitySlider.value = qualityIndex;
        }

        qualityIndex = (int)QualitySlider.value;
        GraphicsSettings.renderPipelineAsset = renderTiers[qualityIndex];
        GetComponent<AudioSource>().volume = VolumeSlider.value;
    }
}
