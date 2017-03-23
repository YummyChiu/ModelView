using UnityEngine;
using System.Collections;

public class Structures : MonoBehaviour {


    //public string PDST_C_B01;//PDST_P1
    //public string PDST_C_BID;//PDST_C_BID
    //public string PDST_C_FC01;//PDST_C_FC01
    //public string PDST_C_FC02;//PDST_C_FC02
    //public string PDST_C_FC03;//PDST_C_FC03
    //public string PDST_C_FC04;//PDST_C_FC04
    //public string PDST_C_FCID;//PDST_C_FCID
    //public string PDST_C_H01;//PDST_C_H01
    //public string PDST_C_P1;//PDST_C_P1
    //public string PDST_C_P2;//PDST_C_P2
    //public string PDST_C_P3;//PDST_C_P3
    //public string PDST_C_P4;//PDST_C_P4
    //public string PDST_P1;//PDST_P1
    //public string PDST_P2;//PDST_P2
    //public string PDST_P3;//PDST_P3
    public string PDMODELID;//PDModelID   
    public int LEVEL_PARAM;//标高
    public string ALL_MODEL_MARK;//标记
    public string PHASE_DEMOLISHED;//拆除的阶段  
    public string PHASE_CREATED;//创建的阶段 
    public int FAMILY_BASE_LEVEL_PARAM;//底部标高
    public int FAMILY_BASE_LEVEL_OFFSET_PARAM;//底部偏移
    public int SCHEDULE_TOP_LEVEL_PARAM;//顶部标高
    public int FAMILY_TOP_LEVEL_OFFSET_PARAM;//顶部偏移
    public int WALL_ATTR_ROOM_BOUNDING;// 房间边界
    public int CLEAR_COVER_BOTTOM;//钢筋保护底面
    public int CLEAR_COVER_TOP;//钢筋保护层顶面
    public int CLEAR_COVER_OTHER;//钢筋保护层其他面
    public int STRUCTURAL_MATERIAL_PARAM;//结构材质
    public double ELEM_CATEGORY_PARAM_MT;// 类别
    public int ELEM_TYPE_PARAM;//类型
    public double SYMBOL_ID_PARAM;// 类型ID
    public string ALL_MODEL_TYPE_NAME;// 类型名称
    public string FLOORSNUM;//楼层
    public double HOST_AREA_COMPUTED;// 面积
    public int STRUCTURAL_ANALYTICAL_MODEL;// 启用分析模型
    public string DESIGN_OPTION_ID;// 设计选项
    public string CONSTRUCTIONTIME;//施工时间
    public int INSTANCE_MOVES_WITH_GRID_PARAM;//随轴网移动
    public double HOST_VOLUME_COMPUTED;// 体积
    public string ALL_MODEL_IMAGE;// 图像
    public double CURVE_ELEM_LENGTH;//长度
    public int HOST_ID_PARAM;//主体ID
    public string ALL_MODEL_INSTANCE_COMMENTS;//注释
    public string COLUMN_LOCATION_MARK;//柱定位标记
    public int SLANTED_COLUMN_TYPE_PARAM;//柱样式
    public string ELEM_FAMILY_PARAM;//族
    public string ALL_MODEL_FAMILY_NAME;//族名称
    public string ELEM_FAMILY_AND_TYPE_PARAM;//族与类型


    public string _PDModelID//PDModelID
    {
        get { return this.PDMODELID; }
        set { this.PDMODELID = value; }
    }
    //public string _PDST_C_B01//PDST_C_B01
    //{
    //    get { return this.PDST_C_B01; }
    //    set { this.PDST_C_B01 = value; }
    //}
    //public string _PDST_C_BID//PDST_C_BID
    //{
    //    get { return this.PDST_C_BID; }
    //    set { this.PDST_C_BID = value; }
    //}
    //public string _PDST_C_FC01//PDST_C_FC01
    //{
    //    get { return this.PDST_C_FC01; }
    //    set { this.PDST_C_FC01 = value; }
    //}
    //public string _PDST_C_FC02//PDST_C_FC02
    //{
    //    get { return this.PDST_C_FC02; }
    //    set { this.PDST_C_FC02 = value; }
    //}
    //public string _PDST_C_FC03//PDST_C_FC03
    //{
    //    get { return this.PDST_C_FC03; }
    //    set { this.PDST_C_FC03 = value; }
    //}
    //public string _PDST_C_FC04//PDST_C_FC04
    //{
    //    get { return this.PDST_C_FC04; }
    //    set { this.PDST_C_FC04 = value; }
    //}
    //public string _PDST_C_FCID//PDST_C_FCID
    //{
    //    get { return this.PDST_C_FCID; }
    //    set { this.PDST_C_FCID = value; }
    //}
    //public string _PDST_C_H01//PDST_C_H01
    //{
    //    get { return this.PDST_C_H01; }
    //    set { this.PDST_C_H01 = value; }
    //}
    //public string _PDST_C_P1//PDST_C_P1
    //{
    //    get { return this.PDST_C_P1; }
    //    set { this.PDST_C_P1 = value; }
    //}
    //public string _PDST_C_P2//PDST_C_P2
    //{
    //    get { return this.PDST_C_P2; }
    //    set { this.PDST_C_P2 = value; }
    //}
    //public string _PDST_C_P3//PDST_C_P3
    //{
    //    get { return this.PDST_C_P3; }
    //    set { this.PDST_C_P3 = value; }
    //}
    //public string _PDST_C_P4//PDST_C_P4
    //{
    //    get { return this.PDST_C_P4; }
    //    set { this.PDST_C_P4 = value; }
    //}
    //public string _PDST_P1//PDST_P1
    //{
    //    get { return this.PDST_P1; }
    //    set { this.PDST_P1 = value; }
    //}
    //public string _PDST_P2//PDST_P2
    //{
    //    get { return this.PDST_P2; }
    //    set { this.PDST_P2 = value; }
    //}
    //public string _PDST_P3//PDST_P3
    //{
    //    get { return this.PDST_P3; }
    //    set { this.PDST_P3 = value; }
    //}
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
    public string _PHASE_DEMOLISHED//拆除的阶段
    {
        get { return this.PHASE_DEMOLISHED; }
        set { this.PHASE_DEMOLISHED = value; }
    }
    public string _PHASE_CREATED//创建的阶段
    {
        get { return this.PHASE_CREATED; }
        set { this.PHASE_CREATED = value; }
    }
    public int _FAMILY_BASE_LEVEL_PARAM//底部标高
    {
        get { return this.FAMILY_BASE_LEVEL_PARAM;}
        set { this.FAMILY_BASE_LEVEL_PARAM = value; }
    }
    public int _FAMILY_BASE_LEVEL_OFFSET_PARAM//底部偏移
    {
        get { return this.FAMILY_BASE_LEVEL_OFFSET_PARAM; }
        set { this.FAMILY_BASE_LEVEL_OFFSET_PARAM = value; }
    }
    public int _SCHEDULE_TOP_LEVEL_PARAM//顶部标高
    {
        get { return this.SCHEDULE_TOP_LEVEL_PARAM; }
        set { this.SCHEDULE_TOP_LEVEL_PARAM = value; }
    }
    public int _FAMILY_TOP_LEVEL_OFFSET_PARAM//顶部偏移
    {
        get { return this.FAMILY_TOP_LEVEL_OFFSET_PARAM; }
        set { this.FAMILY_TOP_LEVEL_OFFSET_PARAM = value; }
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
    public int _STRUCTURAL_MATERIAL_PARAM//结构材质
    {
        get { return this.STRUCTURAL_MATERIAL_PARAM; }
        set { this.STRUCTURAL_MATERIAL_PARAM = value; }
    }


    public double _ELEM_CATEGORY_PARAM_MT// 类别
    {
        get { return this.ELEM_CATEGORY_PARAM_MT; }
        set { this.ELEM_CATEGORY_PARAM_MT = value; }
    }
    public int _ELEM_TYPE_PARAM// 类型
    {
        get { return this.ELEM_TYPE_PARAM; }
        set { this.ELEM_TYPE_PARAM = value; }
    }
    public double _SYMBOL_ID_PARAM// 类型ID
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
    public double _HOST_AREA_COMPUTED// 面积
    {
        get { return this.HOST_AREA_COMPUTED; }
        set { this.HOST_AREA_COMPUTED = value; }
    }
    public int _STRUCTURAL_ANALYTICAL_MODEL// 启用分析模型
    {
        get { return this.STRUCTURAL_ANALYTICAL_MODEL; }
        set { this.STRUCTURAL_ANALYTICAL_MODEL = value; }
    }
    public string _DESIGN_OPTION_ID// 设计选项
    {
        get { return this.DESIGN_OPTION_ID; }
        set { this.DESIGN_OPTION_ID = value; }
    }
    public string _CONSTRUCTIONTIME//施工时间
    {
        get { return this.CONSTRUCTIONTIME; }
        set { this.CONSTRUCTIONTIME = value; }
    }
    public int _INSTANCE_MOVES_WITH_GRID_PARAM//随轴网移动
    {
        get { return this.INSTANCE_MOVES_WITH_GRID_PARAM; }
        set { this.INSTANCE_MOVES_WITH_GRID_PARAM = value; }
    }

    public double _HOST_VOLUME_COMPUTED//体积
    {
        get { return this.HOST_VOLUME_COMPUTED; }
        set { this.HOST_VOLUME_COMPUTED = value; }
    }
    public string _ALL_MODEL_IMAGE// 图像
    {
        get { return this.ALL_MODEL_IMAGE; }
        set { this.ALL_MODEL_IMAGE = value; }
    }
    public double _CURVE_ELEM_LENGTH// 长度
    {
        get { return this.CURVE_ELEM_LENGTH; }
        set { this.CURVE_ELEM_LENGTH = value; }
    }
    public int _HOST_ID_PARAM// 主体ID
    {
        get { return this.HOST_ID_PARAM; }
        set { this.HOST_ID_PARAM = value; }
    }
    public string _ALL_MODEL_INSTANCE_COMMENTS//注释
    {
        get { return this.ALL_MODEL_INSTANCE_COMMENTS; }
        set { this.ALL_MODEL_INSTANCE_COMMENTS = value; }
    }
    public string _COLUMN_LOCATION_MARK//柱定位标记
    {
        get { return this.COLUMN_LOCATION_MARK; }
        set { this.COLUMN_LOCATION_MARK = value; }
    }
    public int _SLANTED_COLUMN_TYPE_PARAM//柱样式
    {
        get { return this.SLANTED_COLUMN_TYPE_PARAM; }
        set { this.SLANTED_COLUMN_TYPE_PARAM = value; }
    }
    public string _ELEM_FAMILY_PARAM//族
    {
        get { return this.ELEM_FAMILY_PARAM; }
        set { this.ELEM_FAMILY_PARAM = value; }
    }
    public string _ALL_MODEL_FAMILY_NAME//族名称
    {
        get { return this.ALL_MODEL_FAMILY_NAME; }
        set { this.ALL_MODEL_FAMILY_NAME = value; }
    }
    public string _ELEM_FAMILY_AND_TYPE_PARAM//族与类型
    {
        get { return this.ELEM_FAMILY_AND_TYPE_PARAM; }
        set { this.ELEM_FAMILY_AND_TYPE_PARAM = value; }
    }
}
