using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeReference] private float m_acceleration = 1;
    [SerializeReference] private float m_decceleration = 1;
    [SerializeField] private float m_maxSpeed = 5;

    // Mouse Variables
    [Header("Rotation Variables")]
    [SerializeField] private float m_rotationSpeedAngle = 30;
    [SerializeField] private float m_mouseSensibility = 2;
    private Vector2 m_mouseDelta = Vector2.zero;
    private Vector2 m_prevMousePosition;
    private Vector2 m_mouseInputData;

    private float m_currentSpeed;
    private Rigidbody m_rigidbodyComponent;
    private Vector2 m_mvtAxis;
    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        InitComponent();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }



    // Update is called once per frame
    void Update()
    {
        GetMouvementInput();

        // Rotation Camera
        m_mouseDelta = m_mouseInputData;
        m_prevMousePosition = Input.mousePosition;

        transform.Rotate(new Vector3(0, m_rotationSpeedAngle * m_mouseDelta.x*Time.deltaTime,0));

        // Player speed
        if (m_mvtAxis != Vector2.zero)
        {
            m_currentSpeed += m_acceleration * Time.deltaTime;
        }
        else
        {
            m_currentSpeed -= m_decceleration * Time.deltaTime;

        }
        m_currentSpeed = Mathf.Clamp(m_currentSpeed, 0, m_maxSpeed);

    }

    private void FixedUpdate()  
    {
        //Debug.Log("Current Direction : " +( Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f) * new Vector3(m_mvtAxis.x, 0, m_mvtAxis.y)));
        //Debug.Log("Current Velocity : " + m_rigidbodyComponent.velocity);
        m_rigidbodyComponent.velocity = Quaternion.Euler(0.0f,transform.eulerAngles.y,0.0f) * new Vector3(m_mvtAxis.x, 0, m_mvtAxis.y) * m_currentSpeed;
        m_rigidbodyComponent.velocity = Vector3.ClampMagnitude(m_rigidbodyComponent.velocity , m_maxSpeed);
    }
    #endregion

    private void InitComponent()
    {
        m_rigidbodyComponent = GetComponent<Rigidbody>();
    }

    private void GetMouvementInput()
    {
        m_mvtAxis = Vector2.zero;
        m_mvtAxis.y += Convert.ToInt32(Input.GetKey(KeyCode.W));
        m_mvtAxis.y += -1 * Convert.ToInt32(Input.GetKey(KeyCode.S));
        m_mvtAxis.x += Convert.ToInt32(Input.GetKey(KeyCode.D));
        m_mvtAxis.x += -1 * Convert.ToInt32(Input.GetKey(KeyCode.A));

        m_mouseInputData.x = Input.GetAxis("Mouse X") * m_mouseSensibility;
        m_mouseInputData.y = Input.GetAxis("Mouse Y")  * m_mouseSensibility;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
    }
}
