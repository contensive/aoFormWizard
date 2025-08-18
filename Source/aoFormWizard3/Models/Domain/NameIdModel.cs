using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.FormWidget.Models.Domain {

    //
    public class NameIdModel {
        public string name { get; set; }
        public int id { get; set; }
        public NameIdModel(string name, int id) {
            this.name = name;
            this.id = id;
        }
        public NameIdModel() {
            // default constructor
        }
    }
}
