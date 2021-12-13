using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePiston : MonoBehaviour
{
    public GameObject activator;
    public GameObject extender;
    public GameObject extraExtender;

    public float pistonMinExtention;
    public float pistonMaxExtention;
    public float extentionSpeed;
    IPressable activatorInterface;

    public bool StartReversed;

    private void Start()
    {
        activatorInterface = activator.GetComponent<IPressable>();
    }

    private void Update()
    {
        float movePiston = 1;
        if (activatorInterface != null)
        {
            if (!StartReversed)
            {
                bool active = activatorInterface.activateObject;
                if (active)
                {
                    movePiston = movePiston * extentionSpeed;
                }
                else
                {
                    movePiston = -movePiston * extentionSpeed;
                }
            }
            else
            {
                bool active = activatorInterface.activateObject;
                if (active)
                {
                    movePiston = -movePiston * extentionSpeed;
                }
                else
                {
                    movePiston = movePiston * extentionSpeed;
                }
            }
        }

        float minTotalExtention = pistonMinExtention / 10;
        float minFirstExtention = minTotalExtention, minSecondExtention = minTotalExtention;
        if (minTotalExtention < 0)
        {
            minFirstExtention = 0;
            minSecondExtention = 0;
        }

        float maxTotalExtention = pistonMaxExtention / 10;
        float maxFirstExtention = maxTotalExtention, maxSecondExtention = maxTotalExtention;
        if (maxTotalExtention > extender.transform.localScale.y * 2) maxFirstExtention = extender.transform.localScale.y * 2;
        if (maxTotalExtention > extraExtender.transform.localScale.y * 2) maxSecondExtention = extraExtender.transform.localScale.y * 2;

        extender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extender.transform.localPosition = new Vector3(extender.transform.localPosition.x, Mathf.Clamp(extender.transform.localPosition.y, minFirstExtention , maxFirstExtention), extender.transform.localPosition.z);

        extraExtender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extraExtender.transform.localPosition = new Vector3(extraExtender.transform.localPosition.x, Mathf.Clamp(extraExtender.transform.localPosition.y, minSecondExtention, maxSecondExtention), extraExtender.transform.localPosition.z);
    }

}
