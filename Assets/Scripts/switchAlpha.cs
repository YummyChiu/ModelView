using UnityEngine;

public class switchAlpha : MonoBehaviour
{
    Transform prObj;//保存上一次点击的物体
    private float R;
    private float G;
    private float B;
    private float A;//保存上一次物体的颜色
    public Shader myshader;
    Shader prshader;

    void Start()
    {
        //   myshader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
    }

    void Update()
    {
        if (MenuManager.isSwitchRenderObjInfoActive == true)
        {
            changeAlpha();
        }
    }

    void changeAlpha()
    {
       // if (ToolsClass.Instance.CheckGuiRaycastObjects()) return;
        if (ToolsClass.Instance.IsPointerOverUIObject()) return;

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 1000f))
        {

            Transform currentObj = hitInfo.transform;
            if (prObj == null)
            {//第一帧，直接变色
                changemyAlpha(currentObj);
            }
            else
            {

                changebackcolor(prObj);//变回上一次的颜色
                changemyAlpha(currentObj);//设置红色
            }

        }
        else
        { // 点了其他地方，上一个变回去
            if (prObj != null)
            {
                changebackcolor(prObj);
            }
        }
    }
    void changemyAlpha(Transform myTemGo)//当前物体变颜色
    {
        prObj = myTemGo;
        //获取当前物体
        Renderer temRen = myTemGo.GetComponent<Renderer>();
        //获取当前物体的材质
        Material mat = temRen.material;
        prshader = temRen.material.shader;

        //为当前物体赋予带透明通道的shader
        mat.shader = myshader;
        //新的颜色信息，透明度将为50%
        R = mat.color.r;
        G = mat.color.g;
        B = mat.color.b;
        A = mat.color.a;
        Color co = new Color(R, G, B, A / 0.8f);
        //为当前材质赋予带透明的颜色
        mat.color = co;
    }
    void changebackcolor(Transform myCurrent)//变回上一次的颜色
    {
        Renderer temRen = myCurrent.GetComponent<Renderer>();
        temRen.material.shader = prshader;
    }
}
