using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using AutoMarshal.Models;
using System.Reflection;
using System.Xml;

namespace AutoMarshal
{
    //Описание колонки грида и соответствующего ей поля в таблице данных 
    public class AutoMarshalColumn
    {
        //Заголовок колонки грида
        public string ColumnTitle { get; set; } 
        //Поле колонки в таблице данных
        public string DataMember { get; set; }
        //Флаг что в колонке изображения
        public bool isImage { get; set; }
        //Флаг видимости колонки в гриде
        public bool VisibleInGrid { get; set; }
        //Выражение XPath по которому лежат данные для колонки в XML
        public string XPath { get; set; }

        public AutoMarshalColumn(string Title, string DataMember, string XPathExpr, bool isImage = false, bool visibleInGrid = true)
        {
            this.ColumnTitle = Title;
            this.DataMember = DataMember;
            this.isImage = isImage;
            this.VisibleInGrid = visibleInGrid;
            this.XPath = XPathExpr;
        }
    }

    //Класс со списком колонок грда и полей таблицы данных
    public class AColumns
    {
        private static AColumns instance;

        public List<AutoMarshalColumn> columns;

        private AColumns() {
            columns = new List<AutoMarshalColumn>();
            setupColumns();
        }

        public static AColumns Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AColumns();
                }
                return instance;
            }
        }

        //Создаем список колонок/полей
        private void setupColumns()
        {
            columns.Add(new AutoMarshalColumn("Номер транспорта", "plate", "./d:Plate"));
            columns.Add(new AutoMarshalColumn("Направление", "","", true, true));
            columns.Add(new AutoMarshalColumn("Направление2", "direction", "./d:Direction", false, false));
            columns.Add(new AutoMarshalColumn("Название направления", "directionName", "./d:DirectionName"));
            columns.Add(new AutoMarshalColumn("Статус", "status", "./d:Status"));
            columns.Add(new AutoMarshalColumn("Проезд", "", "", true,  true));
            columns.Add(new AutoMarshalColumn("Проезд2", "passage", "./d:Passage", false, false));
            columns.Add(new AutoMarshalColumn("Время и дата", "timestamp", "./d:Timestamp"));
            columns.Add(new AutoMarshalColumn("Видеоканал", "videoChannel.name", "./d:VideoChannel/d:Name"));
            columns.Add(new AutoMarshalColumn("Images", "vehicleImages.images","", false, false));
            columns.Add(new AutoMarshalColumn("Списки", "links.displayName", "./d:Links/d:EntryLink/d:DisplayName"));
            columns.Add(new AutoMarshalColumn("СпискиЦвет", "links.color", "./d:Links/d:EntryLink/d:Color", false, false));
            columns.Add(new AutoMarshalColumn("Тип ТС", "vehicleType.name", "./d:VehicleType/d:Name"));
            columns.Add(new AutoMarshalColumn("record made by", "",""));
            columns.Add(new AutoMarshalColumn("срок действия", "",""));
            columns.Add(new AutoMarshalColumn("государственный регистрационный знак прицепа", "",""));
            columns.Add(new AutoMarshalColumn("телефон водителя", "",""));
            columns.Add(new AutoMarshalColumn("отчество водителя", "",""));
            columns.Add(new AutoMarshalColumn("имя водителя", "",""));
            columns.Add(new AutoMarshalColumn("фамилия водителя", "",""));
            columns.Add(new AutoMarshalColumn("тарификатор", "fields.Tarificator", "./d:Fields/d:FieldValue[d:FieldId=9]/d:Value"));
            columns.Add(new AutoMarshalColumn("валюта", "fields.Currency", "./d:Fields/d:FieldValue[d:FieldId=8]/d:Value"));
            columns.Add(new AutoMarshalColumn("дата создания записи", "",""));
            columns.Add(new AutoMarshalColumn("тип автомобиля", "",""));
            columns.Add(new AutoMarshalColumn("модель автомобиля", "",""));
            columns.Add(new AutoMarshalColumn("фио водителя", "",""));
            columns.Add(new AutoMarshalColumn("запись создана пользователем", "",""));
            columns.Add(new AutoMarshalColumn("ФИО", "fields.ExtraField_a412be5f-1ce9-4260-87f4-a1ec3f554c03", "./d:Fields/d:FieldValue[d:FieldId=2]/d:Value"));
            columns.Add(new AutoMarshalColumn("К оплате", "fields.Payment", "./d:Fields/d:FieldValue[d:FieldId=1]/d:Value"));

        }
    }

    //Вспомогательные статические методы
    public static class Utils
    {
        //Шаблон URI для получения изображения по его id
        static string imageURItemplate = Properties.Settings.Default.ImageUriTemplate;

        //Создаем колонку грида (текст)
        private static DataGridViewColumn createColumn(string columnTitle, string DataPropertyName, bool Visible)
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Name = columnTitle;
            column.HeaderText = columnTitle;
            column.DataPropertyName = DataPropertyName;
            column.Visible = Visible;
            return column;
        }

        //Создаем колонку грида (image)
        private static DataGridViewImageColumn createImageColumn(string columnTitle, string DataPropertyName)
        {
            DataGridViewImageColumn column = new DataGridViewImageColumn();
            column.Name = columnTitle;
            column.HeaderText = columnTitle;
            column.DataPropertyName = DataPropertyName;
            return column;
        }

        //Создать колонки для грида по их описанию 
        public static void SetupGridColumnsEx(DataGridView grid)
        {
            grid.Columns.Clear();
            //цикл по списку с описанием колонок грида
            foreach (AutoMarshalColumn col in AColumns.Instance.columns)
            {
                if (col.isImage)
                {
                    //нужна колонка для изображений
                    grid.Columns.Add(createImageColumn(col.ColumnTitle, col.DataMember));
                }
                else
                {
                    //иначе просто колонка для текстовых данных
                    grid.Columns.Add(createColumn(col.ColumnTitle, col.DataMember, col.VisibleInGrid));
                }
            }
        }

        //Рекурсивное извлечение значения свойства вложенных объектов
        public static string extractPropertyValue(object obj, string propertyName)
        {
            string strResult = null;
            if (propertyName.Contains("."))
            {
                string basePropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
                PropertyInfo[] arrayProperties;
                arrayProperties = obj.GetType().GetProperties();
                //ищем в массиве свойств объекта нужный нам экземпляр по имени
                foreach (PropertyInfo propInfo in arrayProperties)
                {
                    if (propInfo.Name == basePropertyName)
                    {
                        //рекурсивно читаем вложенное свойство с именем справа от точки
                        strResult = extractPropertyValue(propInfo.GetValue(obj, null), propertyName.Substring(propertyName.IndexOf(".") + 1));
                        break;
                    }
                }
            }
            else
            {
                //имя свойства не содержит точек. мы внизу иерархии
                Type propertyType;
                PropertyInfo propertyInfo;
                propertyType = obj.GetType();
                propertyInfo = propertyType.GetProperty(propertyName);
                
                //поле images обрабатываем через запись в строку списка идентификаторов
                if (propertyName == "images")
                {
                    if (propertyType != typeof(List<System.Int32>))
                    {
                        List<System.Int32> ims = propertyInfo.GetValue(obj, null) as List<System.Int32>;
                        foreach (int i in ims)
                        {
                            strResult += String.Format("{0},", i);
                        }
                        if (strResult.EndsWith(",")) strResult = strResult.Substring(0, strResult.Length - 1);
                    }
                }
                else
                {
                    //просто читаем значение свойства у объекта
                    strResult = propertyInfo.GetValue(obj, null).ToString();
                }
            }
            return strResult;
        }

        //Наполнить таблицу данных из объекта, вернуть биндингсорс
        public static BindingSource createDataTable(List<Entry> Entries, string dataTableName)
        {
            DataTable dt = new DataTable(dataTableName);
            BindingSource SBind = new BindingSource();

            //инициализация полей таблицы данных
            foreach (AutoMarshalColumn col in AColumns.Instance.columns) if (col.DataMember != "") dt.Columns.Add(new DataColumn(col.DataMember));

            //нам потребуется рефлексия для доступа к свойствам по имени
            Type propertyType;
            PropertyInfo propertyInfo;

            //цикл по строкам журнала - элементам <Entry>
            foreach (Entry en in Entries)
            {
                propertyType = en.GetType();
                DataRow dr = dt.NewRow();

                //внутри строки - цикл по полям из описания таблицы
                foreach (AutoMarshalColumn col in AColumns.Instance.columns)
                {
                    if (!col.isImage)
                    {
                        //для links: берем первый элемент списка и читаем свойство (имя свойства вложенного объекта находится после уточняющей точки)
                        if (col.DataMember.StartsWith("links."))
                        {
                            if ((en.links != null) && (en.links.Count > 0))
                            {
                                Link link = en.links.First();
                                string detailName = col.DataMember.Substring(col.DataMember.IndexOf(".") + 1);
                                propertyType = link.GetType();
                                propertyInfo = propertyType.GetProperty(detailName);
                                dr.SetField(col.DataMember, propertyInfo.GetValue(link, null));
                            }
                        }
                        //здесь структура иная. список объектов, идентифицируемых по значению атрибута. Значение лежит в атрибуте <value>
                        else if (col.DataMember.StartsWith("fields."))
                        {
                            if ((en.fields != null) && (en.fields.Count > 0))
                            {
                                string detailName = col.DataMember.Substring(col.DataMember.IndexOf(".") + 1);
                                EntryField field = en.fields.FirstOrDefault(f => f.name == detailName);
                                if (field != null) dr.SetField(col.DataMember, field.value);
                            }
                        }
                        //если свойство не links и не fields, но в имени есть уточняющая точка
                        else if (col.DataMember.Contains(".") || (col.DataMember != ""))
                        {
                            //получаем парент-объект и рекурисвно читаем его свойство правее точки
                            dr.SetField(col.DataMember, extractPropertyValue(en, col.DataMember));
                        }
                        else
                        {
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            SBind.DataSource = dt;
            return SBind;
        }

        //Скроллер изображений для текущей строки грида
        public static void SlideImage(this string images, PictureBox pb, Boolean up)
        {
            //разбиваем строку идентификаторв изображений в массив
            string[] imagesArr = images.Split(',');
            if (imagesArr.Count() > 0)
            {
                //сейчас в pictureBox такое изображение
                string CurrentURI = pb.ImageLocation;
                string id = CurrentURI.Remove(0, 1 + CurrentURI.IndexOf("="));
                //индекс текущего изображения в массиве
                int currentIndex = Array.IndexOf(imagesArr, id);
                //получаем следующий или предыдущий индекс. с учетом что текущий может быть не найден в массиве
                int newImageIndex = (currentIndex == -1) ? 0 : (up ? ++currentIndex : --currentIndex);
                //коррекция индекса по длине массива
                newImageIndex = (newImageIndex > imagesArr.Count() - 1) ? 0 : (newImageIndex < 0 ? imagesArr.Count() - 1 : newImageIndex);
                //загружаем новое изображение
                pb.LoadAsync(Properties.Settings.Default.BaseURI + String.Format(imageURItemplate, imagesArr[newImageIndex]));
            }
            else
            {
                //у текущей строки грида нет изображений
                pb.ImageLocation = null;
            }
        }

        //Установка начального изображения для строки грида
        public static void SetImage(this string images, PictureBox pb)
        {
            //разбиваем строку со списком id изображений, берем первый элемент и загружаем изображение 
            string[] imagesArr = images.Split(',');
            if (imagesArr.Count() > 0)
            {
                string url = Properties.Settings.Default.BaseURI + String.Format(imageURItemplate, imagesArr[0]);
                pb.LoadAsync(url);
            }
            else
            {
                //в этой строке данных нет изображений
                pb.ImageLocation = null;
            }
        }

        //Получить значение, лежащее внутри DOM по указанному выражению XPath
        public static string GetXPathValue(this XmlNode Contex, string XPathExpr, XmlNamespaceManager XManager)
        {
            //получаем искомый элемент по указанному пути
            var targetElement = Contex.SelectSingleNode(XPathExpr, XManager);
            //Проверка на нулл и на тип найденного элемента
            return (targetElement != null) ? (targetElement.NodeType == XmlNodeType.Attribute ? targetElement.Value : targetElement.InnerText) : null;
        }

        //Перебрать элементы спика XMLnodeList, для каждого вызвать функцию обратного вызова, передав в нее элемент и пользовательские данные. Возвращается количество элементов, для которых Callback вернул true
        public static int EnumXMLNodes(this XmlNode Context, string XPathExpr, XmlNamespaceManager XManager, object Custom, Func<XmlNode, XmlNamespaceManager, object, bool> CallBack)
        {
            int res = 0;
            foreach (XmlNode iNode in Context.SelectNodes(XPathExpr, XManager))
            {
                //для каждого элемента в списке вызываем колбэк. Если он вернул true - увеличиваем счетчик и продолжаем. Иначе - выходим.
                if (CallBack(iNode, XManager, Custom)) ++res; else break;
            }
            return res;
        }
    }
}
