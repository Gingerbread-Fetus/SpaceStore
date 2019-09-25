using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] SpriteAtlas itemsAtlas;

    SpriteRenderer spriteHolder;
    Sprite[] tileSet;
    // Start is called before the first frame update
    void Start()
    {
        tileSet = new Sprite[itemsAtlas.spriteCount];
        //Use the sprite atlas to get the sprite array
        itemsAtlas.GetSprites(tileSet);
        //Get renderer sprite is set on
        spriteHolder = GetComponent<SpriteRenderer>();
        Debug.Log("Tileset loaded: " + tileSet.Length);
    }

    public void ChangeItem()
    {
        //Get a random number in range of the length of the array
        int spriteIndex = Random.Range(0, tileSet.Length + 1);
        Debug.Log("Index used: " + spriteIndex);
        spriteHolder.sprite = tileSet[spriteIndex];
    }
}
