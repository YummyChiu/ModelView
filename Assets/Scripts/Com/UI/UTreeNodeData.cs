using System.Collections.Generic;

namespace Com.UI {
    public class UTreeNodeData {

        private int id;
        private string title;
        //private bool check ;
        private bool isLeaf = true;
        private UTreeNodeData parentData;
        public List<UTreeNodeData> childDataList;
        public UTreeBaseItem connectItem;

        public UTreeNodeData(int id,string title) {
            this.id = id;
            this.title = title;
            this.Check = true;
        }

        public string Title {
            get {
                return title;
            }
        }
        public bool Check {
            get;
            set;
            //get { return check; }
            //set { }
            
        }

        public int ID {
            get {
                return id;
            }
        }

        public bool IsLeaf {
            get {
                return isLeaf;
            }
        }

        public bool IsBranch {
            get {
                return isLeaf == false;
            }
        }

        public List<UTreeNodeData> Children {
            get {
                return childDataList;
            }
        }

        public UTreeNodeData Parent {
            get {
                return parentData;
            }
        }
        public void AddChild(UTreeNodeData child) {
            child.parentData = this;
            childDataList = childDataList ?? new List<UTreeNodeData>();
            childDataList.Add(child);
            isLeaf = false;
        }
    }
}
