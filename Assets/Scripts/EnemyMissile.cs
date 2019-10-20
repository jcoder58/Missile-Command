using System.Collections;
using System.Collections.Generic;
using Graphics;
using UnityEngine;
using static Graphics.SpacePointStatic;

public class EnemyMissile : MonoBehaviour {
    public GameObject MissileCommand;
    public Vector3 Offset = new Vector3(-0.5f, 0.5f, 0.0f);
    private EnemyLaunch Launcher;
    private float Velocity;
    private float Distance;
    private float Counter;
    private LineRenderer lineRenderer;
    private SpacePoint LaunchPoint;
    private SpacePoint TargetPoint;
    BoxCollider2D Collider;
    Rigidbody2D Rigidbody;
    GameObject Explosion;
    MissileCommand missileCommnandScript;

public static int MissileCount = 0;

    // Start is called before the first frame update
    void Start() {
        MissileCommand = GameObject.FindGameObjectWithTag("MainCamera");
        missileCommnandScript = MissileCommand.GetComponent<MissileCommand>();
        Velocity = missileCommnandScript.EnemyVelocity;
        Explosion = missileCommnandScript.Explosion;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
        Collider = GetComponent<BoxCollider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Launch(SpacePoint launchPoint, SpacePoint targetPoint) {
        Start();
        Debug.Log($"Missile {MissileCount}");
        Debug.Log($"Target {targetPoint}");
        MissileCount++;
        LaunchPoint = launchPoint.ToWorldSpace();
        TargetPoint = targetPoint.ToWorldSpace();
        Distance = Vector3.Distance(LaunchPoint.Position, TargetPoint.Position);
        Counter = 0;

        Vector3[] poses = new Vector3[3];
        var start = LaunchPoint.Position;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start);
        Debug.Log("LineRenderer");

        Rigidbody.MovePosition(start);

        lineRenderer.enabled = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (lineRenderer.enabled ) {
            Counter += 0.1f / Velocity;
            float delta = Mathf.Lerp(0, Distance, Counter);

            Vector3 pointAt = delta * Vector3.Normalize(TargetPoint.Position - LaunchPoint.Position) + LaunchPoint.Position;

            lineRenderer.SetPosition(1, pointAt);
            Rigidbody.position = pointAt;
        }
    }

    void FixedUpdate() {

    }

    public enum Layers {
        EnemyMissile = 8,
        Bases = 9,
        EnemyTarget = 10,
        Explosion = 11,
        City = 12
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch ((Layers) other.gameObject.layer) {
            case Layers.Bases:
                Rigidbody.velocity = new Vector2(0, 0);
                Collider.enabled = false;
                lineRenderer.enabled = false;
                var missileBase = other.gameObject.GetComponent<MissileBase>();
                missileBase.BlowUp();

                MissileCount--;
                Explode();
                break;
            case Layers.EnemyMissile:
                break;
            case Layers.EnemyTarget:
                Collider.enabled = false;
                Rigidbody.velocity = new Vector2(0, 0);
                lineRenderer.enabled = false;
                MissileCount--;
                Explode();
                break;
            case Layers.Explosion:
                Collider.enabled = false;
                MissileCount--;
                missileCommnandScript.MissileKilled();
                Object.Destroy(this.gameObject);
                break;
            case Layers.City:
                Collider.enabled = false;
                Rigidbody.velocity = new Vector2(0, 0);
                missileCommnandScript.CityDied();
                other.gameObject.SetActive(false);
                lineRenderer.enabled = false;
                MissileCount--;
                Explode();
                break;
            default:
                Debug.Log($"Unkown layer {other.gameObject.layer}");
                break;
        }

    }

    void Explode() {
        Explosion = Instantiate(Explosion, TargetPoint.Position + Offset, Quaternion.identity);
        var animation = Explosion.GetComponent<ExplosionAnimation>();
        animation.EnemyMissile = this;
    }
}
