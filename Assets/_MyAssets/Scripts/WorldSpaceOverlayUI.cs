using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceOverlayUI : MonoBehaviour
{
    private const string shaderTestMode = "unity_GUIZTestMode";
    [SerializeField] UnityEngine.Rendering.CompareFunction desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always;
    [SerializeField] Graphic[] uiElementsToApplyEffectTo;

    private Dictionary<Material, Material> materialsMapping = new Dictionary<Material, Material>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var graphic in uiElementsToApplyEffectTo ){
            Material material = graphic.materialForRendering;
            if (material == null) {
                Debug.Log("target element does not have rendering component");
                continue;
            }  

            if (materialsMapping.TryGetValue(material, out Material materialCopy)==false){
                materialCopy = new Material(material);
                materialsMapping.Add(material, materialCopy);

            }

            materialCopy.SetInt(shaderTestMode,(int)desiredUIComparison);
            graphic.material = materialCopy;
        }
    }

 
}
