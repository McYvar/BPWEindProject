using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePiston : MonoBehaviour
{
    public GameObject activator;
    public GameObject extender;
    public GameObject extraExtender;
    public GameObject platformPlane;

    public float pistonMinExtention;
    public float pistonMaxExtention;
    public float extentionSpeed;
    IPressable activatorInterface;

    public bool StartReversed;

    public List<GameObject> pushList;

    private void Start()
    {
        activatorInterface = activator.GetComponent<IPressable>();
    }

    private void Update()
    {
        // Based on a bool, and starting point for the piston, it either starts normal or inversed, and has has a minimal and maximal extention, on activion the piston moves
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

        // Calculation for all extentions
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

        // Actually move the pistons based on the given values
        extender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extender.transform.localPosition = new Vector3(extender.transform.localPosition.x, Mathf.Clamp(extender.transform.localPosition.y, minFirstExtention , maxFirstExtention), extender.transform.localPosition.z);

        extraExtender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extraExtender.transform.localPosition = new Vector3(extraExtender.transform.localPosition.x, Mathf.Clamp(extraExtender.transform.localPosition.y, minSecondExtention, maxSecondExtention), extraExtender.transform.localPosition.z);

        foreach (GameObject obj in pushList)
        {
            if (obj != null)
            {
                obj.transform.position = platformPlane.transform.position;
            }
        }
    }

}
