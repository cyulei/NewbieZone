using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUITween : MonoBehaviour
{
    [Tooltip("实际掉血Fill")]
    public RectTransform realFill;
    [Tooltip("缓慢掉血Fill")]
    public RectTransform tweenFill;
    [Tooltip("渐变速度")]
    public float speed = 1.2f;

    private float _lastMaxX = 0;
    private bool _flag = false;

    // 缓慢掉血的血条起点
    private float _start = 0;
    // 缓慢掉血的血条终点
    private float _end = 0;      

    private float _now = 0;
    private float _time = 0;

    private void Start()
    {
        // 确保真实fill在上面
        realFill.SetAsLastSibling();
        tweenFill.anchorMax = realFill.anchorMax;
        _lastMaxX = realFill.anchorMax.x;
    }
    private void Update()
    {
        if(_flag)
        {
            _time += speed * Time.deltaTime;
            if (_time >= 1)
            {
                _flag = false;      
                _lastMaxX = _end;  
            }
            // 进行过渡
            _now = Mathf.Lerp(_start, _end, _time);
            tweenFill.anchorMax = new Vector2(_now, realFill.anchorMax.y);
        }
    }

    public void StartTween()
    {
        _start = _lastMaxX;
        _end = realFill.anchorMax.x;
        _flag = true;
        _time = 0;
    }
}
