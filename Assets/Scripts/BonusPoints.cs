using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusPoints : MonoBehaviour {
    public GameObject MissileCommand;
    public GameObject[] MissileBases;
    public GameObject[] CitiesBonus;
    public GameObject[] Cities;
    public GameObject[] Missiles;
    public GameObject CityBonus;
    TextMeshProUGUI CityBonusText;
    public GameObject MissileBonus;
    TextMeshProUGUI MissileBonusText;
    GameObject Canvas;
    Coroutine BonusPointLoop;
    MissileCommand missileCommnandScript;

    // Start is called before the first frame update
    void Start() {
        // Cities = GameObject.FindGameObjectsWithTag("City");
        // Missiles = GameObject.FindGameObjectsWithTag("PM");
        missileCommnandScript = MissileCommand.GetComponent<MissileCommand>();
        CityBonusText = CityBonus.GetComponent<TextMeshProUGUI>();
        MissileBonusText = MissileBonus.GetComponent<TextMeshProUGUI>();
        Canvas = transform.GetChild(0).gameObject;
    }

    IEnumerator ShowPoints(int cityCount, int missileCount) {
        int score = 0;
        int missileBase = 0;
        CityBonusText.text = "0";
        MissileBonusText.text = "0";
        for (int i = 0; i < missileCount; i++) {
            yield return new WaitForSecondsRealtime(0.2f);
            for (; missileBase < 3; missileBase++) {
                var mb = MissileBases[missileBase].GetComponent<MissileBase>();
                if (mb.Discard())
                    break;
            }
            Missiles[i].SetActive(true);
            score += 10;
            MissileBonusText.text = score.ToString();
        }

        missileCommnandScript.AddScore(score);
        int city = 0;
        score = 0;
        for (int i = 0; i < cityCount; i++) {
            yield return new WaitForSecondsRealtime(0.2f);
            CitiesBonus[i].SetActive(true);
            score += 100;
            CityBonusText.text = score.ToString();
            for (; city < 6; city++) {
                if (Cities[city].activeSelf) {
                    var shader = Cities[city].GetComponent<SpriteRenderer>();
                    if (shader.enabled) {
                        shader.enabled = false;
                        break;
                    }
                }
            }
        }
        missileCommnandScript.AddScore(score);

        yield return new WaitForSecondsRealtime(2);
        Canvas.SetActive(false);
        BonusPointLoop = null;
        missileCommnandScript.NextLevel();

        for (int i = 0; i < 6; i++) {
            if (Cities[i].activeSelf) {
                var shader = Cities[i].GetComponent<SpriteRenderer>();
                shader.enabled = true;
            }
        }
    }

    public void Score(int cityCount, int missileCount) {
        for (int i = 0; i < Missiles.Length; i++)
            Missiles[i].SetActive(false);
        for (int i = 0; i < CitiesBonus.Length; i++)
            CitiesBonus[i].SetActive(false);
        Canvas.SetActive(true);
        BonusPointLoop = StartCoroutine(ShowPoints(cityCount, missileCount));
    }

    public bool Done() {
        return false;
    }
}
