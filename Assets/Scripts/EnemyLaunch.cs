using System.Collections;
using System.Collections.Generic;
using Graphics;
using UnityEngine;

public class EnemyLaunch : MonoBehaviour {
    public GameObject MissileCommand;
    public GameObject EnemyTarget;
    public GameObject EnemyMissile;

    private Bounds LaunchBounds;
    private Bounds TargetBounds;

    public int MissilesToLaunch = 8;
    public int LastMissilesToLaunch = 0;
    public float LastLaunchTime = 0f;
    public float DelayTime = 2.0f;

    public int levelCount = 1;
    private float Z;

    // Start is called before the first frame update
    void Start() {
        var missileCommnandScript = MissileCommand.GetComponent<MissileCommand>();
        Z = missileCommnandScript.Z;
        LaunchBounds = GetComponent<MeshCollider>().bounds;
        TargetBounds = EnemyTarget.GetComponent<MeshCollider>().bounds;
    }

    void LaunchMissiles() {
        for (int i = 0; i < 4; i++) {
            var missile = Instantiate(EnemyMissile);
            //missile.SetActive(true);
            var missileScript = missile.GetComponent<EnemyMissile>();
            var launchX = Random.Range(LaunchBounds.min.x, LaunchBounds.max.x);
            var targetX = Random.Range(TargetBounds.min.x, TargetBounds.max.x);
            var launchPos = new SpacePoint(Graphics.Space.World, new Vector3(launchX, LaunchBounds.center.y, Z));
            var targetPos = new SpacePoint(Graphics.Space.World, new Vector3(targetX, TargetBounds.center.y, Z));
            missileScript.Launch(launchPos, targetPos);
            MissilesToLaunch--;
        }
    }

    // Update is called once per frame
    void Update() {
        if (LastMissilesToLaunch == 0)
            LastMissilesToLaunch = MissilesToLaunch;
        if (MissilesToLaunch > 0 && Time.time - LastLaunchTime >= DelayTime) {
            LaunchMissiles();
            LastLaunchTime = Time.time;
        }
    }
}
