using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal partial class Day7 : IDay
    {
        public int Day => 7;

        public string TestInput => """
            $ cd /
            $ ls
            dir a
            14848514 b.txt
            8504156 c.dat
            dir d
            $ cd a
            $ ls
            dir e
            29116 f
            2557 g
            62596 h.lst
            $ cd e
            $ ls
            584 i
            $ cd ..
            $ cd ..
            $ cd d
            $ ls
            4060174 j
            8033020 d.log
            5626152 d.ext
            7214296 k
            """;

        public object Ex1TestResult => 95437;

        public object Ex2TestResult => 24933642;

        public object Exercise1(StreamReader input, bool isTest)
        {
            Directiory root = Parse(input);

            List<int> directorySizes = new();

            CalcDirSize(root, directorySizes);

            return directorySizes.Where(x => x <= 100000).Sum();

        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            Directiory root = Parse(input);
            List<int> directorySizes = new();
            int rootSize = CalcDirSize(root, directorySizes);
            int freeSpace = 70000000 - rootSize;
            return directorySizes.Where(x => x >= 30000000 - freeSpace).Min();
        }

        private int CalcDirSize(Directiory directiory, List<int> directorySizes)
        {
            int dirSize = directiory.Children.Sum(x => x is File file ? file.Size : CalcDirSize((Directiory)x, directorySizes));
            directorySizes.Add(dirSize);
            return dirSize;
        }

        private Directiory Parse(StreamReader input)
        {
            Directiory root = new("/", null);
            Directiory currentDirectory = root;
            while (!input.EndOfStream)
            {
                string line = input.ReadLine();

                if (line[0] != '$')
                    throw new InvalidOperationException("expecting command");

                Command cmd = Command.Parse(line);
                if (cmd.Operation == Operation.cd)
                {
                    currentDirectory = cmd.Parameter switch
                    {
                        ".." => currentDirectory.Parent,
                        "/" => root,
                        _ => (currentDirectory.Children?.Where(x => x is Directiory)
                                                        .SingleOrDefault(x => x.Name == cmd.Parameter) as Directiory)
                                                        ?? throw new InvalidOperationException($"subdirectory {cmd.Parameter} not found"),
                    };
                }
                else
                {
                    currentDirectory.Children ??= Ls(input, currentDirectory).ToList();
                }
            }
            return root;
        }

        private IEnumerable<FileSystemItem> Ls(StreamReader input, Directiory currentDirectory)
        {
            while (input.Peek() != '$' && input.Peek() != -1)
            {
                yield return FileSystemItem.Parse(input.ReadLine(), currentDirectory);
            }
        }

        private abstract partial class FileSystemItem
        {
            protected FileSystemItem(string name, Directiory parent)
            {
                Name = name;
                Parent = parent;
            }

            public string Name { get; set; }

            public Directiory Parent { get; set; }

            public static FileSystemItem Parse(string line, Directiory currentDirectory)
            {
                if (line[..3] == "dir")
                {
                    return new Directiory(line[4..], currentDirectory);
                }
                else
                {
                    Regex fileRegex = new(@"(\d+) (\w+)");
                    Match result = fileRegex.Match(line);
                    return new File(result.Groups[2].Value, int.Parse(result.Groups[1].Value), currentDirectory);
                }
            }
        }

        private class File : FileSystemItem
        {
            public int Size { get; set; }
            public File(string name, int size, Directiory parent) : base(name, parent)
            {
                Size = size;
            }
        }

        private class Directiory : FileSystemItem
        {
            public Directiory(string name, Directiory parent) : base(name, parent) { }

            public List<FileSystemItem> Children { get; set; }
        }

        private record Command(Operation Operation, string Parameter)
        {
            public static Command Parse(string line)
            {
                string parameter = string.Empty;
                Operation op = Enum.Parse<Operation>(line[2..4]);
                if (op == Operation.cd)
                    parameter = line[5..];

                return new Command(op, parameter);
            }
        }

        public enum Operation
        {
            cd,
            ls
        }
    }
}
