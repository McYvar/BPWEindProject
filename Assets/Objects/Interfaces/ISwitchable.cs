using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable
{
    // To perform a switch I need a location, a scale of the object this interface is on
    // and a method that defines the switching
    Vector3 location { get; set; }
    float yScale { get; set; }
    void Switch(Vector3 location);
}
