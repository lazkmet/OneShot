using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerMeter : MonoBehaviour
{
    public Image powerBar;
    public TextMeshProUGUI readyMessage;
    public TextMeshProUGUI countdown;
    public Color chargingColor;
    public Color readyColor;
    public void Awake()
    {
        readyMessage.enabled = false;
    }
    public void setCooldown(float aMax, float aCurrent) {
        if (aMax > 0)
        {
            powerBar.fillAmount = (aMax - aCurrent) / aMax;
        }
        else {
            powerBar.fillAmount = 0;
            aCurrent = 0;
        }
        countdown.text = Mathf.CeilToInt(aCurrent).ToString() + "s";
        if (aCurrent > 0)
        {
            readyMessage.enabled = false;
            powerBar.color = chargingColor;
        }
        else {
            readyMessage.enabled = true;
            powerBar.color = readyColor;
        }

    }
}
