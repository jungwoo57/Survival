using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    public static float Speed
    {
        get {
            switch (GameManager.instance.playerId){
                case 0:
                case 2:
                    return 1.1f;            
                default: 
                    return 1.0f; 
            }
        }
            //return GameManager.instance.playerId ==0 ? 1.1f : 1f;
    }
    public static float WeaponSpeed
    {
        get
        {
            switch (GameManager.instance.playerId)
            {
                case 1:
                case 2:
                    return 1.1f;
                default:
                    return 1.0f;
            }
        }
    }
    public static float WeaponRate
    {
        get {
            switch (GameManager.instance.playerId)
            {
                case 1:
                case 2:
                    return 0.9f;
                case 3:
                    return 0.8f;
                default:
                    return 1.0f;
            }
        }
    }

}
