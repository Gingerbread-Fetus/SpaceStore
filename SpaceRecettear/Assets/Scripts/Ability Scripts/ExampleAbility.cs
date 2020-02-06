using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ExampleAbility")]
public class ExampleAbility : Ability
{

    public override void Initialize(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerAbility()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return base.aName;
    }
}
