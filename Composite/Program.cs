using System;
using System.Collections;
using System.IO;

namespace Composite
{
    public abstract class Component
    {
        public string Name;
        public string Path;
        public string ParentPath;

        public Component(string n)
        {
            Name = n;
            Path = @"c:\MyTest\";
            ParentPath = @"c:\MyTest\"; 
        }
        public abstract void Read();
        public abstract void Create();
        public abstract void Add(Component component);
        public abstract void Remove(Component component);
        public abstract Component GetChild(int index);
    }

    public class Folder : Component
    {
        private ArrayList childs = new ArrayList();
        public Folder(string n) : base(n)
        { }

        public override void Read()
        {
            Console.WriteLine("You are in "+Name+ " directory");
            foreach (Component component in childs)
            {
                component.Read();
            }
        }

        public override void Create()
        {
            Directory.CreateDirectory(this.Path);
        }

        public override void Add(Component component)
        {
            component.Path = this.Path;
            component.ParentPath = this.Path;
            component.Path += @"\" + component.Name;
            component.Create();
            childs.Add(component);
        }

        public override void Remove(Component component)
        {
            childs.Remove(component);
        }

        public override Component GetChild(int index)
        {
            return childs[index] as Component;
        }
    }
    public class FileMy : Component
    {
        public FileMy(string n) : base(n)
        { }

        public override void Read()
        {
            using (StreamReader sr = File.OpenText(this.Path + ".txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        public override void Add(Component c)
        { throw new InvalidOperationException(); }

        public override void Remove(Component c)
        { throw new InvalidOperationException(); }

        public override Component GetChild(int i)
        { throw new InvalidOperationException(); }

        public override void Create()
        {
            string name = this.Name;
            if (!File.Exists(ParentPath))
            {
                using (StreamWriter sw = File.CreateText(ParentPath+@"\"+Name+".txt"))
                {
                    sw.WriteLine("You are read " + name + ".txt");
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Component root = new Folder("Root Folder");
            Component folder1 = new Folder("Folder1");
            Component folder2 = new Folder("Folder2");
            Component file1 = new FileMy("File1");
            Component file2 = new FileMy("File2");
            Component file3 = new FileMy("File3");
            root.Add(file1);
            root.Add(folder1);
            folder1.Add(folder2);
            folder1.Add(file2);
            folder2.Add(file3);
            root.Read();
        }
    }
}
