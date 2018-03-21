using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Xml;
using AutoMarshal.Models;
using System.Data;
using System.Windows.Forms;

namespace AutoMarshal.Controllers
{
    //Класс для взаимодействия с сервером
    public class WebApiClass
    {
        static private List<Entry> _entries = new List<Entry>();
        static private DateTime _tillDateTime;

        //асинхронный метод загрузки данных в JSON формате
        public static async Task GetVehicleListAsyncJson(DateTime dtTill, Action<List<Entry>> CallBack)
        {
            RootObject ro = null;
            int offSet = 0;
            int vCount = Properties.Settings.Default.VehiclesLimit;
            //Можно запросить сразу все за период, но в API упоминается только смещение и лимит, поэтому работаем в цикле
            while (true)
            {
                ro = await WebApiClass.GetVehiclesAsyncJson(offSet * vCount, vCount);
                if (ro != null)
                {
                    ++offSet;
                    //Если все элементы не старше установленного периода (дней)
                    if (ro.entries[ro.entries.Count - 1].timestamp >= dtTill)
                    {
                        _entries.AddRange(ro.entries);
                    }
                    else
                    {
                        //Выбираем поддиапазон элементов до граничной даты
                        int lastIndex = ro.entries.FindLastIndex(t => t.timestamp >= dtTill);
                        _entries.AddRange(ro.entries.GetRange(0, ++lastIndex));
                        break;
                    }
                }
                else
                {
                    break;
                }

            } //while
            CallBack(_entries);
        }

        //Получение части журнала. <count> записей  по смещению <offset>
        static async Task<RootObject> GetVehiclesAsyncJson(int offset, int count)
        {
            RootObject root = null;
            HttpClient client = new HttpClient();
            string url = String.Format(Properties.Settings.Default.VehiclesUriTemplate, offset, count);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(String.Format("{0}{1}", Properties.Settings.Default.BaseURI, url)),
                Method = HttpMethod.Get,
            };
            //Запрашиваем данные ф формате JSON указывая заголовок "Accept"
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                //Десериализуем строку Json-ответа в наш объект.
                root = await response.Content.ReadAsAsync<RootObject>();
            }
            //Возвращаем готовый объект со списком Entry
            return root;
        }

        //Колбэк фунция для сбора списка изображений текущей строки (XML фариант ответа)
        private static bool CollectImagesCallback(XmlNode ImageNode, XmlNamespaceManager XManager, object sb)
        {
            StringBuilder stringBuilder = (StringBuilder)sb;
            //Добавляем значение очередного элемента  VehicleImagesList/Images/a:long
            stringBuilder.AppendFormat("{0},", ImageNode.InnerText);
            return true;
        }

        //Колбэк функция для элементов <VehicleRegistrationLogEntry>. Вызывается один раз на каждую строку данных
        public static bool AddRowCallback(XmlNode EntryNode, XmlNamespaceManager XManager, object DataTable)
        {
            DataTable dt = (DataTable)DataTable;
            DataRow dr = dt.NewRow();
            bool result = true;

            foreach (AutoMarshalColumn col in AColumns.Instance.Columns)
            {
                if (col.XPath != "")
                {
                    //Читаем значение свойтства по XPath пути из описания полей таблицы данных
                    string propertyValue = EntryNode.GetXPathValue(col.XPath, XManager);
                    
                    //обработка поля TDateTime c контролем заданного диапазона дат
                    if (col.DataMember == "timestamp")
                    {
                        DateTime timestamp = Convert.ToDateTime(propertyValue);
                        dr.SetField(col.DataMember, timestamp);
                        //если начались строки страше заданной даты - прекащаем вставку строк в таблицу
                        result = _tillDateTime < timestamp;
                    }
                    else
                    {
                        dr.SetField(col.DataMember, propertyValue);
                    }
                }
            } // end foreach cycle

            //Соберем список изображений по пути : ./d:VehicleImagesList/d:Images/a:long
            StringBuilder sb = new StringBuilder();
            int imagesCount = EntryNode.EnumXMLNodes("./d:VehicleImagesList/d:Images/a:long", XManager, sb, CollectImagesCallback);
            if (imagesCount > 0)
            {
                //убираем завершающую запятую
                sb.Remove(sb.Length - 1, 1);
                dr.SetField("vehicleImages.images", sb.ToString());
            }
            dt.Rows.Add(dr);
            return result;
        }

        //асинхронный метод загрузки данных в XML формате
        public static async Task GetVehicleListAsyncXML(DateTime dtTill, Action<BindingSource> CallBack)
        {
            //Граничная дата, после которой нас не интересуют записи журнала
            _tillDateTime = dtTill;
            DataTable dt = new DataTable();
            //Создаем поля в таблице данных 
            foreach (AutoMarshalColumn col in AColumns.Instance.Columns) if (col.DataMember != "") dt.Columns.Add(new DataColumn(col.DataMember));

            BindingSource SBind = new BindingSource();
            int offSet = 0;
            int vCount = Properties.Settings.Default.VehiclesLimit;
            int addedCount = vCount;
            
            //пока не встретилась граничная дата (контролируется по количеству созданных строк в таблице данных. Если меньше чем запрошено, значит достигли конца заданного периода)
            while (addedCount == vCount)
            {
                var xdoc = await GetVehiclesAsyncXML(offSet, vCount);
                if (xdoc != null)
                {
                    // Вариант с XML обработаем руками, не используя наши модели
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
                    nsmgr.AddNamespace("d", xdoc.DocumentElement.NamespaceURI);
                    nsmgr.AddNamespace("a", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");

                    //Добавим строки в таблицу извлекая их из элементов XML документа, используя колбэк <AddRowCallback>
                    addedCount = xdoc.EnumXMLNodes("//d:Entries/d:VehicleRegistrationLogEntry", nsmgr, dt, AddRowCallback);
                    ++offSet;
                }
                else
                {
                    break;
                }
            }//while

            SBind.DataSource = dt;
            //Вызываем делегат, передавая в него биндингсорс
            CallBack(SBind);
        }

        //Получение части журнала в формате xml. <count> записей  по смещению <offset>
        static async Task<XmlDocument> GetVehiclesAsyncXML(int offset, int count)
        {
            XmlDocument xdoc = null; 
            HttpClient client = new HttpClient();
            string url = String.Format(Properties.Settings.Default.VehiclesUriTemplate, offset, count);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(String.Format("{0}{1}", Properties.Settings.Default.BaseURI, url)),
                Method = HttpMethod.Get,
            };
            //Запрашиваем данные в XML формате, указав HTTP-заголовок <Accept>
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    xdoc = new XmlDocument(new NameTable());
                    xdoc.LoadXml(await response.Content.ReadAsStringAsync());
                }
                catch {
                    xdoc = null;
                }
            }
            return xdoc;
        }
    } 
}
