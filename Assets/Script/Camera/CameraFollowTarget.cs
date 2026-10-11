using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    GameObject _playerObj;

    void Start()
    {
        _playerObj = 
            GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (_playerObj == null)
        {
            Debug.Log("return");
            return;
        }
        transform.position = _playerObj.transform.position;
    }
}
