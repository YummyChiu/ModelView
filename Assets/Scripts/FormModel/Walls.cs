using UnityEngine;
using System.Collections;

public  class Walls:MonoBehaviour
{
   // public int Model_Id;//模型的ID
    public string PDMODELID;//PDModelID
    public int PDSTMATERIAL;//PDST材质
    public string ALL_MODEL_MARK;//标记
    public int PHASE_DEMOLISHED;//拆除的阶段  
    public int PHASE_CREATED;//创建的阶段 
    public float WALL_BASE_OFFSET;//底部偏移 
    public int WALL_BASE_CONSTRAINT;//底部限制条件
    public float WALL_BOTTOM_EXTENSION_DIST_PARAM;// 底部延伸距离 
    public float WALL_TOP_OFFSET;// 顶部偏移
    public float WALL_TOP_EXTENSION_DIST_PARAM;// 顶部延伸距离
    public int WALL_HEIGHT_TYPE;// 顶部约束
    public int WALL_KEY_REF_PARAM;// 定位线
    public int WALL_ATTR_ROOM_BOUNDING;// 房间边界
    public int CLEAR_COVER_INTERIOR;//钢筋保护层 - 内部面
    public int CLEAR_COVER_OTHER;//钢筋保护层 - 其他面
    public int CLEAR_COVER_EXTERIOR;//钢筋保护层 - 外部面
    public int WALL_STRUCTURAL_SIGNIFICANT;// 结构
    public int WALL_STRUCTURAL_USAGE_PARAM;// 结构用途
    public int ELEM_CATEGORY_PARAM_MT;// 类别
    public int ELEM_TYPE_PARAM;//类型
    public int SYMBOL_ID_PARAM;// 类型ID
    public string ALL_MODEL_TYPE_NAME;// 类型名称
    public string FLOORSNUM;//楼层
    public float HOST_AREA_COMPUTED;// 面积
    public int STRUCTURAL_ANALYTICAL_MODEL;// 启用分析模型
    public int DESIGN_OPTION_ID;// 设计选项
    public string CONSTRUCTIONTIME;//施工时间
    public float HOST_VOLUME_COMPUTED;// 体积
    public int ALL_MODEL_IMAGE;// 图像
    public float WALL_USER_HEIGHT_PARAM;// 无连接高度
    public int WALL_BOTTOM_IS_ATTACHED;//已附着底部
    public int WALL_TOP_IS_ATTACHED;//已附着顶部
    public int RELATED_TO_MASS;//与体量相关
    public float CURVE_ELEM_LENGTH;//长度
    public string ALL_MODEL_INSTANCE_COMMENTS;//注释
    public int ELEM_FAMILY_PARAM;//族
    public string ALL_MODEL_FAMILY_NAME;//族名称
    public int ELEM_FAMILY_AND_TYPE_PARAM;//族与类型

    //public string ANZHUGUJING;//暗柱箍筋
    //public string ANZHUGUJINLEIXING;//暗柱箍筋类型
    //public string ANZHUZONGJING;//暗柱纵筋
    //public string WallNum;//墙编号
    //public string WallLAJING;//墙身拉筋
    //public int IFSETREBAR;//是否已配筋
    //public string VERTICALSETREBAR;//竖向分布钢筋
    //public string HORIZONTALSETREBAR;//水平分布钢筋
   public Walls()
    {
       
    }
   public Walls(Walls wall)
    {

    }

    //public int _Model_Id//模型的ID
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
    public float _WALL_BASE_OFFSET//底部偏移
    {
        get { return this.WALL_BASE_OFFSET; }
        set { this.WALL_BASE_OFFSET = value; }
    }
    public int _WALL_BASE_CONSTRAINT//底部限制条件
    {
        get { return this.WALL_BASE_CONSTRAINT; }
        set { this.WALL_BASE_CONSTRAINT = value; }
    }
    public float _WALL_BOTTOM_EXTENSION_DIST_PARAM// 底部延伸距离 
    {
        get { return this.WALL_BOTTOM_EXTENSION_DIST_PARAM; }
        set { this.WALL_BOTTOM_EXTENSION_DIST_PARAM = value; }
    }
    public float _WALL_TOP_OFFSET// 顶部偏移
    {
        get { return this.WALL_TOP_OFFSET; }
        set { this.WALL_TOP_OFFSET = value; }
    }
    public float _WALL_TOP_EXTENSION_DIST_PARAM// 顶部延伸距离
    {
        get { return this.WALL_TOP_EXTENSION_DIST_PARAM; }
        set { this.WALL_TOP_EXTENSION_DIST_PARAM = value; }
    }
    public int _WALL_HEIGHT_TYPE// 顶部约束
    {
        get { return this.WALL_HEIGHT_TYPE; }
        set { this.WALL_HEIGHT_TYPE = value; }
    }
    public int _WALL_KEY_REF_PARAM// 定位线
    {
        get { return this.WALL_KEY_REF_PARAM; }
        set { this.WALL_KEY_REF_PARAM = value; }
    }
    public int _WALL_ATTR_ROOM_BOUNDING// 房间边界
    {
        get { return this.WALL_ATTR_ROOM_BOUNDING; }
        set { this.WALL_ATTR_ROOM_BOUNDING = value; }
    }
    public int _CLEAR_COVER_INTERIOR//钢筋保护层 - 内部面
    {
        get { return this.CLEAR_COVER_INTERIOR; }
        set { this.CLEAR_COVER_INTERIOR = value; }
    }
    public int _CLEAR_COVER_OTHER//钢筋保护层 - 其他面
    {
        get { return this.CLEAR_COVER_OTHER; }
        set { this.CLEAR_COVER_OTHER = value; }
    }
    public int _CLEAR_COVER_EXTERIOR//钢筋保护层 - 外部面
    {
        get { return this.CLEAR_COVER_EXTERIOR; }
        set { this.CLEAR_COVER_EXTERIOR = value; }
    }

    public int _WALL_STRUCTURAL_SIGNIFICANT// 结构
    {
        get { return this.WALL_STRUCTURAL_SIGNIFICANT; }
        set { this.WALL_STRUCTURAL_SIGNIFICANT = value; }
    }
    public int _WALL_STRUCTURAL_USAGE_PARAM// 结构用途
    {
        get { return this.WALL_STRUCTURAL_USAGE_PARAM; }
        set { this.WALL_STRUCTURAL_USAGE_PARAM = value; }
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
    public float _WALL_USER_HEIGHT_PARAM// 无连接高度
    {
        get { return this.WALL_USER_HEIGHT_PARAM; }
        set { this.WALL_USER_HEIGHT_PARAM = value; }
    }
    public int _WALL_BOTTOM_IS_ATTACHED//已附着底部
    {
        get { return this.WALL_BOTTOM_IS_ATTACHED; }
        set { this.WALL_BOTTOM_IS_ATTACHED = value; }
    }
    public int _WALL_TOP_IS_ATTACHED//已附着顶部
    {
        get { return this.WALL_TOP_IS_ATTACHED; }
        set { this.WALL_TOP_IS_ATTACHED = value; }
    }
    public int _RELATED_TO_MASS//与体量相关
    {
        get { return this.RELATED_TO_MASS; }
        set { this.RELATED_TO_MASS = value; }
    }
    public float _CURVE_ELEM_LENGTH//长度
    {
        get { return this.CURVE_ELEM_LENGTH; }
        set { this.CURVE_ELEM_LENGTH = value; }
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
    //public string _ANZHUGUJING//暗柱箍筋
    //{
    //    get { return this.ANZHUGUJING; }
    //    set { this.ANZHUGUJING = value; }
    //}
    //public string _ANZHUGUJINLEIXING//暗柱箍筋类型
    //{ get { return this.ANZHUGUJINLEIXING; }
    //    set { this.ANZHUGUJINLEIXING = value; }
    //}
    //public string _ANZHUZONGJING//暗柱纵筋
    //{
    //    get { return this.ANZHUZONGJING; }
    //    set { this.ANZHUZONGJING = value; }
    //}

    //public string _WallNum//墙编号
    //{
    //    get { return this.WallNum; }
    //    set { this.WallNum = value; }
    //}
    //public string _WallLAJING//墙身拉筋
    //{
    //    get { return this.WallLAJING; }
    //    set { this.WallLAJING = value; }
    //}
    //public int _IFSETREBAR//是否已配筋
    //{
    //    get { return this.IFSETREBAR; }
    //    set { this.IFSETREBAR = value; }
    //}
    //public string _VERTICALSETREBAR//竖向分布钢筋
    //{
    //    get { return this.VERTICALSETREBAR; }
    //    set { this.VERTICALSETREBAR = value; }
    //}

    //public string _HORIZONTALSETREBAR//水平分布钢筋
    //{
    //    get { return this.HORIZONTALSETREBAR; }
    //    set { this.HORIZONTALSETREBAR = value; }
    //}

}
