using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedbackClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<OrderPOCO> _orderPOCOs;
        public MainWindow()
        {
            InitializeComponent();
            tbUrl.Text = GetConfig(keyName: "ServerURL");
            tbCoCode.Text = GetConfig(keyName: "CompanyCode");
            tbSortingLineCode.Text = GetConfig(keyName: "SortLineCode");
            _orderPOCOs = GetOrderDatas();
        }
        private List<OrderPOCO> GetOrderDatas()
        {
            var data = DAO.getPack(1);
            var list = new List<OrderPOCO>();
            int index = 0;
            foreach (var item in data)
            {
                list.Add(new OrderPOCO
                {
                    Amount = int.Parse(item["need_num"].ToString()),
                    CustomerCode = item["customer_code"].ToString(),
                    CustomerName = item["customer_name"].ToString(),
                    DeliveryName = item["route_name"].ToString(),
                    OrderID = item["order_id"].ToString(),
                    ProductName = item["product_name"].ToString(),
                    TagAddress = item["tag_id"].ToString(),
                    State = item["state"].ToString() == "0" ? "未分拣" : "已分拣",
                    DeliverySeq = int.Parse(item["deliver_seq"].ToString()),
                    TotalAmount = int.Parse(item["sum_num"].ToString()),
                    DeliveryCode = item["route_id"].ToString(),
                    PackingMaximumNO = int.Parse(item["max_pack"].ToString()),
                    SortingNO = int.Parse(item["sort_num"].ToString()),
                    PackingNO = int.Parse(item["pack_num"].ToString()),
                    OrderNO = int.Parse(item["order_sort"].ToString()),
                    Index = index,
                    ProductID = int.Parse(item["product_id"].ToString())
                });
                index++;
            }
            var p = list.GroupBy(g => g.OrderID);
            foreach (var item in p)
            {
                var q = item.GroupBy(g => g.TagAddress).Where(w => w.Count() > 1);
                if (q != null && q.Count() > 0)
                {
                    foreach (var it in q)
                    {
                        int amount = 0;
                        var tmp = new List<int>();
                        foreach (var im in it)
                        {
                            tmp.Add(im.Index);
                            amount += im.Amount;
                        }
                        for (int i = 0; i < tmp.Count; i++)
                        {
                            if (i == 0)
                            {
                                list.FirstOrDefault(f => f.Index == tmp[i]).Amount = amount;
                            }
                            else
                            {
                                list.Remove(list.FirstOrDefault(f => f.Index == tmp[i]));
                            }
                        }
                    }
                }
            }
            return list;
        }
        private async void SendFeedback_Click(object sender, RoutedEventArgs e)
        {
            var p = _orderPOCOs.GroupBy(g => g.OrderID).ToList();
            var q = p.Sum(s => s.First().TotalAmount);
            var co_list = new List<COList>();
            foreach (var co in p)
            {
                var CO_DETAIL_LIST = new List<CODetail>();
                foreach (var co2 in co)
                {
                    CO_DETAIL_LIST.Add(new CODetail() { ITEM_ID = co2.ProductID.ToString(), QTY = co2.Amount });
                }
                co_list.Add(new COList { PICK_CO_NUM = co.First().SortingNO.ToString(), SEQ = co.First().SortingNO, CO_NUM = co.First().CustomerCode, CO_DETAIL_LIST = CO_DETAIL_LIST });
            }
            var dataset = new DataSet
            {
                PICK_TASK_NUM = tbTaskNO.Text.Trim(),
                COM_ID = tbCoCode.Text.Trim(),
                PICK_LINE_ID = tbSortingLineCode.Text.Trim(),
                PICK_DATE = DateTime.Now.ToString("yyyyMMdd"),
                ISSUE_TIME = DateTime.Now.ToString("yyyyMMdd"),
                CUST_COUNT = _orderPOCOs.GroupBy(g => g.CustomerCode).Count(),
                QTY_SUM = _orderPOCOs.GroupBy(g => g.OrderID).ToList().Sum(s => s.First().TotalAmount), CO_LIST = co_list
            };
            string apiUrl = tbUrl.Text.Trim();
            var request = new FeedbackRequest() { DATASET = new List<DataSet>()};
            request.DATASET.Add(dataset);

            string sendData = JsonConvert.SerializeObject(request);
            tbJsonText.Text = sendData;
            try
            {
                var response = await SendDataToServer(apiUrl, sendData);
                tbResponse.Text = $"消息: {response}";
                tbResponse.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                tbResponse.Text = $"发生异常:{ex.Message}";
                tbResponse.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private async void TestConnect_Click(object sender, RoutedEventArgs e)
        {
            string apiUrl = tbUrl.Text.Trim();
            string sendData = tbJsonText.Text;
            try
            {
                var parseJson = JsonConvert.DeserializeObject(sendData);
                if (parseJson == null)
                {
                    tbResponse.Text = "数据格式不正确";
                    tbResponse.Foreground = new SolidColorBrush(Colors.Red);
                    return;
                }
                var response = await SendDataToServer(apiUrl, sendData);
                tbResponse.Text = $"消息: {response.ToString()}";
                tbResponse.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                tbResponse.Text = $"发生异常:{ex.Message}";
                tbResponse.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private async Task<string> SendDataToServer(string url, string data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        private string GetConfig(string keyName) => ConfigurationManager.AppSettings[keyName];
    }
}
