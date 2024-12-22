using System;
using System.Xml.Serialization;

namespace modoff.Schema {
	[XmlRoot(ElementName = "IRE", Namespace = "")]
	[Serializable]
	public class ModoffItemDataRelationship {
		[XmlElement(ElementName = "t")]
		public string Type;

		[XmlElement(ElementName = "id")]
		public int ItemId;

		[XmlElement(ElementName = "wt")]
		public int Weight;

		[XmlElement(ElementName = "q")]
		public int Quantity;

		[XmlElement(ElementName = "mxq")]
		public int? MaxQuantity;
	}
}
