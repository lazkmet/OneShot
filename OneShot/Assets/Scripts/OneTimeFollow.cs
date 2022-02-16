using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeFollow : BezierFollow
{
    public BezierFollow permanentPath;
    protected override IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0;
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;

        while (tParam < 1)
        {
            p0 = routes[routeNum].controlPoints[0].position;
            p1 = routes[routeNum].controlPoints[1].position;
            p2 = routes[routeNum].controlPoints[2].position;
            p3 = routes[routeNum].controlPoints[3].position;

            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            permanentPath.enabled = true;
            Destroy(this);        
        }

        coroutineAllowed = true;

    }
}
