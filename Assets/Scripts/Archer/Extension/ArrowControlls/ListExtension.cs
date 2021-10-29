using System.Collections.Generic;
using Archer.Controlls.ArrowControlls;

namespace Archer.Extension.ArrowControlls {
    public static class ListExtension {
        public static void UpdateAvalibility(this List<ArrowController> arrowControlList) {
            for (int i = arrowControlList.Count - 1; i > -1; i--) {
                if (arrowControlList[i] == null)
                    arrowControlList.RemoveAt(i);
            }
        }
    }
}
