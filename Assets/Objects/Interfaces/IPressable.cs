using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressable
{    
    bool pressed { get; set; }

    void PressObject();

    void UnpressObject();
}