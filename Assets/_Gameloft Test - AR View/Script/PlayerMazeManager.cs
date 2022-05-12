using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMazeManager : MonoBehaviour
{
    bool spawned;

    public GameObject playerPrefab;
    public Transform playerParents;
    public string side;

    public void Spawn(Vector3 hitPos)
    {
        if (FieldMazeManager.instance.isInit && !spawned)
        {
            spawned = true;
            GameObject player = Instantiate(playerPrefab, playerParents);
            player.transform.position = new Vector3(hitPos.x, 0, hitPos.z);
            player.gameObject.SetActive(true);
            player.GetComponent<PathFindingSoldier>().manager = this;
            //player.Activate(this);
        }
    }

}
