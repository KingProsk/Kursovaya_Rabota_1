using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya_Proskyrnich
{   
    public partial class Form1 : Form
    {
        public Graphics g;                
        public Deque deque;
        public Font font;         
        public Form1()
        {
            InitializeComponent();            
            ClientSize = new Size(813, 494); 
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "СКЛАД";
            textBox1.MaxLength = 4;
            deque = new Deque();
            g = this.CreateGraphics();                     
            font = new Font("Times New Roman", 14); // шрифт для надписей на форме
            Bitmap BM = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics h = Graphics.FromImage(BM);
            h.DrawString("Введите ID Груза: ", font, SystemBrushes.WindowText, new PointF(10,0));
            pictureBox1.Image = BM;
            Bitmap BM2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics h2 = Graphics.FromImage(BM2);
            h2.DrawString("Терминал №2", font, SystemBrushes.WindowText, new PointF(10, 0));
            pictureBox2.Image = BM2;
            Bitmap BM3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            Graphics h3 = Graphics.FromImage(BM3);
            h3.DrawString("Терминал №1", font, SystemBrushes.WindowText, new PointF(120, 0));
            pictureBox3.Image = BM3;
            
        }       
        private void MyDraw(Deque d) // функция для отрисовки содержимого Deque
        {
            font = new Font("Times New Roman", 20); // шрифт для отрисовки ID грузов
            g.Clear(Color.White);
            string str;
            SizeF MessSize;
            PointF p = new PointF((ClientSize.Width) / 2, (ClientSize.Height) / 2); // координаты середины формы
            g.DrawLine(new Pen(Color.Red, 3), p.X, p.Y + 50, p.X, p.Y - 10);
            for (int i = d.index - 1; i >= 0; i--) // цикл отрисовки для левой стороны Дека
            {
                str = d.elements[i].ToString();
                MessSize = g.MeasureString(str, font);
                p.X = p.X - MessSize.Width - 10;
                g.DrawString(str, font, SystemBrushes.WindowText, p);
            }
            p = new PointF((ClientSize.Width) / 2, (ClientSize.Height) / 2);
            for (int i = d.index + 1; i < d.Size(); i++) // цикл отрисовки для правой стороны дека
            {
                str = d.elements[i].ToString();
                MessSize = g.MeasureString(str, font);
                if (i == d.index + 1)
                {
                    p.X = p.X + 10;
                }
                else
                {
                    p.X = p.X + MessSize.Width + 10;
                }
                g.DrawString(str, font, SystemBrushes.WindowText, p);
            }
        }
        private void Term_1_Click(object sender, EventArgs e) // при нажатии на кнопку, добавляет ID, с текстового поля, в начало Дека 
        {
            if (textBox1.Text.Length < 4) // ограничение на ввод символов
            {
                MessageBox.Show("Номер груза должен состоять из 4 - х символов!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }          
            else
            {
                deque.Push_Front(textBox1.Text); // применение метода "вставить в начало"
                MyDraw(deque); // отрисовка Дека на форме                
                textBox1.Clear(); // очистка текст бокса
            }
        }
        private void Term_2_Click(object sender, EventArgs e) // при нажатии на кнопку, добавляет ID, с текстового поля, в конец Дека
        {            
            if (textBox1.Text.Length < 4)
            {
                MessageBox.Show("Номер груза должен состоять из 4 - х символов!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }                    
            else
            {
                deque.Push_Back(textBox1.Text); // применение метода "вставить в конец"
                MyDraw(deque);
                textBox1.Clear();
            }
        }
        private void Otgruzka_Click(object sender, EventArgs e) // при нажатии на кнопку, удаляет, введённый в текстовое поле, ID груза, помещает все ID, находящиеся перед ним, в Стек, и возвращает после удаления этого ID
        {
            if (textBox1.Text.Length < 4)
            {
                MessageBox.Show("Номер груза должен состоять из 4 - х символов!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                deque.Remove_Element(textBox1.Text); // применение метода "удалить ID"
                MyDraw(deque);
                textBox1.Clear();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) // запрет на ввод пробеля в текст бокс
        {         
            if(e.KeyChar == 32)
            {
                e.Handled = true;
            }
        }
    }
    public class Deque // создание класса Дек
    {
        public ArrayList elements;  // динамический массив 
        public string key; 
        public int index;
        public Stack<string> memory; // Стек
        public Deque() // конструктор класса
        {
            elements = new ArrayList();
            key = "*"; // элемент, разделяющий левую и правую часть Дека
            elements.Add(key);
            index = 0;           
        }
        public void Remove_Element(string element) // метод удаления элемента 
        {                       
            memory = new Stack<string>();
            if(elements.Contains(element) == true)
            {                
                if (elements.IndexOf(element) < index)
                {
                    while (elements[0].ToString() != element) // для левой части Дека 
                    {
                        memory.Push(elements[0].ToString()); // добавление элементов, "мешающих" изъятию заданного, в Стек
                        this.Pop_Front();// удаление элемента из начала дека
                    }
                    this.Pop_Front(); // удаление заданного элемента из начала дека
                    while (memory.Count > 0)
                    {
                        this.Push_Front(memory.Pop()); // возвращение "мешающих" изъятию элементов, обратно в начало Дека
                    }
                }
                else
                {
                    while (elements[elements.Count - 1].ToString() != element) // для правой части Дека
                    {
                        memory.Push(elements[elements.Count - 1].ToString());
                        this.Pop_Back(); // удаление элемента из конца Дека
                    }
                    this.Pop_Back(); // удаление заданного элемента из конца дека
                    while (memory.Count > 0)
                    {
                        this.Push_Back(memory.Pop()); // возвращение "мешающих" изъятию элементов, обратно в конец Дека
                    }
                }
            }
            else
            {
                MessageBox.Show("Груз отсутствует на складе!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Push_Back(string element) // метод добавления элемента в конец Дека
        {
            if (elements.Contains(element) == false && element != "*") 
            {
                elements.Add(element);
            }
            else
            {
                MessageBox.Show("Груз с таким номером уже находится на складе!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string Pop_Back() // метод удаления элемента с конеца Дека
        {
            string result = (string)elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            return result;
        }
        public void Push_Front(string element) // метод добавления элемента в начало Дека
        {            
            if (elements.Contains(element) == false)
            {
                elements.Insert(0, element);
                index++;
            }
            else
            {
                MessageBox.Show("Груз с таким номером уже находится на складе!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string Pop_Front() // метод удаления елемента с начала Дека
        {
            string result = (string)elements[0];
            elements.RemoveAt(0);
            index--;
            return result;
        }
        public int Size() // метод, возвращающий размер Дека
        {
            return elements.Count;
        }     
    }
}
