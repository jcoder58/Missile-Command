using System;
using System.Collections.Generic;
using Graphics;
using Rx;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using static Rx.RxStatic;
using static Graphics.GraphicsObjects;
using static Graphics.SpacePointStatic;

public enum GameState {
    Starting,
    PlayStartSound,
    Play,
    EndOfLevel,
    Scoring,
    NextLevel,
    EndOfGame
}

//TODO: Add more sound effects
//TODO: Add Top 10 list
//TODO: Add Start button
//TODO: BUG: Missiles not removed when Explosion

public class MissileCommand : MonoBehaviour {
    public GameObject LeftMissileBase;
    public GameObject CenterMissileBase;
    public GameObject RightMissileBase;
    public GameObject EnemyLaunch;
    public GameObject Explosion;
    public GameObject BonusPoints;
    public GameObject TheEnd;
    public float EnemyVelocity = 0.0017f;
    public float PlayerVelocity = 0.015f;
    public float ExplosionVelocity = 0.1f;
    public float Z = -0.2f;
    //public float ExplosionScaleGrow = 5e-6f;
    public float ToLow = -4.2f;
    GameState State = GameState.Starting;
    int Score = 0;
    int CityCount = 6;
    int MissileCount = 30;
    bool ScoreChanged = true;
    TextMeshProUGUI ScoreText;
    BonusPoints BonusUI;

    void Start() {
        int throttleTime = 150;

        var scoreGameObject = GameObject.Find("Score");
        ScoreText = scoreGameObject.GetComponent<TextMeshProUGUI>();
        BonusUI = BonusPoints.GetComponent<BonusPoints>();

        Physics2D.IgnoreLayerCollision(8, 8, true);
        var x = EnemyLaunch.GetComponent<MeshCollider>();
        var bounds = x.bounds;
        Func<TimeSpan, bool> lowerLimt =
            _ => MousePosition.ToWorldSpace().Position.y > ToLow;
        var left = LeftMissileBase.GetComponent<MissileBase>();
        OnKey(left.Key).Throttle(throttleTime).Where(lowerLimt).Sink(left.Launch);
        var center = CenterMissileBase.GetComponent<MissileBase>();
        OnKey(center.Key).Throttle(throttleTime).Where(lowerLimt).Sink(center.Launch);
        var right = RightMissileBase.GetComponent<MissileBase>();
        OnKey(right.Key).Throttle(throttleTime).Where(lowerLimt).Sink(right.Launch);

    }

    public void MissileKilled() {
        Score += 25;
        ScoreChanged = true;
    }

    public void CityDied() {
        CityCount--;
    }

    public void BaseDied(int missilesLeft) {
        MissileCount -= missilesLeft;
    }

    public void MissileLaunched() {
        MissileCount--;
    }

    public void AddScore(int score) {
        ScoreChanged = true;
        Score += score;
    }

    void ShowScore() {
        ScoreText.SetText("{0}", Score);
        ScoreChanged = false;

    }

    public void NextLevel() {
        State = GameState.NextLevel;
    }

    void SetupNewLevel() {
        var left = LeftMissileBase.GetComponent<MissileBase>();
        left.NewLevel();
        var center = CenterMissileBase.GetComponent<MissileBase>();
        center.NewLevel();
        var right = RightMissileBase.GetComponent<MissileBase>();
        right.NewLevel();
        var el = EnemyLaunch.GetComponent<EnemyLaunch>();
        el.MissilesToLaunch = el.LastMissilesToLaunch + 3;
        el.LastLaunchTime = 0;
        el.LastMissilesToLaunch = 0;
    }

    // Update is called once per frame
    void Update() {
        var totalCount = PlayerMissile.MissileCount + EnemyMissile.MissileCount + ExplosionAnimation.ExplosionCount;
        if (ScoreChanged)
            ShowScore();
        switch (State) {
            case GameState.Starting:
                if (totalCount > 0)
                    State = GameState.PlayStartSound;
                break;
            case GameState.PlayStartSound:
                var startSound = gameObject.GetComponent<AudioSource>();
                startSound.Play();
                State = GameState.Play;
                break;
            case GameState.Play:
                if (totalCount == 0)
                    State = GameState.EndOfLevel;
                break;
            case GameState.EndOfLevel:
                if (CityCount > 0) {
                    BonusUI.Score(CityCount, MissileCount);
                    State = GameState.Scoring;
                } else {
                    State = GameState.EndOfGame;
                }
                break;
            case GameState.Scoring:
                break;
            case GameState.NextLevel:
                SetupNewLevel();
                State = GameState.Starting;
                break;
            case GameState.EndOfGame:
                if (!TheEnd.activeSelf)
                    TheEnd.SetActive(true);
                break;
            default:
                throw new NotImplementedException();
        }
    }

}
