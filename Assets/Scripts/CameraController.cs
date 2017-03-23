using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    public Transform target;//目标
    public float initialDistance = 10;//初始化摄像机距离目标的距离
    float nextDragTime = 0.0f;//下次触碰的时间（防止触碰冲突）
    public bool clampYawAngle = true;//是否限制上下的旋转角度
    public float minYaw = -45;//上下最小的旋转角度
    public float maxYaw = 45;//上下最大的旋转角度
    float IdealYaw = 0;//水平旋转
    float yaw = 0;//实际使用的水平旋转值
    float idealPitch = 0;//垂直旋转
    float pitch = 0;//实际使用的垂直旋转值
    public float yawSensitivity = 15;//水平旋转系数
    public float pitchSensitivity = 15;//垂直旋转系数
    float IdealDistance = 30;//目标的距离
    float distance = 0;//实际使用的目标的距离
    public float pinchZoomSensitivity = 15;//放大系数
    Vector3 IdealPanOffset = Vector3.zero;//平移的位置
    Vector3 panOffset = Vector3.zero;//实际平移的位置
    public float panningSensitivity = 15;//平移的系数
    public bool rotateSmooth;//是否平滑旋转和移动
    public float smoothZoomSpeed = 5.0f;//平滑缩放系数
    public float smoothOrbitSpeed = 10.0f;//平滑旋转系数
    public float smoothPanningSpeed = 12.0f;//平滑平移系数
    public float minDistance = 15f;//最小距离
    public float maxDistance = 100f;//最大距离
    bool isUI = false;
    public float IdealPitch//设置上下旋转的角度
    {
        get { return idealPitch; }
        set { idealPitch = clampYawAngle ? ClampAngle(value, minYaw, maxYaw) : value; }
    }
    void Start()
    {
        Initalize();//初始化
        distance = IdealDistance = initialDistance;//设置摄像机距离目标距离
        //Apply();
    }
    void Initalize()//初始化
    {
        List<GestureRecognizer> recogniers = new List<GestureRecognizer>(GetComponents<GestureRecognizer>());
        DragRecognizer drag = recogniers.Find(r => r.EventMessageName == "OnDrag") as DragRecognizer;
        DragRecognizer twoFingerDrag = recogniers.Find(r => r.EventMessageName == "OnTwoFingerDrag") as DragRecognizer;
        PinchRecognizer pinch = recogniers.Find(r => r.EventMessageName == "OnPinch") as PinchRecognizer;

        //以下是监听器脚本的属性设置
        //if (!drag)
        //{
        //    drag = gameObject.AddComponent<DragRecognizer>();
        //    drag.RequiredFingerCount = 1;//手指数量
        //    drag.IsExclusive = true;//是否根据手指数量触发事件
        //    drag.MaxSimultaneousGestures = 1;//最多同时触发的事件
        //    drag.SendMessageToSelection = GestureRecognizer.SelectionType.None;
        //}

        //if (!pinch)
        //    pinch = gameObject.AddComponent<PinchRecognizer>();

        //if (!twoFingerDrag)
        //{
        //    twoFingerDrag = gameObject.AddComponent<DragRecognizer>();
        //    twoFingerDrag.RequiredFingerCount = 2;
        //    twoFingerDrag.IsExclusive = true;
        //    twoFingerDrag.MaxSimultaneousGestures = 1;
        //    twoFingerDrag.ApplySameDirectionConstraint = true;
        //    twoFingerDrag.EventMessageName = "OnTwoFingerDrag";
        //}
    }
    void Apply()//摄像机各种运动
    {
        if (rotateSmooth)//是否平滑旋转，平移或缩放
        {
            distance = Mathf.Lerp(distance, IdealDistance, Time.deltaTime * smoothZoomSpeed);
            yaw = Mathf.Lerp(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
            pitch = Mathf.LerpAngle(pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed);
            panOffset = Vector3.Lerp(panOffset, IdealPanOffset, Time.deltaTime * smoothPanningSpeed);
        }
        else
        {
            distance = IdealDistance;
            yaw = IdealYaw;
            pitch = IdealPitch;
            panOffset = IdealPanOffset;
        }
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);//摄像机的X轴和Y轴的旋转
        var rotation = Quaternion.Euler(pitch, yaw, 0);
        var position = rotation * panOffset + target.position - ClampDistance() * transform.forward;
        /**
        一个四元数乘以一个向量 表示以该向量为中心 绕着它旋转 加上目标的位置表示以目标为中心平移得到向量 减去距离表示摄像机距离目标的距离
        **/
        transform.position = position;

    }
    void LateUpdate()
    {
        if (!ToolsClass.Instance.IsPointerOverUIObject())
            Apply();
    }
    void OnDrag(DragGesture gesture)//单个手指拖动事件
    {  
        if (!ToolsClass.Instance.IsPointerOverUIObject())
        {
            if (Time.time < nextDragTime)//防止误操作
                return;
            if (target)
            {
                IdealYaw += gesture.DeltaMove.x.Centimeters() * yawSensitivity;//水平旋转
                IdealPitch -= gesture.DeltaMove.y.Centimeters() * pitchSensitivity;//垂直旋转
            }
        }
            
       
    }
    void OnPinch(PinchGesture gesture)//两个手指捏事件
    {
        if (!ToolsClass.Instance.IsPointerOverUIObject())
        {
            IdealDistance -= gesture.Delta.Centimeters() * pinchZoomSensitivity;
            nextDragTime = Time.time + 0.25f;
        }
    }
    void OnTwoFingerDrag(DragGesture gesture)//两个手指拖拽事件
    {
   
        if (!ToolsClass.Instance.IsPointerOverUIObject())
        {
            Vector3 move = -panningSensitivity * (Vector3.right * gesture.DeltaMove.x.Centimeters() + Vector3.up * gesture.DeltaMove.y.Centimeters());
            //判断方向是否相反
            //if (invertPanningDirections)
            //    IdealPanOffset -= move;
            //else
            IdealPanOffset += move;

            nextDragTime = Time.time + 0.25f;
        }


    }
    static float ClampAngle(float angle, float min, float max)//限制角度
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
    //public Transform target;//目标
    //public float initialDistance = 10;//初始化摄像机距离目标的距离
    //float nextDragTime = 0.0f;//下次触碰的时间（防止触碰冲突）
    //public bool clampYawAngle = true;//是否限制上下的旋转角度
    //public float minYaw = -45;//上下最小的旋转角度
    //public float maxYaw = 45;//上下最大的旋转角度
    //float IdealYaw = 0;//水平旋转
    //float yaw = 0;//实际使用的水平旋转值
    //float idealPitch = 0;//垂直旋转
    //float pitch = 0;//实际使用的垂直旋转值
    //public float yawSensitivity = 15;//水平旋转系数
    //public float pitchSensitivity = 15;//垂直旋转系数
    //float IdealDistance = 30;//目标的距离
    //float distance = 0;//实际使用的目标的距离
    //public float pinchZoomSensitivity = 15;//放大系数
    //Vector3 IdealPanOffset = Vector3.zero;//平移的位置
    //Vector3 panOffset = Vector3.zero;//实际平移的位置
    //public float panningSensitivity = 15;//平移的系数
    //public bool rotateSmooth = true;//是否平滑旋转和移动
    //public float smoothZoomSpeed = 5.0f;//平滑缩放系数
    //public float smoothOrbitSpeed = 10.0f;//平滑旋转系数
    //public float smoothPanningSpeed = 12.0f;//平滑平移系数
    //float clampDistance;

    //public float IdealPitch//设置上下旋转的角度
    //{
    //    get { return idealPitch; }
    //    set { idealPitch = clampYawAngle ? ClampAngle(value, minYaw, maxYaw) : value; }
    //}
    //void Start()
    //{
    //    // Initalize();//初始化
    //    distance = IdealDistance = initialDistance;//设置摄像机距离目标距离
    //    Apply();
    //}
    //void Apply()//摄像机各种运动
    //{
    //    if (rotateSmooth)//是否平滑旋转，平移或缩放
    //    {
    //        distance = Mathf.Lerp(distance, IdealDistance, Time.fixedDeltaTime * smoothZoomSpeed);
    //        yaw = Mathf.Lerp(yaw, IdealYaw, Time.fixedDeltaTime * smoothOrbitSpeed);
    //        pitch = Mathf.LerpAngle(pitch, IdealPitch, Time.fixedDeltaTime * smoothOrbitSpeed);
    //        panOffset = Vector3.Lerp(panOffset, IdealPanOffset, Time.fixedDeltaTime * smoothPanningSpeed);
    //    }
    //    else
    //    {
    //        distance = IdealDistance;
    //        yaw = IdealYaw;
    //        pitch = IdealPitch;
    //        panOffset = IdealPanOffset;
    //    }

    //    transform.rotation = Quaternion.Euler(pitch, yaw, 0);//摄像机的X轴和Y轴的旋转
    //    var rotation = Quaternion.Euler(pitch, yaw, 0);
    //    var position = rotation * panOffset + target.position - ClampDistance() * transform.forward * 0.1f;
    //    /**
    //    一个四元数乘以一个向量 表示以该向量为中心 绕着它旋转 加上目标的位置表示以目标为中心平移得到向量 减去距离表示摄像机距离目标的距离
    //    **/
    //    transform.position = position;

    //}
    //static float ClampAngle(float angle, float min, float max)//限制角度
    //{
    //    if (angle < -360)
    //        angle += 360;

    //    if (angle > 360)
    //        angle -= 360;

    //    return Mathf.Clamp(angle, min, max);
    //}
    float ClampDistance()//限制距离
    {

        IdealDistance = Mathf.Clamp(IdealDistance, minDistance, maxDistance);
        return distance;
    }
    //void LateUpdate()
    //{
    //    Apply();
    //}

    //public void MyEasyTouch_On_Swipe(Vector2 gesture)
    //{
    //    //Debug.Log(gesture.x);
    //    //Vector3 move = -panningSensitivity * (Vector3.right * gesture.x + Vector3.up * gesture.y);
    //    //IdealPanOffset += move;
    //    //nextDragTime = Time.time + 0.25f;
    //    if (Time.time < nextDragTime)//防止误操作
    //        return;
    //    if (target)
    //    {
    //        IdealYaw += Centimeters(gesture.x) * yawSensitivity;//水平旋转
    //        IdealPitch -= Centimeters(gesture.y) * pitchSensitivity;//垂直旋转
    //    }

    //}
    //public void MyEasyTouch_On_Pinch(Gesture gesture)
    //{
    //    Debug.Log("缩放");
    //    IdealDistance -= Centimeters(gesture.deltaPinch) * pinchZoomSensitivity;
    //    nextDragTime = Time.time + 0.25f;
    //}
    //public void MyEasyTouch_On_TwoFingerSwipe(Gesture gesture)//两个手指拖拽事件
    //{
    //    Vector3 move = -panningSensitivity * (Vector3.right * Centimeters(gesture.swipeVector.x) + Vector3.up * Centimeters(gesture.swipeVector.y));
    //    IdealPanOffset += move;

    //    nextDragTime = Time.time + 0.25f;

    //}
    //public float Centimeters(float value)
    //{
    //    return value / 96 * 2.54f;
    //}

    //    void Update()
    //    {
    //        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
    //        {
    //#if IPHONE || ANDROID
    //            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //#else
    //            if (EventSystem.current.IsPointerOverGameObject())
    //#endif
    //                // Debug.Log("当前触摸在UI上");
    //                isUI = true;
    //            else
    //                // Debug.Log("当前没有触摸在UI上");
    //                isUI = false;
    //        }
    //    }
}
