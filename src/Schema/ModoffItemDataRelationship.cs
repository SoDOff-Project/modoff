using System;
using System.Xml.Serialization;

namespace modoff.Schema {
	[XmlRoot(ElementName = "IRE", Namespace = "")]
	[Serializable]
	public class ModoffItemDataRelationship : ItemDataRelationship {
		[XmlElement(ElementName = "mxq")]
		public int? MaxQuantity;
	}
}
