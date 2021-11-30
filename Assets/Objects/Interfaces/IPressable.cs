using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressable
{    
    int amountOfObjectsOnButton { get; set; }

    bool pressed { get; set; }

    void PressObject();

    void UnpressObject();
}
