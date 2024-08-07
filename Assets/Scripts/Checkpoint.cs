using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if(!activationStatus)
            AudioManager.instance.PlaySFX(8, transform);
        anim.SetBool("active", true);
        activationStatus = true;
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
