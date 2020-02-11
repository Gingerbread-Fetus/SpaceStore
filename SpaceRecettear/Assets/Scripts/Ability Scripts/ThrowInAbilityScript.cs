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
    GameObject canvas;
    SpritePathMovement movementComponent;

    public override void Initialize(GameObject obj)
    {
        //Select item from inventory.

        //Set this to the active item to be 'thrown'

        //Display mini-game canvas
        canvas = Instantiate(aMinigameCanvas, null,true);
        canvas.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        path = ChooseRandomPath();
        movementComponent = canvas.GetComponentInChildren<SpritePathMovement>();
        
        GameObject pathobject = Instantiate(path, canvas.transform, true);
        pathobject.transform.localPosition = new Vector3();
        movementComponent.SetWaypoints(pathobject);
    }

    public override void TriggerAbility()
    {
    }

    public void Cleanup()
    {
    }

    private GameObject ChooseRandomPath()
    {
        return paths[Random.Range(0, paths.Count)];
    }
}
