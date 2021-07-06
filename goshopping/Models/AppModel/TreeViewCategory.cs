using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class Node
{
    public Node() {
        nodes = new List<Node>();
    }
    public Node(string Text)
    {
        text = Text;
        nodes = new List<Node>();
    }
    public Node(string Text, string Href)
    {
        text = Text;
        href = Href;
        nodes = new List<Node>();
    }
    public Node(string Text, string Href, List<Node> Node)
    {
        text = Text;
        href = Href;
        nodes = Node;
        nodes = new List<Node>();
    }
    public string text { get; set; }
    public string icon { get; set; }
    public string selectedIcon { get; set; }
    public string href { get; set; }
    public bool selectable { get; set; }
    public string color { get; set; }
    public string backColor { get; set; }
    public string[] tags { get; set; }
    public List<Node> nodes { get; set; }
}

//節點屬性介紹
//下面的參數可用於樹節點的屬性定義，如節點的文本、顏色和標簽等。
//參數名稱 參數類型    參數描述
//text                      String(必選項) 列表樹節點上的文本，通常是節點右邊的小圖標。
//icon                     String(可選項) 列表樹節點上的圖標，通常是節點左邊的圖標。
//selectedIcon      String(可選項) 當某個節點被選擇后顯示的圖標，通常是節點左邊的圖標。
//href                      String(可選項) 結合全局enableLinks選項為列表樹節點指定URL。
//selectable           Boolean.Default: true	指定列表樹的節點是否可選擇。設置為false將使節點展開，並且不能被選擇。
//state                     Object(可選項) 一個節點的初始狀態。
//state.checked	  Boolean，默認值false 指示一個節點是否處於checked狀態，用一個checkbox圖標表示。
//state.disabled     Boolean，默認值false 指示一個節點是否處於disabled狀態。（不是selectable，expandable或checkable）
//state.expanded  Boolean，默認值false 指示一個節點是否處於展開狀態。
//state.selected     Boolean，默認值false 指示一個節點是否可以被選擇。
//color String.        Optional 節點的前景色，覆蓋全局的前景色選項。
//backColor            String. Optional 節點的背景色，覆蓋全局的背景色選項。
//tags                       Array of Strings. Optional 通過結合全局showTags選項來在列表樹節點的右邊添加額外的信息。