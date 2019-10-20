using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : MonoBehaviour {
    public KeyCode Key;
    public GameObject MissileCommand;
    public GameObject launchSite;
    public GameObject playerMissile;
    public GameObject firstMissilePosition;
    private PlayerMissile playerMissileScript;
    public bool BlownUp = false;
    Queue<GameObject> missilesOnScreen = new Queue<GameObject>();
    MissileCommand missileCommnandScript;

    // Start is called before the first frame update
    void Start() {
        missileCommnandScript = MissileCommand.GetComponent<MissileCommand>();
        playerMissileScript = playerMissile.GetComponent<PlayerMissile>();
        NewLevel();
    }

    public void NewLevel() {
        PlaceMissiles(firstMissilePosition.transform.position, firstMissilePosition.transform.rotation);
    }

    public void PlaceMissiles(Vector3 position, Quaternion rotation, int count = 1) {
        if (count <= 4) {
            var startPos = position;
            if (count == 1) {
                var missile = Instantiate(playerMissile, position, rotation);
                missilesOnScreen.Enqueue(missile);
                position += Vector3.down * playerMissileScript.Height;
                position += Vector3.left * playerMissileScript.Width / 2;
                PlaceMissiles(position, rotation, 2);
            } else {

                for (int i = 1; i <= count; i++) {
                    var missile = Instantiate(playerMissile, position, rotation);
                    missilesOnScreen.Enqueue(missile);
                    position += Vector3.right * playerMissileScript.Width;
                }

                startPos += Vector3.down * playerMissileScript.Height;
                startPos += Vector3.left * playerMissileScript.Width / 2;
                PlaceMissiles(startPos, rotation, count + 1);
            }
        }
    }

    public void BlowUp() {
        while (missilesOnScreen.Count > 0) {
            missileCommnandScript.MissileLaunched();
            var missile = missilesOnScreen.Dequeue();
            UnityEngine.Object.Destroy(missile);
        }
        BlownUp = true;
    }

    public bool Discard() {
        if (missilesOnScreen.Count == 0)
            return false;
        var missile = missilesOnScreen.Dequeue();
        UnityEngine.Object.Destroy(missile);
        return true;
    }

    public void Launch(TimeSpan time) {
        if (missilesOnScreen.Count > 0) {
            var missile = missilesOnScreen.Dequeue();
            var script = missile.GetComponent<PlayerMissile>();
            script.Launch(launchSite, missilesOnScreen.Count);
        }
    }
}
