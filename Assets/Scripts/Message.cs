using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    private Animation anim;
    public void PlayAnim()
    {
        anim = GetComponent<Animation>();
        anim.Play("DecreaseColor");
        Debug.Log("ASDSDSDSDDD  ");
    }
}
