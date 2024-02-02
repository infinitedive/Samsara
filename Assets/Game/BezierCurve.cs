using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Game.Controllers;

// [ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{


    public float totalDistance;
    public float estimatedTime;
    public float estimatedVelocity;
    public float initialVelocityMag = 0f;
    public GameObject emptyParent;
    public Vector3 swingOffset = Vector3.zero;
    public float lookingAtPoint = 0f;
    public Vector3 contactOffset = Vector3.zero;
    public LineRenderer lr;
    // public OrientedPoint control;
    GameObject[] controlPoints = new GameObject[4];
    Vector3[] projectedPoints = new Vector3[stepNumber];
    Vector3 projectedVelocity = Vector3.zero;
    SkateCharacterController ctx;
    static int stepNumber = 500;

    Plane cutoffPlane;

    GameObject xPlaneObj = null;
    GameObject yPlaneObj = null;
    GameObject zPlaneObj = null;
    GameObject xPoint = null;
    GameObject yPoint = null;
    GameObject zPoint = null;
    GameObject lookPoint = null;
    public GameObject arrow = null;
    
    CatmullRomCurve[] catmullRomSegments;

    public BezierCurve(SkateCharacterController ls) {

        ctx = ls;
        projectedPoints = new Vector3[stepNumber];
        controlPoints = new GameObject[4];
        emptyParent = new GameObject();
        lr = emptyParent.AddComponent<LineRenderer>();
        swingOffset = Vector3.zero;

        Material[] mats = new Material[2];

        mats[0] = Resources.Load("Materials/arrow") as Material;
        mats[1] = Resources.Load("Materials/M3") as Material;

        lr.materials = mats;
        lr.textureMode = LineTextureMode.RepeatPerSegment;
        lr.alignment = LineAlignment.View;
        lr.startWidth = 2f;

        SphereCollider collider;

        catmullRomSegments = new CatmullRomCurve[3];
        
        for ( int i = 0; i < 4; i++ ) {
            controlPoints[i] = new GameObject();
            controlPoints[i].name = "p" + i;
            collider = controlPoints[i].AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 1f;
            controlPoints[i].transform.SetParent(emptyParent.transform);
            // MeshFilter tmpFilter = controlPoints[i].AddComponent<MeshFilter>();
            // tmpFilter.mesh = Resources.Load("Meshes/Arrow") as Mesh;
            // MeshRenderer tmpRender = controlPoints[i].AddComponent<MeshRenderer>();
            // tmpRender.material = mats[1];
        }


        // arrow = new GameObject();
        // arrow.name = "arrow";
        // arrow.transform.SetParent(controlPoints[3].transform);

        // MeshFilter meshFilter = arrow.AddComponent<MeshFilter>();
        // MeshRenderer meshRender = arrow.AddComponent<MeshRenderer>();
        // meshRender.material = Resources.Load("Materials/OrangeEmission") as Material;
        // meshFilter.mesh = Resources.Load("Meshes/Arrow") as Mesh;
        

        // CreateArcFromVelocity(contactNormal, target);
        
    }

    Vector3 GetPos( int i ) => controlPoints[i].transform.position;

    // OrientedPoint GetCirclePoint( Vector3 start, Vector3 end, Vector3 centerPivot, float t) {

    //     Vector3 pos = ctx.CenteredSlerp(start, end, centerPivot, t);


    //     Vector3 originRelative = ctx.moveData.origin - centerPivot;
    //     Vector3 endRelative = end - centerPivot;

    //     Vector3 lookingIn = (centerPivot - ctx.moveData.origin).normalized;
    //     Vector3 discUp = Vector3.Cross(originRelative, endRelative);

    //     Debug.Log(discUp);

    //     Quaternion centerRot = Quaternion.LookRotation(centerPivot - start, discUp);

    //     Vector3 dir = centerRot * Vector3.right;

    //     return new OrientedPoint(pos, dir);

    // }

    // public void InterpolateAcrossCircleC1(Vector3 start, Vector3 end, Vector3 centerPivot, float t) { // TODO:

    //     OrientedPoint op = GetCirclePoint(start, end, centerPivot, t);

    //     float length = (end - start).magnitude * Mathf.PI / 2f;
    //     estimatedTime =  length / Mathf.Max(ctx.moveConfig.runSpeed, ctx.moveData.velocity.magnitude);
    //     estimatedVelocity = length / estimatedTime;

    //     // ctx.moveData.velocity = (op.rot * Vector3.forward) * Mathf.Lerp(initialVelocityMag, estimatedVelocity, t / estimatedTime);
    //     // ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, (op.rot * Vector3.forward) * estimatedVelocity, Time.deltaTime * 50f);
    //     ctx.moveData.velocity = op.rot * Vector3.forward * estimatedVelocity;
    //     // ctx.moveData.origin = Vector3.Slerp(ctx.moveData.origin, op.pos, Time.deltaTime * 5f);
    //     ctx.moveData.origin = op.pos;
    // }

    public void DrawCircle(Vector3 centerPivot) {
        
        lr.positionCount = 9;

        lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);
        
        // lr.SetPosition(0, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.1f).pos);
        // lr.SetPosition(1, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.2f).pos);
        // lr.SetPosition(2, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.3f).pos);
        // lr.SetPosition(3, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.4f).pos);
        // lr.SetPosition(4, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.5f).pos);
        // lr.SetPosition(5, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.6f).pos);
        // lr.SetPosition(6, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.7f).pos);
        // lr.SetPosition(7, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.8f).pos);
        // lr.SetPosition(8, GetCirclePoint(ctx.moveData.origin, ctx.moveData.focusPoint, centerPivot,.9f).pos);
    }

    public void DrawCurve() {

        // lr.positionCount = 9;
        
        // lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);
        
        // lr.SetPosition(0, GetBezierPoint(.1f).pos);
        // lr.SetPosition(1, GetBezierPoint(.2f).pos);
        // lr.SetPosition(2, GetBezierPoint(.3f).pos);
        // lr.SetPosition(3, GetBezierPoint(.4f).pos);
        // lr.SetPosition(4, GetBezierPoint(.5f).pos);
        // lr.SetPosition(5, GetBezierPoint(.6f).pos);
        // lr.SetPosition(6, GetBezierPoint(.7f).pos);
        // lr.SetPosition(7, GetBezierPoint(.8f).pos);
        // lr.SetPosition(8, GetBezierPoint(.99f).pos);

        // Vector3 p1 = GetBezierPoint(.333f).pos;
        // Vector3 p2 = GetBezierPoint(.666f).pos;
        
        ctx.vfxController._grappleArc.SetVector3("Pos0", ctx.characterData.moveData.origin + ctx.characterData.avatarLookForward);
        // ctx._grappleArc.SetVector3("Pos1", Vector3.Lerp(ctx.moveData.origin, ctx.moveData.grapplePoint, .33f));
        // ctx._grappleArc.SetVector3("Pos2", Vector3.Lerp(ctx.moveData.origin, ctx.moveData.grapplePoint, .66f));
        // ctx._grappleArc.SetVector3("Pos3", ctx.moveData.grapplePoint);
        ctx.vfxController._grappleArc.SetVector4("Color", ctx.characterData.moveConfig.grappleColor);

    }

    public void DrawProjection() {

        lr.positionCount = stepNumber;
        
        lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);

        for (int i = 0; i < stepNumber; i++) {

            lr.SetPosition(i, projectedPoints[i]);

        }

    }

    private float ApproximateArcLength() {

        float DistanceBetween(Vector3 a, Vector3 b) {
            return (a - b).magnitude;
        }

        float chord = DistanceBetween(GetPos(3), GetPos(0));
        float control_net = DistanceBetween(GetPos(0), GetPos(1)) + DistanceBetween(GetPos(2), GetPos(1)) + DistanceBetween(GetPos(3), GetPos(2));

        return (control_net + chord) / 2f;
    }

    // public void PredictGravityArc(Vector3 origin, float gravity, Vector3 initialVel, Vector3 targetNormal, Vector3 targetPos) {

    //     Vector3 projectedOrigin = origin;
    //     Vector3 projectedVelocity = initialVel;

    //     cutoffPlane = new Plane(targetNormal, targetPos);

    //     for (int i = 0; i < stepNumber; i++) {

    //         projectedPoints[i] = projectedOrigin;

    //         if (cutoffPlane.GetDistanceToPoint(projectedOrigin) < 1f) {
    //             continue;
    //         }
            
    //         projectedVelocity = projectedVelocity + Vector3.down * gravity * Time.fixedDeltaTime;
    //         projectedOrigin = projectedOrigin + projectedVelocity * Time.fixedDeltaTime;

    //     }

    // }

    public void PredictGravityArc(Vector3 origin, float gravity, Vector3 initialVel) {

        Vector3 projectedOrigin = origin;
        Vector3 projectedVelocity = initialVel;

        bool contact = false;

        for (int i = 0; i < stepNumber; i++) {

            projectedPoints[i] = projectedOrigin;

            if (contact || Physics.CheckSphere(projectedOrigin, .45f, LayerMask.GetMask("Ground"))) {
                contact = true;
                continue;
            } else {
                projectedVelocity = projectedVelocity + Vector3.down * gravity * Time.fixedDeltaTime;
                projectedOrigin = projectedOrigin + projectedVelocity * Time.fixedDeltaTime;
            }
            

        }

    }


    public void GrappleArc(Vector3 contactNormal, Vector3 target) {

        var initialVelocityDir = ctx.characterData.moveData.velocity.normalized;
        initialVelocityMag = ctx.characterData.moveData.velocity.magnitude;

        Vector3 centerPoint = Vector3.Lerp(ctx.characterData.moveData.origin, target, .95f);
        Vector3 hyp = centerPoint - ctx.characterData.moveData.origin;


        // Quaternion YRotate = Quaternion.AngleAxis(90f, Vector3.up);
        // Quaternion XRotate = Quaternion.AngleAxis(90f, Vector3.right);
        // Quaternion ZRotate = Quaternion.AngleAxis(90f, Vector3.forward);
        
        Vector3 contactX = Vector3.zero;
        Vector3 contactY = Vector3.zero;

        Vector3.OrthoNormalize(ref contactNormal, ref contactY, ref contactX);

        if (Vector3.Dot(contactX, -hyp.normalized) < 0f) contactX *= -1f;
        if (Vector3.Dot(contactY, -hyp.normalized) < 0f) contactY *= -1f;

        float zoneRadius = hyp.magnitude * 10f;
        Vector3 zoneScale = Vector3.one * zoneRadius / 5f; // half of ten

        Plane hypPlane = new Plane(hyp.normalized, centerPoint);

        // Plane contactPlaneZ = new Plane(contactNormal, centerPoint - zoneRadius * contactNormal);
        // Plane contactPlaneX = new Plane(contactX, centerPoint - zoneRadius * contactX);
        // Plane contactPlaneY = new Plane(contactY, centerPoint - zoneRadius * contactY);

        if (zPlaneObj == null) {

            // xPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // xPoint.name = "xpoint";
            // yPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // yPoint.name = "ypoint";
            // zPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // zPoint.name = "zpoint";

            // Material m3 = Resources.Load("Materials/Invisible") as Material;

            // lookPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // lookPoint.name = "lookPoint";
            // lookPoint.GetComponent<MeshRenderer>().material = m3;

            // xPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // yPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // zPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            // xPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // yPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // zPlaneObj.layer = LayerMask.NameToLayer("Trajectory");


            // xPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // yPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // zPlaneObj.GetComponent<MeshRenderer>().material = m3;

        }

        // xPlaneObj.transform.position = centerPoint - zoneRadius * contactX;
        // yPlaneObj.transform.position = centerPoint - zoneRadius * contactY;
        // zPlaneObj.transform.position = centerPoint - zoneRadius * contactNormal;

        // xPlaneObj.transform.LookAt(centerPoint);
        // zPlaneObj.transform.LookAt(centerPoint);
        // yPlaneObj.transform.rotation = zPlaneObj.transform.rotation;

        // xPlaneObj.transform.Rotate(90f, 0f, 0f);
        // zPlaneObj.transform.Rotate(90f, 0f, 0f);
        // yPlaneObj.transform.Rotate(180f, 0f, 0f);

        // xPlaneObj.transform.localScale = zoneScale;
        // yPlaneObj.transform.localScale = zoneScale;
        // zPlaneObj.transform.localScale = zoneScale;

        // float zd;
        // float xd;
        // float yd;

        // Ray ray = new Ray(ctx.cam.transform.position, ctx.viewForward);
        // RaycastHit hit;

        // Physics.SphereCast(ray, 1f + (initialVelocityMag / ctx.moveConfig.runSpeed), out hit, 5000f, LayerMask.GetMask (new string[] { "Trajectory" }));
        
        // contactPlaneZ.Raycast(ray, out zd);
        // contactPlaneX.Raycast(ray, out xd);
        // contactPlaneY.Raycast(ray, out yd);

        // xPoint.transform.position = ctx.moveData.origin + ctx.viewForward * xd;
        // yPoint.transform.position = ctx.moveData.origin + ctx.viewForward * yd;
        // zPoint.transform.position = ctx.moveData.origin + ctx.viewForward * zd;

        // Vector3 zoneTrajectoryPoint = ctx.cam.transform.position + ctx.viewForward * hit.distance;
        // lookPoint.transform.position = zoneTrajectoryPoint;

        // Vector3 avatarTrajectoryDir = (lookPoint.transform.position - ctx.moveData.origin).normalized;

        Vector3 influenceVel = (ctx.characterData.avatarLookForward).normalized  * (hyp.magnitude / 2f);
        Vector3 influenceVel2 = (initialVelocityDir + ctx.characterData.avatarLookForward).normalized  * (initialVelocityMag + 1f);

        Vector3 contactProjectedVel = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel);
        Vector3 contactProjectedVel2 = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel2);

        Vector3 hypeUp = Vector3.Cross(hyp.normalized, influenceVel2.normalized).normalized;

        Vector3 test = ProjectOnTwoPlanes(contactNormal, hyp.normalized, influenceVel);

        // Debug.Log(Vector3.ProjectOnPlane(contactProjectedVel, contactNormal) + " " + test);

        // Vector3 endTrajectory = zoneTrajectoryPoint - centerPoint + influenceVel2;

        // Plane trajectoryPlane = new Plane(ctx.moveData.origin, centerPoint, zoneTrajectoryPoint);
        // Vector3 trajectoryNormal = trajectoryPlane.normal;

        // Vector3 circularProjectedVel = Vector3.ProjectOnPlane(influenceVel, trajectoryNormal);

        

        controlPoints[0].transform.position = ctx.characterData.moveData.origin;
        controlPoints[1].transform.position = ctx.characterData.moveData.origin + contactProjectedVel2;
        
        controlPoints[2].transform.position = Vector3.Lerp(centerPoint, controlPoints[1].transform.position, lookingAtPoint / 2f + .1f) + contactOffset;
        controlPoints[3].transform.position = centerPoint;

        if (false) {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.Reflect(ctx.characterData.moveData.velocity, hypeUp), Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Lerp(lookingAtPoint, .4f, Time.deltaTime);
            contactOffset = Vector3.Lerp(contactOffset, Vector3.zero, Time.deltaTime);
        } else {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.zero, Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Clamp01(Vector3.Dot(ctx.characterData.avatarLookForward, hyp.normalized));
            contactOffset = Vector3.Reflect(contactProjectedVel, hyp.normalized);
        }

        // arrow.transform.rotation = Quaternion.LookRotation(-contactProjectedVel, contactNormal);

        totalDistance = ApproximateArcLength();
        // estimatedTime = totalDistance / Mathf.Max(initialVelocityMag, 15f);
        estimatedVelocity = Mathf.Max(initialVelocityMag, ctx.characterData.moveConfig.runSpeed);
        estimatedTime = totalDistance / estimatedVelocity;

        

        DrawCurve();

    }

    /*         controlPoints[0].transform.position = ctx.moveData.origin;
        controlPoints[1].transform.position = ctx.moveData.origin + contactProjectedVel2;
        
        controlPoints[2].transform.position = Vector3.Lerp(centerPoint + ctx.moveData.velocity, controlPoints[1].transform.position, lookingAtPoint / 2f + .1f) + Vector3.Reflect(contactProjectedVel, hyp.normalized);
        // controlPoints[2].transform.position = centerPoint + contactProjectedVel * 4f;
        controlPoints[3].transform.position = centerPoint + ctx.moveData.velocity;
*/

    public void FlyByArc(Vector3 contactNormal, Vector3 target) {

        var initialVelocityDir = ctx.characterData.moveData.velocity.normalized;
        initialVelocityMag = ctx.characterData.moveData.velocity.magnitude;

        Vector3 centerPoint = Vector3.Lerp(ctx.characterData.moveData.origin, target, .95f);
        Vector3 hyp = centerPoint - ctx.characterData.moveData.origin;


        // Quaternion YRotate = Quaternion.AngleAxis(90f, Vector3.up);
        // Quaternion XRotate = Quaternion.AngleAxis(90f, Vector3.right);
        // Quaternion ZRotate = Quaternion.AngleAxis(90f, Vector3.forward);
        
        Vector3 contactX = Vector3.zero;
        Vector3 contactY = Vector3.zero;

        Vector3.OrthoNormalize(ref contactNormal, ref contactY, ref contactX);

        if (Vector3.Dot(contactX, -hyp.normalized) < 0f) contactX *= -1f;
        if (Vector3.Dot(contactY, -hyp.normalized) < 0f) contactY *= -1f;

        float zoneRadius = hyp.magnitude * 10f;
        Vector3 zoneScale = Vector3.one * zoneRadius / 5f; // half of ten

        Plane hypPlane = new Plane(hyp.normalized, centerPoint);

        // Plane contactPlaneZ = new Plane(contactNormal, centerPoint - zoneRadius * contactNormal);
        // Plane contactPlaneX = new Plane(contactX, centerPoint - zoneRadius * contactX);
        // Plane contactPlaneY = new Plane(contactY, centerPoint - zoneRadius * contactY);

        if (zPlaneObj == null) {

            // xPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // xPoint.name = "xpoint";
            // yPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // yPoint.name = "ypoint";
            // zPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // zPoint.name = "zpoint";

            // Material m3 = Resources.Load("Materials/Invisible") as Material;

            // lookPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // lookPoint.name = "lookPoint";
            // lookPoint.GetComponent<MeshRenderer>().material = m3;

            // xPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // yPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // zPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            // xPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // yPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // zPlaneObj.layer = LayerMask.NameToLayer("Trajectory");


            // xPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // yPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // zPlaneObj.GetComponent<MeshRenderer>().material = m3;

        }

        // xPlaneObj.transform.position = centerPoint - zoneRadius * contactX;
        // yPlaneObj.transform.position = centerPoint - zoneRadius * contactY;
        // zPlaneObj.transform.position = centerPoint - zoneRadius * contactNormal;

        // xPlaneObj.transform.LookAt(centerPoint);
        // zPlaneObj.transform.LookAt(centerPoint);
        // yPlaneObj.transform.rotation = zPlaneObj.transform.rotation;

        // xPlaneObj.transform.Rotate(90f, 0f, 0f);
        // zPlaneObj.transform.Rotate(90f, 0f, 0f);
        // yPlaneObj.transform.Rotate(180f, 0f, 0f);

        // xPlaneObj.transform.localScale = zoneScale;
        // yPlaneObj.transform.localScale = zoneScale;
        // zPlaneObj.transform.localScale = zoneScale;

        // float zd;
        // float xd;
        // float yd;

        // Ray ray = new Ray(ctx.cam.transform.position, ctx.viewForward);
        // RaycastHit hit;

        // Physics.SphereCast(ray, 1f + (initialVelocityMag / ctx.moveConfig.runSpeed), out hit, 5000f, LayerMask.GetMask (new string[] { "Trajectory" }));
        
        // contactPlaneZ.Raycast(ray, out zd);
        // contactPlaneX.Raycast(ray, out xd);
        // contactPlaneY.Raycast(ray, out yd);

        // xPoint.transform.position = ctx.moveData.origin + ctx.viewForward * xd;
        // yPoint.transform.position = ctx.moveData.origin + ctx.viewForward * yd;
        // zPoint.transform.position = ctx.moveData.origin + ctx.viewForward * zd;

        // Vector3 zoneTrajectoryPoint = ctx.cam.transform.position + ctx.viewForward * hit.distance;
        // lookPoint.transform.position = zoneTrajectoryPoint;

        // Vector3 avatarTrajectoryDir = (lookPoint.transform.position - ctx.moveData.origin).normalized;

        Vector3 influenceVel = (ctx.characterData.avatarLookForward).normalized  * (hyp.magnitude / 2f);
        Vector3 influenceVel2 = (initialVelocityDir + ctx.characterData.avatarLookForward).normalized  * (initialVelocityMag + 1f);

        Vector3 contactProjectedVel = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel);
        Vector3 contactProjectedVel2 = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel2);

        Vector3 hypeUp = Vector3.Cross(hyp.normalized, influenceVel2.normalized).normalized;

        Vector3 test = ProjectOnTwoPlanes(contactNormal, hyp.normalized, influenceVel);

        // Debug.Log(Vector3.ProjectOnPlane(contactProjectedVel, contactNormal) + " " + test);

        // Vector3 endTrajectory = zoneTrajectoryPoint - centerPoint + influenceVel2;

        // Plane trajectoryPlane = new Plane(ctx.moveData.origin, centerPoint, zoneTrajectoryPoint);
        // Vector3 trajectoryNormal = trajectoryPlane.normal;

        // Vector3 circularProjectedVel = Vector3.ProjectOnPlane(influenceVel, trajectoryNormal);

        

        controlPoints[0].transform.position = ctx.characterData.moveData.origin;
        controlPoints[1].transform.position = Vector3.Lerp(ctx.characterData.moveData.origin, centerPoint, .33f) + contactProjectedVel * .33f;
        
        controlPoints[2].transform.position = Vector3.Lerp(ctx.characterData.moveData.origin, centerPoint, .66f) + contactProjectedVel * .66f;
        controlPoints[3].transform.position = centerPoint  + contactProjectedVel;

        if (false) {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.Reflect(ctx.characterData.moveData.velocity, hypeUp), Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Lerp(lookingAtPoint, .4f, Time.deltaTime);
            contactOffset = Vector3.Lerp(contactOffset, Vector3.zero, Time.deltaTime);
        } else {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.zero, Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Clamp01(Vector3.Dot(ctx.characterData.avatarLookForward, hyp.normalized));
            contactOffset = Vector3.Reflect(contactProjectedVel, hyp.normalized);
        }

        // controlPoints[3].transform.rotation = Quaternion.LookRotation(-contactProjectedVel, contactNormal);

        totalDistance = ApproximateArcLength();
        // estimatedTime = totalDistance / Mathf.Max(initialVelocityMag, 15f);
        estimatedVelocity = Mathf.Max(initialVelocityMag, ctx.characterData.moveConfig.runSpeed - 5f);
        estimatedTime = totalDistance / estimatedVelocity;

        DrawCurve();

    }

    public void ReachAroundArc(Vector3 contactNormal, Vector3 target) {

        var initialVelocityDir = ctx.characterData.moveData.velocity.normalized;
        initialVelocityMag = ctx.characterData.moveData.velocity.magnitude;

        Vector3 centerPoint = Vector3.Lerp(ctx.characterData.moveData.origin, target, .95f);
        Vector3 hyp = centerPoint - ctx.characterData.moveData.origin;


        // Quaternion YRotate = Quaternion.AngleAxis(90f, Vector3.up);
        // Quaternion XRotate = Quaternion.AngleAxis(90f, Vector3.right);
        // Quaternion ZRotate = Quaternion.AngleAxis(90f, Vector3.forward);
        
        Vector3 contactX = Vector3.zero;
        Vector3 contactY = Vector3.zero;

        Vector3.OrthoNormalize(ref contactNormal, ref contactY, ref contactX);

        if (Vector3.Dot(contactX, -hyp.normalized) < 0f) contactX *= -1f;
        if (Vector3.Dot(contactY, -hyp.normalized) < 0f) contactY *= -1f;

        float zoneRadius = hyp.magnitude * 10f;
        Vector3 zoneScale = Vector3.one * zoneRadius / 5f; // half of ten

        Plane hypPlane = new Plane(hyp.normalized, centerPoint);

        // Plane contactPlaneZ = new Plane(contactNormal, centerPoint - zoneRadius * contactNormal);
        // Plane contactPlaneX = new Plane(contactX, centerPoint - zoneRadius * contactX);
        // Plane contactPlaneY = new Plane(contactY, centerPoint - zoneRadius * contactY);

        if (zPlaneObj == null) {

            // xPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // xPoint.name = "xpoint";
            // yPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // yPoint.name = "ypoint";
            // zPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // zPoint.name = "zpoint";

            // Material m3 = Resources.Load("Materials/Invisible") as Material;

            // lookPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // lookPoint.name = "lookPoint";
            // lookPoint.GetComponent<MeshRenderer>().material = m3;

            // xPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // yPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // zPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            // xPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // yPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            // zPlaneObj.layer = LayerMask.NameToLayer("Trajectory");


            // xPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // yPlaneObj.GetComponent<MeshRenderer>().material = m3;
            // zPlaneObj.GetComponent<MeshRenderer>().material = m3;

        }

        // xPlaneObj.transform.position = centerPoint - zoneRadius * contactX;
        // yPlaneObj.transform.position = centerPoint - zoneRadius * contactY;
        // zPlaneObj.transform.position = centerPoint - zoneRadius * contactNormal;

        // xPlaneObj.transform.LookAt(centerPoint);
        // zPlaneObj.transform.LookAt(centerPoint);
        // yPlaneObj.transform.rotation = zPlaneObj.transform.rotation;

        // xPlaneObj.transform.Rotate(90f, 0f, 0f);
        // zPlaneObj.transform.Rotate(90f, 0f, 0f);
        // yPlaneObj.transform.Rotate(180f, 0f, 0f);

        // xPlaneObj.transform.localScale = zoneScale;
        // yPlaneObj.transform.localScale = zoneScale;
        // zPlaneObj.transform.localScale = zoneScale;

        // float zd;
        // float xd;
        // float yd;

        // Ray ray = new Ray(ctx.cam.transform.position, ctx.viewForward);
        // RaycastHit hit;

        // Physics.SphereCast(ray, 1f + (initialVelocityMag / ctx.moveConfig.runSpeed), out hit, 5000f, LayerMask.GetMask (new string[] { "Trajectory" }));
        
        // contactPlaneZ.Raycast(ray, out zd);
        // contactPlaneX.Raycast(ray, out xd);
        // contactPlaneY.Raycast(ray, out yd);

        // xPoint.transform.position = ctx.moveData.origin + ctx.viewForward * xd;
        // yPoint.transform.position = ctx.moveData.origin + ctx.viewForward * yd;
        // zPoint.transform.position = ctx.moveData.origin + ctx.viewForward * zd;

        // Vector3 zoneTrajectoryPoint = ctx.cam.transform.position + ctx.viewForward * hit.distance;
        // lookPoint.transform.position = zoneTrajectoryPoint;

        // Vector3 avatarTrajectoryDir = (lookPoint.transform.position - ctx.moveData.origin).normalized;

        Vector3 influenceVel = (ctx.characterData.avatarLookForward).normalized  * (hyp.magnitude / 2f);
        Vector3 influenceVel2 = (initialVelocityDir + ctx.characterData.avatarLookForward).normalized  * (initialVelocityMag + 1f);

        Vector3 contactProjectedVel = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel);
        Vector3 contactProjectedVel2 = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel2);

        Vector3 hypeUp = Vector3.Cross(hyp.normalized, influenceVel2.normalized).normalized;

        Vector3 test = ProjectOnTwoPlanes(contactNormal, hyp.normalized, influenceVel);

        // Debug.Log(Vector3.ProjectOnPlane(contactProjectedVel, contactNormal) + " " + test);

        // Vector3 endTrajectory = zoneTrajectoryPoint - centerPoint + influenceVel2;

        // Plane trajectoryPlane = new Plane(ctx.moveData.origin, centerPoint, zoneTrajectoryPoint);
        // Vector3 trajectoryNormal = trajectoryPlane.normal;

        // Vector3 circularProjectedVel = Vector3.ProjectOnPlane(influenceVel, trajectoryNormal);

        

        controlPoints[0].transform.position = ctx.characterData.moveData.origin;
        controlPoints[1].transform.position = ctx.characterData.moveData.origin + contactProjectedVel + (1f - lookingAtPoint) * hyp.normalized / 2f;
        
        controlPoints[2].transform.position = centerPoint + contactOffset + (1f - lookingAtPoint) * hyp.normalized;
        controlPoints[3].transform.position = centerPoint + contactOffset / 4f + (1f - lookingAtPoint) * hyp.normalized * 2f;

        if (false) {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.Reflect(ctx.characterData.moveData.velocity, hypeUp), Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Lerp(lookingAtPoint, .4f, Time.deltaTime);
            contactOffset = Vector3.Lerp(contactOffset, Vector3.zero, Time.deltaTime);
        } else {
            swingOffset = Vector3.Lerp(swingOffset, Vector3.zero, Time.deltaTime * 2f);
            lookingAtPoint = Mathf.Clamp01(Vector3.Dot(ctx.characterData.avatarLookForward, hyp.normalized));
            contactOffset = Vector3.Reflect(contactProjectedVel, hyp.normalized);
        }

        // controlPoints[3].transform.rotation = Quaternion.LookRotation(-contactProjectedVel, contactNormal);

        totalDistance = ApproximateArcLength();
        // estimatedTime = totalDistance / Mathf.Max(initialVelocityMag, 15f);
        estimatedVelocity = Mathf.Max(initialVelocityMag, ctx.characterData.moveConfig.runSpeed - 5f) + 5f;
        estimatedTime = totalDistance / estimatedVelocity;

        DrawCurve();

    }

    public void AttackArc(Vector3 contactNormal, Vector3 target) {

        var initialVelocityDir = ctx.characterData.moveData.velocity.normalized;
        initialVelocityMag = ctx.characterData.moveData.velocity.magnitude;

        Vector3 centerPoint = Vector3.Lerp(ctx.characterData.moveData.origin, target, .95f);
        Vector3 hyp = centerPoint - ctx.characterData.moveData.origin;

        float lookingAtPoint = Mathf.Clamp01(Vector3.Dot(ctx.characterData.avatarLookForward, hyp.normalized));

        // Quaternion YRotate = Quaternion.AngleAxis(90f, Vector3.up);
        // Quaternion XRotate = Quaternion.AngleAxis(90f, Vector3.right);
        // Quaternion ZRotate = Quaternion.AngleAxis(90f, Vector3.forward);
        
        Vector3 contactX = Vector3.zero;
        Vector3 contactY = Vector3.zero;

        Vector3.OrthoNormalize(ref contactNormal, ref contactY, ref contactX);

        if (Vector3.Dot(contactX, -hyp.normalized) < 0f) contactX *= -1f;
        if (Vector3.Dot(contactY, -hyp.normalized) < 0f) contactY *= -1f;

        float zoneRadius = hyp.magnitude * 10f;
        Vector3 zoneScale = Vector3.one * zoneRadius / 5f; // half of ten

        Plane hypPlane = new Plane(hyp.normalized, centerPoint);

        Plane contactPlaneZ = new Plane(contactNormal, centerPoint - zoneRadius * contactNormal);
        Plane contactPlaneX = new Plane(contactX, centerPoint - zoneRadius * contactX);
        Plane contactPlaneY = new Plane(contactY, centerPoint - zoneRadius * contactY);

        if (zPlaneObj == null) {

            // xPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // xPoint.name = "xpoint";
            // yPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // yPoint.name = "ypoint";
            // zPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // zPoint.name = "zpoint";

            lookPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lookPoint.name = "lookPoint";
            lookPoint.GetComponent<MeshRenderer>().material = Resources.Load("Materials/OrangeEmission") as Material;

            xPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            yPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            zPlaneObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            xPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            yPlaneObj.layer = LayerMask.NameToLayer("Trajectory");
            zPlaneObj.layer = LayerMask.NameToLayer("Trajectory");

            Material m3 = Resources.Load("Materials/Invisible") as Material;

            xPlaneObj.GetComponent<MeshRenderer>().material = m3;
            yPlaneObj.GetComponent<MeshRenderer>().material = m3;
            zPlaneObj.GetComponent<MeshRenderer>().material = m3;

        }

        xPlaneObj.transform.position = centerPoint - zoneRadius * contactX;
        yPlaneObj.transform.position = centerPoint - zoneRadius * contactY;
        zPlaneObj.transform.position = centerPoint - zoneRadius * contactNormal;

        xPlaneObj.transform.LookAt(centerPoint);
        zPlaneObj.transform.LookAt(centerPoint);
        yPlaneObj.transform.rotation = zPlaneObj.transform.rotation;

        xPlaneObj.transform.Rotate(90f, 0f, 0f);
        zPlaneObj.transform.Rotate(90f, 0f, 0f);
        yPlaneObj.transform.Rotate(180f, 0f, 0f);

        xPlaneObj.transform.localScale = zoneScale;
        yPlaneObj.transform.localScale = zoneScale;
        zPlaneObj.transform.localScale = zoneScale;

        float zd;
        float xd;
        float yd;

        Ray ray = new Ray(ctx.cameraController.GetComponent<Camera>().transform.position, ctx.characterData.viewForward);
        RaycastHit hit;

        Physics.SphereCast(ray, 1f + (initialVelocityMag / ctx.characterData.moveConfig.runSpeed), out hit, 5000f, LayerMask.GetMask (new string[] { "Ground", "Trajectory" }));
        
        contactPlaneZ.Raycast(ray, out zd);
        contactPlaneX.Raycast(ray, out xd);
        contactPlaneY.Raycast(ray, out yd);

        // xPoint.transform.position = ctx.moveData.origin + ctx.viewForward * xd;
        // yPoint.transform.position = ctx.moveData.origin + ctx.viewForward * yd;
        // zPoint.transform.position = ctx.moveData.origin + ctx.viewForward * zd;

        Vector3 zoneTrajectoryPoint = ctx.cameraController.GetComponent<Camera>().transform.position + ctx.characterData.avatarLookForward * hit.distance;
        lookPoint.transform.position = zoneTrajectoryPoint;

        // Vector3 influenceVel = (ctx.avatarLookForward).normalized  * (ctx.moveData.distanceFromTarget / 4f);
        Vector3 influenceVel = ctx.characterData.avatarLookForward;
        Vector3 influenceVel2 = (initialVelocityDir + ctx.characterData.avatarLookForward).normalized  * (initialVelocityMag + 1f);

        Vector3 contactProjectedVel = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel);
        Vector3 contactProjectedVel2 = ProjectOnTwoPlanes(hyp.normalized, contactNormal, influenceVel2);

        Vector3 endTrajectory = zoneTrajectoryPoint - centerPoint + influenceVel2;

        Plane trajectoryPlane = new Plane(ctx.characterData.moveData.origin, centerPoint, zoneTrajectoryPoint);
        Vector3 trajectoryNormal = trajectoryPlane.normal;

        Vector3 circularProjectedVel = Vector3.ProjectOnPlane(influenceVel, trajectoryNormal);

        controlPoints[0].transform.position = ctx.characterData.moveData.origin;
        controlPoints[1].transform.position = ctx.characterData.moveData.origin + contactProjectedVel2;

        controlPoints[2].transform.position = centerPoint + (1f - lookingAtPoint) * hyp.normalized * 2f - endTrajectory.normalized * (centerPoint - ctx.characterData.moveData.origin).magnitude / 2f;
        controlPoints[3].transform.position = centerPoint + contactProjectedVel; // all one equation

        arrow.transform.rotation = Quaternion.LookRotation(endTrajectory.normalized);
        
        arrow.transform.localScale = new Vector3(1f, 1f, 1f + (initialVelocityMag / ctx.characterData.moveConfig.runSpeed));
        arrow.transform.position = centerPoint + endTrajectory.normalized * arrow.transform.localScale.z * 4f;

        totalDistance = ApproximateArcLength();
        estimatedTime = 1f;
        estimatedVelocity = Mathf.Max(15f, initialVelocityMag);

        float future_t = 0f;
        Vector3 projectedOrigin = centerPoint + contactProjectedVel;
        Vector3 projectedVelocity = endTrajectory.normalized * estimatedVelocity;

        for (int i = 0; i < stepNumber; i++) {

            future_t += Time.smoothDeltaTime;
            projectedOrigin += projectedVelocity * Time.smoothDeltaTime;

            if (future_t >= .5f) {

                projectedVelocity.y += -ctx.characterData.moveConfig.gravity * Time.smoothDeltaTime;

            }

            projectedPoints[i] = projectedOrigin;


        }

        DrawCurve();
        // DrawProjection();
    }

    public void InterpolateAcrossCurveC1(float t) { // TODO:

        OrientedPoint op = GetBezierPoint( Mathf.Min(t / estimatedTime, .95f ));

        // Debug.Log(estimatedTime);

        ctx.characterData.moveData.velocity = (op.rot * Vector3.forward) * Mathf.Lerp(initialVelocityMag, estimatedVelocity, t / estimatedTime);
        // ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, (op.rot * Vector3.forward) * estimatedVelocity, Time.deltaTime * 50f);
        ctx.characterData.moveData.origin = op.pos;
    }

    public void InterpolateAcrossCurveC2(float t) { // TODO:

        OrientedPoint op = GetBezierPoint( Mathf.Min(t / estimatedTime, .95f ));

        // ctx.moveData.velocity = (op.rot * Vector3.forward) * Mathf.Lerp(initialVelocityMag, estimatedVelocity, t / estimatedTime);
        // ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, (op.rot * Vector3.forward) * estimatedVelocity, Time.deltaTime * 50f);
        ctx.characterData.moveData.velocity = (op.rot * Vector3.forward) * (estimatedVelocity);
        ctx.characterData.moveData.origin = Vector3.Lerp(ctx.characterData.moveData.origin, op.pos, Time.deltaTime * 2f);
    }

    private Vector3 ProjectOnTwoPlanes(Vector3 plane1, Vector3 plane2, Vector3 force) {
        return Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(force, plane1), plane2);
    }

    OrientedPoint GetBezierPoint( float t ) {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return new OrientedPoint(Vector3.Lerp(d, e, t), (e-d).normalized);
    }

    

    // public void SpiralCurve(float centerForce, Vector3 gTarget, Vector3 initialV, Vector3 targetV) {

    //     Vector3 projectedOrigin = ctx.moveData.origin;
    //     float distanceFromPoint = ctx.moveData.distanceFromPoint;
        
    //     Vector3 gTargetDir = (gTarget - ctx.moveData.origin).normalized;
    //     float targetDist = (gTarget - ctx.moveData.origin).magnitude;

    //     // Vector3 pTarget = 


    //     Vector3 velocityInTargetDir = Vector3.Dot(initialV, gTargetDir) * gTargetDir;
        
    //     Vector3 velocityOrthagonal = initialV - velocityInTargetDir;

    //     // float grappleVelocityMag = distanceFromPoint * centerForce;
    //     float grappleVelocityMag = 10f * centerForce;

    //     projectedVelocity = grappleVelocityMag * gTargetDir + velocityOrthagonal;
    //     // ctx.moveData.velocity = projectedVelocity;

    //     float future_t = 0f;

    //     for (int i = 0; i < stepNumber; i++) {
            
    //         if (Vector3.Distance(projectedOrigin, gTarget) < 2f) {
    //             projectedPoints[i] = projectedOrigin;
    //             continue;
    //         }

    //         projectedVelocity = grappleVelocityMag * gTargetDir + velocityOrthagonal;
    //         projectedOrigin = Vector3.Lerp(projectedOrigin, projectedOrigin + projectedVelocity, .1f);


    //         projectedPoints[i] = projectedOrigin;

    //         gTargetDir = (gTarget - projectedOrigin).normalized;
    //         distanceFromPoint = (gTarget - projectedOrigin).magnitude;
    //         velocityInTargetDir = Vector3.Dot(projectedVelocity, gTargetDir) * gTargetDir;
    //         velocityOrthagonal = projectedVelocity - velocityInTargetDir;
    //         future_t += Time.smoothDeltaTime;
    //         // grappleVelocityMag = distanceFromPoint * centerForce;
    //         // grappleVelocityMag = 2f;
            
    //     }

    // }

    public void CancelVelocityAgainst(Vector3 wishDir) {
        ctx.characterData.moveData.velocity += Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir) * wishDir;
    }

    Quaternion GetBezierOrientation( float t ) {

        Vector3 tangent = GetBezierTangent( t );

        return Quaternion.LookRotation(tangent);
    }

    Vector3 GetBezierTangent( float t ) {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return (e-d).normalized;
    }


}
