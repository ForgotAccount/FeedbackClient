using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackClient
{
    public class FeedbackRequest
    {
        public List<DataSet> DATASET { get; set; }
    }

    public class DataSet
    {
        /// <summary>
        /// 分拣任务号
        /// </summary>
        public string PICK_TASK_NUM { get; set; }
        /// <summary>
        /// 分拣日期
        /// </summary>
        public string PICK_DATE { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        public string COM_ID { get; set; }
        /// <summary>
        /// 分拣线编码
        /// </summary>
        public string PICK_LINE_ID { get; set; }
        /// <summary>
        /// 零售户总数
        /// </summary>
        public int CUST_COUNT { get; set; }
        /// <summary>
        /// 分拣总量(条)
        /// </summary>
        public int QTY_SUM { get; set; }
        /// <summary>
        /// 下发条零关联时间
        /// </summary>
        public string ISSUE_TIME { get; set; }
        public List<COList> CO_LIST { get; set; }
    }

    public class COList
    {
        /// <summary>
        /// 分拣执行订单编号
        /// </summary>
        public string PICK_CO_NUM { get; set; }
        /// <summary>
        /// 分拣顺序号
        /// </summary>
        public int SEQ { get; set; }
        /// <summary>
        /// 零售户订单编码
        /// </summary>
        public string CO_NUM { get; set; }
        public List<CODetail> CO_DETAIL_LIST { get; set; }
    }

    public class CODetail
    {
        /// <summary>
        /// 短号
        /// </summary>
        public string ITEM_ID { get; set; }
        /// <summary>
        /// 分拣数量(条)
        /// </summary>
        public int QTY { get; set; }
    }
    /// <summary>
    /// 订单基础对象
    /// </summary>
    class OrderBaseModel
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 分拣序号
        /// </summary>
        public int SortingNO { get; set; }
        /// <summary>
        /// 包序号
        /// </summary>
        public int PackingNO { get; set; }
        /// <summary>
        /// 最大包数量
        /// </summary>
        public int PackingMaximumNO { get; set; }
        /// <summary>
        /// 订单标识
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 烟名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 电子标签地址
        /// </summary>
        public string TagAddress { get; set; }
        /// <summary>
        /// 当前条烟所需的数量
        /// </summary>
        public int Amount { get; set; }
        public int ProductID { get; set; }
    }
    class OrderPOCO : OrderBaseModel
    {
        /// <summary>
        /// 分拣状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 客户名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 送货序号
        /// </summary>
        public int DeliverySeq { get; set; }
        /// <summary>
        /// 送货线路名
        /// </summary>
        public string DeliveryName { get; set; }
        /// <summary>
        /// 送货线路编码
        /// </summary>
        public string DeliveryCode { get; set; }
        /// <summary>
        /// 订单下的总条烟数量
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 订单序号
        /// </summary>
        public int OrderNO { get; set; }
    }

}
