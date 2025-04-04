using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingController : MonoBehaviour
{

    public GameObject leftJump, rightJump, settingsPanel;
    public int jumpButton;

    public void JumpSliderChanged(float state)
    {
        jumpButton = (int)state;
        if (jumpButton == 0 && !leftJump.activeSelf)
        {
            leftJump.SetActive(true);
            rightJump.SetActive(false);
        }
        else if (jumpButton == 1 && !rightJump.activeSelf)
        {
            leftJump.SetActive(false);
            rightJump.SetActive(true);
        }
    }

    public void OpCloSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
