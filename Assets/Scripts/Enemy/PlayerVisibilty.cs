using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibilty : MonoBehaviour
{
    AIMovement AIMovement;
    public void isVisible()
    {
        AIMovement.isVisible = true;
    }
    public void notVisible()
    {
        AIMovement.isVisible = false;
    }
}
