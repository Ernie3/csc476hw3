using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine S; // singleton

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    public GameObject poi
    {
        get
        {
            return _poi;
        }
        set
        {
            _poi = value;
            if(_poi != null)
            {
                AddPoint();
            }
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(poi == null)
        {
            // if there is no poi then search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.CompareTag("Projectile"))
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        AddPoint();
        if(FollowCam.POI == null)
        {
            poi = null;
        }
    }

    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void AddPoint()
    {
        var pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist) // if point isn't far enough from the last point
        {
            return;
        }
        if(points.Count == 0) // if this is the launch point
        {
            var launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            // normal behavior adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
}
