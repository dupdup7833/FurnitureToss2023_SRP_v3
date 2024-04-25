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

    public Slider VignetteAmtSlider;
    public TextMeshProUGUI CurrentVignetteAmountText;
    string[] qualitySettingNames;
    FT_PlayerController ftPlayerController;


    // HURRICANE UPGRADE NOTE: Had to change the accessibility of Start in DemoUIManager
    public override void Start()
    {
       base.Start();
        //FT_GameController.GC.LoadPlayerOptions();
        SetUpGraphicsQuality();
        SetUpComfortSetting();
        ftPlayerController = Player.GetComponent<FT_PlayerController>();
        Debug.Log("ftPlayerController" + ftPlayerController);

    }

    private void SetUpGraphicsQuality()
    {
        GraphicsQualitySlider.onValueChanged.AddListener(OnGraphicsQualityChanged);
        GraphicsQualitySlider.SetValueWithoutNotify(QualitySettings.GetQualityLevel());

        qualitySettingNames = QualitySettings.names;
        CurrentQualitySettingText.text = qualitySettingNames[QualitySettings.GetQualityLevel()];
    }

    private void SetUpComfortSetting()
    {
        VignetteAmtSlider.onValueChanged.AddListener(OnComfortSettingChanged);
      //  Debug.Log("FT_GameController.GC.playerOptions.comfortSetting "+FT_GameController.GC.playerOptions.comfortSetting);
        VignetteAmtSlider.SetValueWithoutNotify(FT_GameController.GC.playerOptions.comfortSetting);

        CurrentVignetteAmountText.text = FT_GameController.comfortSettingNames[FT_GameController.GC.playerOptions.comfortSetting];
    }
    private void OnGraphicsQualityChanged(float level)
    {
        //  Debug.Log("About to SetQualityLevel");
        QualitySettings.SetQualityLevel(((int)level), true);
        CurrentQualitySettingText.text = qualitySettingNames[(int)level];
        // Debug.Log("SetQualityLevel: "+(int)level);
    }


    private void OnComfortSettingChanged(float amt)
    {

        ftPlayerController.postProcessing.VignetteAmount =FT_GameController.vignetteAmtSettings[ (int)amt];
        CurrentVignetteAmountText.text = FT_GameController.comfortSettingNames[(int)amt];
        PlayerPrefs.SetInt("comfortSetting", (int)amt);
        // FT_GameController.GC.playerOption.vignetteAmt", .75f);

    }
}

