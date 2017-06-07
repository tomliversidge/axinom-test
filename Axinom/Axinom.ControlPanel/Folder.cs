using System.Collections.Generic;

namespace Axinom.ControlPanel
{
    public class Folder
    {
        public Dictionary<string, Folder> Folders { get; set; }
        public HashSet<string> Files { get; set; }

        public Folder()
        {
            Folders = new Dictionary<string, Folder>();
            Files = new HashSet<string>();
        }

        public Folder Add(string path, bool mightBeFile = true)
        {
            int i = path.IndexOf('/');
            if (i > -1)
            {
                Folder folder = Add(path.Substring(0, i), false);
                return folder.Add(path.Substring(i + 1), true);
            }

            if (path == "") return this;

            // if the name is at the end of a path and contains a "." 
            // we assume it is a file (unless it is "." by itself)
            if (mightBeFile && path != "." && path.Contains("."))
            {
                Files.Add(path);
                return this;
            }

            Folder child;
            if (Folders.ContainsKey(path))
            {
                child = Folders[path];
            }
            else
            {
                child = new Folder();
                Folders.Add(path, child);
            }
            return child;
        }
    }
}