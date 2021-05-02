using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    private void Start(){
        collectableName = "Coin";
        description = "Increases Score by 10";
    }
    override public void Use(){
        player.GetComponent<PlayerManagerScript>().ChangeScore(10);
        
    }
}
