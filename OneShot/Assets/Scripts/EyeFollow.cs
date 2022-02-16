using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public EnemyScript enemy;
    private Transform target;
    public GameObject eye;
    public GameObject iris;
    private Vector2 eyeSize;
    private Vector2 irisZone = Vector2.zero;
    private Vector2 irisUp;

    private void Awake()
    {
        target = enemy.target;
        eyeSize = eye.transform.lossyScale / 2f;
        irisUp = iris.transform.up;
    }
    private void Start()
    {
        //Will have undefined behavior if iris is larger than eye
        irisZone = 0.4f * (eyeSize - (Vector2)iris.GetComponent<SpriteRenderer>().bounds.extents);
    }
    private void Update()
    {
        if (target != null)
        {
            AimEye();
        }
        else
        {
            iris.transform.localPosition = Vector2.zero;
        }
    }
    private void AimEye()
    {
        Vector2 targetAngle = (target.position - this.transform.position).normalized;
        //aim
        gameObject.transform.up = targetAngle;
        //calculate position within eye (linear interpolate from width to height based on angle, in a similar manner to an ellipse)
        iris.transform.localPosition = Vector2.up * Mathf.Lerp(irisZone.x, irisZone.y, Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(Vector2.right, targetAngle)));
        iris.transform.up = irisUp;
    }
}
