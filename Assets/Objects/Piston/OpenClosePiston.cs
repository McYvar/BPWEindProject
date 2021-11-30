using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePiston : MonoBehaviour
{
    public GameObject activator;
    public GameObject extender;
    public GameObject extraExtender;

    public float pistonMaxExtention;
    public float extentionSpeed;
    IPressable activatorInterface;
    private void Start()
    {
        activatorInterface = activator.GetComponent<IPressable>();
    }

    private void Update()
    {
        float movePiston = 1;
        if (activatorInterface != null)
        {
            bool active = activatorInterface.pressed;
            if (active)
            {
                movePiston = movePiston * extentionSpeed;
            }
            else
            {
                movePiston = -movePiston * extentionSpeed;
            }
        }

        float maxTotalExtention = pistonMaxExtention / 10;
        float maxFirstExtention = maxTotalExtention, maxSeccondExtention = maxTotalExtention;
        if (maxTotalExtention > extender.transform.localScale.y * 2) maxFirstExtention = extender.transform.localScale.y * 2;
        if (maxTotalExtention > extraExtender.transform.localScale.y * 2) maxSeccondExtention = extraExtender.transform.localScale.y * 2;

        extender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extender.transform.localPosition = new Vector3(extender.transform.localPosition.x, Mathf.Clamp(extender.transform.localPosition.y, 0 , maxFirstExtention), extender.transform.localPosition.z);

        extraExtender.transform.localPosition += Vector3.up * movePiston * Time.deltaTime;
        extraExtender.transform.localPosition = new Vector3(extraExtender.transform.localPosition.x, Mathf.Clamp(extraExtender.transform.localPosition.y, 0, maxSeccondExtention), extraExtender.transform.localPosition.z);
    }
}
