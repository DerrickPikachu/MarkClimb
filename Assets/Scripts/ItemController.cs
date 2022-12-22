using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    SpeedUp,
    SpeedDown,
    JumpUp,
    GetHealth
}
public class ItemController : MonoBehaviour
{
    public GameObject player;
    public Material[] materials = new Material[4];
    private ItemType itemType;

    public void Init(ItemType itemType)
    {
        this.itemType = itemType;
        GetComponent<MeshRenderer>().material = materials[(int)itemType];
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            Destroy(gameObject);
            switch (itemType)
            {
                case ItemType.SpeedUp:
                    player.GetComponent<PlayerController>().AddEffect(EffectType.SpeedUp, 5);
                    break;
                case ItemType.SpeedDown:
                    player.GetComponent<PlayerController>().AddEffect(EffectType.SpeedDown, 5);
                    break;
                case ItemType.JumpUp:
                    player.GetComponent<PlayerController>().AddEffect(EffectType.JumpUp, 0.4f);
                    break;
                case ItemType.GetHealth:
                    player.GetComponent<HealthManager>().health += 3;
                    break;
            }
            Debug.Log(itemType);
            SoundManager.instance.PlaySound(SoundClip.GetItem);
            ParticleManager.instance.SpawnParticle(Particle.GetItem, player.transform.position);
        }
    }
}
