using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Configuration;
using AutoMarshal.Controllers;
using AutoMarshal.Models;
using AutoMarshal.Properties;
using System.Xml;

namespace AutoMarshal
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = "АвтоМаршал - журнал";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitGridState();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //загружаем данные в формате Json
            DateTime today = DateTime.Now;
            DateTime till = today.AddDays(-(Properties.Settings.Default.DaysBefore));
            Task t = WebApiClass.GetVehicleListAsyncJson(till, VehiclesReadyJSON);
        }

        //Очистка грида в начальное состояние
        private void InitGridState()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowEnter -= new DataGridViewCellEventHandler(dataGridView1_RowEnter);
            //Очищаем изображение в pictureBox
            pictureBox1.ImageLocation = null;
            dataGridView1.Cursor = Cursors.WaitCursor;
        }

        //метод передается в асинхронную задачу загрузки данных в формате JSON. Вызывается в момент готовности данных
        public void VehiclesReadyJSON(List<Entry> entries)
        {
            //Настраиваем грид на новый источник данных, инициализируем изображение первой строки, подписываемся на события
            Utils.SetupGridColumnsEx(dataGridView1);
            dataGridView1.DataSource = Utils.CreateDataTable(entries, "journal");
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.RowEnter += dataGridView1_RowEnter;
            dataGridView1.Cursor = Cursors.Default;
            if (dataGridView1.Rows.Count > 0)
            {
                string images = dataGridView1.SelectedRows[0].Cells["Images"].Value.ToString();
                images.SetImage(pictureBox1);
            }
            pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
        }

        //метод передается в асинхронную задачу загрузки данных в формате XML. Вызывается в момент готовности данных
        public void VehiclesReadyXML(BindingSource bs)
        {
            //Настраиваем грид на новый источник данных, инициализируем изображение первой строки, подписываемся на события
            Utils.SetupGridColumnsEx(dataGridView1);
            dataGridView1.Cursor = Cursors.Default;
            dataGridView1.DataSource = bs;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.RowEnter += dataGridView1_RowEnter;
            if (dataGridView1.Rows.Count > 0)
            {
                string images = dataGridView1.SelectedRows[0].Cells["Images"].Value.ToString();
                images.SetImage(pictureBox1);
            }
            pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
        }

        #region Обработчики событий
        //Условное форматирование ячеек грида
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                try
                {
                    //устанавливаем графический значок для колонки "Направление" на основе значения невидимого поля "Направление2"
                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Направление")
                    {
                        string directionValue = this.dataGridView1["Направление2", e.RowIndex].Value.ToString();
                        e.Value = (directionValue.StartsWith("down")) ? Resources.down_16 : ((directionValue.StartsWith("up") ? Resources.up_16 : Resources.question_24));

                    }
                    //устанавливаем графический значок для колонки "Проезд" на основе значения невидимого поля "Проезд2"
                    else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Проезд")
                    {
                        string passageValue = this.dataGridView1["Проезд2", e.RowIndex].Value.ToString();
                        e.Value = (passageValue.StartsWith("entry")) ? Resources.door_enter : ((passageValue.StartsWith("exit") ? Resources.door_exit : Resources.question_24));
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    //Цвет фона колонки "Списки" читаем из невидимого поля "СпискиЦвет" 
                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Списки")
                    {
                        string colorValue = this.dataGridView1["СпискиЦвет", e.RowIndex].Value.ToString();
                        if (colorValue != "")
                        {
                            e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(colorValue); ;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Произошла смена текущей строки грида. необходимо загрузить pictureBox новыми данными
            string images = dataGridView1.SelectedRows[0].Cells["Images"].Value.ToString();
            images.SetImage(pictureBox1);
        }

        //Клик на изображении. Смена картинки на следующую или предыдущую
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            //список идентификаторов изображений текущей строки грида
            string images = dataGridView1.SelectedRows[0].Cells["Images"].Value.ToString();
            //вызываем смену изображения. Клик по левой половине pictureBox'а - предыдущее изображение, по правой - следующее изображение
            images.SlideImage(pictureBox1, e.Location.X > pb.Width / 2);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //Изменяем вид курсора над изображением (клик приводит к листанию изображений)
            pictureBox1.Cursor = (e.Location.X > pictureBox1.Width / 2) ? Cursors.PanEast : Cursors.PanWest;
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void показыватьИзображенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            menuItem.Checked = !(menuItem.Checked);
            //показываем или скрываем pictureBox
            tableLayoutPanel1.ColumnStyles[1].Width = menuItem.Checked ? 360 : 0;
        }

        //Просмотр-Редактирование настроек приложения
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = SettingsForm.ShowSettingsForm();
        }

        //Получение данных в формате Json
        private void обновитьДанныеJSONToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //Очистка грида
            InitGridState();
            //Вычисляем граничную дату периода
            DateTime today = DateTime.Now;
            DateTime tillDate = today.AddDays(-(Properties.Settings.Default.DaysBefore));
            //Грузим асинхронно данные (async/await)
            Task tJson = WebApiClass.GetVehicleListAsyncJson(tillDate, VehiclesReadyJSON);
        }

        //Получение данных в формате XML
        private void обновитьДанныеXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Очистка грида
            InitGridState();
            //Вычисляем граничную дату периода
            DateTime today = DateTime.Now;
            DateTime tillDate = today.AddDays(-(Properties.Settings.Default.DaysBefore));
            //Грузим асинхронно данные (async/await)
            Task tXML = WebApiClass.GetVehicleListAsyncXML(tillDate, VehiclesReadyXML);
        }
        #endregion
    }

}


