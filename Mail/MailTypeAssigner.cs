using UnityEngine;

public class MailTypeAssigner : MonoBehaviour
{
    public MailboxType mailType;
    public ParticleSystem fx;

    public void SetType(MailboxType type)
    {
        mailType = type;
        Debug.Log($"Mail set to type: {type}", this);

        // Optional: Change mail color/texture based on type
        //ApplyMailTypeVisual(type);
    }

    public void PlayFx() 
    { 
        fx.Play();
    }

    

    //void ApplyMailTypeVisual(MailboxType type)
    //{
    //    // Change material/color based on type
    //    Renderer rend = GetComponent<Renderer>();
    //    if (rend != null)
    //    {
    //        switch (type)
    //        {
    //            case MailboxType.Type0: rend.material.color = Color.red; break;
    //            case MailboxType.Type1: rend.material.color = Color.blue; break;
    //            case MailboxType.Type2: rend.material.color = Color.green; break;
    //            case MailboxType.Type3: rend.material.color = Color.yellow; break;
    //            case MailboxType.Type4: rend.material.color = Color.azure; break;
    //        }
    //    }
    //}
}
