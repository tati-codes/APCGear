using APC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APCGear.Audio
{
    public class LinkSliderArgs : IAPCArgs
    {
        public int slider { get; set; }
        public Process process { get; set; } 
    }
    public class Process : IAPCArgs
    {
        public static Process nullProcess { get => new Process() { id = -2, name = "nullProcess" }; }
        public int id { get; set; }
        public string name {get;set; }
        public bool Equals(Process p)
        {
            if (p is null)
            {
                return false;
            }
            
            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }

            return (id == p.id) && (name == p.name);
        }

        public static bool operator ==(Process p1 , Process p2) { return p1.Equals(p2); }
        public static bool operator !=(Process p1, Process p2) { return !p1.Equals(p2); }
        public Godot.Collections.Dictionary<string, int> export()
        {
            return new Godot.Collections.Dictionary<string, int> { [name] = id };
        }

        public static Process import(Godot.Collections.Dictionary<string, int> p)
        {
            if (p.Count != 1)
            {
                throw new Exception("not the right import");
            }
            else
            {
                foreach (KeyValuePair<string, int> kvp in p)
                {
                    return new Process() { id = kvp.Value, name = kvp.Key };
                }
            }
            throw new Exception("this should never even trigger in the first place. i guess p didn't have a count property");
        }
    }
    public class AudioSessionsRefreshArgs : IAPCArgs { }
    public class LinkSliderToProcess : APCEvent<LinkSliderArgs> { }
    public class AudioSessionsRefresh: APCEvent<AudioSessionsRefreshArgs> { }
    public class ButtonMutesProcess : APCEvent<Process> { }
}
