using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ThrowAbility")]
public class ThrowInAbilityScript : Ability
{
    [SerializeField] List<GameObject> paths;
    [SerializeField] GameObject inventoryDisplayPrefab;

    TextMeshProUGUI hitOrMissText;
    HagglingManager hagglingManager;
    GameObject path;
    GameObject miniGameCanvas;
    GameObject inventoryDisplay;
    AbilityInventoryHandler inventoryHandler;
    SpritePathMovement movementComponent;

    public override void Initialize(GameObject obj)
    {
        //Select item from inventory.

        //Set this to the active item to be 'thrown'

        //Disable the haggling canvas
        hagglingManager = FindObjectOfType<HagglingManager>();
        hagglingManager.HideCanvas();

        //Create mini-game canvas
        miniGameCanvas = Instantiate(aMinigameCanvas, null,true);
        miniGameCanvas.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        
        //Create and display inventory canvas to select item
        inventoryDisplay = Instantiate(inventoryDisplayPrefab.gameObject, null, true);
        inventoryDisplay.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        inventoryHandler = inventoryDisplay.GetComponent<AbilityInventoryHandler>();
        
        //Choose a path and instantiate it.
        path = ChooseRandomPath();
        movementComponent = miniGameCanvas.GetComponentInChildren<SpritePathMovement>();
        
        GameObject pathobject = Instantiate(path, miniGameCanvas.transform, true);
        pathobject.transform.localPosition = new Vector3();

        inventoryHandler.MiniGameObject = miniGameCanvas;
        inventoryHandler.SetAbility(this);
        inventoryHandler.SetFlavortext(this.abilityDescription);
        movementComponent.SetWaypoints(pathobject);
        //Hiding the mini-game canvas so that the inventory handler shows on top
        miniGameCanvas.SetActive(false);
    }

    public override void TriggerAbility()
    {
        miniGameCanvas.SetActive(true);
        movementComponent.ChangeItem(inventoryHandler.selectedItem);
        //movementComponent.ChangeCustomer(activeCustomer.sprite);//TODO: Implement reference to the active customer
        movementComponent.isMoving = true;
    }

    public void Cleanup()
    {

    }

    private GameObject ChooseRandomPath()
    {
        return paths[Random.Range(0, paths.Count)];
    }
}
