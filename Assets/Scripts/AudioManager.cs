using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static bool playingSound = true;
	// Use this for initialization
	void Awake()
    {
		

	}
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
