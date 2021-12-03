using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressable
{    
    bool pressed { get; set; }

    bool stayActive { get; set; }

    void PressObject();

    void UnpressObject();
}
