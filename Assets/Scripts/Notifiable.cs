using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Notifiable : MonoBehaviour, NotifiableIface
{
    public abstract int priority { get; } 
    public bool activeNotifier;
    public bool ActiveNotifier
        { get { return activeNotifier; }
        set { if (value)
                gameObject.tag = "ColorNotified";
            else
                gameObject.tag = "Untagged";
            activeNotifier = value; } }

    public abstract void ChangeColour(bool isNewColorRed);
    public abstract void initializeNotifiable(bool isNewColorRed);
}
