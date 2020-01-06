using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : MonoBehaviour
{
    public int BaseSellPrice;
    public abstract int CalculateSellPrice();//Here for sake of argument, probably remove later
}
