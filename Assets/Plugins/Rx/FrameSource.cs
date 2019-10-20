using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    public class FrameSource : Source<TimeSpan> {
        private static readonly DateTime GameStart = DateTime.Now;


        public void OnFrame() {
            this.Publish(DateTime.Now - GameStart);
        }
    }
}
