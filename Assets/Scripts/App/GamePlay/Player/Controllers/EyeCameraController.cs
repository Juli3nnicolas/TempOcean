﻿using UnityEngine;
using System.Collections;

namespace App
{
	namespace Gameplay
	{
		namespace Controllers
		{ 
			public class EyeCameraController : MonoBehaviour 
			{
				public GameObject p_Camera;
			
				// Use this for initialization
				void Start () 
				{
					m_initialOrientation = GetComponent<App.Gameplay.InitPlayerPosition>().getInitialOrientation();
				}
				
				// Update is called once per frame
				void Update () 
				{
					float dt = Time.deltaTime;
					rotateCamera(dt);
				}
				
				private void rotateCamera(float deltaTime)
				{
					float mouse_h = Input.GetAxis("Mouse X");
					float mouse_v = Input.GetAxis("Mouse Y");
					
					// Request to rotate the camera?
					if ( mouse_h != 0 || mouse_v != 0)
					{	
						// M A N A G E   S I D E   R O T A T I O N S
						
						// Position reorientation to prevent unexpected rotations
						p_Camera.transform.rotation = m_initialOrientation;
						
						// Rotate to the right
						setAngleIfLower(ref m_angle.y, 90.0f, mouse_h, deltaTime);
						
						// Rotate to the left
						setAngleIfGreater(ref m_angle.y, -90.0f, mouse_h, deltaTime);
						
						// Apply the rotation
						p_Camera.transform.RotateAround(p_Camera.transform.position, m_forwardAxis, m_angle.y);
						
						
						// M A N A G E   F O R W AR D - B A C K W A R D   R O T A T I O N S
						
						// Rotate forward
						setAngleIfLower(ref m_angle.x, 25.0f, mouse_v, deltaTime);
						
						// Rotate backward
						setAngleIfGreater(ref m_angle.x, -20.0f, mouse_v, deltaTime);
						
						// Orient the camera whether it moved or not
						p_Camera.transform.RotateAround(p_Camera.transform.position, m_sideAxis, m_angle.x);
					}
				}
				
				private void setAngleIfLower(ref float angle, float threshold, float sensitivity, float deltaTime)
				{
					float new_angle = angle + sensitivity*m_angularSpeed.x*deltaTime;
					
					if ( new_angle < threshold && sensitivity > 0 ) // The rotation angle won't exceed the threshold value
						angle = new_angle;
					else
					{
						// The complete rotation can't be performed because the angle is greater than the max accepted value
						// However, if this test is true then part of the rotation can be done to reach the max value.
						if ( angle < threshold && sensitivity > 0 )
							angle = threshold;
					}
				}
				
				private void setAngleIfGreater(ref float angle, float threshold, float sensitivity, float deltaTime)
				{
					float new_angle = angle + sensitivity*m_angularSpeed.x*deltaTime;
					
					if ( new_angle > threshold && sensitivity < 0 ) // The rotation angle won't exceed the threshold value
						angle = new_angle;
					else
					{
						// The complete rotation can't be performed because the angle is greater than the max accepted value
						// However, if this test is true then part of the rotation can be done to reach the max value.
						if ( angle > threshold && sensitivity < 0 )
							angle = threshold;
					}
				}
				
				////////////// Attributes //////////////
				
				private Quaternion m_initialOrientation;
				private Vector2    m_angularSpeed = new Vector2(30.0f, 30.0f); // In degree per seconds
				private Vector2    m_angle        = new Vector2(0.0f, 0.0f);   // 1st coordinate: side; 2nd: forward
				private Vector3    m_sideAxis     = new Vector3(1.0f, 0.0f, 0.0f);
				private Vector3    m_forwardAxis  = new Vector3(0.0f, 0.0f, 1.0f);
			}
		}
	}
}
