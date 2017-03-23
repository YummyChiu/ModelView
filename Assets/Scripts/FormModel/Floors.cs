using UnityEngine;
using System.Collections;

public class Floors : MonoBehaviour {
  //  public int Model_Id;//模型的id
    public string PDMODELID;//PDModelID    
    public int PDSTMATERIAL;//PDST材质
    public int LEVEL_PARAM;//标高
    public string ALL_MODEL_MARK;//标记
    public int PHASE_DEMOLISHED;//拆除的阶段  
    public int PHASE_CREATED;//创建的阶段 
    public float STRUCTURAL_ELEVATION_AT_BOTTOM;//底部高程
    public float STRUCTURAL_ELEVATION_AT_BOTTOM_CORE;//底部核心高程
    public float STRUCTURAL_ELEVATION_AT_TOP;//顶部高程
    public float STRUCTURAL_ELEVATION_AT_TOP_CORE;//顶部核心高程
    public int WALL_ATTR_ROOM_BOUNDING;// 房间边界
    public int CLEAR_COVER_BOTTOM;//钢筋保护底面
    public int CLEAR_COVER_TOP;//钢筋保护层顶面
    public int CLEAR_COVER_OTHER;//钢筋保护层其他面
    public float FLOOR_ATTR_THICKNESS_PARAM;//厚度
    public int WALL_STRUCTURAL_SIGNIFICANT;// 结构
    public int ELEM_CATEGORY_PARAM_MT;// 类别
    public int ELEM_TYPE_PARAM;//类型
    public int SYMBOL_ID_PARAM;// 类型ID
    public string ALL_MODEL_TYPE_NAME;// 类型名称
    public string FLOORSNUM;//楼层
    public float HOST_AREA_COMPUTED;// 面积
    public float ROOF_SLOPE;//坡度
    public int STRUCTURAL_ANALYTICAL_MODEL;// 启用分析模型
    public int DESIGN_OPTION_ID;// 设计选项
    public string CONSTRUCTIONTIME;//施工时间
    public float HOST_VOLUME_COMPUTED;// 体积
    public int ALL_MODEL_IMAGE;// 图像
    public int RELATED_TO_MASS;//与体量相关
    public float HOST_PERIMETER_COMPUTED;//周长
    public string ALL_MODEL_INSTANCE_COMMENTS;//注释
    public float FLOOR_HEIGHTABOVELEVEL_PARAM;//自标高的高度偏移
    public int ELEM_FAMILY_PARAM;//族
    public string ALL_MODEL_FAMILY_NAME;//族名称
    public int ELEM_FAMILY_AND_TYPE_PARAM;//族与类型

    //public string XDirectBottom;//x方向底筋
    //public string XDirectFace;//x方向面筋
    //public string YDirectBottom;//Y方向底筋
    //public string YDirectFace;//Y方向面筋
    //public string FloorsNum;//板编号
    //public string DistributeRebar;//分布筋
    //public string RebarUnionNum;//钢筋关联板编号
    //public int IFSETREBAR;//是否已配筋
    //public int HOST_SSE_CURVED_EDGE_CONDITION_PARAM;//弯曲边缘条件
    //public string TemperatureRebar;//温度筋


    //public int _Model_Id //模型的ID
    //{
    //    get { return this.Model_Id; }
    //    set { this.Model_Id = value; }
    //}
    public string _PDModelID//PDModelID
    {
        get { return this.PDMODELID; }
        set { this.PDMODELID = value; }
    }
    public int _PDSTMATERIAL//PDST材质
    {
        get { return this.PDSTMATERIAL; }
        set { this.PDSTMATERIAL = value; }
    }
    public int _LEVEL_PARAM//标高 
    {
        get { return this.LEVEL_PARAM; }
        set { this.LEVEL_PARAM = value; }
    }
    public string _ALL_MODEL_MARK//标记
    {
        get { return this.ALL_MODEL_MARK; }
        set { this.ALL_MODEL_MARK = value; }
    }
    public int _PHASE_DEMOLISHED//拆除的阶段
    {
        get { return this.PHASE_DEMOLISHED; }
        set { this.PHASE_DEMOLISHED = value; }
    }
    public int _PHASE_CREATED//创建的阶段
    {
        get { return this.PHASE_CREATED; }
        set { this.PHASE_CREATED = value; }
    }
    public float _STRUCTURAL_ELEVATION_AT_BOTTOM//底部高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_BOTTOM; }
        set { this.STRUCTURAL_ELEVATION_AT_BOTTOM = value; }
    }
    public float _STRUCTURAL_ELEVATION_AT_BOTTOM_CORE//底部核心高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_BOTTOM_CORE; }
        set { this.STRUCTURAL_ELEVATION_AT_BOTTOM_CORE = value; }
    }
    public float _STRUCTURAL_ELEVATION_AT_TOP//顶部高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_TOP; }
        set { this.STRUCTURAL_ELEVATION_AT_TOP = value; }
    }
    public float _STRUCTURAL_ELEVATION_AT_TOP_CORE//顶部核心高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_TOP_CORE; }
        set { this.STRUCTURAL_ELEVATION_AT_TOP_CORE = value; }
    }
    public int _WALL_ATTR_ROOM_BOUNDING// 房间边界
    {
        get { return this.WALL_ATTR_ROOM_BOUNDING; }
        set { this.WALL_ATTR_ROOM_BOUNDING = value; }
    }

    public int _CLEAR_COVER_BOTTOM//钢筋保护层底面
    {
        get { return this.CLEAR_COVER_BOTTOM; }
        set { this.CLEAR_COVER_BOTTOM = value; }
    }
    public int _CLEAR_COVER_TOP//钢筋保护层顶面
    {
        get { return this.CLEAR_COVER_TOP; }
        set { this.CLEAR_COVER_TOP = value; }
    }
    public int _CLEAR_COVER_OTHER//钢筋保护层其他面
    {
        get { return this.CLEAR_COVER_OTHER; }
        set { this.CLEAR_COVER_OTHER = value; }
    }
    public float _FLOOR_ATTR_THICKNESS_PARAM// 厚度
    {
        get { return this.FLOOR_ATTR_THICKNESS_PARAM; }
        set { this.FLOOR_ATTR_THICKNESS_PARAM = value; }
    }

    public int _WALL_STRUCTURAL_SIGNIFICANT// 结构
    {
        get { return this.WALL_STRUCTURAL_SIGNIFICANT; }
        set { this.WALL_STRUCTURAL_SIGNIFICANT = value; }
    }

    public int _ELEM_CATEGORY_PARAM_MT// 类别
    {
        get { return this.ELEM_CATEGORY_PARAM_MT; }
        set { this.ELEM_CATEGORY_PARAM_MT = value; }
    }
    public int _ELEM_TYPE_PARAM// 类型
    {
        get { return this.ELEM_TYPE_PARAM; }
        set { this.ELEM_TYPE_PARAM = value; }
    }
    public int _SYMBOL_ID_PARAM// 类型ID
    {
        get { return this.SYMBOL_ID_PARAM; }
        set { this.SYMBOL_ID_PARAM = value; }
    }
    public string _ALL_MODEL_TYPE_NAME// 类型名称
    {
        get { return this.ALL_MODEL_TYPE_NAME; }
        set { this.ALL_MODEL_TYPE_NAME = value; }
    }
    public string _FLOORSNUM//楼层
    {
        get { return this.FLOORSNUM; }
        set { this.FLOORSNUM = value; }
    }
    public float _HOST_AREA_COMPUTED// 面积
    {
        get { return this.HOST_AREA_COMPUTED; }
        set { this.HOST_AREA_COMPUTED = value; }
    }
    public float _ROOF_SLOPE// 坡度
    {
        get { return this.ROOF_SLOPE; }
        set { this.ROOF_SLOPE = value; }
    }
    public int _STRUCTURAL_ANALYTICAL_MODEL// 启用分析模型
    {
        get { return this.STRUCTURAL_ANALYTICAL_MODEL; }
        set { this.STRUCTURAL_ANALYTICAL_MODEL = value; }
    }
    public int _DESIGN_OPTION_ID// 设计选项
    {
        get { return this.DESIGN_OPTION_ID; }
        set { this.DESIGN_OPTION_ID = value; }
    }
    public string _CONSTRUCTIONTIME//施工时间
    {
        get { return this.CONSTRUCTIONTIME; }
        set { this.CONSTRUCTIONTIME = value; }
    }
    public float _HOST_VOLUME_COMPUTED//体积
    {
        get { return this.HOST_VOLUME_COMPUTED; }
        set { this.HOST_VOLUME_COMPUTED = value; }
    }
    public int _ALL_MODEL_IMAGE// 图像
    {
        get { return this.ALL_MODEL_IMAGE; }
        set { this.ALL_MODEL_IMAGE = value; }
    }

    public int _RELATED_TO_MASS//与体量相关
    {
        get { return this.RELATED_TO_MASS; }
        set { this.RELATED_TO_MASS = value; }
    }
    public float _HOST_PERIMETER_COMPUTED//周长
    {
        get { return this.HOST_PERIMETER_COMPUTED; }
        set { this.HOST_PERIMETER_COMPUTED = value; }
    }

    public string _ALL_MODEL_INSTANCE_COMMENTS//注释
    {
        get { return this.ALL_MODEL_INSTANCE_COMMENTS; }
        set { this.ALL_MODEL_INSTANCE_COMMENTS = value; }
    }
    public float _FLOOR_HEIGHTABOVELEVEL_PARAM//自标高的高度偏移
    {
        get { return this.FLOOR_HEIGHTABOVELEVEL_PARAM; }
        set { this.FLOOR_HEIGHTABOVELEVEL_PARAM = value; }
    }
    public int _ELEM_FAMILY_PARAM//族
    {
        get { return this.ELEM_FAMILY_PARAM; }
        set { this.ELEM_FAMILY_PARAM = value; }
    }
    public string _ALL_MODEL_FAMILY_NAME//族名称
    {
        get { return this.ALL_MODEL_FAMILY_NAME; }
        set { this.ALL_MODEL_FAMILY_NAME = value; }
    }
    public int _ELEM_FAMILY_AND_TYPE_PARAM//族与类型
    {
        get { return this.ELEM_FAMILY_AND_TYPE_PARAM; }
        set { this.ELEM_FAMILY_AND_TYPE_PARAM = value; }
    }
    //public string _XDirectBottom//x方向底筋
    //{
    //    get { return this.XDirectBottom; }
    //    set { this.XDirectBottom = value; }
    //}
    //public string _XDirectFace//x方向面筋
    //{
    //    get { return this.XDirectFace; }
    //    set { this.XDirectFace = value; }
    //}
    //public string _YDirectBottom//Y方向底筋
    //{
    //    get { return this.YDirectBottom; }
    //    set { this.YDirectBottom = value; }
    //}
    //public string _YDirectFace//Y方向面筋
    //{
    //    get { return this.YDirectFace; }  
    //    set { this.YDirectFace = value; }
    //}
    //public string _FloorsNum//板编号
    //{
    //    get { return this.FloorsNum; }
    //    set { this.FloorsNum = value; }
    //}
    //public string _DistributeRebar//分布筋
    //{
    //    get { return this.DistributeRebar; }
    //    set { this.DistributeRebar = value; }
    //}
    //public string _RebarUnionNum//钢筋关联板编号
    //{
    //    get { return this.RebarUnionNum; }
    //    set { this.RebarUnionNum = value; }
    //}
    //public int _IFSETREBAR//是否已配筋
    //{
    //    get { return this.IFSETREBAR; }
    //    set { this.IFSETREBAR = value; }
    //}
    //public int _HOST_SSE_CURVED_EDGE_CONDITION_PARAM//弯曲边缘条件
    //{
    //    get { return this.HOST_SSE_CURVED_EDGE_CONDITION_PARAM; }
    //    set { this.HOST_SSE_CURVED_EDGE_CONDITION_PARAM = value; }
    //}
    //public string _TemperatureRebar//温度筋
    //{
    //    get { return this.TemperatureRebar; }
    //    set { this.TemperatureRebar = value; }
    //}

}
