using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehavior : MonoBehaviour {
	public GameObject predator;

	Vector3 prey_to_pred_vec;

	float smellSensitivity = 10f;
	float hearingSensitivity = 10f;

	float halfVisualAngle = 60f;
	float visualAdaptionAlertness = 1f;
	float visualSensitivity = 10f;
	float visualObstacle = 0f;
	float visualThreashold = 5f;

	bool alerted = false;
	float alertness = 0f;
	float alertThreshold_run = 1.5f;
	float alertThreshold_visualSearch = 1f;
	float alertnessAdaption = 10f;

	float roundingPrecision = 0.1f;

	bool isTurning;
	bool isFleeing;
	float rotSpeed = 50f;
	float angleTolerance = .1f;
	float rotEndAngle;

	//-----------------------------------------------------------------------------------------
	//--------------------------- prey behavior description -----------------------------------
	//-----------------------------------------------------------------------------------------

	void casual ()
	{
		// without wind in consideration
		// calculated with square instead of cubic
		// predator odor strongness has to be considered as well
		float odorIndex = 0f;
		float soundIndex = 0f;
		odorIndex = smellSensitivity * getPredatorOdor (predator) / prey_to_pred_vec.sqrMagnitude;
		soundIndex = hearingSensitivity * getPredatorSound (predator) / prey_to_pred_vec.sqrMagnitude;
		alertness += MathExt.floorWithPrecision (odorIndex, roundingPrecision) + MathExt.floorWithPrecision (soundIndex, roundingPrecision);

		alertnessManagement ();
	}

	void visualSearch ()
	{
		float visualIndex = 0f;
		bool calcVisibility = false;
		float leftAngle = MathExt.getValidAngleDegrees (this.transform.eulerAngles.y + halfVisualAngle);
		float rightAnle = MathExt.getValidAngleDegrees (this.transform.eulerAngles.y - halfVisualAngle);

		// see if predator direction is within vision boundaries
		if (leftAngle < rightAnle) {
			if ((rotEndAngle > leftAngle) || (rotEndAngle < rightAnle)) {
				calcVisibility = true;
			}	
		} else {
			if ((rotEndAngle < leftAngle) || (rotEndAngle > rightAnle)) {
				calcVisibility = true;
			}
		}

		if (calcVisibility) {
			visualIndex = visualSensitivity * getPredatorVisibility(predator) / prey_to_pred_vec.magnitude
							- getVisualObstacleIndex(prey_to_pred_vec);
			MathExt.floorWithPrecision (visualIndex, roundingPrecision);

			if (visualIndex > visualThreashold)
				isFleeing = true;
		}
	}

	float getPredatorVisibility (GameObject pred)
	{
		// based on predator appearance: camoflage, color, etc.
		// also influenced by how good the predator is hidden: obstacles
		float predatorVisibility = 0f;
		predatorVisibility = pred.gameObject.GetComponent<CreatureProperties> ().Visibility;
		return predatorVisibility;
	}

	float getPredatorOdor (GameObject pred)
	{
		float predatorOdor = 0f;
		predatorOdor = pred.gameObject.GetComponent<CreatureProperties> ().OdorIntensity;
		return predatorOdor;
	}

	float getPredatorSound (GameObject pred)
	{
		float predatorSound = 0f;
		predatorSound = pred.gameObject.GetComponent<CreatureProperties> ().Loudness;
		return predatorSound;
	}

	float getVisualObstacleIndex (Vector3 dir)
	{
		float visualObstacleIndex = 0f;

		return visualObstacleIndex;
	}

	void alertnessManagement ()
	{
		if (alertness > alertThreshold_run)
			alerted = true;
		else if (alertness > alertThreshold_visualSearch)
			visualSearch ();
		else 
			alerted = false;
	}

	void adapt ()
	{
		alertness -= alertnessAdaption;
		if (alertness < 0) {
			alertness = 0;
			alerted = false;
		}
	}

	void turnStart (Vector3 endDir)
	{
		isTurning = true;
		Quaternion endRotation = Quaternion.LookRotation(endDir);
		rotEndAngle = endRotation.eulerAngles.y;
	}

	void turn ()
	{
		float startAngle = this.transform.eulerAngles.y;
		float tempEndAngle = 0f;

		if ((Mathf.Abs (startAngle - rotEndAngle) > angleTolerance)||(isTurning)) {
			Debug.Log ("turning:" + this.transform.eulerAngles.y);

			if (MathExt.isMinAngleRotPositive (startAngle, rotEndAngle)) {
				tempEndAngle = this.transform.eulerAngles.y + Time.deltaTime * rotSpeed;
				MathExt.getValidAngleDegrees(tempEndAngle);
				if (tempEndAngle > rotEndAngle)
					tempEndAngle = rotEndAngle;
			} else {
				tempEndAngle = this.transform.eulerAngles.y - Time.deltaTime * rotSpeed;
				MathExt.getValidAngleDegrees(tempEndAngle);
				if (tempEndAngle < rotEndAngle)
					tempEndAngle = rotEndAngle;
			}
			this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, tempEndAngle, this.transform.eulerAngles.z);
		} else {
			Debug.Log ("turned");
			this.transform.eulerAngles = Vector3.up * rotEndAngle;
			isTurning = false;
		}
	}

	void run ()
	{
		
	}







	//-----------------------------------------------------------------------------------------
	//---------------------------------- prey by frame ----------------------------------------
	//-----------------------------------------------------------------------------------------

	// Use this for initialization
	void Start () {
		prey_to_pred_vec = predator.transform.position - this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
//*
		prey_to_pred_vec = predator.transform.position - this.transform.position;

		if (!alerted) {
			casual ();
			adapt ();
		} else {
			turnStart (prey_to_pred_vec);
			adapt ();
		}

		if (isTurning) {
			turn ();
		}
//*/

	}
}
