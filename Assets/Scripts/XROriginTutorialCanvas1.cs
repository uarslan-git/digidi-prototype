using UnityEngine;
using UnityEngine.UI;
   using UnityEngine;
   using UnityEngine.UI;
   using UnityEngine.XR;
   using Unity.Netcode;
   
   public class XROriginTutorialCanvas : NetworkBehaviour
   {
       public Canvas tutorialCanvas;

       void Start()
       {
           bool isQuest = Application.platform == RuntimePlatform.Android;
           if (tutorialCanvas != null)
               tutorialCanvas.enabled = isQuest;
       }

       public void HideCanvas()
       {
           if (tutorialCanvas != null)
           {
               tutorialCanvas.enabled = false;
               CanvasGroup cg = tutorialCanvas.GetComponent<CanvasGroup>();
               if (cg == null)
                   cg = tutorialCanvas.gameObject.AddComponent<CanvasGroup>();
               cg.blocksRaycasts = false;
               cg.interactable = false;
           }
       }
   
       private bool isLocalPlayer = false;
       private bool aButtonPrev = false;
       private bool canToggle = false;
   
   
   
       public override void OnNetworkSpawn()
       {
           if (!IsOwner) return;
   
           // Disable the pre-join Camera XR Origin (not this network-spawned XR Origin)
           var preJoinCamera = GameObject.Find("Camera");
           if (preJoinCamera != null && preJoinCamera != this.gameObject)
               preJoinCamera.SetActive(false);
   
           bool isQuest = Application.platform == RuntimePlatform.Android;

           if (isQuest)
           {
               isLocalPlayer = true;
               if (tutorialCanvas != null)
                   tutorialCanvas.enabled = true;
               canToggle = true;
           }
           else
           {
               isLocalPlayer = false;
               if (tutorialCanvas != null)
                   tutorialCanvas.enabled = false;
               canToggle = false;
           }
   
           // Assign the button event via Inspector
       }
   
       void Update()
       {
           if (!isLocalPlayer || tutorialCanvas == null || !canToggle) return;
   
           InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
           bool aButton = false;
           if (device.TryGetFeatureValue(CommonUsages.primaryButton, out aButton))
           {
               if (aButton && !aButtonPrev)
               {
                   tutorialCanvas.enabled = !tutorialCanvas.enabled;
               }
               aButtonPrev = aButton;
           }
       }
   
       public void OnConfirmClicked()
       {
           if (tutorialCanvas != null && canToggle)
               tutorialCanvas.enabled = !tutorialCanvas.enabled;
       }
   }
