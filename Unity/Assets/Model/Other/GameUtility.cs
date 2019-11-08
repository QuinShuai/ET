using System.Collections.Generic;
using UnityEngine;

namespace ETModel {
    public class GameUtility {

        /// <summary>
        /// 查找子节点
        /// </summary>
        public static Transform FindDeepChild(string name, string childName) {
            return FindDeepChild(GameObject.Find(name), childName);
        }

        /// <summary>
        /// 查找子节点
        /// </summary>
        public static Transform FindDeepChild(GameObject target, string childName) {
            return FindDeepChild(target.transform, childName);
        }

        /// <summary>
        /// 查找子节点
        /// </summary>
        public static Transform FindDeepChild(Transform target, string childName) {
            Transform resultTrs = null;
            resultTrs = target.Find(childName);
            if (resultTrs == null) {
                foreach (Transform trs in target) {
                    resultTrs = GameUtility.FindDeepChild(trs, childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }

            return resultTrs;
        }

        /// <summary>
        /// 查找子节点脚本
        /// </summary>
        public static T FindDeepChild<T>(GameObject target, string childName) where T : UnityEngine.Component {
            Transform resultTrs = GameUtility.FindDeepChild(target, childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }

        /// <summary>
        /// 查找子节点脚本
        /// </summary>
        public static List<T> FindDeepChildsByTag<T>(Transform target, string tag) where T : UnityEngine.Component {
            var list = new List<T>();
            foreach (Transform child in target) {
                if (child.CompareTag(tag)) {
                    list.Add(child.GetComponent<T>());
                }
            }

            return list;
        }

        /// <summary>
        /// 查找子节点脚本
        /// </summary>
        public static T FindDeepChild<T>(Transform target, string childName) where T : UnityEngine.Component {
            Transform resultTrs = GameUtility.FindDeepChild(target, childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        public static void AddChildToTarget(Transform target, Transform child) {
            child.SetParent(target);
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;

            ChangeChildLayer(child, target.gameObject.layer);
        }

        /// <summary>
        /// 修改子节点Layer  NGUITools.SetLayer();
        /// </summary>
        public static void ChangeChildLayer(Transform t, int layer) {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i) {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChildLayer(child, layer);
            }
        }

        /// <summary>
        /// 返回最大或者最小Depth界面
        /// Get the max or min depth UIPanel
        /// </summary>
        public static GameObject GetPanelDepthMaxMin(GameObject target, bool maxDepth, bool includeInactive) {
            var lsPanels = GetPanelSorted(target, includeInactive);
            if (lsPanels != null) {
                if (maxDepth)
                    return lsPanels[lsPanels.Count - 1].gameObject;
                else
                    return lsPanels[0].gameObject;
            }

            return null;
        }

        private class CompareSubPanels : IComparer<Canvas> {
            public int Compare(Canvas left, Canvas right) {
                return left.sortingOrder - right.sortingOrder;
            }
        }

        private static List<Canvas> GetPanelSorted(GameObject target, bool includeInactive = false) {
            var panels = target.transform.GetComponentsInChildren<Canvas>(includeInactive);
            if (panels.Length > 0) {
                var lsPanels = new List<Canvas>(panels);
                lsPanels.Sort(new CompareSubPanels());
                return lsPanels;
            }

            return null;
        }

        /// <summary>
        /// Set the mini depth to target with given Sorted list
        /// </summary>
        public static void SetTargetMinPanelDepth(GameObject obj, int depth) {
            var lsPanels = GetPanelSorted(obj, true);
            if (lsPanels != null) {
                int i = 0;
                while (i < lsPanels.Count) {
                    lsPanels[i].sortingOrder = depth + i;
                    i++;
                }
            }
        }

        /// <summary>
        /// 获得指定目标最大depth值
        /// Get the target Max depth
        /// </summary>
        public static int GetMaxTargetDepth(GameObject obj, bool includeInactive = false) {
            int minDepth = -1;
            var lsPanels = GetPanelSorted(obj, includeInactive);
            if (lsPanels != null)
                return lsPanels[lsPanels.Count - 1].sortingOrder;
            return minDepth;
        }

        /// <summary>
        /// 给目标添加Collider背景
        /// Add Collider Background for target
        /// </summary>
        public static GameObject AddColliderBgToTarget(GameObject target, string maskNames, bool isTransparent) {
            // 添加UIPaneldepth最小上面
            // 保证添加的Collider放置在屏幕中间
            //Transform windowBg = GameUtility.FindDeepChild(target, "WindowBg");
            //if (windowBg == null) {
            //    GameObject targetParent = GameUtility.GetPanelDepthMaxMin(target, false, true);
            //    if (targetParent == null)
            //        targetParent = target;

            //    windowBg = (new GameObject("WindowBg")).transform;
            //    GameUtility.AddChildToTarget(targetParent.transform, windowBg);
            //}

            //Transform bg = GameUtility.FindDeepChild(target, "WindowColliderBg(Cool)");
            //if (bg == null) {
            //    // add sprite or widget to ColliderBg
            //    UIWidget widget = null;
            //    if (!isTransparent)
            //        widget = NGUITools.AddSprite(windowBg.gameObject, altas, maskName);
            //    else
            //        widget = NGUITools.AddWidget<UIWidget>(windowBg.gameObject);

            //    widget.name = "WindowColliderBg(Cool)";
            //    bg = widget.transform;

            //    // fill the screen
            //    // You can use the new Anchor system
            //    UIStretch stretch = bg.gameObject.AddComponent<UIStretch>();
            //    stretch.style = UIStretch.Style.Both;
            //    // set relative size bigger
            //    stretch.relativeSize = new Vector2(1.5f, 1.5f);

            //    // set a lower depth
            //    widget.depth = -5;

            //    // set alpha
            //    widget.alpha = 0.6f;

            //    // add collider
            //    NGUITools.AddWidgetCollider(bg.gameObject);
            //}
            //return bg.gameObject;
            return null;
        }
    }
}