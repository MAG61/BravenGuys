using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void JumpPressed()
    {
        FindObjectOfType<CharacterController>().PlayerJump();
    }
}
