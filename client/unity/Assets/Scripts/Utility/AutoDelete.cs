using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    void Start()
    {
        // �� 2 ���������ٷ���
        Invoke("DestroyObject", 2.0f);
    }

    void DestroyObject()
    {
        // ���ٵ�ǰ��Ϸ����
        Destroy(gameObject);
    }
}

