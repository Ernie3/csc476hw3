using System;

using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private static Slingshot S;

    [Header("Set in Inspector")]
    public GameObject prefabProejctile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidBody;
    private LineRenderer line;

    public static Vector3 LAUNCH_POS
    {
        get
        {
            if(S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
        }
    }


    void Awake()
    {
        S = this;
        var launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPos = launchPoint.transform.position;
        launchPoint.SetActive(false);
        line = GetComponent<LineRenderer>();
    }

    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProejctile);
        projectile.transform.position = launchPos;
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
        projectileRigidBody.isKinematic = true;

        line.enabled = true;
        line.positionCount = 3;
        line.SetPosition(0, transform.Find("LeftArm").position);
        line.SetPosition(1, projectile.transform.position);
        line.SetPosition(2, transform.Find("RightArm").position);
    }

    void Update()
    {
        if (!aimingMode)
            return;

        var mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        var mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        var mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        var projPos = launchPos + mouseDelta;
        line.SetPosition(1, projPos);
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            // mouse button released
            line.enabled = false;
            aimingMode = false;
            projectileRigidBody.isKinematic = false;
            projectileRigidBody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
