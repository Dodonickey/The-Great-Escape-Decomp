using UnityEngine;

public static class InputManager
{
    public static TLTouch[] m_touches = new TLTouch[10];

    public static int m_touchAmount;

    public static Vector2 m_mouseOldPos;

    public static Vector2 m_mouseOldPos2;

    private static Vector3 m_secondaryPos;

    // Mouse simulation variables
    private static bool m_mouseWasDown = false;

    private static void AddTouch(Vector2 _pos, Vector2 _deltaPos, int _fingerId, TouchPhase _phase)
    {
        int freeTouchId = GetFreeTouchId();
        if (freeTouchId >= 0)
        {
            m_touches[freeTouchId] = new TLTouch();
            m_touches[freeTouchId].position = _pos;
            m_touches[freeTouchId].deltaPosition = _deltaPos;
            m_touches[freeTouchId].fingerId = _fingerId;
            m_touches[freeTouchId].phase = _phase;
            m_touches[freeTouchId].masked = false;
            m_touches[freeTouchId].consumed = false;
            m_touches[freeTouchId].consumingTAC = null;
            m_touchAmount++;
        }
    }

    private static int GetFreeTouchId()
    {
        if (m_touchAmount >= m_touches.Length)
        {
            return -1;
        }
        return m_touchAmount;
    }

    public static void FlushTouches()
    {
        m_touchAmount = 0;
    }

    public static int GetFirstTouchId(TouchPhase phase)
    {
        for (int i = 0; i < m_touchAmount; i++)
        {
            if (m_touches[i].phase == phase)
            {
                return i;
            }
        }
        return -1;
    }

    public static int GetTouchIdWithFingerId(int fingerId)
    {
        for (int i = 0; i < m_touchAmount; i++)
        {
            if (m_touches[i].fingerId == fingerId)
            {
                return i;
            }
        }
        return -1;
    }

    public static void Update()
    {
        m_touchAmount = 0;
        int touchCount = Input.touchCount;

        // 1. Process standard mobile touch inputs
        for (int i = 0; i < touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            AddTouch(touch.position, touch.deltaPosition, touch.fingerId, touch.phase);
        }

        // 2. Fallback to mouse simulation if no active native touch inputs are present
        if (touchCount == 0)
        {
            Vector2 mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                m_mouseWasDown = true;
                m_mouseOldPos = mousePos;
                AddTouch(mousePos, Vector2.zero, 99, TouchPhase.Began); // Finger ID 99 reserved for simulated mouse
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_mouseWasDown)
                {
                    Vector2 delta = mousePos - m_mouseOldPos;
                    AddTouch(mousePos, delta, 99, TouchPhase.Ended);
                    m_mouseWasDown = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (m_mouseWasDown)
                {
                    Vector2 delta = mousePos - m_mouseOldPos;
                    TouchPhase phase = (delta.sqrMagnitude > 0f) ? TouchPhase.Moved : TouchPhase.Stationary;
                    AddTouch(mousePos, delta, 99, phase);
                    m_mouseOldPos = mousePos;
                }
            }
            else
            {
                m_mouseWasDown = false;
            }
        }
        else
        {
            m_mouseWasDown = false;
        }
    }
}