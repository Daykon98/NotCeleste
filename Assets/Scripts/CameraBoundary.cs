using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    private Vector2 possibleXYvalues;
    [SerializeField] private GameObject sceneGO;

    private sceneController scene;
    

    // Start is called before the first frame update
    void Start()
    {
        scene = sceneGO.GetComponent<sceneController>();
        EdgeCollider2D edgeCollider;
        edgeCollider = GetComponent<EdgeCollider2D>();
        possibleXYvalues = new Vector2(Mathf.Abs(edgeCollider.points[0].x), Mathf.Abs(edgeCollider.points[0].y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        
        Vector2 pointGlobal = contact.point;
        Vector2 point = transform.InverseTransformPoint(pointGlobal);

        //Las esquinas dan problemas un 99% seguro con esto, pero que mas da, quien cojones quiere pasar a otra habitacion por las esquinas
        if (point.x >= possibleXYvalues.x - 0.01f && point.x <= possibleXYvalues.x + 0.01f) //Me temo que igual floats no es la mejor idea, pero soy un valiente y no me apetece currarme nada
            scene.edgeReached(CustomEnums.directions.right);
        else if (point.x >= -possibleXYvalues.x - 0.01f && point.x <= -possibleXYvalues.x + 0.01f)
            scene.edgeReached(CustomEnums.directions.left);
        else if (point.y >= possibleXYvalues.y - 0.01f && point.y <= possibleXYvalues.y + 0.01f)
            scene.edgeReached(CustomEnums.directions.up);
        else
            scene.edgeReached(CustomEnums.directions.down);

    }
}
