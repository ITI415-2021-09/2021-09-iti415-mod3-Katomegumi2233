using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyRopeBridge // Use own namespace to avoid conflicts with other assets.
{
    public class plankton : MonoBehaviour
    {
        [Header("Plank Settings")]
        [Tooltip("Best results are up to 50 planks.  If you remove the slider and exceed this, you may see unpredictable results.")]
        [Range(10, 50)] public int totalPlanks = 50;            // Stores total number of planks, as specified in the inspector.

        [Tooltip("Best results are up to a 1.1 gap.  If you remove the slider and exceed this, you may see unpredictable results.")]
        [Range(1.0f, 1.1f)] public float _plankGap = 1.0f;      // Distance between planks.

        [Tooltip("Best results are up to a plank width of 10.  If you remove the slider and exceed this, you may see unpredictable results. Stylized bridges will default to 4.")]
        [Range(2.0f, 10.0f)] public float plankWidth = 4.0f;    // Define width (Z) of plank.

        [Space(10)]

        [Header("Bridge Settings")]
        [Tooltip("Do you want to use primitive prefabs, or have a more stylized look?")]
        public bool stylizedLook = false;                       // Create bridge using stylized art prefabs.       

        [Tooltip("Do you want a straight wooden bridge, or a rope bridge that hangs down and reacts to physics?")]
        public bool rigidPlatform = false;                      // Create level bridge with no hinge joints.

        [SerializeField] private bool includeHandRails = true;  // Add hand rail ropes.

        [SerializeField] private bool includeEndPosts = true;   // Add end posts.

        [Space(20)]

        [SerializeField] private bool autoRenameBridge = true;  // Rename bridge object automatically, to ensure when you drag object to create prefab, different bridge types are easily identifiable.

        private GameObject _planky;                             // Stores instantiated plank.
        private Vector3 _offsetPlankPosition;                   // Offset position for each plank.

        private Vector3 _offsetPostPosition1;                   // Offset position for endpost1;
        private Vector3 _offsetPostPosition2;                   // Offset position for endpost2;
        private Vector3 _offsetPostPosition3;                   // Offset position for endpost3;
        private Vector3 _offsetPostPosition4;                   // Offset position for endpost4;

        private int counter = 1;                                // Stores plank item number.

        private GameObject _endPostStart1;                      // Stores instantated endpost.
        private GameObject _endPostStart2;                      // Stores instantated endpost.
        private GameObject _endPostStart3;                      // Stores instantated endpost.
        private GameObject _endPostStart4;                      // Stores instantated endpost.

        private Transform topLeftRopeS;                         // Rope location for start rails.
        private Transform topRightRopeS;                        // Rope location for start rails.
        private Transform topLeftRopeE;                         // Rope location for end rails.
        private Transform topRightRopeE;                        // Rope location for end rails.

        List<GameObject> plankList = new List<GameObject>();    // List of planks.

        private string Bridgename;                              // Stores name of bridge that gets created after pressing Play.

        // Add a menu item named "Easy Rope Bridge" to GameObject/3D Object menu with shortcut keys Alt+B.
        [MenuItem("GameObject/3D Object/Create Easy Rope Bridge &b", false, -1)]
        private static void CreateBridge()
        {
            var newBridge = new GameObject();                   // Create an empty game object in the Hierarchy
            newBridge.name = "EasyBridgeGenerator";             // Rename new object that generates the bridge when pressing Play.
            newBridge.AddComponent<plankton>();                 // Add the bridge-generating script to the new object.
        }

        void Start()
        {
            GenerateBridge();                                   // Run the code to generate the bridge per settings.
        }

        void GenerateBridge()
        {
            // Generate planks.
            for (int plankNumber = 0; plankNumber < totalPlanks; plankNumber++)                                                                                           // Loop until all planks are generated.
            {
                if (rigidPlatform == false)         // If you selected a physics-based bridge...
                {
                    if (includeHandRails == false)  // ...WITHOUT handrails.
                    {
                        if (stylizedLook)
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Stylized/sPlanky_w_rope"));                                   // Generate stylized planks with hinge joints.
                            Bridgename = "StylizedRopebridge_" + totalPlanks.ToString() + "_WithoutHandrails";                              // Update bridge name.
                        }
                        else
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Primitives/Planky_w_rope"));                                  // Generate primitive planks with hinge joints.
                            Bridgename = "Ropebridge_" + totalPlanks.ToString() + "_WithoutHandrails";                                      // Update bridge name.
                        }
                        
                    }
                    else                            // ...WITH handrails.
                    {
                        if (stylizedLook)
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Stylized/sPlanky_w_rope_and_rails"));                         // Generate primitive planks with hinge joints and rails.
                            Bridgename = "StylizedRopebridge_" + totalPlanks.ToString() + "_WithHandrails";                                             // Update bridge name.
                        }
                        else
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Primitives/Planky_w_rope_and_rails"));                        // Generate primitive planks with hinge joints and rails.
                            Bridgename = "Ropebridge_" + totalPlanks.ToString() + "_WithHandrails";                                         // Update bridge name.
                        }
                    }
                }
                else                                // If you selected a rigid bridge...
                {
                    if (includeHandRails == false)  // ...WITHOUT handrails.
                    {
                        if (stylizedLook)
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Stylized/sPlanky_w_rope_RIGID"));                             // Generate stylized planks without hinge joints.
                            _planky.GetComponent<Rigidbody>().isKinematic = true;                                                           // Set rigidbody kinematic for rigid plank.
                            Bridgename = "StylizedRigidbridge_" + totalPlanks.ToString() + "_WithoutHandrails";                             // Update bridge name.
                        }
                        else
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Primitives/Planky_w_rope_RIGID"));                            // Generate primitive planks without hinge joints.
                            Bridgename = "Rigidbridge_" + totalPlanks.ToString() + "_WithoutHandrails";                                     // Update bridge name.
                        }
                    }
                    else                            // ...WITH handrails.
                    {
                        if (stylizedLook)
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Stylized/sPlanky_w_rope_RIGID_and_rails"));                   // Generate primitive planks without hinge joints and with rails.
                            _planky.GetComponent<Rigidbody>().isKinematic = true;                                                           // Set rigidbody kinematic for rigid plank.
                            Bridgename = "StylizedRigidbridge_" + totalPlanks.ToString() + "_WithHandrails";                                // Update bridge name.
                        }
                        else
                        {
                            _planky = (GameObject)Instantiate(Resources.Load("Primitives/Planky_w_rope_RIGID_and_rails"));                  // Generate primitive planks without hinge joints and with rails.
                            Bridgename = "Rigidbridge_" + totalPlanks.ToString() + "_WithHandrails";                                        // Update bridge name.
                        }
                    }
                }

                _planky.transform.position = transform.position;                                                                            // Set plank position.

                if (!stylizedLook)
                {
                    _planky.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _planky.transform.rotation.z);
                }
                else
                {
                    _planky.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
                }

                _planky.transform.parent = transform;                                                                                       // Set plank parent to be the 'PlanksGenerated' object.

                _offsetPlankPosition =
                    new Vector3(transform.position.x + (_plankGap * plankNumber), transform.position.y, transform.position.z);              // Calculate plank offset position.
                _planky.transform.position = _offsetPlankPosition;                                                                          // Set offset position.

                if (stylizedLook)
                {
                    _planky.transform.localScale = new Vector3(1f, 1f, plankWidth - 3f);                                                    // Resize planks according to inspector settings.
                }
                else
                {
                    _planky.transform.localScale = new Vector3(0.8f, 0.2f, plankWidth);                                                     // Resize planks according to inspector settings.
                }

                if (plankNumber == 0)                                                                                                       // If first plank...
                {
                    _planky.name = "PlankFirst";                                                                                            // Rename instantiated object to PlankFirst.
                    _planky.GetComponent<Rigidbody>().isKinematic = true;                                                                   // Set rigidbody kinematic for first plank.
                    Destroy(_planky.GetComponent<HingeJoint>());                                                                            // Remove hingejoint for first plank.

                    if (stylizedLook && includeHandRails)
                    {
                        _planky.transform.Find("Rails (1)/rope (10)").gameObject.SetActive(true);                                           // Enable additional rope to reach from first plank.
                        _planky.transform.Find("Rails (2)/rope (10)").gameObject.SetActive(true);                                           // Enable additional rope to reach from first plank.
                    }

                    if (includeHandRails && !stylizedLook)                                                                                  // If you chose to include handrails...
                    {
                        // ...find rails.
                        topLeftRopeS = transform.Find("PlankFirst/LeftRail");
                        topRightRopeS = transform.Find("PlankFirst/RightRail");

                        if (rigidPlatform == false)
                        {
                            // ...move rails up.
                            topLeftRopeS.transform.position += new Vector3(0f, 0.05f, 0f);
                            topRightRopeS.transform.position += new Vector3(0f, 0.05f, 0f);
                        }
                    }
                }
                else if (plankNumber < totalPlanks - 1)                                                                                     // ...if not first or last plank...
                {
                    if(plankNumber == 1)
                    {
                        if (includeHandRails && stylizedLook)
                        {
                            var plankyRope1 = _planky.transform.Find("Rails (1)/rope (10)");
                            var plankyRope2 = _planky.transform.Find("Rails (2)/rope (10)");
                            plankyRope1.gameObject.SetActive(true);                                                                         // Enable additional rope to reach from first plank.
                            plankyRope2.gameObject.SetActive(true);                                                                         // Enable additional rope to reach from first plank.
                            Vector3 newRotation = new Vector3(0f, 0f, 94f);
                            plankyRope1.transform.eulerAngles = newRotation;
                            plankyRope2.transform.eulerAngles = newRotation;
                        }
                    }

                    _planky.name = "Plank" + (plankNumber);                                                                                 // Rename instantiated object to "Plank" and plank number.
                }
                else                                                                                                                        // ...but if last plank...
                {
                    if (includeHandRails && stylizedLook)
                    {
                        var plankyRope3 = _planky.transform.Find("Rails (1)/rope (10)");
                        var plankyRope4 = _planky.transform.Find("Rails (2)/rope (10)");
                        plankyRope3.gameObject.SetActive(true);                                                                             // Enable additional rope to reach from first plank.
                        plankyRope4.gameObject.SetActive(true);                                                                             // Enable additional rope to reach from first plank.
                        Vector3 newRotation = new Vector3(0f, 0f, 94f);
                        plankyRope3.transform.eulerAngles = newRotation;
                        plankyRope4.transform.eulerAngles = newRotation;
                    }

                    _planky.name = "PlankLast";                                                                                             // Rename instantiated object to PlankLast.
                    _planky.GetComponent<Rigidbody>().isKinematic = true;                                                                   // Set rigidbody kinematic for last plank.

                    if (includeHandRails && stylizedLook)                                                                                  // If you chose to include handrails...
                    {
                        // ...find rails.
                        topLeftRopeE = transform.Find("PlankLast/LeftRail");
                        topRightRopeE = transform.Find("PlankLast/RightRail");

                        if (rigidPlatform == false && !stylizedLook)
                        {
                            // ...move rails up.
                            topLeftRopeE.transform.position += new Vector3(0f, -0.05f, 0f);
                            topRightRopeE.transform.position += new Vector3(0f, -0.05f, 0f);
                        }
                    }
                }

                plankList.Add(_planky);                                                                                                     // Add this plank to the list.
            }

            AddConnectedBodies();

            // Add end posts if enabled.
            if (includeEndPosts == true)
            {
                Bridgename = Bridgename + "WithEndposts";
                AddEndPosts();
            }
            else
            {
                Bridgename = Bridgename + "WithoutEndposts";
            }

            if (autoRenameBridge)                                                                                                            // If you asked for the bridge object to be automatically named...
            {
                this.name = Bridgename;                                                                                                      // ...rename the bridge generating object before you create a prefab.
            }

            Destroy(GetComponent("plankton"));                                                                                               // Remove the bridge generator script, to allow creating of prefabs that do not include the script.
        }

        private void AddConnectedBodies()
        {
            // Add connected bodies to join planks together.
            HingeJoint[] hinges = transform.GetComponentsInChildren<HingeJoint>();                                                          // Store renderers from all children.
            foreach (HingeJoint h in hinges)                                                                                                // Loop through all renderers.
            {
                GameObject previousPlank = h.gameObject;

                if (counter == 2)                                                                                                           // If plank is first...
                {
                    h.connectedBody = GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
                }

                if (counter > 1 && counter < totalPlanks + 2)                                                                               // If plank is not first or last...
                {
                    h.connectedBody = plankList[counter - 2].GetComponent<Rigidbody>();
                }

                if (counter == totalPlanks + 2) // If plank is last...
                {
                    h.connectedBody = plankList[counter - 2].GetComponent<Rigidbody>();
                }
                counter++;
            }
        }

        private void AddEndPosts()
        {
            // End post start left.
            if (stylizedLook)
            {
                _endPostStart1 = (GameObject)Instantiate(Resources.Load("Stylized/sEndPost"));                                                          // Generate stylized endpost.
                _endPostStart1.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart1.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.

                _offsetPostPosition1 = new Vector3
                    (transform.position.x - 0.98f, transform.position.y - 0.15f, transform.position.z + 1.75f);                                         // Calculate endpost offset position.
                if (includeHandRails) { _endPostStart1.transform.Find("UpperRopeRing").gameObject.SetActive(true); }                                    // Display the upper rope ring if rails are selected.
                
                Vector3 newPostRotation1 = new Vector3(0f, 180f, 0f);
                _endPostStart1.transform.eulerAngles = newPostRotation1;
            }
            else
            {
                _endPostStart1 = (GameObject)Instantiate(Resources.Load("Primitives/EndPost"));                                                         // Generate primitive endpost.
                _endPostStart1.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart1.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.

                _offsetPostPosition1 = new Vector3
                    (transform.position.x - 0.98f, transform.position.y + 0.47f, transform.position.z + (0.435f * plankWidth));                         // Calculate endpost offset position.
            }           
            _endPostStart1.transform.position = _offsetPostPosition1;                                                                                   // Set offset position.
            _endPostStart1.name = "EndpostStart(Left)";                                                                                                 // Rename instantiated object


            // End post start right.
            if (stylizedLook)
            {
                _endPostStart2 = (GameObject)Instantiate(Resources.Load("Stylized/sEndPost"));                                                          // Generate endpost.
                _endPostStart2.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart2.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition2 = new Vector3
                    (transform.position.x - 0.98f, transform.position.y - 0.15f, transform.position.z - 1.75f);                                          // Calculate endpost offset position.
                if (includeHandRails) { _endPostStart2.transform.Find("UpperRopeRing").gameObject.SetActive(true); }                                    // Display the upper rope ring if rails are selected.

                Vector3 newPostRotation1 = new Vector3(0f, 180f, 0f);
                _endPostStart2.transform.eulerAngles = newPostRotation1;
            }
            else
            {
                _endPostStart2 = (GameObject)Instantiate(Resources.Load("Primitives/EndPost"));                                                         // Generate endpost.
                _endPostStart2.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart2.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition2 = new Vector3
                    (transform.position.x - 0.98f, transform.position.y + 0.47f, transform.position.z - (0.45f * plankWidth));                          // Calculate endpost offset position.
            }
            _endPostStart2.transform.position = _offsetPostPosition2;                                                                                   // Set offset position.
            _endPostStart2.name = "EndpostStart(Right)";                                                                                                // Rename instantiated object

            // End post end left.
            if (stylizedLook)
            {
                _endPostStart3 = (GameObject)Instantiate(Resources.Load("Stylized/sEndPost"));                                                          // Generate endpost.
                _endPostStart3.transform.position = _planky.transform.position;                                                                         // Set endpost position.
                _endPostStart3.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition3 = new Vector3
                    (_planky.transform.position.x + (_plankGap / 2) + 0.3f, transform.position.y - 0.15f, transform.position.z + 1.75f);                // Calculate endpost offset position.
                if (includeHandRails) { _endPostStart3.transform.Find("UpperRopeRing").gameObject.SetActive(true); }                                    // Display the upper rope ring if rails are selected.
            }
            else
            {
                _endPostStart3 = (GameObject)Instantiate(Resources.Load("Primitives/EndPost"));                                                         // Generate endpost.
                _endPostStart3.transform.position = _planky.transform.position;                                                                         // Set endpost position.
                _endPostStart3.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition3 = new Vector3
                    (_planky.transform.position.x + (_plankGap / 2) + 0.3f, transform.position.y + 0.47f, transform.position.z + (0.45f * plankWidth)); // Calculate endpost offset position.
            }
            _endPostStart3.transform.position = _offsetPostPosition3;                                                                                   // Set offset position.
            _endPostStart3.name = "EndpostEnd(Left)";                                                                                                   // Rename instantiated object

            // End post end right.
            if (stylizedLook)
            {
                _endPostStart4 = (GameObject)Instantiate(Resources.Load("Stylized/sEndPost"));                                                          // Generate endpost.
                _endPostStart4.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart4.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition4 = new Vector3
                    (_planky.transform.position.x + (_plankGap / 2) + 0.3f, transform.position.y - 0.15f, transform.position.z - 1.75f);                // Calculate endpost offset position.
                if (includeHandRails) { _endPostStart4.transform.Find("UpperRopeRing").gameObject.SetActive(true); }                                    // Display the upper rope ring if rails are selected.
            }
            else
            {
                _endPostStart4 = (GameObject)Instantiate(Resources.Load("Primitives/EndPost"));                                                         // Generate endpost.
                _endPostStart4.transform.position = transform.position;                                                                                 // Set endpost position.
                _endPostStart4.transform.parent = transform;                                                                                            // Set endpost parent to be the 'PlanksGenerated' object.
                _offsetPostPosition4 = new Vector3
                    (_planky.transform.position.x + (_plankGap / 2) + 0.3f, transform.position.y + 0.47f, transform.position.z - (0.45f * plankWidth)); // Calculate endpost offset position.
            }
            _endPostStart4.transform.position = _offsetPostPosition4;                                                                                   // Set offset position.
            _endPostStart4.name = "EndpostEnd(Right)";                                                                                                  // Rename instantiated object
        }
    }
}
