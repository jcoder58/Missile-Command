using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    public class Unit {
        public readonly Unit Value = new Unit();

        private Unit() { }
    }
}
