using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к файлу:");
        string filePath = Console.ReadLine();

        TextEditor textEditor = new TextEditor(filePath);
        textEditor.LoadFile();

        while (true)
        {
            Console.WriteLine("Текстовый редактор (нажмите Esc для выхода, F1 для сохранения):");
            Console.WriteLine(textEditor.GetText());

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }
            else if (keyInfo.Key == ConsoleKey.F1)
            {
                textEditor.SaveFile();
                Console.WriteLine("Файл сохранен.");
            }
            else
            {
                break;
            }
        }
    }
}

class TextEditor
{
    private string filePath;
    private string[] contents;

    public TextEditor(string filePath)
    {
        this.filePath = filePath;
    }

    public void LoadFile()
    {
        string extension = Path.GetExtension(filePath);

        if (extension == ".txt")
        {
            contents = File.ReadAllLines(filePath);
        }
        else if (extension == ".json")
        {
            string json = File.ReadAllText(filePath);
            contents = JsonConvert.DeserializeObject<string[]>(json);
        }
        else if (extension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            using (StreamReader reader = new StreamReader(filePath))
            {
                contents = (string[])serializer.Deserialize(reader);
            }
        }
        else
        {
            throw new NotSupportedException("Неподдерживаемый формат файла.");
        }
    }

    public string GetText()
    {
        return string.Join(Environment.NewLine, contents);
    }

    public void SaveFile()
    {
        string extension = Path.GetExtension(filePath);

        if (extension == ".txt")
        {
            File.WriteAllLines(filePath, contents);
        }
        else if (extension == ".json")
        {
            string json = JsonConvert.SerializeObject(contents);
            File.WriteAllText(filePath, json);
        }
        else if (extension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, contents);
            }
        }
        else
        {
            throw new NotSupportedException("Формат файла не поддерживается.");
        }
    }
}