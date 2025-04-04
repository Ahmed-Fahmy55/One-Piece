using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] ParticleSystem moveParticle;
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem landParticle;
    [Header("Sounds")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip runSound;


    bool isGrounded;
    bool isPlayingRunSound;
    private PlayerController _player;
    private AudioSource audioSource;


    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _player.OnPlayerJump += OnPlayerJump;
        _player.OnGroundChange += OnGroundChange;

        moveParticle.Play();
    }

    private void OnDisable()
    {
        _player.OnPlayerJump -= OnPlayerJump;
        _player.OnGroundChange -= OnGroundChange;

        moveParticle.Stop();
    }

    private void Update()
    {
        HandleFlip();
        PlaySound();
    }

    private void HandleFlip()
    {
        _player.transform.localScale = new Vector3(_player.IsPlayerFacingLeft() ? -1 : 1, 1, 1);
        animator.SetFloat("Movement", Mathf.Abs(_player.GetPlayerInputVelocity()));
    }

    private void PlaySound()
    {
        if (!isGrounded) return;

        if (_player.GetPlayerInputVelocity() != 0)
        {
            if (!isPlayingRunSound)
            {
                PlaySound(runSound, true);
                isPlayingRunSound = true;
            }
        }
        else
        {
            audioSource.Stop();
            isPlayingRunSound = false;
        }
    }

    private void OnGroundChange(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            animator.SetBool("IsGrounded", true);
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Land");
            landParticle.Play();
            moveParticle.Play();
        }
        else
        {
            animator.SetBool("IsGrounded", false);
            moveParticle.Stop();
            isPlayingRunSound = false;
        }
    }

    private void PlaySound(AudioClip sound, bool loop)
    {
        audioSource.Stop();
        audioSource.clip = sound;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private void OnPlayerJump()
    {
        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
        jumpParticle.Play();
        PlaySound(jumpSound, false);
    }
}
