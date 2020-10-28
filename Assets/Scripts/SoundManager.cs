using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public List<AudioClip> footsteps_Grass;

    public List<AudioClip> weaponSFX;
    /* Sound Effects List
     * 00 = whoosh
     * 01 = slingshot draw
     * 02 = slingshot release
     */

    public AudioSource playerFootsteps;
    public AudioSource playerWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPlayerFootstep()
    {
        AudioClip sound = footsteps_Grass[(int)(Random.Range(0, footsteps_Grass.Count - 0.01f))];
        playerFootsteps.Stop();
        playerFootsteps.clip = sound;
        playerFootsteps.Play();
    }

    public void PlayWeaponSound(int index, float pitch)
    {
        AudioClip sound = weaponSFX[index];
        playerWeapon.Stop();
        playerWeapon.clip = sound;
        playerWeapon.pitch = pitch;
        playerWeapon.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 location)
    {
        GameObject obj = new GameObject();
        obj.transform.position = location;
        obj.AddComponent<AudioSource>();
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
        StartCoroutine(DeleteAudioObject(obj, clip.length));
    }

    IEnumerator DeleteAudioObject(GameObject obj, float time)
    {
        while(time > 0)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        GameObject.Destroy(obj);
    }
}
