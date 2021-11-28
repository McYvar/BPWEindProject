using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable
{
    Vector3 location { get; set; }
    void Switch(Vector3 location);
}
