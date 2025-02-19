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
        public int process { get; set; }
    }
    public class AudioSessionsRefreshArgs : IAPCArgs { }
    public class LinkSliderToProcess : APCEvent<LinkSliderArgs> { }
    public class AudioSessionsRefresh: APCEvent<AudioSessionsRefreshArgs> { }
}
