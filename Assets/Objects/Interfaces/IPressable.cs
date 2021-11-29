using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressable
{
    void buttonPressed();

    void buttonUnpressed();

    bool pressed { get; set; }
}
