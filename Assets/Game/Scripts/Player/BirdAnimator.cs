using UnityEngine;

public class BirdAnimator : MonoBehaviour
{
    private Animator _animator;
    private Flying _bird;
    private BirdDash _dash;
    private PickUpMail _mail;
    private int _dashHash = Animator.StringToHash("IsDashing");
    private int _standardMailHash = Animator.StringToHash("HasStandardMail");
    private int _fragileMailHash = Animator.StringToHash("HasFragileMail");
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _bird = GetComponent<Flying>();
        _dash = GetComponent<BirdDash>();
        _mail = GetComponent<PickUpMail>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _animator.SetBool(_dashHash, _dash.IsDashing);
        _animator.SetBool(_standardMailHash, _mail.Mail != null && _mail.Mail.CompareTag("StandardMail"));
        _animator.SetBool(_fragileMailHash, _mail.Mail != null && _mail.Mail.CompareTag("FragileMail"));
        
    }
    
}
