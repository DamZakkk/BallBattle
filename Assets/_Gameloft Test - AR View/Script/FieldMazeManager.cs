using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sound;

public class FieldMazeManager : MonoBehaviour
{
    public static FieldMazeManager instance;

    public Transform ball;
    public BoxCollider SpawnField;

    [Space]
    public TextMeshProUGUI completeText;
    public GameObject matchCompletePanel;
    public AudioClip cheerSound;

    [Space]
    public bool isInit;
    public bool isPaused;
    public bool isComplete;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnBall();
    }

    private void Update()
    {
    
}

    public void Complete(PlayerMazeManager pm)
    {
        if (isComplete) return;
        isComplete = true;

        completeText.text = pm.side + " Win";
        matchCompletePanel.SetActive(true);

        SoundManager.PlaySound(cheerSound, 0.17f);
    }

    private void SpawnBall()
    {
        BoxCollider _bc = SpawnField;
        Transform cubeTrans = _bc.GetComponent<Transform>();

        Vector3 cubeSize;
        Vector3 cubeCenter;
        cubeCenter = cubeTrans.position;
        cubeSize.x = cubeTrans.lossyScale.x * _bc.size.x;
        cubeSize.y = cubeTrans.lossyScale.y * _bc.size.y;
        cubeSize.z = cubeTrans.lossyScale.z * _bc.size.z;
        var rt = 0;

        while (true)
        {
            //rt++;
            //if(rt>= 20)
            //{
            //    print("ewror");
            //    return;
            //}
            Vector3 randomPosition = new Vector3(Random.Range(-cubeSize.x / 2, cubeSize.x / 2), 0.1855386f, Random.Range(-cubeSize.z / 2, cubeSize.z / 2));

            var checkResult = Physics.OverlapSphere(cubeCenter + randomPosition, 0.1f);
            if (checkResult.Length == 0)
            {
                ball.position = cubeCenter + randomPosition;
                isInit = true;
                break;
            }

            bool found = true;

            foreach (var col in checkResult)
            {
                if (col.CompareTag("Wall"))
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                ball.position = cubeCenter + randomPosition;
                isInit = true;
                break;
            }
        }

        isInit = true;
    }
}
