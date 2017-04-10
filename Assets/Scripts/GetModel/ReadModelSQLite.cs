using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System;
using System.Linq;
using System.Xml;

public class ReadModelSQLite : MonoBehaviour
{
    SqliteConnection conn;
    string path = "";
    public List<ModelMessage> mmList;
    string ProjectName = "";
    List<List<Triangles>> trianglesLists = new List<List<Triangles>>();
    List<List<VerticesAndNormals>> verticesAndeNormalsLists = new List<List<VerticesAndNormals>>();
    public static Dictionary<int, Walls> dictionaryWall = new Dictionary<int, Walls>();
    public static Dictionary<int, Floors> dictionaryFloors = new Dictionary<int, Floors>();
    public static Dictionary<int, Frameworks> dictionaryFrameWorks = new Dictionary<int, Frameworks>();



    public GameObject go;
    GameObject tempScripts;
    public Walls Wall;
    public Floors floor;
    public Frameworks framework;
    public static List<GameObject> goList = new List<GameObject>();//定义一个游戏物体集合，用于存放场景的所有物体
    GameObject myModelCenter;//模型的中心点位置

    public GameObject LoadingSlider;
    public Text sliderText;
    public Image loadingImg;//精度条图片
    GameObject OpenModelButton;
    public Transform CountBtn;
    public Transform RebarBtn;

    private static ReadModelSQLite instance;
    private ReadModelSQLite() { }
    public static ReadModelSQLite Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(ReadModelSQLite)) as ReadModelSQLite;
            return instance;
        }
    }

    public void LoadModel()
    {
        mmList = new List<ModelMessage>();
        dictionaryWall = new Dictionary<int, Walls>();
        dictionaryFloors = new Dictionary<int, Floors>();
        dictionaryFrameWorks = new Dictionary<int, Frameworks>();


        Stopwatch watch = new Stopwatch();
        watch.Start();
        myModelCenter = GameObject.Find("ModelCenter");
        tempScripts = GameObject.Find("TempScripts");
        ProjectName = "38#结构模型.0005";
        //LoadingSlider = GameObject.Find("Loading");
        // OpenModelButton = GameObject.Find("OpenModelBtn");
        LoadingSlider.SetActive(true);//激活进度条
        LoadSQLite();//获取数据
        watch.Stop();
        UnityEngine.Debug.Log("总共使用了" + watch.Elapsed + "秒！");
    }
    void LoadSQLite()
    {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//        //string path = "URI=file:" + Application.dataPath + "/Plugins/Android/assets/38Database.db";
//        // path = "Data Source=" + Application.dataPath + "/38DatabaseV1.0.db";
//        //  path = "Data Source=" + Application.dataPath + "/AllModelTest.db";
//        //  path = @"Data Source=E:\RevitProject\RevitGeometry2017-3-2\AllModelTest.db";
//        path = "Data Source=" + LoadXMLGetModelPath();

//        //string path = "Data Source =E:/UnityProject/20161118_v4/Assets/Plugins/Android/assets/38Database.db";
//#elif UNITY_ANDROID
//         path= "URI=file:"+LoadXMLGetModelPath();
//              //      //将第三方数据库拷贝至Android可找到的地方
//        		    //path = Application.persistentDataPath + "/" + "AllModelTest.db";
//              //     //string appDBPath = "jar:file://" + Application.dataPath + "!/assets/" + "38Database.db";
//              //      string appDBPath = Application.streamingAssetsPath + "/AllModelTest.db";
//              //      if (!File.Exists(path))
//              //      {
//              //          WWW loadDB = new WWW(appDBPath);
//              //          while (!loadDB.isDone) {}
//              //          File.WriteAllBytes(path, loadDB.bytes);
//              //      }
//              //  path= "URI=file:"+path;
//#endif
        List<VerticesAndNormals> verticesAndNormalsList = new List<VerticesAndNormals>();
        List<Triangles> trianglesList = new List<Triangles>();
        List<ModelID> modelIdList = new List<ModelID>();
        conn = Tools.Instance.SqlConnection();
        //conn = new SqliteConnection(path);
        //try
        //{
        //    conn.Open();
        //    //MessageBox.Show("connection is ok","Normal");
        //}
        //catch (Exception e)
        //{
        //    MessageBox.Show(e.Message, "Eroor");
        //}

        SqliteCommand cmd1 = conn.CreateCommand();
        cmd1.CommandText = @"SELECT * FROM [Model_Geometry]";// where ProjectName = '"38#结构模型.0005'"";
        SqliteDataReader dr1 = cmd1.ExecuteReader();//读取几何信息
        if (dr1.HasRows)
        {
            int id = 0;//判断Id是否相同
            string name = "";//模型的名字
            bool first = true;
            List<int> triList = new List<int>();
            List<Vector3> verticesList = new List<Vector3>();
            List<Vector3> normalsList = new List<Vector3>();
            while (dr1.Read())
            {
                name = dr1.GetString(2);
                if (first)
                {
                    first = false;
                    id = dr1.GetInt32(3);
                }
                Vector3 ver = new Vector3();
                Vector3 nor = new Vector3();
                ver.x = dr1.GetFloat(4);
                ver.y = dr1.GetFloat(5);
                ver.z = dr1.GetFloat(6);
                nor.x = dr1.GetFloat(7);
                nor.y = dr1.GetFloat(8);
                nor.z = dr1.GetFloat(9);
                if (id != dr1.GetInt32(3))
                {
                    ModelMessage mm = new ModelMessage();
                    mm.Name = dr1.GetString(2);
                    mm.Id = id;
                    mm.Vertices = verticesList.ToArray();
                    mm.Normals = normalsList.ToArray();
                    mm.Triangles = triList.ToArray();
                    mmList.Add(mm);

                    verticesList.Clear();
                    normalsList.Clear();
                    triList.Clear();
                }
                verticesList.Add(ver);
                normalsList.Add(nor);
                if (!dr1.IsDBNull(10) && !dr1.IsDBNull(11) && !dr1.IsDBNull(12))
                {
                    triList.Add(dr1.GetInt32(10));
                    triList.Add(dr1.GetInt32(11));
                    triList.Add(dr1.GetInt32(12));
                }
                id = dr1.GetInt32(3);
            }
            ModelMessage mmLast = new ModelMessage();
            mmLast.Name = name;
            mmLast.Id = id;
            mmLast.Vertices = verticesList.ToArray();
            mmLast.Normals = normalsList.ToArray();
            mmLast.Triangles = triList.ToArray();
            mmList.Add(mmLast);

            verticesList.Clear();
            normalsList.Clear();
            triList.Clear();
        }
       // SqliteReadWallParameters(conn);//读取墙的属性
       // SqliteReadFloorsParameters(conn);//读取楼板属性
       // SqliteReadFrameWorksParameters(conn);//读取结构框架（梁）属性

        StartCoroutine(GenerateModel());//生成模型


    }
    void SqliteReadWallParameters(SqliteConnection conn)//读取墙的属性
    {
        SqliteTransaction transaction = conn.BeginTransaction();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "select * from Param_Wall";
        SqliteDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Walls myWall = Instantiate(Wall);

            for (int i = 0; i < dr.FieldCount; i++)
            {
                #region
                switch (dr.GetName(i))
                {
                    case "PDModelID":
                        myWall._PDModelID = dr.GetString(i);
                        break;
                    case "PDST材质":
                        myWall._PDSTMATERIAL = dr.GetInt32(i);
                        break;
                    case "标记":
                        myWall._ALL_MODEL_MARK = dr.GetString(i);
                        break;
                    case "拆除的阶段":
                        myWall._PHASE_DEMOLISHED = dr.GetInt32(i);
                        break;
                    case "创建的阶段":
                        myWall._PHASE_CREATED = dr.GetInt32(i);
                        break;
                    case "底部偏移":
                        myWall._WALL_BASE_OFFSET = dr.GetFloat(i);
                        break;
                    case "底部限制条件":
                        myWall._WALL_BASE_CONSTRAINT = dr.GetInt32(i);
                        break;
                    case "底部延伸距离":
                        myWall._WALL_BOTTOM_EXTENSION_DIST_PARAM = dr.GetFloat(i);
                        break;
                    case "顶部偏移":
                        myWall._WALL_TOP_OFFSET = dr.GetFloat(i);
                        break;
                    case "顶部延伸距离":
                        myWall._WALL_TOP_EXTENSION_DIST_PARAM = dr.GetFloat(i);
                        break;
                    case "顶部约束":
                        myWall._WALL_HEIGHT_TYPE = dr.GetInt32(i);
                        break;
                    case "定位线":
                        myWall._WALL_KEY_REF_PARAM = dr.GetInt32(i);
                        break;
                    case "房间边界":
                        myWall._WALL_ATTR_ROOM_BOUNDING = dr.GetInt32(i);
                        break;
                    case "钢筋保护层内部面":
                        myWall._CLEAR_COVER_INTERIOR = dr.GetInt32(i);
                        break;
                    case "钢筋保护层其他面":
                        myWall._CLEAR_COVER_OTHER = dr.GetInt32(i);
                        break;
                    case "钢筋保护层外部面":
                        myWall._CLEAR_COVER_EXTERIOR = dr.GetInt32(i);
                        break;
                    case "结构":
                        myWall._WALL_STRUCTURAL_SIGNIFICANT = dr.GetInt32(i);
                        break;
                    case "结构用途":
                        myWall._WALL_STRUCTURAL_USAGE_PARAM = dr.GetInt32(i);
                        break;
                    case "类别":
                        myWall._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
                        break;
                    case "类型":
                        myWall._ELEM_TYPE_PARAM = dr.GetInt32(i);
                        break;
                    case "类型ID":
                        myWall._SYMBOL_ID_PARAM = dr.GetInt32(i);
                        break;
                    case "类型名称":
                        myWall._ALL_MODEL_TYPE_NAME = dr.GetString(i);
                        break;
                    case "楼层":
                        myWall._FLOORSNUM = dr.GetString(i);
                        break;
                    case "面积":
                        myWall._HOST_AREA_COMPUTED = dr.GetFloat(i);
                        break;
                    case "启用分析模型":
                        myWall._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
                        break;
                    case "设计选项":
                        myWall._DESIGN_OPTION_ID = dr.GetInt32(i);
                        break;
                    case "施工时间":
                        myWall._CONSTRUCTIONTIME = dr.GetString(i);
                        break;
                    case "体积":
                        myWall._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
                        break;
                    case "图像":
                        myWall._ALL_MODEL_IMAGE = dr.GetInt32(i);
                        break;
                    case "无连接高度":
                        myWall._WALL_USER_HEIGHT_PARAM = dr.GetFloat(i);
                        break;
                    case "已附着底部":
                        myWall._WALL_BOTTOM_IS_ATTACHED = dr.GetInt32(i);
                        break;
                    case "已附着顶部":
                        myWall._WALL_TOP_IS_ATTACHED = dr.GetInt32(i);
                        break;
                    case "与体量相关":
                        myWall._RELATED_TO_MASS = dr.GetInt32(i);
                        break;
                    case "长度":
                        myWall._CURVE_ELEM_LENGTH = dr.GetFloat(i);
                        break;
                    case "注释":
                        myWall._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
                        break;
                    case "族":
                        myWall._ELEM_FAMILY_PARAM = dr.GetInt32(i);
                        break;
                    case "族名称":
                        myWall._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
                        break;
                    case "族与类型":
                        myWall._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
                        break;
                        //case "Model_Id":
                        //    myWall._Model_Id = dr.GetInt32(i);
                        //    break;
                        //case "暗柱箍筋":
                        //    myWall._ANZHUGUJING = dr.GetString(i);
                        //    break;
                        //case "暗柱箍筋类型":
                        //    myWall._ANZHUGUJINLEIXING = dr.GetString(i);
                        //    break;
                        //case "暗柱纵筋":
                        //    myWall._ANZHUZONGJING = dr.GetString(i);
                        //    break;
                        //case "墙编号":
                        //    myWall._WallNum = dr.GetString(i);
                        //    break;
                        //case "墙身拉筋":
                        //    myWall._WallLAJING = dr.GetString(i);
                        //    break;
                        //case "是否已配筋":
                        //    myWall._IFSETREBAR = dr.GetInt32(i);
                        //    break;
                        //case "竖向分布钢筋":
                        //    myWall._VERTICALSETREBAR = dr.GetString(i);
                        //    break;
                        //case "水平分布钢筋":
                        //    myWall._HORIZONTALSETREBAR = dr.GetString(i);
                        //    break;                    
                }
                #endregion
            }
            //idAndLevels.Add("ID" + dr.GetInt32(1).ToString(), dr.GetString(13));//把id和楼层添加到字典里面去
            dictionaryWall.Add(dr.GetInt32(1), myWall);

            myWall.transform.parent = tempScripts.transform;//把临时生成的脚本放在一个临时物体下（生成完成的时候统一删除)

        }
        transaction.Commit();
    }
    void SqliteReadFloorsParameters(SqliteConnection conn)//读取楼板属性
    {
        SqliteTransaction transaction = conn.BeginTransaction();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "select * from Param_Floors";
        SqliteDataReader dr = cmd.ExecuteReader();


        while (dr.Read())
        {
            Floors floors = Instantiate(floor);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                switch (dr.GetName(i))
                {
                    case "PDModelID":
                        floors._PDModelID = dr.GetString(i);
                        break;
                    case "PDST材质":
                        floors._PDSTMATERIAL = dr.GetInt32(i);
                        break;
                    case "标高":
                        floors._LEVEL_PARAM = dr.GetInt32(i);
                        break;
                    case "标记":
                        floors._ALL_MODEL_MARK = dr.GetString(i);
                        break;
                    case "拆除的阶段":
                        floors._PHASE_DEMOLISHED = dr.GetInt32(i);
                        break;
                    case "创建的阶段":
                        floors._PHASE_CREATED = dr.GetInt32(i);
                        break;
                    case "底部高程":
                        floors._STRUCTURAL_ELEVATION_AT_BOTTOM = dr.GetFloat(i);
                        break;
                    case "底部核心高程":
                        floors._STRUCTURAL_ELEVATION_AT_BOTTOM_CORE = dr.GetFloat(i);
                        break;
                    case "顶部高程":
                        floors._STRUCTURAL_ELEVATION_AT_TOP = dr.GetFloat(i);
                        break;
                    case "顶部核心高程":
                        floors._STRUCTURAL_ELEVATION_AT_TOP_CORE = dr.GetFloat(i);
                        break;
                    case "房间边界":
                        floors._WALL_ATTR_ROOM_BOUNDING = dr.GetInt32(i);
                        break;
                    case "钢筋保护层底面":
                        floors._CLEAR_COVER_BOTTOM = dr.GetInt32(i);
                        break;
                    case "钢筋保护层顶面":
                        floors._CLEAR_COVER_TOP = dr.GetInt32(i);
                        break;
                    case "钢筋保护层其他面":
                        floors._CLEAR_COVER_OTHER = dr.GetInt32(i);
                        break;
                    case "厚度":
                        floors._FLOOR_ATTR_THICKNESS_PARAM = dr.GetFloat(i);
                        break;
                    case "结构":
                        floors._WALL_STRUCTURAL_SIGNIFICANT = dr.GetInt32(i);
                        break;
                    case "类别":
                        floors._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
                        break;
                    case "类型":
                        floors._ELEM_TYPE_PARAM = dr.GetInt32(i);
                        break;
                    case "类型ID":
                        floors._SYMBOL_ID_PARAM = dr.GetInt32(i);
                        break;
                    case "类型名称":
                        floors._ALL_MODEL_TYPE_NAME = dr.GetString(i);
                        break;
                    case "楼层":
                        floors._FLOORSNUM = dr.GetString(i);
                        break;
                    case "面积":
                        floors._HOST_AREA_COMPUTED = dr.GetFloat(i);
                        break;
                    case "坡度":
                        floors._ROOF_SLOPE = dr.GetFloat(i);
                        break;
                    case "启用分析模型":
                        floors._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
                        break;
                    case "设计选项":
                        floors._DESIGN_OPTION_ID = dr.GetInt32(i);
                        break;
                    case "施工时间":
                        floors._CONSTRUCTIONTIME = dr.GetString(i);
                        break;
                    case "体积":
                        floors._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
                        break;
                    case "图像":
                        floors._ALL_MODEL_IMAGE = dr.GetInt32(i);
                        break;
                    case "与体量相关":
                        floors._RELATED_TO_MASS = dr.GetInt32(i);
                        break;
                    case "周长":
                        floors._HOST_PERIMETER_COMPUTED = dr.GetFloat(i);
                        break;
                    case "注释":
                        floors._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
                        break;
                    case "自标高的高度偏移":
                        floors._FLOOR_HEIGHTABOVELEVEL_PARAM = dr.GetFloat(i);
                        break;
                    case "族":
                        floors._ELEM_FAMILY_PARAM = dr.GetInt32(i);
                        break;
                    case "族名称":
                        floors._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
                        break;
                    case "族与类型":
                        floors._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
                        break;
                        //case "Model_Id":
                        //    floors.Model_Id = dr.GetInt32(i);
                        //    break;
                        //case "X方向底筋":
                        //    floors.XDirectBottom = dr.GetString(i);
                        //    break;
                        //case "X方向面筋":
                        //    floors.XDirectFace = dr.GetString(i);
                        //    break;
                        //case "Y方向底筋":
                        //    floors.YDirectBottom = dr.GetString(i);
                        //    break;
                        //case "Y方向面筋":
                        //    floors.YDirectFace = dr.GetString(i);
                        //    break;
                        //case "板编号":
                        //    floors.FloorsNum = dr.GetString(i);
                        //    break;
                        //case "分布筋":
                        //    floors.DistributeRebar = dr.GetString(i);
                        //    break;
                        //case "钢筋关联板编号":
                        //    floors.RebarUnionNum = dr.GetString(i);
                        //    break;
                        //case "是否已配筋":
                        //    floors.IFSETREBAR = dr.GetInt32(i);
                        //    break;
                        //case "弯曲边缘条件":
                        //    floors.HOST_SSE_CURVED_EDGE_CONDITION_PARAM = dr.GetInt32(i);
                        //    break;
                        //case "温度筋":
                        //    floors.TemperatureRebar = dr.GetString(i);
                        //    break;
                }
            }
            //ReadModelXML.idAndLevels.Add("ID" + dr.GetInt32(1).ToString(), dr.GetString(40));//把id和楼层添加到字典里面去 
            dictionaryFloors.Add(dr.GetInt32(1), floors);
            floors.transform.parent = tempScripts.transform;//把临时生成的脚本放在一个临时物体下（生成完成的时候统一删除)
        }
        transaction.Commit();
    }
    void SqliteReadFrameWorksParameters(SqliteConnection conn)//读取结构框架（梁）属性
    {
        SqliteTransaction transaction = conn.BeginTransaction();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "select * from Param_Beam";
        SqliteDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Frameworks frameworks = Instantiate(framework);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                switch (dr.GetName(i))
                {
                    case "PDModelID":
                        frameworks._PDModelID = dr.GetString(i);
                        break;
                    case "Y轴对正":
                        frameworks._Y_JUSTIFICATION = dr.GetInt32(i);
                        break;
                    case "Y轴偏移值":
                        frameworks._Y_OFFSET_VALUE = dr.GetFloat(i);
                        break;
                    case "XY轴对正":
                        frameworks._YZ_JUSTIFICATION = dr.GetInt32(i);
                        break;
                    case "Z轴对正":
                        frameworks._Z_JUSTIFICATION = dr.GetInt32(i);
                        break;
                    case "Z轴偏移值":
                        frameworks._Z_OFFSET_VALUE = dr.GetFloat(i);
                        break;
                    case "标高":
                        frameworks._LEVEL_PARAM = dr.GetInt32(i);
                        break;
                    case "标记":
                        frameworks._ALL_MODEL_MARK = dr.GetString(i);
                        break;
                    case "参照标高":
                        frameworks._INSTANCE_REFERENCE_LEVEL_PARAM = dr.GetInt32(i);
                        break;
                    case "参照标高高程":
                        frameworks._STRUCTURAL_REFERENCE_LEVEL_ELEVATION = dr.GetFloat(i);
                        break;
                    case "拆除的阶段":
                        frameworks._PHASE_DEMOLISHED = dr.GetInt32(i);
                        break;
                    case "创建的阶段":
                        frameworks._PHASE_CREATED = dr.GetInt32(i);
                        break;
                    case "底部高程":
                        frameworks._STRUCTURAL_ELEVATION_AT_BOTTOM = dr.GetFloat(i);
                        break;
                    case "顶部高程":
                        frameworks._STRUCTURAL_ELEVATION_AT_TOP = dr.GetFloat(i);
                        break;
                    case "方向":
                        frameworks._STRUCTURAL_BEAM_ORIENTATION = dr.GetInt32(i);
                        break;
                    case "钢筋保护层底面":
                        frameworks._CLEAR_COVER_BOTTOM = dr.GetInt32(i);
                        break;
                    case "钢筋保护层顶面":
                        frameworks._CLEAR_COVER_TOP = dr.GetInt32(i);
                        break;
                    case "钢筋保护层其他面":
                        frameworks._CLEAR_COVER_OTHER = dr.GetInt32(i);
                        break;
                    case "工作平面":
                        frameworks._SKETCH_PLANE_PARAM = dr.GetString(i);
                        break;
                    case "横截面旋转":
                        frameworks._STRUCTURAL_BEND_DIR_ANGLE = dr.GetFloat(i);
                        break;
                    case "剪切长度":
                        frameworks._STRUCTURAL_FRAME_CUT_LENGTH = dr.GetFloat(i);
                        break;
                    case "结构材质":
                        frameworks._STRUCTURAL_MATERIAL_PARAM = dr.GetInt32(i);
                        break;
                    case "结构用途":
                        frameworks._INSTANCE_STRUCT_USAGE_PARAM = dr.GetInt32(i);
                        break;
                    case "类别":
                        frameworks._ELEM_CATEGORY_PARAM_MT = dr.GetInt32(i);
                        break;
                    case "类型":
                        frameworks._ELEM_TYPE_PARAM = dr.GetInt32(i);
                        break;
                    case "类型ID":
                        frameworks._SYMBOL_ID_PARAM = dr.GetInt32(i);
                        break;
                    case "类型名称":
                        frameworks._ALL_MODEL_TYPE_NAME = dr.GetString(i);
                        break;
                    case "连接状态":
                        frameworks._STRUCT_FRAM_JOIN_STATUS = dr.GetInt32(i);
                        break;
                    case "楼层":
                        frameworks._FLOORSNUM = dr.GetString(i);
                        break;
                    case "面积":
                        frameworks._HOST_AREA_COMPUTED = dr.GetFloat(i);
                        break;
                    case "启用分析模型":
                        frameworks._STRUCTURAL_ANALYTICAL_MODEL = dr.GetInt32(i);
                        break;
                    case "起点标高偏移":
                        frameworks._STRUCTURAL_BEAM_END0_ELEVATION = dr.GetFloat(i);
                        break;
                    case "设计选项":
                        frameworks._DESIGN_OPTION_ID = dr.GetInt32(i);
                        break;
                    case "施工时间":
                        frameworks._CONSTRUCTIONTIME = dr.GetString(i);
                        break;
                    case "体积":
                        frameworks._HOST_VOLUME_COMPUTED = dr.GetFloat(i);
                        break;
                    case "图像":
                        frameworks._ALL_MODEL_IMAGE = dr.GetInt32(i);
                        break;
                    case "长度":
                        frameworks._CURVE_ELEM_LENGTH = dr.GetFloat(i);
                        break;
                    case "终点标高偏移":
                        frameworks._STRUCTURAL_BEAM_END1_ELEVATION = dr.GetFloat(i);
                        break;
                    case "主体ID":
                        frameworks._HOST_ID_PARAM = dr.GetInt32(i);
                        break;
                    case "注释":
                        frameworks._ALL_MODEL_INSTANCE_COMMENTS = dr.GetString(i);
                        break;
                    case "族":
                        frameworks._ELEM_FAMILY_PARAM = dr.GetInt32(i);
                        break;
                    case "族名称":
                        frameworks._ALL_MODEL_FAMILY_NAME = dr.GetString(i);
                        break;
                    case "族与类型":
                        frameworks._ELEM_FAMILY_AND_TYPE_PARAM = dr.GetInt32(i);
                        break;
                        //case "Model_Id":
                        //    frameworks.Model_Id = dr.GetInt32(i);
                        //    break;
                        //case "对角斜筋":
                        //    frameworks.AnglesRebar = dr.GetString(i);
                        //    break;
                        //case "梁编号":
                        //    frameworks.BeamNum = dr.GetString(i);
                        //    break;
                        //case "梁箍筋":
                        //    frameworks.BeamGEJING = dr.GetString(i);
                        //    break;
                        //case "梁上部纵筋":
                        //    frameworks.BeamUp = dr.GetString(i);
                        //    break;
                        //case "梁腰筋":
                        //    frameworks.WaistBeam = dr.GetString(i);
                        //    break;
                        //case "梁右支座筋":
                        //    frameworks.BeamRight = dr.GetString(i);
                        //    break;
                        //case "梁左支座筋":
                        //    frameworks.BeamLeft = dr.GetString(i);
                        //    break;
                        //case "是否已配筋":
                        //    frameworks.IFSETREBAR = dr.GetInt32(i);
                        //    break;
                }
            }

            // ReadModelXML.idAndLevels.Add("ID" + dr.GetInt32(1).ToString(), dr.GetString(39));//把id和楼层添加到字典里面去 
            dictionaryFrameWorks.Add(dr.GetInt32(1), frameworks);
            frameworks.transform.parent = tempScripts.transform;//把临时生成的脚本放在一个临时物体下（生成完成的时候统一删除)
        }

        transaction.Commit();
    }

    IEnumerator GenerateModel()//生成模型
    {

        sliderText.text = "正在生产模型...";
        float f = 0.0f;
        int index = 0;
        int count = mmList.Count;

        //SqliteTransaction tran = conn.BeginTransaction();
        foreach (ModelMessage m in mmList)
        {
            index++;
            f += 1;
            if (index >= count / 20)
            {
                loadingImg.fillAmount = f / count;
                yield return new WaitForSeconds(0.1f);
                index = 0;
            }

            GameObject myGo = Instantiate(go) as GameObject;//实例化一个物体出来
            myGo.name = m.Name;//设置物体的名称
            myGo.AddComponent<myMesh>();//把脚本作为组件附加给游戏物体
            myMesh mygoMesh = myGo.GetComponent<myMesh>();
            mygoMesh.Init(m);
            //mygoMesh.DrawModel(m.Id, m.Name, m.Triangles, m.Vertices, m.Normals, conn);//调用物体的本身组件的方法，把物体的形状绘制出来
            goList.Add(myGo);//把所有物体添加到一个集合里面，以便以后查询或调用等等。
            myGo.tag = "Player";
            // myGo.transform.localEulerAngles = new Vector3(270, 0, 0);
            myGo.transform.localScale = new Vector3(-1, 1, 1);
            myGo.transform.Rotate(-90, 0, 0);

            myGo.transform.parent = myModelCenter.transform;
        }
        sliderText.text = "模型生成完毕！";
        StartCoroutine(HideLoading());
        StartCoroutine(DestoryTempGameObjects());
        //DestroyImmediate(tempScripts);
        conn.Close();
        conn.Dispose();
        UnityEngine.Debug.Log("模型的构件数量为："+ goList.Count);
        //   tran.Commit();
    }

    IEnumerator DestoryTempGameObjects()
    {
        yield return 0;
        foreach (Transform child in tempScripts.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public void DestoryGameObjects()
    {
        goList.Clear();
        foreach (Transform child in myModelCenter.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    IEnumerator HideLoading()
    {
        yield return new WaitForSeconds(1.5f);
        LoadingSlider.SetActive(false);
        // OpenModelButton.SetActive(false);
        //CountBtn.gameObject.SetActive(true);
        //RebarBtn.gameObject.SetActive(true);
    }

}
