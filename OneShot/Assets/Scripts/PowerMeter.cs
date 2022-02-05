using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{
    public Image powerBar;
    public Text readyMessage;
    public Color chargingColor;
    public Color readyColor;
    public void setCooldown(float aMax, float aCurrent) {
        if (aMax > 0)
        {
            powerBar.fillAmount = aMax - aCurrent / aMax;
        }
        else {
            powerBar.fillAmount = 0;
            aCurrent = 0;
        }
        if (aCurrent > 0)
        {
            readyMessage.text = ((int)aCurrent).ToString();
            readyMessage.color = chargingColor;
        }
        else {
            readyMessage.text = "Ready!";
            readyMessage.color = readyColor;
        }

    }
}
