using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureProperties : MonoBehaviour {
	public float Visibility { get; set; }
	public float OdorIntensity { get; set; }
	public float Loudness { get; set; }

	public float visualSensibility { get; set; }
	public float smellSensibility { get; set; }

	void Start ()
	{
		this.Visibility = 5f;
		this.OdorIntensity = 5f;
		this.Loudness = 5f;
	}
}
