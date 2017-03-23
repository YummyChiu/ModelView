using UnityEngine;
using System.Collections;

public class Frameworks : MonoBehaviour {
   // public int Model_Id;//模型的ID
    public string PDMODELID;//PDModelID    
    public int Y_JUSTIFICATION;//Y轴对正
    public float Y_OFFSET_VALUE;//Y轴偏移值
    public int YZ_JUSTIFICATION;//XY轴对正
    public int Z_JUSTIFICATION;//Z轴对正
    public float Z_OFFSET_VALUE;//Z轴偏移值
    public int LEVEL_PARAM;//标高
    public string ALL_MODEL_MARK;//标记
    public int INSTANCE_REFERENCE_LEVEL_PARAM;//参照标高
    public float STRUCTURAL_REFERENCE_LEVEL_ELEVATION;//参照标高高程
    public int PHASE_DEMOLISHED;//拆除的阶段  
    public int PHASE_CREATED;//创建的阶段 
    public float STRUCTURAL_ELEVATION_AT_BOTTOM;//底部高程
    public float STRUCTURAL_ELEVATION_AT_TOP;//顶部高程
    public int STRUCTURAL_BEAM_ORIENTATION;//方向
    public int CLEAR_COVER_BOTTOM;//钢筋保护底面
    public int CLEAR_COVER_TOP;//钢筋保护层顶面
    public int CLEAR_COVER_OTHER;//钢筋保护层其他面
    public string SKETCH_PLANE_PARAM;//工作平面
    public float STRUCTURAL_BEND_DIR_ANGLE;//横截面旋转
    public float STRUCTURAL_FRAME_CUT_LENGTH;//剪切长度
    public int STRUCTURAL_MATERIAL_PARAM;//结构材质
    public int INSTANCE_STRUCT_USAGE_PARAM;// 结构用途
    public int ELEM_CATEGORY_PARAM_MT;// 类别
    public int ELEM_TYPE_PARAM;//类型
    public int SYMBOL_ID_PARAM;// 类型ID
    public string ALL_MODEL_TYPE_NAME;// 类型名称
    public int STRUCT_FRAM_JOIN_STATUS;//连接状态
    public string FLOORSNUM;//楼层
    public float HOST_AREA_COMPUTED;// 面积
    public int STRUCTURAL_ANALYTICAL_MODEL;// 启用分析模型
    public float STRUCTURAL_BEAM_END0_ELEVATION;//起点标高偏移
    public int DESIGN_OPTION_ID;// 设计选项
    public string CONSTRUCTIONTIME;//施工时间
    public float HOST_VOLUME_COMPUTED;// 体积
    public int ALL_MODEL_IMAGE;// 图像
    public float CURVE_ELEM_LENGTH;//长度
    public float STRUCTURAL_BEAM_END1_ELEVATION;//终点标高偏移
    public int HOST_ID_PARAM;//主体ID
    public string ALL_MODEL_INSTANCE_COMMENTS;//注释
    public int ELEM_FAMILY_PARAM;//族
    public string ALL_MODEL_FAMILY_NAME;//族名称
    public int ELEM_FAMILY_AND_TYPE_PARAM;//族与类型

    //public string AnglesRebar;//对角斜筋
    //public string BeamNum;//梁编号
    //public string BeamGEJING;//梁箍筋
    //public string BeamUp;//梁上部纵筋
    //public string BeamDown;//梁下部纵筋
    //public string WaistBeam;//梁腰筋
    //public string BeamRight;//梁右支座筋
    //public string BeamLeft;//梁左支座筋
    //public int IFSETREBAR;//是否已配筋
    //public int _Model_Id
    //{
    //    get { return this.Model_Id; }
    //    set { this.Model_Id = value; }
    //}
    public string _PDModelID//PDModelID
    {
        get { return this.PDMODELID; }
        set { this.PDMODELID = value; }
    }
    public int _Y_JUSTIFICATION //Y轴对正
    {
        get { return this.Y_JUSTIFICATION; }
        set { this.Y_JUSTIFICATION = value; }
    }
    public float _Y_OFFSET_VALUE  //Y轴偏移值
    {
        get { return this.Y_OFFSET_VALUE; }
        set { this.Y_OFFSET_VALUE = value; }
    }
    public int _YZ_JUSTIFICATION  //XY轴对正
    {
        get { return this.YZ_JUSTIFICATION; }
        set { this.YZ_JUSTIFICATION = value; }
    }
    public int _Z_JUSTIFICATION //Z轴对正
    {
        get { return this.Z_JUSTIFICATION ; }
        set { this.Z_JUSTIFICATION  = value; }
    }
    public float _Z_OFFSET_VALUE  //Z轴偏移值
    {
        get { return this.Z_OFFSET_VALUE; }
        set { this.Z_OFFSET_VALUE = value; }
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
    public int _INSTANCE_REFERENCE_LEVEL_PARAM //参照标高
    {
        get { return this.INSTANCE_REFERENCE_LEVEL_PARAM; }
        set { this.INSTANCE_REFERENCE_LEVEL_PARAM = value; }
    }
    public float _STRUCTURAL_REFERENCE_LEVEL_ELEVATION  //参照标高高程
    {
        get { return this.STRUCTURAL_REFERENCE_LEVEL_ELEVATION; }
        set { this.STRUCTURAL_REFERENCE_LEVEL_ELEVATION = value; }
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
    public float _STRUCTURAL_ELEVATION_AT_BOTTOM //底部高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_BOTTOM; }
        set { this.STRUCTURAL_ELEVATION_AT_BOTTOM = value; }
    }
    public float _STRUCTURAL_ELEVATION_AT_TOP  //顶部高程
    {
        get { return this.STRUCTURAL_ELEVATION_AT_TOP; }
        set { this.STRUCTURAL_ELEVATION_AT_TOP = value; }
    }
    public int _STRUCTURAL_BEAM_ORIENTATION   //方向
    {
        get { return this.STRUCTURAL_BEAM_ORIENTATION; }
        set { this.STRUCTURAL_BEAM_ORIENTATION = value; }
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
    public string _SKETCH_PLANE_PARAM //工作平面
    {
        get { return this.SKETCH_PLANE_PARAM; }
        set { this.SKETCH_PLANE_PARAM = value; }
    }
    public float _STRUCTURAL_BEND_DIR_ANGLE  //横截面旋转
    {
        get { return this.STRUCTURAL_BEND_DIR_ANGLE; }
        set { this.STRUCTURAL_BEND_DIR_ANGLE = value; }
    }
    public float _STRUCTURAL_FRAME_CUT_LENGTH   //剪切长度
    {
        get { return this.STRUCTURAL_FRAME_CUT_LENGTH; }
        set { this.STRUCTURAL_FRAME_CUT_LENGTH = value; }
    }

    public int _STRUCTURAL_MATERIAL_PARAM//结构材质
    {
        get { return this.STRUCTURAL_MATERIAL_PARAM; }
        set { this.STRUCTURAL_MATERIAL_PARAM = value; }
    }
    public int _INSTANCE_STRUCT_USAGE_PARAM //结构用途
    {
        get { return this.INSTANCE_STRUCT_USAGE_PARAM; }
        set { this.INSTANCE_STRUCT_USAGE_PARAM = value; }
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
    public int _STRUCT_FRAM_JOIN_STATUS // 连接状态
    {
        get { return this.STRUCT_FRAM_JOIN_STATUS; }
        set { this.STRUCT_FRAM_JOIN_STATUS = value; }
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
    public int _STRUCTURAL_ANALYTICAL_MODEL// 启用分析模型
    {
        get { return this.STRUCTURAL_ANALYTICAL_MODEL; }
        set { this.STRUCTURAL_ANALYTICAL_MODEL = value; }
    }
    public float _STRUCTURAL_BEAM_END0_ELEVATION // 起点标高偏移
    {
        get { return this.STRUCTURAL_BEAM_END0_ELEVATION; }
        set { this.STRUCTURAL_BEAM_END0_ELEVATION = value; }
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
    public float _CURVE_ELEM_LENGTH// 长度
    {
        get { return this.CURVE_ELEM_LENGTH; }
        set { this.CURVE_ELEM_LENGTH = value; }
    }
    public float _STRUCTURAL_BEAM_END1_ELEVATION // 终点标高偏移
    {
        get { return this.STRUCTURAL_BEAM_END1_ELEVATION; }
        set { this.STRUCTURAL_BEAM_END1_ELEVATION = value; }
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
    //public string _AnglesRebar
    //{
    //    get { return this.AnglesRebar; }
    //    set { this.AnglesRebar = value; }
    //}
    //public string _BeamNum
    //{
    //    get { return this.BeamNum; }
    //    set { this.BeamNum = value; }
    //}
    //public string _BeamGEJING
    //{
    //    get { return this.BeamGEJING; }
    //    set { this.BeamGEJING = value; }
    //}
    //public string _BeamUp
    //{
    //    get { return this.BeamUp; }
    //    set { this.BeamUp = value; }
    //}
    //public string _BeamDown
    //{
    //    get { return this.BeamDown; }
    //    set { this.BeamDown = value; }
    //}
    //public string _WaistBeam
    //{
    //    get { return this.WaistBeam; }
    //    set { this.WaistBeam = value; }
    //}
    //public string _BeamRight
    //{
    //    get { return this.BeamRight; }
    //    set { this.BeamRight = value; }
    //}
    //public string _BeamLeft
    //{
    //    get { return this.BeamLeft; }
    //    set { this.BeamLeft = value; }
    //}
    //public int _IFSETREBAR//是否已配筋
    //{
    //    get { return this.IFSETREBAR; }
    //    set { this.IFSETREBAR = value; }
    //}
}
