using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Side side;

    [Header("Stats")]
    public float maxEnergy;
    public float currentEnergy;

    [Space]
    public float energyRegen;
    public float energyCost;
    public float spawnTime;
    public float reactivateTime;
    public float normalSpeed;
    public float carryingSpeed;
    public float ballSpeed;
    public float returnSpeed;
    public float detectionRange;
    public float detectionRangeDistance()
    {
        if (!field) return 0;
        else
        {
            var dist = field.bounds.size.x * detectionRange / 100;
            return dist;
        }
    }


    public Soldier playerPrefab;
    public Transform playerParents;

    public List<Soldier> soldierListAll;
    public List<Soldier> soldierList;
    public Soldier ballCarrier;
    public Ball ball;
    public Transform gate;
    public MeshRenderer field;


    public Material activeMat, inactiveMat;
    public bool isPlaying;
    public PlayerManager EnemyManager;

    private void Start()
    {
        //ResetGame();
    }

    public void ResetGame()
    {
        ball = FieldManager.instance.ball.GetComponent<Ball>();
        isPlaying = true;
        foreach (var item in soldierListAll)
        {
            if (item)
                Destroy(item.gameObject);
        }
        soldierList = new List<Soldier>();
        soldierListAll = new List<Soldier>();
        currentEnergy = 0;
    }

    private void Update()
    {
        currentEnergy += Time.deltaTime * energyRegen;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        int _side = ((side == Side.ATTACKER && GameManager.instance.round % 2 == 0) || (side == Side.DEFENDER && GameManager.instance.round % 2 != 0) ? 0 : 1);
        FieldManager.instance.playerUIs[_side].energyBarFill.fillAmount = currentEnergy / maxEnergy;

        var ep = maxEnergy / 6;
        for (int i = 0; i < 6; i++)
        {
            if (currentEnergy > ep * (i + 1))
                FieldManager.instance.playerUIs[_side].energyFillHighLights[i].SetActive(true);
            else FieldManager.instance.playerUIs[_side].energyFillHighLights[i].SetActive(false);
        }

    }

    public void Spawn(Vector3 hitPos)
    {
        if (currentEnergy >= energyCost)
        {
            currentEnergy -= energyCost;
            Soldier player = Instantiate(playerPrefab, playerParents);
            player.transform.position = new Vector3(hitPos.x, 0, hitPos.z);
            player.transform.eulerAngles = side == Side.ATTACKER ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
            player.Activate(this);
            soldierList.Add(player);
            soldierListAll.Add(player);
        }
    }

    public void DeactiveSoldier(Soldier _soldier)
    {
        soldierList.Remove(_soldier);
        if (soldierList.Count == 0)
        {
            FieldManager.instance.Complete(EnemyManager);
        }
        else
        if (_soldier == ballCarrier) PassBall();
    }

    public void PassBall()
    {
        Soldier closestSoldier = null;
        float dist = 10000;
        foreach (var _s in soldierList)
        {
            float _dist = Vector3.Distance(ball.transform.position, _s.transform.position);
            if (_dist < dist)
                closestSoldier = _s;
        }
        ball.transform.parent = playerParents;
        var lookPos = closestSoldier.transform.position - ball.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        ball.transform.rotation = rotation;

        ballCarrier = null;
        ball.passing = true;
    }
}



public enum Side
{
    ATTACKER, DEFENDER
}