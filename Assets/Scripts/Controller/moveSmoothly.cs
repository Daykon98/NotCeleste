using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSmoothly : MonoBehaviour
{
    private Vector3 _target;
    private Vector3 _startPoint;
    private float _animationTimePosition;
    private bool doMove;

    public bool useLocalPosition;
    public float animationSeconds = 2;
    public AnimationCurve MoveCurve;

    public float moveObjectiveX = 0;
    public float moveObjectiveY = 0;

    // Start is called before the first frame update
    private void Start()
    {
        doMove = false;
        UpdatePath();
    }

    private void Update()
    {
        if (doMove)
        {
            _animationTimePosition += Time.deltaTime / animationSeconds;
            if (useLocalPosition)
            {
                transform.localPosition = Vector3.Lerp(_startPoint, _target, MoveCurve.Evaluate(_animationTimePosition));
                if (_target == transform.localPosition)
                {
                    doMove = false;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(_startPoint, _target, MoveCurve.Evaluate(_animationTimePosition));
                if (_target == transform.position)
                {
                    doMove = false;
                }
            }
        }

    }

    public void UpdatePath()
    {
        if (useLocalPosition)
        {
            _startPoint = transform.localPosition;
            _target = new Vector3(transform.localPosition.x + moveObjectiveX, transform.localPosition.y + moveObjectiveY, transform.localPosition.z);
        }
        else
        {
            _startPoint = transform.position;
            _target = new Vector3(transform.position.x + moveObjectiveX, transform.position.y + moveObjectiveY, transform.position.z);
        }
        
    }

    public void move()
    {
        doMove = true;
        _animationTimePosition = 0;
        UpdatePath();
    }
}