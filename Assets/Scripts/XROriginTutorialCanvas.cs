using UnityEngine;
using UnityEngine.UI;
   using UnityEngine;
   using UnityEngine.UI;
   using UnityEngine.XR;
   using Unity.Netcode;
   
   public class XROriginTutorialCanvas : NetworkBehaviour
   {
       public Canvas tutorialCanvas;
       public Button confirmButton;
   
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
           if (isQuest || gameObject.name.Contains("XR Origin (XR Rig)"))
           {
               isLocalPlayer = true;
               if (tutorialCanvas != null)
                   tutorialCanvas.enabled = true;
               canToggle = false; // Only allow toggling after Confirm
           }
           else
           {
               if (tutorialCanvas != null)
                   tutorialCanvas.enabled = false;
           }
   
           if (confirmButton != null)
               confirmButton.onClick.AddListener(OnConfirmClicked);
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
   
       void OnConfirmClicked()
       {
           if (tutorialCanvas != null)
               tutorialCanvas.enabled = false;
           canToggle = true;
       }
   }
