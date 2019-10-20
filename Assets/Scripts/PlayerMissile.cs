using System;
using System.Collections;
using System.Collections.Generic;
using Graphics;
using UnityEngine;
using static Graphics.GraphicsObjects;
using static Graphics.SpacePointStatic;

public enum PlayerMissileState {
    None,
    Traveling,
    Explosion,
    Dexplosion
}

public class PlayerMissile : MonoBehaviour {
    public GameObject TargetCross1;
    public GameObject TargetCross2;
    public GameObject MissileCommand;
    public float Width = 0.3f;
    public float Height = 0.3f;
    public float ExpStart = 0.01f;
    SpacePoint TargetPoint;
    SpacePoint LaunchPoint;
    Vector2 Delta;
    LineRenderer lineRenderer;
    GameObject Explosion;
    IEnumerator<Vector2> tweener;
    IEnumerator<float> expTweener;
    private DebugGameScript DebugScript;
    MissileCommand missileCommnandScript;

    private float Z;
    private float Velocity;
    private float ExpVelocity;
    public int skipCount = 0;
    private PlayerMissileState State = PlayerMissileState.None;

    public static int MissileCount = 0;

    // Start is called before the first frame update
    void Start() {
        missileCommnandScript = MissileCommand.GetComponent<MissileCommand>();
        Z = missileCommnandScript.Z;
        //ExplosionScaleGrow = missileCommnandScript.ExplosionScaleGrow;
        Explosion = missileCommnandScript.Explosion;
        Velocity = missileCommnandScript.PlayerVelocity;
        ExpVelocity = missileCommnandScript.ExplosionVelocity;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
    }

    public void Launch(GameObject launchSite, int count) {
        // DebugScript = DebugCollider.GetComponent<DebugGameScript>();
        // DebugScript.Collider = Collider;
        MissileCount++;
        missileCommnandScript.MissileLaunched();
        TargetPoint = MousePosition.ToWorldSpace().ChangeZ(Z);
        MarkTarget(TargetPoint);

        var shape2d = gameObject.GetComponent<Shapes2D.Shape>();
        //shape2d.enabled = false;
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        Vector3[] poses1 = new Vector3[2];
        lineRenderer.GetPositions(poses1);

        var pos = launchSite.transform.position;
        pos.z = Z;
        LaunchPoint = new SpacePoint(Graphics.Space.World, pos);
        poses1[0] = poses1[1] = LaunchPoint.Position;
        lineRenderer.SetPositions(poses1);
        Delta = Delta(LaunchPoint, TargetPoint);
        lineRenderer.enabled = true;
        tweener = Tweener2d.LinearTweener(LaunchPoint.Position, TargetPoint.Position, Velocity).GetEnumerator();
        State = PlayerMissileState.Traveling;
    }

    private void MarkTarget(SpacePoint targetPoint) {
        var worldPostion = targetPoint.Position;
        worldPostion.z = 0;

        TargetCross1 = Instantiate(TargetCross1, worldPostion, TargetCross1.transform.rotation);
        TargetCross2 = Instantiate(TargetCross2, worldPostion, TargetCross1.transform.rotation);

        var lr1 = TargetCross1.GetComponent<LineRenderer>();
        var lr2 = TargetCross2.GetComponent<LineRenderer>();
        Vector3[] poses1 = new Vector3[2];
        var pos = lr1.GetPositions(poses1);
        poses1[0] += worldPostion;
        poses1[1] += worldPostion;
        lr1.SetPositions(poses1);
        lr1.enabled = true;

        Vector3[] poses2 = new Vector3[2];
        pos = lr2.GetPositions(poses2);
        poses2[0] += worldPostion;
        poses2[1] += worldPostion;
        lr2.SetPositions(poses2);
        lr2.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        Vector3[] poses1 = new Vector3[2];
        lineRenderer.GetPositions(poses1);
        switch (State) {
            case PlayerMissileState.None:
            default:
                break;
            case PlayerMissileState.Traveling:
                if (tweener.MoveNext()) {
                    poses1[1] = tweener.Current;
                    lineRenderer.SetPositions(poses1);
                } else {
                    lineRenderer.enabled = false;

                    TargetCross1.GetComponent<LineRenderer>().enabled = false;
                    TargetCross2.GetComponent<LineRenderer>().enabled = false;

                    Explosion = Instantiate(Explosion, TargetCross1.transform.position, TargetCross1.transform.rotation);
                    var animation = Explosion.GetComponent<ExplosionAnimation>();
                    animation.PlayerMissile = this;
                    State = PlayerMissileState.None;
                    MissileCount--;
                }
                break;
        }
    }
}
