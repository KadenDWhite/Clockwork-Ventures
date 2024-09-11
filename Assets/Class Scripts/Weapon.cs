using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent equiped;
    public UnityEvent reload;
    public virtual void Use() {}
}
