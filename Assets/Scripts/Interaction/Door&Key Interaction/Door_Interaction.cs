using UnityEngine;

public class Door_Interaction : Interactable
{
    //public GameObject Key;

    //public Get_Key Get_Key;

    //private bool key;

    //public bool islocked;

    public bool isOpened;

    private bool trigger;

    public Animator anim;
	private AudioSource doorClip;
    private void Start()
    {
        //islocked = true;
	    anim.SetBool("isDoorOpen", false);
	    doorClip = GetComponentInChildren<AudioSource>();
        trigger = false;
    }

    private void OpenDoor()
    {
            if (!trigger)
            {
	            //doorClip.pitch = -1.0f;
	            // doorClip.Play();
	            anim.SetBool("isDoorOpen", true);

            }
            else
            {
	            //doorClip.pitch = 1.0f;
	            //doorClip.Play();               
	            anim.SetBool("isDoorOpen", false);

            }   
    }

    public override string GetDescription()
    {
        if (isOpened)
        {
	        return "Press [E] to Close";
        }
	    return "Press [E] to Open";

    }

    public override void Interact()
    {
        isOpened = !isOpened;
        OpenDoor();
    }
}