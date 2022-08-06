using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace Dubli
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public static string putkfile(int nomzna) // Считывание пути к файлу 
        {
            string secondLine = File.ReadLines(@"profit2.txt").Skip(nomzna).First();
            nomzna++;
            return secondLine;
        }


        private string ComputeMD5Checksum(int zn) // контрольная сумма
        {
            string path = putkfile(zn);
            using (FileStream fs = System.IO.File.OpenRead(path))  // контрольная сумма файла путь 
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }

        public void put(string Put, TextWriter text)  // Функция для поиска файлов
        {
            int gg = 0;
            try
            { 
                DirectoryInfo file_cat = new DirectoryInfo(Put);
                FileInfo[] files = file_cat.GetFiles();
                foreach (FileInfo f in files)
                {
                    string ss = ComputeMD5Checksum(gg);
                    gg++;
                    text.WriteLine(f.Name + " " + f.Length + " байт | " + ss);
                }

                foreach (DirectoryInfo d in file_cat.GetDirectories())
                {
                    put(Put + d.Name + @"\", text);
                }
                
            }
            catch (Exception e)
            {

            }
            
        }
        public void put_dir(string Put2, TextWriter text2)  // Функция для записи пути файла
        {
            try
            {
                DirectoryInfo file_cat = new DirectoryInfo(Put2);
                FileInfo[] files = file_cat.GetFiles();
                foreach (FileInfo f in files)
                {
                    text2.WriteLine(f.FullName);
                }

                foreach (DirectoryInfo d in file_cat.GetDirectories())
                {
                    put_dir(Put2 + d.Name + @"\", text2);
                }
            }
            catch (Exception e)
            {

            }
        }
        public void dublicat() // поиск и вывод дублей на экран
        {
            List<string> lines = File.ReadAllLines(@"profit.txt").ToList();
            List<string> dubl = new List<string>();
            for (int i = 0; i < lines.Count; ++i)
            {
                int count = lines.Count(str => str == lines[i]);
                if (count > 1)
                {
                    listBox1.Items.Add
                     (string.Format(
                     "Строка '{0}' повторяется {1} раз(-а).",
                     lines[i], count)
                     );
                    lines.Remove(lines[i]);
                }
            }
        }
        public void dudu()
        {
            List<string> list = File.ReadAllLines(@"profit2.txt").ToList();                          // это список list с путями к файлам
            List<string> temp = new List<string>();                                                    // тут создается массив temp
            for (int i = 0; i < list.Count; i++)                                                      // -это цикл который берет поочереди пути к файлам из списка list 
            {
                string size = Convert.ToString(new FileInfo(list[i]).Length);               // -тут определяется размер файла из списка list          
                if (temp.Contains(size))                                                    //-тут условие если в списке temp есть совпадение размера
                {
                    System.IO.File.Delete(list[i]);                                         //-то файл удаляем
                }
                else
                {
                    temp.Add(size);                                                         // -иначе записываем размер в temp
                }
            }
            MessageBox.Show("Файлы удалены");
        }
        private void textBox1_Click(object sender, EventArgs e) // очищение строки ввода
        {
            textBox1.Clear();
        }
        private void button3_Click(object sender, EventArgs e) // закрытие формы
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e) // поиск дублей
        {

            string road = textBox1.Text; // путь к папке 
            using (TextWriter text2 = new StreamWriter(@"profit2.txt"))
            {
                put_dir(road + @"\", text2); // запуск поиска пути файла
            }
            textBox1.Clear();
            MessageBox.Show("Идет поиск дубликатов");
            using (TextWriter text = new StreamWriter(@"profit.txt"))
            {
                put(road + @"\", text); // запуск поиска файлов 
            }
            dublicat(); // вывод дублей
        }
        private void button2_Click(object sender, EventArgs e) // удаление дубликатов
        {
            dudu();
            listBox1.Items.Clear();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Приложение <<Dubl>> предназначено для поиска и удаления файлов дубликатов. Для начала необходимо ввести путь к необходимой папке в формате название жесткого диска:\ название папки  Пример:  E:\Test");
        }
    }
}
