using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ThrowAbility")]
public class ThrowInAbilityScript : Ability
{
    [SerializeField] List<GameObject> paths;

    TextMeshProUGUI hitOrMissText;
    GameObject path;
    GameObject miniGameCanvas;
    SpritePathMovement movementComponent;

    public override GameObject Initialize()
    {
        //Create mini-game canvas
        miniGameCanvas = Instantiate(aMinigamePrefab, null, true);
        miniGameCanvas.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
                
        //Choose a path and instantiate it.
        path = ChooseRandomPath();
        movementComponent = miniGameCanvas.GetComponentInChildren<SpritePathMovement>();
        
        GameObject pathobject = Instantiate(path, miniGameCanvas.transform, true);
        pathobject.transform.localPosition = new Vector3();
        movementComponent.SetWaypoints(pathobject);
        //Hiding the mini-game canvas so that the inventory handler shows on top
        miniGameCanvas.SetActive(false);

        return miniGameCanvas;
    }

    public override void TriggerAbility()
    {
        miniGameCanvas.SetActive(true);
    }

    public override void Cleanup()
    {
        //Reset UI
    }

    private GameObject ChooseRandomPath()
    {
        return paths[Random.Range(0, paths.Count)];
    }
}