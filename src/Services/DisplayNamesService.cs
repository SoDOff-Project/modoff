using modoff.Util;
using System.Collections.Generic;

namespace modoff.Services;

public class DisplayNamesService {
    Dictionary<int, string> displayNames = new();

    public DisplayNamesService(ItemService itemService) {
        DisplayNameList displayNamesList = XmlUtil.DeserializeXml<DisplayNameList>(XmlUtil.ReadResourceXmlString("displaynames"));
        displayNames.Add(0, "");
        foreach (var n in displayNamesList) {
            displayNames.Add((int)n.DisplayNameID, n.Name);
        }
    }

    public string GetName(int firstNameID, int secondNameID, int thirdNameID) {
        return displayNames[firstNameID] + " " + displayNames[secondNameID] + displayNames[thirdNameID];
    }
}
