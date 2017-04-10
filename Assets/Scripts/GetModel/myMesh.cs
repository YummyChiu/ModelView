using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine.EventSystems;

using Mono.Data.Sqlite;
using System;
using TinyTeam.UI;

//, IPointerDownHandler
public class myMesh : MonoBehaviour,IPointerDownHandler
{
    private Mesh mesh;

    public ModelMessage modelMessage;
  
    public void Init(ModelMessage modelmessage)
    {
        modelMessage = modelmessage;

        DrawModel();
    }



    public void DrawModel()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        string[] myName = name.Split('_');
        Material mat;

        switch (myName[0])
        {
            case "墙":
                mat = Resources.Load("Materials/BVMaterial2") as Material;

                break;
            case "结构框架":
                mat = Resources.Load("Materials/BVMaterial3") as Material;
                break;
            case "楼板":
                mat = Resources.Load("Materials/BVMaterial") as Material;
                break;
            case "卫浴装置":
                mat = Resources.Load("Materials/BVMaterial2") as Material;
                break;
            case "门":
                mat = Resources.Load("Materials/BVMaterial2") as Material;
                break;
            case "窗":
                mat = Resources.Load("Materials/BVMaterial2") as Material;
                break;
            case "常规模型":
                mat = Resources.Load("Materials/BVMaterial2") as Material;
                break;
            default:
                mat = Resources.Load("Materials/BVMaterial2") as Material;
                break;
        }
        this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;



        mesh.vertices = modelMessage.Vertices;//添加顶点到网格
        mesh.triangles = modelMessage.Triangles;//添加顺序数组到网格绘制
        mesh.normals = modelMessage.Normals;
        mesh.RecalculateBounds();
        this.gameObject.AddComponent<MeshCollider>();//添加一个碰撞器，以便以后鼠标进行交互操作
    }

    public void DrawModel(int id, string name, int[] triangles, Vector3[] vertices, Vector3[] normals, SqliteConnection conn, string color = "")
    {
        mesh = GetComponent<MeshFilter>().mesh;
        string[] myName = name.Split('_');
        if (color == "")//如果物体的颜色值为空，则设置颜色值。
        {
            if (myName[0] == "墙")
            {
                Material mat = Resources.Load("Materials/BVMaterial2") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "结构框架")
            {
                Material mat = Resources.Load("Materials/BVMaterial3") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "楼板")
            {
                Material mat = Resources.Load("Materials/BVMaterial") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "卫浴装置" || myName[0] == "专用设备")
            {
                Material mat = Resources.Load("Materials/BathRoom") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "门")
            {
                Material mat = Resources.Load("Materials/Doors") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "窗")
            {
                Material mat = Resources.Load("Materials/Windows") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }
            if (myName[0] == "常规模型")
            {
                Material mat = Resources.Load("Materials/Normals") as Material;
                this.gameObject.GetComponent<Renderer>().sharedMaterial = mat;
            }

        }
        mesh.vertices = vertices;//添加顶点到网格
        mesh.triangles = triangles;//添加顺序数组到网格绘制
        mesh.normals = normals;
        mesh.RecalculateBounds();
        this.gameObject.AddComponent<MeshCollider>();//添加一个碰撞器，以便以后鼠标进行交互操作
        InsertAttribute(name, conn);//调用该方法把属性添加到该物体上面去

    }
    private void InsertAttribute(string goName, SqliteConnection conn)
    {
        //以下是添加物体的所有属性到物体
        string[] strName = goName.Split('_');//获取物体的名称
                                             // string s = strName[0];
        switch (strName[0])
        {
            case "墙":
                this.gameObject.AddComponent<Walls>();//得到属性类组件
                Walls wall = this.gameObject.GetComponent<Walls>();//得到属性类
                GetAttributeMessage(wall, goName);//得到属性的各个值
                break;
            case "楼板":
                this.gameObject.AddComponent<Floors>();//得到属性类组件
                Floors floors = this.gameObject.GetComponent<Floors>();//得到属性类
                GetAttributeMessage(floors, goName);//得到属性的各个值
                break;
            case "结构柱":
                this.gameObject.AddComponent<Structures>();//得到属性类组件
                //Structures structure = this.gameObject.GetComponent<Structures>();//得到属性类
                //GetAttributeMessage(structure, goName);//得到属性的各个值
                break;
            case "结构框架":
                this.gameObject.AddComponent<Frameworks>();//得到属性类组件
                Frameworks framework = this.gameObject.GetComponent<Frameworks>();//得到属性类
                GetAttributeMessage(framework, goName);//得到属性的各个值
                break;
            case "常规模型":
                this.gameObject.AddComponent<Commons>();//得到属性类组件
                //Commons common = this.gameObject.GetComponent<Commons>();//得到属性类
                //GetAttributeMessage(common, goName);//得到属性的各个值
                break;
        }
    }
    private void GetAttributeMessage(Walls myWall, string myId)//墙属性
    {
        string[] attName = myId.Split('_');//把名称拆分出来（格式为：物体名称_ID_时间）
        string myAttName = attName[0] + "_" + attName[1];//把物体的名称和ID重新组合成一个名称，以便查询属性表 
        Walls w = ReadModelSQLite.dictionaryWall[int.Parse(attName[1])];

        // myWall = w;
        // myWall._Model_Id = w._Model_Id;
        myWall._PDModelID = w._PDModelID;
        myWall._PDSTMATERIAL = w._PDSTMATERIAL;
        myWall._ALL_MODEL_MARK = w._ALL_MODEL_MARK;
        myWall._PHASE_DEMOLISHED = w._PHASE_DEMOLISHED;
        myWall._PHASE_CREATED = w._PHASE_CREATED;
        myWall._WALL_BASE_OFFSET = w._WALL_BASE_OFFSET;
        myWall._WALL_BASE_CONSTRAINT = w._WALL_BASE_CONSTRAINT;
        myWall._WALL_BOTTOM_EXTENSION_DIST_PARAM = w._WALL_BOTTOM_EXTENSION_DIST_PARAM;
        myWall._WALL_TOP_OFFSET = w._WALL_TOP_OFFSET;
        myWall._WALL_TOP_EXTENSION_DIST_PARAM = w._WALL_TOP_EXTENSION_DIST_PARAM;
        myWall._WALL_HEIGHT_TYPE = w._WALL_HEIGHT_TYPE;
        myWall._WALL_KEY_REF_PARAM = w._WALL_KEY_REF_PARAM;
        myWall._WALL_ATTR_ROOM_BOUNDING = w._WALL_ATTR_ROOM_BOUNDING;
        myWall._CLEAR_COVER_INTERIOR = w._CLEAR_COVER_INTERIOR;
        myWall._CLEAR_COVER_OTHER = w._CLEAR_COVER_OTHER;
        myWall._CLEAR_COVER_EXTERIOR = w._CLEAR_COVER_EXTERIOR;
        myWall._WALL_STRUCTURAL_SIGNIFICANT = w._WALL_STRUCTURAL_SIGNIFICANT;
        myWall._WALL_STRUCTURAL_USAGE_PARAM = w._WALL_STRUCTURAL_USAGE_PARAM;
        myWall._ELEM_CATEGORY_PARAM_MT = w._ELEM_CATEGORY_PARAM_MT;
        myWall._ELEM_TYPE_PARAM = w._ELEM_TYPE_PARAM;
        myWall._SYMBOL_ID_PARAM = w._SYMBOL_ID_PARAM;
        myWall._ALL_MODEL_TYPE_NAME = w._ALL_MODEL_TYPE_NAME;
        myWall._FLOORSNUM = w._FLOORSNUM;
        myWall._HOST_AREA_COMPUTED = w._HOST_AREA_COMPUTED;
        myWall._STRUCTURAL_ANALYTICAL_MODEL = w._STRUCTURAL_ANALYTICAL_MODEL;
        myWall._DESIGN_OPTION_ID = w._DESIGN_OPTION_ID;
        myWall._CONSTRUCTIONTIME = w._CONSTRUCTIONTIME;
        myWall._HOST_VOLUME_COMPUTED = w._HOST_VOLUME_COMPUTED;
        myWall._ALL_MODEL_IMAGE = w._ALL_MODEL_IMAGE;
        myWall._WALL_USER_HEIGHT_PARAM = w._WALL_USER_HEIGHT_PARAM;
        myWall._WALL_BOTTOM_IS_ATTACHED = w._WALL_BOTTOM_IS_ATTACHED;
        myWall._WALL_TOP_IS_ATTACHED = w._WALL_TOP_IS_ATTACHED;
        myWall._RELATED_TO_MASS = w._RELATED_TO_MASS;
        myWall._CURVE_ELEM_LENGTH = w._CURVE_ELEM_LENGTH;
        myWall._ALL_MODEL_INSTANCE_COMMENTS = w._ALL_MODEL_INSTANCE_COMMENTS;
        myWall._ELEM_FAMILY_PARAM = w._ELEM_FAMILY_PARAM;
        myWall._ALL_MODEL_FAMILY_NAME = w._ALL_MODEL_FAMILY_NAME;
        myWall._ELEM_FAMILY_AND_TYPE_PARAM = w._ELEM_FAMILY_AND_TYPE_PARAM;
        //myWall._ANZHUGUJING = w._ANZHUGUJING;
        //myWall._ANZHUGUJINLEIXING = w._ANZHUGUJINLEIXING;
        //myWall._ANZHUZONGJING = w._ANZHUZONGJING;
        //myWall._WallNum = w._WallNum;
        //myWall._WallLAJING = w._WallLAJING;
        //myWall._IFSETREBAR = w._IFSETREBAR;
        //myWall._VERTICALSETREBAR = w._VERTICALSETREBAR;
        //myWall._HORIZONTALSETREBAR = w._HORIZONTALSETREBAR;
        myWall.WALL_ATTR_ROOM_BOUNDING = w.WALL_ATTR_ROOM_BOUNDING;

        //SqliteCommand cmd = conn.CreateCommand();
        //cmd.CommandText = "select * from Param_Wall where Model_Id = " + attName[1];
        //SqliteDataReader dr = cmd.ExecuteReader();

        //while (dr.Read())
        //{
        //    for (int i = 0; i < dr.FieldCount; i++)
        //    {
        //        switch (dr.GetName(i))
        //        {
        //            case "PDModelID":
        //                myWall._PDModelID = dr.GetString(i);
        //                break;
        //            case "PDST材质":
        //                myWall._PDSTMATERIAL = dr.GetInt32(i);
        //                break;
        //            case "标记":
        //                myWall._ALL_MODEL_MARK = dr.GetString(i);
        //                break;
        //            case "拆除的阶段":
        //                myWall._PHASE_DEMOLISHED = dr.GetInt32(i);
        //                break;
        //            case "创建的阶段":
        //                myWall._PHASE_CREATED = dr.GetInt32(i);
        //                break;
        //            case "底部偏移":
        //                myWall._WALL_BASE_OFFSET = dr.GetFloat(i);
        //                break;
        //            case "底部限制条件":
        //                myWall._WALL_BASE_CONSTRAINT = dr.GetInt32(i);
        //                break;
        //            case "底部延伸距离":
        //                myWall._WALL_BOTTOM_EXTENSION_DIST_PARAM = dr.GetFloat(i);
        //                break;
        //            case "顶部偏移":
        //                myWall._WALL_TOP_OFFSET = dr.GetFloat(i);
        //                break;
        //            case "顶部延伸距离":
        //                myWall._WALL_TOP_EXTENSION_DIST_PARAM = dr.GetFloat(i);
        //                break;
        //            case "顶部约束":
        //                myWall._WALL_HEIGHT_TYPE = dr.GetInt32(i);
        //                break;
        //            case "定位线":
        //                myWall._WALL_KEY_REF_PARAM = dr.GetInt32(i);
        //                break;
        //            case "房间边界":
        //                myWall._WALL_ATTR_ROOM_BOUNDING = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层内部面":
        //                myWall._CLEAR_COVER_INTERIOR = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层其他面":
        //                myWall._CLEAR_COVER_OTHER = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层外部面":
        //                myWall._CLEAR_COVER_EXTERIOR = dr.GetInt32(i);
        //                break;
        //            case "结构":
        //                myWall._WALL_STRUCTURAL_SIGNIFICANT = dr.GetInt32(i);
        //                break;
        //            case "结构用途":
        //                myWall._WALL_STRUCTURAL_USAGE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类别":
        //                myWall._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
        //                break;
        //            case "类型":
        //                myWall._ELEM_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型ID":
        //                myWall._SYMBOL_ID_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型名称":
        //                myWall._ALL_MODEL_TYPE_NAME = dr.GetString(i);
        //                break;
        //            case "楼层":
        //                myWall._FLOORSNUM = dr.GetString(i);
        //                ReadModelXML.idAndLevels.Add("ID" + attName[1], dr.GetString(i));//把id和楼层添加到字典里面去
        //                break;
        //            case "面积":
        //                myWall._HOST_AREA_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "启用分析模型":
        //                myWall._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
        //                break;
        //            case "设计选项":
        //                myWall._DESIGN_OPTION_ID = dr.GetInt32(i);
        //                break;
        //            case "施工时间":
        //                myWall._CONSTRUCTIONTIME = dr.GetString(i);
        //                break;
        //            case "体积":
        //                myWall._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "图像":
        //                myWall._ALL_MODEL_IMAGE = dr.GetInt32(i);
        //                break;
        //            case "无连接高度":
        //                myWall._WALL_USER_HEIGHT_PARAM = dr.GetFloat(i);
        //                break;
        //            case "已附着底部":
        //                myWall._WALL_BOTTOM_IS_ATTACHED = dr.GetInt32(i);
        //                break;
        //            case "已附着顶部":
        //                myWall._WALL_TOP_IS_ATTACHED = dr.GetInt32(i);
        //                break;
        //            case "与体量相关":
        //                myWall._RELATED_TO_MASS = dr.GetInt32(i);
        //                break;
        //            case "长度":
        //                myWall._CURVE_ELEM_LENGTH = dr.GetFloat(i);
        //                break;
        //            case "注释":
        //                myWall._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
        //                break;
        //            case "族":
        //                myWall._ELEM_FAMILY_PARAM = dr.GetInt32(i);
        //                break;
        //            case "族名称":
        //                myWall._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
        //                break;
        //            case "族与类型":
        //                myWall._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "Model_ID":
        //                myWall.Model_Id = dr.GetInt32(i);
        //                break;
        //            case "暗柱箍筋":
        //                myWall._ANZHUGUJING = dr.GetString(i);
        //                break;
        //            case "暗柱箍筋类型":
        //                myWall._ANZHUGUJINLEIXING = dr.GetString(i);
        //                break;
        //            case "暗柱纵筋":
        //                myWall._ANZHUZONGJING = dr.GetString(i);
        //                break;
        //            case "墙编号":
        //                myWall._WallNum = dr.GetString(i);
        //                break;
        //            case "墙身拉筋":
        //                myWall._WallLAJING = dr.GetString(i);
        //                break;
        //            case "是否已配筋":
        //                myWall._IFSETREBAR = dr.GetInt32(i);
        //                break;
        //            case "竖向分布钢筋":
        //                myWall._VERTICALSETREBAR = dr.GetString(i);
        //                break;
        //            case "水平分布钢筋":
        //                myWall._HORIZONTALSETREBAR = dr.GetString(i);
        //                break;
        //        }
        //    }
        //}
    }
    private void GetAttributeMessage(Floors floors, string myId)//楼板属性
    {
        string[] attName = myId.Split('_');//把名称拆分出来（格式为：物体名称_ID_时间）
        string myAttName = attName[0] + "_" + attName[1];//把物体的名称和ID重新组合成一个名称，以便查询属性表 
        Floors f = ReadModelSQLite.dictionaryFloors[int.Parse(attName[1])];

        // floors._Model_Id = f._Model_Id;
        floors._PDModelID = f._PDModelID;
        floors._PDSTMATERIAL = f._PDSTMATERIAL;
        floors._LEVEL_PARAM = f._LEVEL_PARAM;
        floors._ALL_MODEL_MARK = f._ALL_MODEL_MARK;
        floors._PHASE_DEMOLISHED = f._PHASE_DEMOLISHED;
        floors._PHASE_CREATED = f._PHASE_CREATED;
        floors._STRUCTURAL_ELEVATION_AT_BOTTOM = f._STRUCTURAL_ELEVATION_AT_BOTTOM;
        floors._STRUCTURAL_ELEVATION_AT_BOTTOM_CORE = f._STRUCTURAL_ELEVATION_AT_BOTTOM_CORE;
        floors._STRUCTURAL_ELEVATION_AT_TOP = f._STRUCTURAL_ELEVATION_AT_TOP;
        floors._STRUCTURAL_ELEVATION_AT_TOP_CORE = f._STRUCTURAL_ELEVATION_AT_TOP_CORE;
        floors._WALL_ATTR_ROOM_BOUNDING = f._WALL_ATTR_ROOM_BOUNDING;
        floors._CLEAR_COVER_BOTTOM = f._CLEAR_COVER_BOTTOM;
        floors._CLEAR_COVER_TOP = f._CLEAR_COVER_TOP;
        floors._CLEAR_COVER_OTHER = f._CLEAR_COVER_OTHER;
        floors._FLOOR_ATTR_THICKNESS_PARAM = f._FLOOR_ATTR_THICKNESS_PARAM;
        floors._WALL_STRUCTURAL_SIGNIFICANT = f._WALL_STRUCTURAL_SIGNIFICANT;
        floors._ELEM_CATEGORY_PARAM_MT = f._ELEM_CATEGORY_PARAM_MT;
        floors._ELEM_TYPE_PARAM = f._ELEM_TYPE_PARAM;
        floors._SYMBOL_ID_PARAM = f._SYMBOL_ID_PARAM;
        floors._ALL_MODEL_TYPE_NAME = f._ALL_MODEL_TYPE_NAME;
        floors._FLOORSNUM = f._FLOORSNUM;
        floors._HOST_AREA_COMPUTED = f._HOST_AREA_COMPUTED;
        floors._ROOF_SLOPE = f._ROOF_SLOPE;
        floors._STRUCTURAL_ANALYTICAL_MODEL = f._STRUCTURAL_ANALYTICAL_MODEL;
        floors._DESIGN_OPTION_ID = f._DESIGN_OPTION_ID;
        floors._CONSTRUCTIONTIME = f._CONSTRUCTIONTIME;
        floors._HOST_VOLUME_COMPUTED = f._HOST_VOLUME_COMPUTED;
        floors._ALL_MODEL_IMAGE = f._ALL_MODEL_IMAGE;
        floors._RELATED_TO_MASS = f._RELATED_TO_MASS;
        floors._HOST_PERIMETER_COMPUTED = f._HOST_PERIMETER_COMPUTED;
        floors._ALL_MODEL_INSTANCE_COMMENTS = f._ALL_MODEL_INSTANCE_COMMENTS;
        floors._FLOOR_HEIGHTABOVELEVEL_PARAM = f._FLOOR_HEIGHTABOVELEVEL_PARAM;
        floors._ELEM_FAMILY_PARAM = f._ELEM_FAMILY_PARAM;
        floors._ALL_MODEL_FAMILY_NAME = f._ALL_MODEL_FAMILY_NAME;
        floors._ELEM_FAMILY_AND_TYPE_PARAM = f._ELEM_FAMILY_AND_TYPE_PARAM;
        //floors._XDirectBottom = f._XDirectBottom;
        //floors._XDirectFace = f._XDirectFace;
        //floors._YDirectBottom = f._YDirectBottom;
        //floors._YDirectFace = f._YDirectFace;
        //floors._FloorsNum = f._FloorsNum;
        //floors._DistributeRebar = f._DistributeRebar;
        //floors._RebarUnionNum = f._RebarUnionNum;
        //floors._IFSETREBAR = f._IFSETREBAR;
        //floors._HOST_SSE_CURVED_EDGE_CONDITION_PARAM = f._HOST_SSE_CURVED_EDGE_CONDITION_PARAM;
        //floors._TemperatureRebar = f._TemperatureRebar;





        #region 旧代码


        //SqliteCommand cmd = conn.CreateCommand();
        //cmd.CommandText = "select * from Param_Floors where Model_Id = " + attName[1];
        //SqliteDataReader dr = cmd.ExecuteReader();

        //while (dr.Read())
        //{
        //    for (int i = 0; i < dr.FieldCount; i++)
        //    {
        //        switch (dr.GetName(i))
        //        {
        //            case "PDModelID":
        //                floors._PDModelID = dr.GetString(i);
        //                break;
        //            case "PDST材质":
        //                floors._PDSTMATERIAL = dr.GetInt32(i);
        //                break;
        //            case "标高":
        //                floors._LEVEL_PARAM = dr.GetInt32(i);
        //                break;
        //            case "标记":
        //                floors._ALL_MODEL_MARK = dr.GetString(i);
        //                break;
        //            case "拆除的阶段":
        //                floors._PHASE_DEMOLISHED = dr.GetInt32(i);
        //                break;
        //            case "创建的阶段":
        //                floors._PHASE_CREATED = dr.GetInt32(i);
        //                break;
        //            case "底部高程":
        //                floors._STRUCTURAL_ELEVATION_AT_BOTTOM = dr.GetFloat(i);
        //                break;
        //            case "底部核心高程":
        //                floors._STRUCTURAL_ELEVATION_AT_BOTTOM_CORE = dr.GetFloat(i);
        //                break;
        //            case "顶部高程":
        //                floors._STRUCTURAL_ELEVATION_AT_TOP = dr.GetFloat(i);
        //                break;
        //            case "顶部核心高程":
        //                floors._STRUCTURAL_ELEVATION_AT_TOP_CORE = dr.GetFloat(i);
        //                break;
        //            case "房间边界":
        //                floors._WALL_ATTR_ROOM_BOUNDING = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层底面":
        //                floors._CLEAR_COVER_BOTTOM = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层顶面":
        //                floors._CLEAR_COVER_TOP = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层其他面":
        //                floors._CLEAR_COVER_OTHER = dr.GetInt32(i);
        //                break;
        //            case "厚度":
        //                floors._FLOOR_ATTR_THICKNESS_PARAM = dr.GetFloat(i);
        //                break;
        //            case "结构":
        //                floors._WALL_STRUCTURAL_SIGNIFICANT = dr.GetInt32(i);
        //                break;
        //            case "类别":
        //                floors._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
        //                break;
        //            case "类型":
        //                floors._ELEM_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型ID":
        //                floors._SYMBOL_ID_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型名称":
        //                floors._ALL_MODEL_TYPE_NAME = dr.GetString(i);
        //                break;
        //            case "楼层":
        //                floors._FLOORSNUM = dr.GetString(i);
        //                ReadModelXML.idAndLevels.Add("ID" + attName[1], dr.GetString(i));//把id和楼层添加到字典里面去 
        //                break;
        //            case "面积":
        //                floors._HOST_AREA_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "坡度":
        //                floors._ROOF_SLOPE = dr.GetFloat(i);
        //                break;
        //            case "启用分析模型":
        //                floors._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
        //                break;
        //            case "设计选项":
        //                floors._DESIGN_OPTION_ID = dr.GetInt32(i);
        //                break;
        //            case "施工时间":
        //                floors._CONSTRUCTIONTIME = dr.GetString(i);
        //                break;
        //            case "体积":
        //                floors._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "图像":
        //                floors._ALL_MODEL_IMAGE = dr.GetInt32(i);
        //                break;
        //            case "与体量相关":
        //                floors._RELATED_TO_MASS = dr.GetInt32(i);
        //                break;
        //            case "周长":
        //                floors._HOST_PERIMETER_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "注释":
        //                floors._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
        //                break;
        //            case "自标高的高度偏移":
        //                floors._FLOOR_HEIGHTABOVELEVEL_PARAM = dr.GetFloat(i);
        //                break;
        //            case "族":
        //                floors._ELEM_FAMILY_PARAM = dr.GetInt32(i);
        //                break;
        //            case "族名称":
        //                floors._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
        //                break;
        //            case "族与类型":
        //                floors._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "Model_Id":
        //                floors.Model_Id = dr.GetInt32(i);
        //                break;
        //            case "X方向底筋":
        //                floors.XDirectBottom = dr.GetString(i);
        //                break;
        //            case "X方向面筋":
        //                floors.XDirectFace = dr.GetString(i);
        //                break;
        //            case "Y方向底筋":
        //                floors.YDirectBottom = dr.GetString(i);
        //                break;
        //            case "Y方向面筋":
        //                floors.YDirectFace = dr.GetString(i);
        //                break;
        //            case "板编号":
        //                floors.FloorsNum = dr.GetString(i);
        //                break;
        //            case "分布筋":
        //                floors.DistributeRebar = dr.GetString(i);
        //                break;
        //            case "钢筋关联板编号":
        //                floors.RebarUnionNum = dr.GetString(i);
        //                break;
        //            case "是否已配筋":
        //                floors.IFSETREBAR = dr.GetInt32(i);
        //                break;
        //            case "弯曲边缘条件":
        //                floors.HOST_SSE_CURVED_EDGE_CONDITION_PARAM = dr.GetInt32(i);
        //                break;
        //            case "温度筋":
        //                floors.TemperatureRebar = dr.GetString(i);
        //                break;
        //        }
        //    }
        //}
        #endregion
    }
    private void GetAttributeMessage(Structures structure, string myId)//柱属性
    {
        string[] attName = myId.Split('_');//把名称拆分出来（格式为：物体名称_ID_时间）
        string myAttName = attName[0] + "_" + attName[1];//把物体的名称和ID重新组合成一个名称，以便查询属性表         
        //XmlDocument doc = XMLReader._intance.docAtr;
        //XmlNodeList nodeList = doc.SelectSingleNode("/Atribute/" + myAttName).ChildNodes;//查找属性表的特定节点
        XmlDocument doc = new XmlDocument();
        string strAtr = Resources.Load("Atributes/" + attName[1]).ToString();
        doc.LoadXml(strAtr);

        //string path = ReadModelXML._instance.strGeometryPath;
        //path = path.Replace("\\myRevitMessage.xml", "");
        //string strAtr = path + "/Atributes/" + attName[1] + ".xml";
        //doc.Load(strAtr);
        XmlNodeList nodeList = doc.SelectSingleNode("/Atribute/" + myAttName).ChildNodes;//查找属性表的特定节点
        foreach (XmlElement xe in nodeList)
        {
            switch (xe.Name)
            {
                case "PDModelID":
                    structure._PDModelID = xe.InnerText;
                    break;
                //case "PDST_C_B01":
                //    structure._PDST_C_B01 = xe.InnerText;
                //    break;
                //case "PDST_C_BID":
                //    structure._PDST_C_BID = xe.InnerText;
                //    break;
                //case "PDST_C_FC01":
                //    structure._PDST_C_FC01 = xe.InnerText;
                //    break;
                //case "PDST_C_FC02":
                //    structure._PDST_C_FC02 = xe.InnerText;
                //    break;
                //case "PDST_C_FC03":
                //    structure._PDST_C_FC03 = xe.InnerText;
                //    break;
                //case "PDST_C_FC04":
                //    structure._PDST_C_FC04 = xe.InnerText;
                //    break;
                //case "PDST_C_FCID":
                //    structure._PDST_C_FCID = xe.InnerText;
                //    break;
                //case "PDST_C_H01":
                //    structure._PDST_C_H01 = xe.InnerText;
                //    break;
                //case "PDST_C_P1":
                //    structure._PDST_C_P1 = xe.InnerText;
                //    break;
                //case "PDST_C_P2":
                //    structure._PDST_C_P2 = xe.InnerText;
                //    break;
                //case "PDST_C_P3":
                //    structure._PDST_C_P3 = xe.InnerText;
                //    break;
                //case "PDST_C_P4":
                //    structure._PDST_C_P4 = xe.InnerText;
                //    break;
                //case "PDST_P1":
                //    structure._PDST_P1 = xe.InnerText;
                //    break;
                //case "PDST_P2":
                //    structure._PDST_P2 = xe.InnerText;
                //    break;
                //case "PDST_P3":
                //    structure._PDST_P3 = xe.InnerText;
                //    break;
                case "标高":
                    structure._LEVEL_PARAM = int.Parse(xe.InnerText);
                    break;
                case "标记":
                    structure._ALL_MODEL_MARK = xe.InnerText;
                    break;
                case "拆除的阶段":
                    structure._PHASE_DEMOLISHED = xe.InnerText;
                    break;
                case "创建的阶段":
                    structure._PHASE_CREATED = xe.InnerText;
                    break;
                case "底部标高":
                    structure._FAMILY_BASE_LEVEL_PARAM = int.Parse(xe.InnerText);

                    break;
                case "底部偏移":
                    structure._FAMILY_BASE_LEVEL_OFFSET_PARAM = int.Parse(xe.InnerText);
                    break;
                case "顶部标高":
                    structure._SCHEDULE_TOP_LEVEL_PARAM = int.Parse(xe.InnerText);
                    break;
                case "顶部偏移":
                    structure._FAMILY_TOP_LEVEL_OFFSET_PARAM = int.Parse(xe.InnerText);
                    break;
                case "房间边界":
                    structure._WALL_ATTR_ROOM_BOUNDING = int.Parse(xe.InnerText);
                    break;
                case "钢筋保护层底面":
                    structure._CLEAR_COVER_BOTTOM = int.Parse(xe.InnerText);
                    break;
                case "钢筋保护层顶面":
                    structure._CLEAR_COVER_TOP = int.Parse(xe.InnerText);
                    break;
                case "钢筋保护层其他面":
                    structure._CLEAR_COVER_OTHER = int.Parse(xe.InnerText);
                    break;
                case "结构材质":
                    structure._STRUCTURAL_MATERIAL_PARAM = int.Parse(xe.InnerText);
                    break;
                case "类别":
                    structure._ELEM_CATEGORY_PARAM_MT = double.Parse(xe.InnerText);
                    break;
                case "类型":
                    structure._ELEM_TYPE_PARAM = int.Parse(xe.InnerText);
                    break;
                case "类型ID":
                    structure._SYMBOL_ID_PARAM = double.Parse(xe.InnerText);
                    break;
                case "类型名称":
                    structure._ALL_MODEL_TYPE_NAME = xe.InnerText;
                    break;
                case "楼层":
                    structure._FLOORSNUM = xe.InnerText;
                    ReadModelXML.idAndLevels.Add("ID" + attName[1], xe.InnerText);//把id和楼层添加到字典里面去 
                    break;
                case "面积":
                    structure._HOST_AREA_COMPUTED = double.Parse(xe.InnerText);
                    break;
                case "启用分析模型":
                    structure._STRUCTURAL_ANALYTICAL_MODEL = int.Parse(xe.InnerText);
                    break;
                case "设计选项":
                    structure._DESIGN_OPTION_ID = xe.InnerText;
                    break;
                case "施工时间":
                    structure._CONSTRUCTIONTIME = xe.InnerText;
                    break;
                case "随轴网移动":
                    structure._INSTANCE_MOVES_WITH_GRID_PARAM = int.Parse(xe.InnerText);
                    break;
                case "体积":
                    structure._HOST_VOLUME_COMPUTED = double.Parse(xe.InnerText);
                    break;
                case "图像":
                    structure._ALL_MODEL_IMAGE = xe.InnerText;
                    break;
                case "长度":
                    structure._CURVE_ELEM_LENGTH = double.Parse(xe.InnerText);
                    break;
                case "主体ID":
                    structure._HOST_ID_PARAM = int.Parse(xe.InnerText);
                    break;
                case "注释":
                    structure._ALL_MODEL_INSTANCE_COMMENTS = xe.InnerText;
                    break;
                case "柱定位标记":
                    structure._COLUMN_LOCATION_MARK = xe.InnerText;
                    break;
                case "柱样式":
                    structure._SLANTED_COLUMN_TYPE_PARAM = int.Parse(xe.InnerText);
                    break;
                case "族":
                    structure._ELEM_FAMILY_PARAM = xe.InnerText;
                    break;
                case "族名称":
                    structure._ALL_MODEL_FAMILY_NAME = xe.InnerText;
                    break;
                case "族与类型":
                    structure._ELEM_FAMILY_AND_TYPE_PARAM = xe.InnerText;
                    break;
            }
        }

    }
    private void GetAttributeMessage(Frameworks frameworks, string myId)//梁（结构框架）属性
    {
        string[] attName = myId.Split('_');//把名称拆分出来（格式为：物体名称_ID_时间）
        string myAttName = attName[0] + "_" + attName[1];//把物体的名称和ID重新组合成一个名称，以便查询属性表 

        Frameworks f = ReadModelSQLite.dictionaryFrameWorks[int.Parse(attName[1])];

        // frameworks._Model_Id = f._Model_Id;
        frameworks._PDModelID = f._PDModelID;
        frameworks._Y_JUSTIFICATION = f._Y_JUSTIFICATION;
        frameworks._Y_OFFSET_VALUE = f._Y_OFFSET_VALUE;
        frameworks._YZ_JUSTIFICATION = f._YZ_JUSTIFICATION;
        frameworks._Z_JUSTIFICATION = f._Z_JUSTIFICATION;
        frameworks._Z_OFFSET_VALUE = f._Z_OFFSET_VALUE;
        frameworks._LEVEL_PARAM = f._LEVEL_PARAM;
        frameworks._ALL_MODEL_MARK = f._ALL_MODEL_MARK;
        frameworks._INSTANCE_REFERENCE_LEVEL_PARAM = f._INSTANCE_REFERENCE_LEVEL_PARAM;
        frameworks._STRUCTURAL_REFERENCE_LEVEL_ELEVATION = f._STRUCTURAL_REFERENCE_LEVEL_ELEVATION;
        frameworks._PHASE_DEMOLISHED = f._PHASE_DEMOLISHED;
        frameworks._PHASE_CREATED = f._PHASE_CREATED;
        frameworks._STRUCTURAL_ELEVATION_AT_BOTTOM = f._STRUCTURAL_ELEVATION_AT_BOTTOM;
        frameworks._STRUCTURAL_ELEVATION_AT_TOP = f._STRUCTURAL_ELEVATION_AT_TOP;
        frameworks._STRUCTURAL_BEAM_ORIENTATION = f._STRUCTURAL_BEAM_ORIENTATION;
        frameworks._CLEAR_COVER_BOTTOM = f._CLEAR_COVER_BOTTOM;
        frameworks._CLEAR_COVER_TOP = f._CLEAR_COVER_TOP;
        frameworks._CLEAR_COVER_OTHER = f._CLEAR_COVER_OTHER;
        frameworks._SKETCH_PLANE_PARAM = f._SKETCH_PLANE_PARAM;
        frameworks._STRUCTURAL_BEND_DIR_ANGLE = f._STRUCTURAL_BEND_DIR_ANGLE;
        frameworks._STRUCTURAL_FRAME_CUT_LENGTH = f._STRUCTURAL_FRAME_CUT_LENGTH;
        frameworks._STRUCTURAL_MATERIAL_PARAM = f._STRUCTURAL_MATERIAL_PARAM;
        frameworks._INSTANCE_STRUCT_USAGE_PARAM = f._INSTANCE_STRUCT_USAGE_PARAM;
        frameworks._ELEM_CATEGORY_PARAM_MT = f._ELEM_CATEGORY_PARAM_MT;
        frameworks._ELEM_TYPE_PARAM = f._ELEM_TYPE_PARAM;
        frameworks._SYMBOL_ID_PARAM = f._SYMBOL_ID_PARAM;
        frameworks._ALL_MODEL_TYPE_NAME = f._ALL_MODEL_TYPE_NAME;
        frameworks._STRUCT_FRAM_JOIN_STATUS = f._STRUCT_FRAM_JOIN_STATUS;
        frameworks._FLOORSNUM = f._FLOORSNUM;
        frameworks._HOST_AREA_COMPUTED = f._HOST_AREA_COMPUTED;
        frameworks._STRUCTURAL_ANALYTICAL_MODEL = f._STRUCTURAL_ANALYTICAL_MODEL;
        frameworks._STRUCTURAL_BEAM_END0_ELEVATION = f._STRUCTURAL_BEAM_END0_ELEVATION;
        frameworks._DESIGN_OPTION_ID = f._DESIGN_OPTION_ID;
        frameworks._CONSTRUCTIONTIME = f._CONSTRUCTIONTIME;
        frameworks._HOST_VOLUME_COMPUTED = f._HOST_VOLUME_COMPUTED;
        frameworks._ALL_MODEL_IMAGE = f._ALL_MODEL_IMAGE;
        frameworks._CURVE_ELEM_LENGTH = f._CURVE_ELEM_LENGTH;
        frameworks._STRUCTURAL_BEAM_END1_ELEVATION = f._STRUCTURAL_BEAM_END1_ELEVATION;
        frameworks._HOST_ID_PARAM = f._HOST_ID_PARAM;
        frameworks._ALL_MODEL_INSTANCE_COMMENTS = f._ALL_MODEL_INSTANCE_COMMENTS;
        frameworks._ELEM_FAMILY_PARAM = f._ELEM_FAMILY_PARAM;
        frameworks._ALL_MODEL_FAMILY_NAME = f._ALL_MODEL_FAMILY_NAME;
        frameworks._ELEM_FAMILY_AND_TYPE_PARAM = f._ELEM_FAMILY_AND_TYPE_PARAM;
        //frameworks._AnglesRebar = f._AnglesRebar;
        //frameworks._BeamNum = f._BeamNum;
        //frameworks._BeamGEJING = f._BeamGEJING;
        //frameworks._BeamUp = f._BeamUp;
        //frameworks._BeamDown = f._BeamDown;
        //frameworks._WaistBeam = f._WaistBeam;
        //frameworks._BeamRight = f._BeamRight;
        //frameworks._BeamLeft = f._BeamLeft;
        //frameworks._IFSETREBAR = f._IFSETREBAR;




















        //SqliteCommand cmd = conn.CreateCommand();
        //cmd.CommandText = "select * from Param_Beam where Model_Id = " + attName[1];
        //SqliteDataReader dr = cmd.ExecuteReader();

        //while (dr.Read())
        //{

        //    for (int i = 0; i < dr.FieldCount; i++)
        //    {
        //        switch (dr.GetName(i))
        //        {
        //            case "PDModelID":
        //                frameworks._PDModelID = dr.GetString(i);
        //                break;
        //            case "Y轴对正":
        //                frameworks._Y_JUSTIFICATION = dr.GetInt32(i);
        //                break;
        //            case "Y轴偏移值":
        //                frameworks._Y_OFFSET_VALUE = dr.GetFloat(i);
        //                break;
        //            case "XY轴对正":
        //                frameworks._YZ_JUSTIFICATION = dr.GetInt32(i);
        //                break;
        //            case "Z轴对正":
        //                frameworks._Z_JUSTIFICATION = dr.GetInt32(i);
        //                break;
        //            case "Z轴偏移值":
        //                frameworks._Z_OFFSET_VALUE = dr.GetFloat(i);
        //                break;
        //            case "标高":
        //                frameworks._LEVEL_PARAM = dr.GetInt32(i);
        //                break;
        //            case "标记":
        //                frameworks._ALL_MODEL_MARK = dr.GetString(i);
        //                break;
        //            case "参照标高":
        //                frameworks._INSTANCE_REFERENCE_LEVEL_PARAM = dr.GetInt32(i);
        //                break;
        //            case "参照标高高程":
        //                frameworks._STRUCTURAL_REFERENCE_LEVEL_ELEVATION = dr.GetFloat(i);
        //                break;
        //            case "拆除的阶段":
        //                frameworks._PHASE_DEMOLISHED = dr.GetInt32(i);
        //                break;
        //            case "创建的阶段":
        //                frameworks._PHASE_CREATED = dr.GetInt32(i);
        //                break;
        //            case "底部高程":
        //                frameworks._STRUCTURAL_ELEVATION_AT_BOTTOM = dr.GetFloat(i);
        //                break;
        //            case "顶部高程":
        //                frameworks._STRUCTURAL_ELEVATION_AT_TOP = dr.GetFloat(i);
        //                break;
        //            case "方向":
        //                frameworks._STRUCTURAL_BEAM_ORIENTATION = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层底面":
        //                frameworks._CLEAR_COVER_BOTTOM = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层顶面":
        //                frameworks._CLEAR_COVER_TOP = dr.GetInt32(i);
        //                break;
        //            case "钢筋保护层其他面":
        //                frameworks._CLEAR_COVER_OTHER = dr.GetInt32(i);
        //                break;
        //            case "工作平面":
        //                frameworks._SKETCH_PLANE_PARAM = dr.GetString(i);
        //                break;
        //            case "横截面旋转":
        //                frameworks._STRUCTURAL_BEND_DIR_ANGLE = dr.GetFloat(i);
        //                break;
        //            case "剪切长度":
        //                frameworks._STRUCTURAL_FRAME_CUT_LENGTH = dr.GetFloat(i);
        //                break;
        //            case "结构材质":
        //                frameworks._STRUCTURAL_MATERIAL_PARAM = dr.GetInt32(i);
        //                break;
        //            case "结构用途":
        //                frameworks._INSTANCE_STRUCT_USAGE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类别":
        //                frameworks._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
        //                break;
        //            case "类型":
        //                frameworks._ELEM_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型ID":
        //                frameworks._SYMBOL_ID_PARAM = dr.GetInt32(i);
        //                break;
        //            case "类型名称":
        //                frameworks._ALL_MODEL_TYPE_NAME = dr.GetString(i);
        //                break;
        //            case "连接状态":
        //                frameworks._STRUCT_FRAM_JOIN_STATUS = dr.GetInt32(i);
        //                break;
        //            case "楼层":
        //                frameworks._FLOORSNUM = dr.GetString(i);
        //                ReadModelXML.idAndLevels.Add("ID" + attName[1], dr.GetString(i));//把id和楼层添加到字典里面去 
        //                break;
        //            case "面积":
        //                frameworks._HOST_AREA_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "启用分析模型":
        //                frameworks._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
        //                break;
        //            case "起点标高偏移":
        //                frameworks._STRUCTURAL_BEAM_END0_ELEVATION = dr.GetFloat(i);
        //                break;
        //            case "设计选项":
        //                frameworks._DESIGN_OPTION_ID = dr.GetInt32(i);
        //                break;
        //            case "施工时间":
        //                frameworks._CONSTRUCTIONTIME = dr.GetString(i);
        //                break;
        //            case "体积":
        //                frameworks._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
        //                break;
        //            case "图像":
        //                frameworks._ALL_MODEL_IMAGE = dr.GetInt32(i);
        //                break;
        //            case "长度":
        //                frameworks._CURVE_ELEM_LENGTH = dr.GetFloat(i);
        //                break;
        //            case "终点标高偏移":
        //                frameworks._STRUCTURAL_BEAM_END1_ELEVATION = dr.GetFloat(i);
        //                break;
        //            case "主体ID":
        //                frameworks._HOST_ID_PARAM = dr.GetInt32(i);
        //                break;
        //            case "注释":
        //                frameworks._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
        //                break;
        //            case "族":
        //                frameworks._ELEM_FAMILY_PARAM = dr.GetInt32(i);
        //                break;
        //            case "族名称":
        //                frameworks._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
        //                break;
        //            case "族与类型":
        //                frameworks._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
        //                break;
        //            case "Model_Id":
        //                frameworks.Model_Id = dr.GetInt32(i);
        //                break;
        //            case "对角斜筋":
        //                frameworks.AnglesRebar = dr.GetString(i);
        //                break;
        //            case "梁编号":
        //                frameworks.BeamNum = dr.GetString(i);
        //                break;
        //            case "梁箍筋":
        //                frameworks.BeamGEJING = dr.GetString(i);
        //                break;
        //            case "梁上部纵筋":
        //                frameworks.BeamUp = dr.GetString(i);
        //                break;
        //            case "梁腰筋":
        //                frameworks.WaistBeam = dr.GetString(i);
        //                break;
        //            case "梁右支座筋":
        //                frameworks.BeamRight = dr.GetString(i);
        //                break;
        //            case "梁左支座筋":
        //                frameworks.BeamLeft = dr.GetString(i);
        //                break;
        //            case "是否已配筋":
        //                frameworks.IFSETREBAR = dr.GetInt32(i);
        //                break;
        //        }
        //    }
        //}
    }
    private void GetAttributeMessage(Commons common, string myId)//常规模型
    {
        string[] attName = myId.Split('_');//把名称拆分出来（格式为：物体名称_ID_时间）
        string myAttName = attName[0] + "_" + attName[1];//把物体的名称和ID重新组合成一个名称，以便查询属性表 
        //XmlDocument doc = XMLReader._intance.docAtr;
        //XmlNodeList nodeList = doc.SelectSingleNode("/Atribute/" + myAttName).ChildNodes;//查找属性表的特定节点
        XmlDocument doc = new XmlDocument();
        string strAtr = Resources.Load("Atributes/" + attName[1]).ToString();
        doc.LoadXml(strAtr);


        //string path = ReadModelXML._instance.strGeometryPath;
        //path = path.Replace("\\myRevitMessage.xml", "");
        //string strAtr = path + "/Atributes/" + attName[1] + ".xml";
        //doc.Load(strAtr);
        XmlNodeList nodeList = doc.SelectSingleNode("/Atribute/" + myAttName).ChildNodes;//查找属性表的特定节点
        foreach (XmlElement xe in nodeList)
        {
            switch (xe.Name)
            {
                case "标高":
                    common._LEVEL_PARAM = int.Parse(xe.InnerText);
                    break;
                case "标记":
                    common._ALL_MODEL_MARK = xe.InnerText;
                    break;
                case "拆除的阶段":
                    common._PHASE_DEMOLISHED = xe.InnerText;
                    break;
                case "创建的阶段":
                    common._PHASE_CREATED = xe.InnerText;
                    break;
                case "洞口宽":
                    common._CAVE_WIDTH = double.Parse(xe.InnerText);
                    break;
                case "洞口长":
                    common._CAVE_LONG = double.Parse(xe.InnerText);
                    break;
                case "工作平面":
                    common._SKETCH_PLANE_PARAM = xe.InnerText;
                    break;
                case "类别":
                    common._ELEM_CATEGORY_PARAM_MT = double.Parse(xe.InnerText);
                    break;
                case "类型":
                    common._ELEM_TYPE_PARAM = int.Parse(xe.InnerText);
                    break;
                case "类型ID":
                    common._SYMBOL_ID_PARAM = double.Parse(xe.InnerText);
                    break;
                case "类型名称":
                    common._ALL_MODEL_TYPE_NAME = xe.InnerText;
                    break;
                case "楼层":
                    common._FLOORSNUM = xe.InnerText;
                    ReadModelXML.idAndLevels.Add("ID" + attName[1], xe.InnerText);//把id和楼层添加到字典里面去
                    break;
                case "面积":
                    common._HOST_AREA_COMPUTED = double.Parse(xe.InnerText);
                    break;
                case "明细表标高":
                    common._INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM = int.Parse(xe.InnerText);
                    break;
                case "偏移量":
                    common._INSTANCE_FREE_HOST_OFFSET_PARAM = int.Parse(xe.InnerText);
                    break;
                case "设计选项":
                    common._DESIGN_OPTION_ID = xe.InnerText;
                    break;
                case "施工时间":
                    common._CONSTRUCTIONTIME = xe.InnerText;
                    break;
                case "体积":
                    common._HOST_VOLUME_COMPUTED = double.Parse(xe.InnerText);
                    break;
                case "图像":
                    common._ALL_MODEL_IMAGE = xe.InnerText;
                    break;
                case "线顶点":
                    common._TOP_OF_POINT = double.Parse(xe.InnerText);
                    break;
                case "主体ID":
                    common._HOST_ID_PARAM = int.Parse(xe.InnerText);
                    break;
                case "注释":
                    common._ALL_MODEL_INSTANCE_COMMENTS = xe.InnerText;
                    break;
                case "族":
                    common._ELEM_FAMILY_PARAM = xe.InnerText;
                    break;
                case "族名称":
                    common._ALL_MODEL_FAMILY_NAME = xe.InnerText;
                    break;
                case "族与类型":
                    common._ELEM_FAMILY_AND_TYPE_PARAM = xe.InnerText;
                    break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("按下");
        TTUIPage.ShowPage<UICompoentInfo>(modelMessage.Id);
        //TTUIPage.ShowPage<UICompoentInfo>("871714");

    }

    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log("click");
    //}
}

