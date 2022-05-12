using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    public Soldier lastCollide;
    public abstract void Activate(PlayerManager manager);
}
