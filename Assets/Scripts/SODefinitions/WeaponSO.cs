using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Projectile controls")]
    [SerializeField] public Vector2 Size;
    [SerializeField] [Range(2f, 10f)] public float Lifespan;

    [Header("Pattern controls")] 
    [SerializeField] [Range(1f, 5f)] public float MinSpeed;
    [SerializeField] [Range(5f, 10f)] public float MaxSpeed;
    [SerializeField] [Range(1f, 2f)] public float MinForce;
    [SerializeField] [Range(2f, 8f)] public float MaxForce;
    [SerializeField] [Range(1f, 7f)] public float MaxDistance;
    [SerializeField] [Range(0.2f, 1f)] public float FireRate;
    [SerializeField] [Range(1, 20)] public int ProjectilesInOneShot;
    [SerializeField] public bool FlipY;
    [SerializeField] [Range(math.SQRT2, 2f)] public float ReflectiveCircleRadios;
    
    public delegate void UpdateDelegate();
    public UpdateDelegate UpdateParamsEvent;
    
    private void InitializeParams()
    {
        Size = new Vector2(0.03f, 0.18f);
        Lifespan = 5f;
        MinSpeed = 3f;
        MaxSpeed = 6f;
        MinForce = 1f;
        MaxForce = 4f;
        MaxDistance = 3f;
        FireRate = 1f;
        FlipY = true;
        ProjectilesInOneShot = 10;
        ReflectiveCircleRadios = 1.5f;
    }

    public void OnEnable() {
        InitializeParams();
    }

    private void OnValidate() {
        UpdateParamsEvent?.Invoke();
    }
    


}
