using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource source;
    bool isMuted;

    public static MusicManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }
	
  public void Mute(bool value)
  {
      isMuted = value;
      source.mute = isMuted;
  }

  public void ToggleMute()
  {
      Mute(!isMuted);
  }
}
