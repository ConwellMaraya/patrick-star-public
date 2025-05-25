using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggleScript : MonoBehaviour
{
    UnityEngine.UI.Toggle toggle;
    public void OnValueChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
