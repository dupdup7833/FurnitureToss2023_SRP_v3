using System.Linq;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HurricaneVR.TechDemo.Scripts;


public class FT_UIManager : DemoUIManager
{
    public Slider GraphicsQualitySlider;
    public TextMeshProUGUI CurrentQualitySettingText;
    string[] qualitySettingNames;

    // HURRICANE UPGRADE NOTE: Had to change the accessibility of Start in DemoUIManager
    public override void Start()
    {
        base.Start();
        SetUpGraphicsQuality();

    }

    private void SetUpGraphicsQuality()
    {
        GraphicsQualitySlider.onValueChanged.AddListener(OnGraphicsQualityChanged);
        GraphicsQualitySlider.SetValueWithoutNotify(QualitySettings.GetQualityLevel());

        qualitySettingNames = QualitySettings.names;
        CurrentQualitySettingText.text = qualitySettingNames[QualitySettings.GetQualityLevel()];
    }

    private void OnGraphicsQualityChanged(float level)
    {
        //  Debug.Log("About to SetQualityLevel");
        QualitySettings.SetQualityLevel(((int)level), true);
        CurrentQualitySettingText.text = qualitySettingNames[(int)level];
        // Debug.Log("SetQualityLevel: "+(int)level);
    }
}

