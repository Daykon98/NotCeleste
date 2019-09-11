using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitRipple : MonoBehaviour
{

    [SerializeField] private GameObject cameraObject; //La camara a la que se le ordenará emitir el ripple.
    private Camera camera;
    private RippleEffect rippleEffect;
    //Esta debe tener el script de ripple como componente

    // Start is called before the first frame update
    void Start()
    {
        camera = cameraObject.GetComponent<Camera>();
        rippleEffect = camera.GetComponent<RippleEffect>();
    }

    public void changeColor(Color color)
    {
        rippleEffect.reflectionColor = color;
    }
    public void Emit(Vector2 position)
    {
        Vector3 pointOnScreen = camera.WorldToScreenPoint(new Vector3(position.x, position.y, 0));
        Vector2 pointOnScreen2 = new Vector2(pointOnScreen.x, pointOnScreen.y);
        rippleEffect.EmitWithPosition(new Vector2(pointOnScreen2.x / camera.pixelWidth, pointOnScreen2.y / camera.pixelHeight));
    }
}
