using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour {
    public PlayerMissile PlayerMissile = null;
    public EnemyMissile EnemyMissile = null;

    public static int ExplosionCount = 0;

    void Start() {
        ExplosionCount++;
    }

    public void OnAnimationEnd() {
        if (PlayerMissile != null)
            Object.Destroy(PlayerMissile);
        if (EnemyMissile != null)
            Object.Destroy(EnemyMissile);
        ExplosionCount--;
        Object.Destroy(gameObject);
    }
}
