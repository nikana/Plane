using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExt {

	public static float floorWithPrecision (float src, float precision)
	{
		float multiplier = 1 / precision;
		return ((float)((int)(src * multiplier))) / multiplier;
	}

	/// <summary>
	/// Gets the closest integer to the direction zero.
	/// </summary>
	/// <returns>The integer.</returns>
	/// <param name="num">Number.</param>
	public static float getFloorToZero (float num)
	{
		if (num > 0)
			return Mathf.Floor (num);
		else
			return -Mathf.Floor (-num);
	}

	/// <summary>
	/// judge the rotation direction from start angle to end angle.
	/// </summary>
	/// <returns><c>true</c>, if start angle increse to end angle, <c>false</c> start angle decrease to end angle.</returns>
	/// <param name="startAngle">Start angle.</param>
	/// <param name="endAngle">End angle.</param>
	public static bool isMinAngleRotPositive (float startAngle, float endAngle)
	{
		float angleDiff = endAngle - startAngle;
		if (((angleDiff > 0f) && (angleDiff < 180f))||(angleDiff < -180f)&&(angleDiff > -360f))
			return true;
		else 
			return false;
	}

	public static float getValidAngleDegrees (float angle)
	{
		float validAngle = angle;
		if (angle > 360f)
			return (angle % 360);
		if (angle < 0f)
			validAngle = getValidAngleDegrees(360f + angle);

		return validAngle;
	}
}
