using UnityEngine;
using System.Collections;

public class Commons : MonoBehaviour {

    public int LEVEL_PARAM;//标高
    public string ALL_MODEL_MARK;//标记
    public string PHASE_DEMOLISHED;//拆除的阶段  
    public string PHASE_CREATED;//创建的阶段 
    public double CAVE_WIDTH;//洞口宽
    public double CAVE_LONG;//洞口长
    public string SKETCH_PLANE_PARAM;//工作平面
    public double ELEM_CATEGORY_PARAM_MT;// 类别
    public int ELEM_TYPE_PARAM;//类型
    public double SYMBOL_ID_PARAM;// 类型ID
    public string ALL_MODEL_TYPE_NAME;// 类型名称
    public string FLOORSNUM;//楼层
    public double HOST_AREA_COMPUTED;// 面积
    public int INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM;//明细表标高
    public double INSTANCE_FREE_HOST_OFFSET_PARAM;//偏移量
    public string DESIGN_OPTION_ID;// 设计选项
    public string CONSTRUCTIONTIME;//施工时间
    public double HOST_VOLUME_COMPUTED;// 体积
    public string ALL_MODEL_IMAGE;// 图像
    public double TOP_OF_POINT;//线顶点
    public int HOST_ID_PARAM;//主体ID
    public string ALL_MODEL_INSTANCE_COMMENTS;//注释
    public string ELEM_FAMILY_PARAM;//族
    public string ALL_MODEL_FAMILY_NAME;//族名称
    public string ELEM_FAMILY_AND_TYPE_PARAM;//族与类型



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
    public double _CAVE_WIDTH//洞口宽
    {
        get { return this.CAVE_WIDTH; }
        set { this.CAVE_WIDTH = value; }
    }
    public double _CAVE_LONG//洞口长
    {
        get { return this.CAVE_LONG; }
        set { this.CAVE_LONG = value; }
    }

    public string _SKETCH_PLANE_PARAM //工作平面
    {
        get { return this.SKETCH_PLANE_PARAM; }
        set { this.SKETCH_PLANE_PARAM = value; }
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
    public int _INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM// 明细表标高
    {
        get { return this.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM; }
        set { this.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM = value; }
    }
    public double _INSTANCE_FREE_HOST_OFFSET_PARAM// 偏移量
    {
        get { return this.INSTANCE_FREE_HOST_OFFSET_PARAM; }
        set { this.INSTANCE_FREE_HOST_OFFSET_PARAM = value; }
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
    public double _TOP_OF_POINT//线顶点
    {
        get { return this.TOP_OF_POINT; }
        set { this.TOP_OF_POINT = value; }
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
