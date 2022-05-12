using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PlayerManager playerManager;
    public PlayerMazeManager playerMazeManager;

    public Camera normalCamera, arCamera;

    private void OnMouseDown()
    {
        if (FieldManager.instance)
            if (FieldManager.instance.isComplete || FieldManager.instance.isPaused) return;
        if (FieldMazeManager.instance)
            if (!FieldMazeManager.instance.isInit || FieldMazeManager.instance.isPaused) return;
        RaycastHit hit;
        Camera cam= null;

        if (arCamera.gameObject.activeInHierarchy) cam = arCamera;
        else cam = normalCamera;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (playerManager)
                playerManager.Spawn(hit.point);
            if (playerMazeManager)
            {
                print("ada kok");
                playerMazeManager.Spawn(hit.point);
            }
        }
    }
}
