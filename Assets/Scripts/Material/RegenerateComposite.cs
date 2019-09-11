using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateComposite : Notifiable
{
    public override int priority { get { return 100; } }

    private CompositeCollider2D composite;
  

    // Start is called before the first frame update
    void Awake()
    {
        composite = GetComponent<CompositeCollider2D>();
    }

    /// NOTIFIABLE INTERFACE
    public override void ChangeColour(bool isNewColorRed)
    {
        composite.GenerateGeometry();
    }

    public override void initializeNotifiable(bool isNewColorRed)
    {
        ChangeColour(isNewColorRed);
    }
}
