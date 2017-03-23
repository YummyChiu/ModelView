using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ComponentInfo : MonoBehaviour
{
    string objId = "";//objID:选中物体的ID
    string objName = null;//tableName:物体所在的数据库表格
    Walls PPTCPN_Walls;
    Floors PPTCPN_Floors;
    Structures PPTCPN_Structure;
    Frameworks PPTCPN_FrameWorks;
    Commons PPTCPN_Commmon;
    FieldInfo[] fieldInfos;
   
    public bool isCheck = false;
    public Transform ComponentPanel;//显示属性的面板
    private Transform Content; //滚动列表的content
    private GameObject go;
   
    void Start()
    {
        Content = ComponentPanel.Find("Content_Panel/Scroll View/Viewport/Content");
        //Debug.Log(Content.name);
    }
    // Update is called once per frame
    void Update()
    {
        if (isCheck)
            GetFormName();
    }

    //private bool IsPointerOverUIObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    //Debug.Log("result's count:"+ results.Count);
    //    return results.Count > 0;
    //}
    private void GetFormName()
    {
        //ToolsClass.mouseScale = true;
        //if (ToolsClass.Instance.CheckGuiRaycastObjects())
        //    return;

        if (!ToolsClass.Instance.IsPointerOverUIObject())
        {
            if (Input.GetMouseButtonDown(0))//如果鼠标点击了构件
            {
                if (Content.childCount > 0)
                {
                    foreach (Transform children in Content)
                    {
                        Destroy(children.gameObject);
                    }
                }
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                #region 判断是否点击到构件
                if (Physics.Raycast(ray, out hitInfo, 1000f))
                {
                    ComponentPanel.gameObject.SetActive(true);
                    //ToolsClass.mouseScale = false;
                    Transform selectTransform = hitInfo.transform;
                    string selectGameobjectName = selectTransform.name;
                    objId = selectGameobjectName.Substring(selectGameobjectName.IndexOf('_') + 1, 6);
                    objName = selectGameobjectName.Substring(0, selectGameobjectName.IndexOf('_'));
                    string valueStr;
                    string propertyStr;
                    #region 判断构件的类型
                    switch (objName)
                    {
                        case "墙":
                            PPTCPN_Walls = selectTransform.gameObject.GetComponent<Walls>();
                            fieldInfos = PPTCPN_Walls.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            foreach (FieldInfo fi in fieldInfos)
                            {
                                object va = fi.GetValue(PPTCPN_Walls);
                                //propertyStr = fi.Name;
                                if (va == null)
                                {
                                    valueStr = "";
                                    //Debug.Log("该" + fi.Name + "属性值为空");
                                }
                                else
                                {
                                    valueStr = va.ToString();
                                }
                                propertyStr = WallPPT_FieldToCHN(fi.Name);
                                PPTAddContent(propertyStr, valueStr);
                            }
                            break;

                        case "结构柱":
                            PPTCPN_Structure = selectTransform.gameObject.GetComponent<Structures>();
                            fieldInfos = PPTCPN_Structure.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            foreach (FieldInfo fi in fieldInfos)
                            {
                                object va = fi.GetValue(PPTCPN_Structure);
                                //propertyStr = fi.Name;
                                if (va == null)
                                {
                                    valueStr = "";
                                    Debug.Log("该" + fi.Name + "属性值为空");
                                }
                                else
                                {
                                    valueStr = va.ToString();
                                }
                                propertyStr = StructuresPPT_FieldToCHN(fi.Name);
                                PPTAddContent(propertyStr, valueStr);
                            }
                            break;

                        case "楼板":
                            PPTCPN_Floors = selectTransform.gameObject.GetComponent<Floors>();
                            fieldInfos = PPTCPN_Floors.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            foreach (FieldInfo fi in fieldInfos)
                            {
                                object va = fi.GetValue(PPTCPN_Floors);
                                //propertyStr = fi.Name;
                                if (va == null)
                                {
                                    valueStr = "";
                                    Debug.Log("该" + fi.Name + "属性值为空");
                                }
                                else
                                {
                                    valueStr = va.ToString();
                                }
                                propertyStr = FloorsPPT_FieldToCHN(fi.Name);
                                PPTAddContent(propertyStr, valueStr);
                            }
                            break;

                        case "结构框架":
                            PPTCPN_FrameWorks = selectTransform.gameObject.GetComponent<Frameworks>();
                            fieldInfos = PPTCPN_FrameWorks.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            foreach (FieldInfo fi in fieldInfos)
                            {
                                object va = fi.GetValue(PPTCPN_FrameWorks);
                                //propertyStr = fi.Name;
                                if (va == null)
                                {
                                    valueStr = "";
                                    //Debug.Log("该" + fi.Name + "属性值为空");
                                }
                                else
                                {
                                    valueStr = va.ToString();
                                }
                                propertyStr = FrameworksPPT_FieldToCHN(fi.Name);
                                PPTAddContent(propertyStr, valueStr);
                            }
                            break;

                        case "常规模型":
                            PPTCPN_Commmon = selectTransform.gameObject.GetComponent<Commons>();
                            fieldInfos = PPTCPN_Commmon.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                            foreach (FieldInfo fi in fieldInfos)
                            {
                                object va = fi.GetValue(PPTCPN_Commmon);
                                //propertyStr = fi.Name;
                                if (va == null)
                                {
                                    valueStr = "";
                                    Debug.Log("该" + fi.Name + "属性值为空");
                                }
                                else
                                {
                                    valueStr = va.ToString();
                                }
                                propertyStr = CommonsPPT_FieldToCHN(fi.Name);
                                PPTAddContent(propertyStr, valueStr);
                            }
                            break;

                    }
                    #endregion
                }
                else ComponentPanel.gameObject.SetActive(false);
                #endregion
            }
        }
     
    }

    private void PPTAddContent(string name, string value)
    {
        //Debug.Log("属性:" + name+ "_______"+"属性值" + value);
        GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/ComponentInfo", typeof(GameObject))) as GameObject;
        int i = 0;
        foreach (var item in go.transform.GetComponentsInChildren<Text>())
        {
            i++;
            if (i == 1)
            { item.text = name; }
            if (i == 2)
            { item.text = value; }
        }
        //Text leftText = go.transform.Find("ComponentLInfoImg(Clone)").GetComponent<Text>();
        //leftText.text = name;
        //Text rightText = go.transform.Find("ComponentRInfoImg(Clone)").GetComponent<Text>();
        //rightText.text = value;
        go.transform.SetParent(Content);
        go.transform.localScale = Vector3.one;
    }

    private string WallPPT_FieldToCHN(string name)
    {
        switch (name)
        {
            case "PDMODELID":
                return "PDModelID";
            case "PDSTMATERIAL":
                return "PDST材质";
            case "ALL_MODEL_MARK":
                return "标记";
            case "PHASE_DEMOLISHED":
                return "拆除的阶段";
            case "PHASE_CREATED":
                return "创建的阶段";
            case "WALL_BASE_OFFSET":
                return "底部偏移";
            case "WALL_BASE_CONSTRAINT":
                return "底部限制条件";
            case "WALL_BOTTOM_EXTENSION_DIST_PARAM":
                return "底部延伸距离";
            case "WALL_TOP_OFFSET":
                return "顶部偏移";
            case "WALL_TOP_EXTENSION_DIST_PARAM":
                return "顶部延伸距离";
            case "WALL_HEIGHT_TYPE":
                return "顶部约束";
            case "WALL_KEY_REF_PARAM":
                return "定位线";
            case "CLEAR_COVER_INTERIOR":
                return "钢筋保护层 - 内部面";
            case "WALL_ATTR_ROOM_BOUNDING":
                return "房间边界";
            case "CLEAR_COVER_OTHER":
                return "钢筋保护层 - 其他面";
            case "CLEAR_COVER_EXTERIOR":
                return "钢筋保护层 - 外部面";
            case "WALL_STRUCTURAL_SIGNIFICANT":
                return "结构";
            case "WALL_STRUCTURAL_USAGE_PARAM":
                return "结构用途";
            case "ELEM_CATEGORY_PARAM_MT":
                return "类别";
            case "ELEM_TYPE_PARAM":
                return "类型";
            case "SYMBOL_ID_PARAM":
                return "类型ID";
            case "ALL_MODEL_TYPE_NAME":
                return "类型名称";
            case "FLOORSNUM":
                return "楼层";
            case "HOST_AREA_COMPUTED":
                return "面积/㎡";
            case "STRUCTURAL_ANALYTICAL_MODEL":
                return "启用分析模型";
            case "DESIGN_OPTION_ID":
                return "设计选项";
            case "CONSTRUCTIONTIME":
                return "施工时间";
            case "HOST_VOLUME_COMPUTED":
                return "体积/m3";
           case "ALL_MODEL_IMAGE":
                return "图像";
            case "WALL_USER_HEIGHT_PARAM":
                return "无连接高度";
            case "WALL_BOTTOM_IS_ATTACHED":
                return "已附着底部";
            case "WALL_TOP_IS_ATTACHED":
                return "已附着顶部";
            case "RELATED_TO_MASS":
                return "与体量相关";
            case "CURVE_ELEM_LENGTH":
                return "长度/mm";
            case "ALL_MODEL_INSTANCE_COMMENTS":
                return "注释";
            case "ELEM_FAMILY_PARAM":
                return "族";
           case "ALL_MODEL_FAMILY_NAME":
                return "族名称";
            case "ELEM_FAMILY_AND_TYPE_PARAM":
                return "族与类型";
            default:
                return " ";
        }
    }

    string StructuresPPT_FieldToCHN(string name)
    {
        switch (name)
        {
            case "PDMODELID":
                return "PDModelID";
            case "LEVEL_PARAM":
                return "标高";
            case "ALL_MODEL_MARK":
                return "标记";
            case "PHASE_DEMOLISHED":
                return "拆除的阶段";
            case "PHASE_CREATED":
                return "创建的阶段";
            case "FAMILY_BASE_LEVEL_PARAM":
                return "底部标高";
            case "FAMILY_BASE_LEVEL_OFFSET_PARAM":
                return "底部偏移";
            case "SCHEDULE_TOP_LEVEL_PARAM":
                return "顶部标高";
            case "FAMILY_TOP_LEVEL_OFFSET_PARAM":
                return "顶部偏移";
            case "WALL_ATTR_ROOM_BOUNDING":
                return "房间边界";
            case "CLEAR_COVER_BOTTOM":
                return "钢筋保护底面";
            case "CLEAR_COVER_TOP":
                return "钢筋保护层顶面";
            case "CLEAR_COVER_OTHER":
                return "钢筋保护层其他面";
            case "STRUCTURAL_MATERIAL_PARAM":
                return "结构材质";
            case "ELEM_CATEGORY_PARAM_MT":
                return "类别";
            case "ELEM_TYPE_PARAM":
                return "类型";
            case "SYMBOL_ID_PARAM":
                return "类型ID";
            case "ALL_MODEL_TYPE_NAME":
                return "类型名称";
            case "FLOORSNUM":
                return "楼层";
            case "HOST_AREA_COMPUTED":
                return "面积/㎡";
            case "STRUCTURAL_ANALYTICAL_MODEL":
                return "启用分析模型";
            case "DESIGN_OPTION_ID":
                return "设计选项";
            case "CONSTRUCTIONTIME":
                return "施工时间";
            case "INSTANCE_MOVES_WITH_GRID_PARAM":
                return "随轴网移动";
            case "HOST_VOLUME_COMPUTED":
                return "体积/m3";
            case "ALL_MODEL_IMAGE":
                return "图像";
            case "CURVE_ELEM_LENGTH":
                return "长度/mm";
            case "HOST_ID_PARAM":
                return "主体ID";
            case "ALL_MODEL_INSTANCE_COMMENTS":
                return "注释";
            case "COLUMN_LOCATION_MARK":
                return "柱定位标记";
            case "SLANTED_COLUMN_TYPE_PARAM":
                return "柱样式";
            case "ELEM_FAMILY_PARAM":
                return "族";
            case "ALL_MODEL_FAMILY_NAME":
                return "族名称";
            case "ELEM_FAMILY_AND_TYPE_PARAM":
                return "族与类型";
            default:
                return " ";
        }
    }

    string FrameworksPPT_FieldToCHN(string name)
    {
        switch (name)
        {
            case "PDMODELID":
                return "PDModelID";
            case "Y_JUSTIFICATION":
                return "Y轴对正";
            case "Y_OFFSET_VALUE":
                return "Y轴偏移值";
            case "YZ_JUSTIFICATION":
                return "XY轴对正";
            case "Z_JUSTIFICATION":
                return "Z轴对正";
            case "Z_OFFSET_VALUE":
                return "Z轴偏移值";
            case "LEVEL_PARAM":
                return "标高";
            case "ALL_MODEL_MARK":
                return "标记";
            case "INSTANCE_REFERENCE_LEVEL_PARAM":
                return "参照标高";
            case "STRUCTURAL_REFERENCE_LEVEL_ELEVATION":
                return "参照标高高程";
            case "PHASE_DEMOLISHED":
                return "拆除的阶段";
            case "PHASE_CREATED":
                return "创建的阶段";
            case "STRUCTURAL_ELEVATION_AT_BOTTOM":
                return "底部高程";
            case "STRUCTURAL_ELEVATION_AT_TOP":
                return "顶部高程";
            case "STRUCTURAL_BEAM_ORIENTATION":
                return "方向";
            case "CLEAR_COVER_BOTTOM":
                return "钢筋保护底面";
            case "CLEAR_COVER_TOP":
                return "钢筋保护层顶面";
            case "CLEAR_COVER_OTHER":
                return "钢筋保护层其他面";
            case "SKETCH_PLANE_PARAM":
                return "工作平面";
            case "STRUCTURAL_BEND_DIR_ANGLE":
                return "横截面旋转";
            case "STRUCTURAL_FRAME_CUT_LENGTH":
                return "剪切长度";
            case "STRUCTURAL_MATERIAL_PARAM":
                return "结构材质";
            case "INSTANCE_STRUCT_USAGE_PARAM":
                return "结构用途";
            case "ELEM_CATEGORY_PARAM_MT":
                return "类别";
            case "ELEM_TYPE_PARAM":
                return "类型";
            case "SYMBOL_ID_PARAM":
                return "类型ID";
            case "ALL_MODEL_TYPE_NAME":
                return "类型名称";
            case "STRUCT_FRAM_JOIN_STATUS":
                return "连接状态";
            case "FLOORSNUM":
                return "楼层";
            case "HOST_AREA_COMPUTED":
                return "面积/㎡";
            case "STRUCTURAL_ANALYTICAL_MODEL":
                return "启用分析模型";
            case "STRUCTURAL_BEAM_END0_ELEVATION":
                return "起点标高偏移";
            case "DESIGN_OPTION_ID":
                return "设计选项";
            case "CONSTRUCTIONTIME":
                return "施工时间";
            case "HOST_VOLUME_COMPUTED":
                return "体积/m3";
            case "ALL_MODEL_IMAGE":
                return "图像";
            case "CURVE_ELEM_LENGTH":
                return "长度/mm";
            case "STRUCTURAL_BEAM_END1_ELEVATION":
                return "终点标高偏移";
            case "HOST_ID_PARAM":
                return "主体ID";
            case "ALL_MODEL_INSTANCE_COMMENTS":
                return "注释";
            case "ELEM_FAMILY_PARAM":
                return "族";
            case "ALL_MODEL_FAMILY_NAME":
                return "族名称";
            case "ELEM_FAMILY_AND_TYPE_PARAM":
                return "族与类型";
            default:
                return " ";
        }
    }

    string FloorsPPT_FieldToCHN(string name)
    {
        switch (name)
        {
            case "PDMODELID":
                return "PDModelID";
            case "PDSTMATERIAL":
                return "PDST材质";
            case "LEVEL_PARAM":
                return "标高";
            case "ALL_MODEL_MARK":
                return "标记";
            case "PHASE_DEMOLISHED":
                return "拆除的阶段";
            case "PHASE_CREATED":
                return "创建的阶段";
            case "STRUCTURAL_ELEVATION_AT_BOTTOM":
                return "底部高程";
            case "STRUCTURAL_ELEVATION_AT_BOTTOM_CORE":
                return "底部核心高程";
            case "STRUCTURAL_ELEVATION_AT_TOP":
                return "顶部高程";
            case "STRUCTURAL_ELEVATION_AT_TOP_CORE":
                return "顶部核心高程";
            case "WALL_ATTR_ROOM_BOUNDING":
                return "房间边界";
            case "CLEAR_COVER_BOTTOM":
                return "钢筋保护底面";
            case "CLEAR_COVER_TOP":
                return "钢筋保护层顶面";
            case "CLEAR_COVER_OTHER":
                return "钢筋保护层其他面";
            case "FLOOR_ATTR_THICKNESS_PARAM":
                return "厚度";
            case "WALL_STRUCTURAL_SIGNIFICANT":
                return "结构";
            case "ELEM_CATEGORY_PARAM_MT":
                return "类别";
            case "ELEM_TYPE_PARAM":
                return "类型";
            case "SYMBOL_ID_PARAM":
                return "类型ID";
            case "ALL_MODEL_TYPE_NAME":
                return "类型名称";
            case "FLOORSNUM":
                return "楼层";
            case "HOST_AREA_COMPUTED":
                return "面积/㎡";
            case "ROOF_SLOPE":
                return "坡度";
            case "STRUCTURAL_ANALYTICAL_MODEL":
                return "启用分析模型";
            case "DESIGN_OPTION_ID":
                return "设计选项";
            case "CONSTRUCTIONTIME":
                return "施工时间";
            case "HOST_VOLUME_COMPUTED":
                return "体积/m3";
            case "ALL_MODEL_IMAGE":
                return "图像";
            case "RELATED_TO_MASS":
                return "与体量相关";
            case "HOST_PERIMETER_COMPUTED":
                return "周长/mm";
            case "ALL_MODEL_INSTANCE_COMMENTS":
                return "注释";
            case "FLOOR_HEIGHTABOVELEVEL_PARAM":
                return "自标高的高度偏移";
            case "ELEM_FAMILY_PARAM":
                return "族";
            case "ALL_MODEL_FAMILY_NAME":
                return "族名称";
            case "ELEM_FAMILY_AND_TYPE_PARAM":
                return "族与类型";
            default:
                return " ";
        }
    }

    string CommonsPPT_FieldToCHN(string name)
    {
        switch (name)
        {
            case "PDMODELID":
                return "PDModelID";
            case "LEVEL_PARAM":
                return "标高";
            case "ALL_MODEL_MARK":
                return "标记";
            case "PHASE_DEMOLISHED":
                return "拆除的阶段";
            case "PHASE_CREATED":
                return "创建的阶段";
            case "CAVE_WIDTH":
                return "洞口宽";
            case "CAVE_LONG":
                return "洞口长";
            case "SKETCH_PLANE_PARAM":
                return "工作平面";
            case "ELEM_CATEGORY_PARAM_MT":
                return "类别";
            case "ELEM_TYPE_PARAM":
                return "类型";
            case "SYMBOL_ID_PARAM":
                return "类型ID";
            case "ALL_MODEL_TYPE_NAME":
                return "类型名称";
            case "FLOORSNUM":
                return "楼层";
            case "HOST_AREA_COMPUTED":
                return "面积/㎡";
            case "INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM":
                return "明细表标高";
            case "INSTANCE_FREE_HOST_OFFSET_PARAM":
                return "偏移量";
            case "DESIGN_OPTION_ID":
                return "设计选项";
            case "CONSTRUCTIONTIME":
                return "施工时间";
            case "HOST_VOLUME_COMPUTED":
                return "体积/m3";
            case "ALL_MODEL_IMAGE":
                return "图像";
            case "TOP_OF_POINT":
                return "线顶点";
            case "HOST_ID_PARAM":
                return "主体ID";
            case "ALL_MODEL_INSTANCE_COMMENTS":
                return "注释";
            case "ELEM_FAMILY_PARAM":
                return "族";
            case "ALL_MODEL_FAMILY_NAME":
                return "族名称";
            case "ELEM_FAMILY_AND_TYPE_PARAM":
                return "族与类型";
            default:
                return " ";
        }
    }
}
