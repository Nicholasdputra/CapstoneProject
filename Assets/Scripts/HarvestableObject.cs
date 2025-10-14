using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableObject : MonoBehaviour, IClickable
{
    [SerializeField] private int health;
    public int Health
    {
        get => health;
        set => health = Mathf.Max(0, value);
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Health = health;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            OnDestroy();
        }
    }

    public void OnClick()
    {
        PlayerData.instance.AddSoulEssences(1);
        Health -= 1;
        Debug.Log("Harvested! Current Health: " + Health);
    }

    public void OnHover()
    {

    }

    public void OnUnhover()
    {

    }
    
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}