using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

#if UNITY_EDITOR
[CustomEditor(typeof(GunManager))]
public class GunManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GunManager gunManager = (GunManager)target;  // anything can be an object
        if (GUILayout.Button("Prev"))
        {
            gunManager.PrevGun();
        }

        if (GUILayout.Button("Next"))
        {
            gunManager.NextGun();
        }

        if (GUILayout.Button("Use"))
        {
            gunManager.Shoot();
        }
    }
}
#endif

public class GunManager : MonoBehaviour
{
    public UnityEvent swapped;
    public List<Weapon> weapons;
    public Weapon weapon;
    private int weaponIndex = 0;

    public void Shoot()
    {
        if (weapon)
        {
            weapon.Use();
        }
            
    }

    public void PrevGun()
    {
        if (weapons.Count == 0)
            return;

        if (weaponIndex == 0)
            weaponIndex = weapons.Count;

        weaponIndex--;
        weapon = weapons[weaponIndex];
        weapon.equiped.Invoke();
        swapped.Invoke();
    }

    public void NextGun()
    {
        if (weapons.Count == 0)
            return;

        weaponIndex++;

        if (weaponIndex == weapons.Count)
            weaponIndex = 0;

        weapon = weapons[weaponIndex];
        swapped.Invoke();
    }
}