using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public GameObject currentItem;
    public List<BaseItem> playerInventory;
    private SpriteRenderer playerSprite;

    public bool isDead,isFacingRight,isHiding,damageable,hasYarn;
    public static float playerHealth;

    public void Update()
    {
        Debug.Log("Items Count:" + playerInventory.Count);

    }
}
