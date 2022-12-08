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

    private void Start()
    {
        //islocked = true;
        anim.SetBool("isDoorOpen", false);
        trigger = false;
    }

    private void OpenDoor()
    {
            if (!trigger)
            {
                anim.SetBool("isDoorOpen", true);
            }
            else
            {
                
                anim.SetBool("isDoorOpen", false);
            }   
    }

    public override string GetDescription()
    {
        if (isOpened)
        {
            return "close";
        }
        return "open";

    }

    public override void Interact()
    {
        isOpened = !isOpened;
        OpenDoor();
    }
}