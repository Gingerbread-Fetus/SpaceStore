using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ThrowAbility")]
public class ThrowInAbilityScript : Ability
{
    [SerializeField] List<GameObject> paths;
    [SerializeField] AbilityInventoryHandler inventoryDisplay;

    TextMeshProUGUI hitOrMissText;
    GameObject path;
    GameObject canvas;
    GameObject inventoryCanvas;
    SpritePathMovement movementComponent;

    public override void Initialize(GameObject obj)
    {
        //Select item from inventory.

        //Set this to the active item to be 'thrown'

        //Disable the haggling canvas
        HagglingManager hagglingManager = FindObjectOfType<HagglingManager>();
        hagglingManager.HideCanvas();

        //Display mini-game canvas
        canvas = Instantiate(aMinigameCanvas, null,true);
        canvas.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        
        //Create and display inventory canvas to select item
        inventoryCanvas = Instantiate(inventoryDisplay.gameObject, null, true);
        inventoryCanvas.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        
        //Choose a path and instantiate it.
        path = ChooseRandomPath();
        movementComponent = canvas.GetComponentInChildren<SpritePathMovement>();
        
        GameObject pathobject = Instantiate(path, canvas.transform, true);
        pathobject.transform.localPosition = new Vector3();

        inventoryDisplay.MiniGameObject = canvas;
        movementComponent.SetWaypoints(pathobject);
        movementComponent.inventoryHandler = inventoryDisplay;
        //Hiding the mini-game canvas so that the inventory handler shows on top
        canvas.SetActive(false);
    }

    public override void TriggerAbility()
    {
        //movementComponent.isMoving = true;
    }

    public void Cleanup()
    {

    }

    private GameObject ChooseRandomPath()
    {
        return paths[Random.Range(0, paths.Count)];
    }
}
