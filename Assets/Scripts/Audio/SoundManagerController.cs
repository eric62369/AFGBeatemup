using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerController : MonoBehaviour
{
    public static AudioClip hitLvl1Sound { get; private set; }
    public static AudioClip whiffLvl1Sound { get; private set; }
    public static AudioClip airdashSound { get; private set; }
    public static AudioClip jumpSound { get; private set; }
    private static AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        hitLvl1Sound = Resources.Load<AudioClip>("AnimeSFX - Track 01 (HitLvl1)");
        whiffLvl1Sound = Resources.Load<AudioClip>("AnimeSFX - Track 04 (WhiffLvl1)");
        airdashSound = Resources.Load<AudioClip>("AnimeSFX - Track 13 (Airdash)");
        jumpSound = Resources.Load<AudioClip>("AnimeSFX - Track 12 (Jump)");
        source = GetComponent<AudioSource>();
    }

    public static void playSFX(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
