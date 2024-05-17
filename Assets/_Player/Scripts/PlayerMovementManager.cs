using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerJsonData :SaveLoadBase
{
    [SerializeField]
    public SaveLoadBase m_SaveLoadBase;

    [HideInInspector] public Vector3 m_PlayerPosition; 
    [HideInInspector] public Quaternion m_PlayerRotation;

    public void Save(Vector3 playerPosition, Quaternion playerRotation)
    {
        m_PlayerPosition = playerPosition;
        m_PlayerRotation = playerRotation;  
        Serializer.SaveJsonData<PlayerJsonData>(this, true);
    }

    public PlayerJsonData Load()
    {
        return Serializer.LoadJsonData<PlayerJsonData>(this);
    }

    public void Clear()
    {
        Serializer.DeleteFile<PlayerJsonData>(this);
    }
}
[RequireComponent(typeof(CharacterController),typeof(Animator))]
public class PlayerMovementManager : MonoBehaviour, ISaveAble
{
    [SerializeField] public PlayerJsonData m_JsonData;
    public float m_MoveSpeed, m_RotateSpeed;
    public VariableJoystick m_Joystick;
    public Animator m_Animator;
    public CharacterController m_CharacterController;   
    Vector3 m_MoveDirection, m_LookRotation;
    public Transform m_PlayerChestTransform;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer() 
    {
        if(m_Joystick == null || m_Joystick.Direction.magnitude == 0)
        {
            if (m_Animator.GetBool("Running"))
                m_Animator.SetBool("Running", false);
            return;
        }
        if(!m_Animator.GetBool("Running"))
            m_Animator.SetBool("Running", true);

        m_MoveDirection = new Vector3(m_Joystick.Direction.x, 0, m_Joystick.Direction.y);
        m_CharacterController.SimpleMove(m_MoveDirection.normalized * m_MoveSpeed);

        m_LookRotation = Vector3.RotateTowards(m_CharacterController.transform.forward, m_MoveDirection, m_RotateSpeed * Time.deltaTime, 0.0f);
        m_CharacterController.transform.rotation = Quaternion.LookRotation(m_LookRotation);

    }


    private void OnApplicationQuit()
    {
        Debug.Log("Application Quit called");
        Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
            Save();
        else
            Load();
    }
    #region Save and Load
    public void Save()
    {
        m_JsonData.Save(transform.position,transform.rotation);

    }

    public void Load()
    {
        PlayerJsonData data = m_JsonData.Load();
        if (data == null)
        {
            Save();
            return;
        }
        m_JsonData = data;
        transform.position = data.m_PlayerPosition;
        transform.rotation = data.m_PlayerRotation;



    }

    public void Clear()
    {
        
    }

    #endregion
}
