using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "UIPSR", Namespace = "")]
    [Serializable]
    public class ModoffUserItemPositionSetRequest : ModoffUserItemPosition {
        [XmlElement(ElementName = "pix")]
        public int? ParentIndex;
    }
}
