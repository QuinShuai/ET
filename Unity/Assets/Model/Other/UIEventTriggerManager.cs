using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel {
    public class UIEventTriggerManager : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler,
            IPointerExitHandler {
        
        public event Action<GameObject> OnClick;
        public event Action<GameObject> OnDown;
        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;
        public event Action<GameObject> OnUp;

        public static UIEventTriggerManager Get(GameObject go) {
            UIEventTriggerManager manager = go.GetComponent<UIEventTriggerManager>();
            if (manager == null)
                manager = go.AddComponent<UIEventTriggerManager>();

            return manager;
        }

        public static UIEventTriggerManager Get(GameObject go, string name) {
            return Get(GameUtility.FindDeepChild(go, name).gameObject);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (OnClick != null) {
                if (GetComponent<Button>() != null && !GetComponent<Button>().interactable) {
                    return;
                }

                OnClick(gameObject);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (OnDown != null) {
                if (GetComponent<Button>() != null && !GetComponent<Button>().interactable) {
                    return;
                }

                OnDown(gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (OnEnter != null) {
                OnEnter(gameObject);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (OnExit != null) {
                OnExit(gameObject);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (OnUp != null) {
                if (GetComponent<Button>() != null && !GetComponent<Button>().interactable) {
                    return;
                }

                OnUp(gameObject);
            }
        }
    }
}
