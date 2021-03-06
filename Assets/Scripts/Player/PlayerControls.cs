// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""OneHook"",
            ""id"": ""71cc17c4-07f9-4a11-aacf-80a0e9a3c292"",
            ""actions"": [
                {
                    ""name"": ""HoriztonalAxis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""835b59ba-1d02-4cfd-b3e5-8b2733d31dba"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalAxis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""314a2246-b8e2-41ca-a456-525223acd860"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""07d4643a-b3c5-4778-95e1-762762a667ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""8923d5d4-1e40-47b9-8cad-1213127faf53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9887a6db-9b69-4afa-ad1a-46af8655f986"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard X Axis"",
                    ""id"": ""9145567b-a2dd-40d8-901e-548ff73af343"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7e333ec2-5f36-475b-a359-70177883221a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e070da72-4323-44b3-889f-d64c6a9967a7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller X Axis"",
                    ""id"": ""1d469bc7-a573-4b1e-baed-b656992f2915"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""021b7614-f8d3-4d3f-b4e9-447994ac5dc1"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2707186b-c3bf-4cf6-a8c4-69c761067dcb"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Y Axis"",
                    ""id"": ""15616a4b-c8a9-4fad-91e9-9d09f4264144"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7ff78e92-b5ca-4975-bb63-44091f380db0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3cd99fea-76e7-43c4-8689-3e16c76c1d69"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller Y Axis"",
                    ""id"": ""5bf35be7-6b87-4eb4-bb22-fd0ed017f231"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f409cb3a-8284-4fbf-95f7-e2b55d0fc30e"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0c527801-8d4c-4b9a-9b70-5757b06207d5"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cf402dfb-1728-4eb9-b8d5-5b0f4186a061"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43b3f993-d101-4f95-aa0c-dd863882e5da"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9cb3c6e1-355c-483d-b8cb-9ad9bd4699a1"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69ffe0d0-d1be-4dd6-9abe-8e7db8f30833"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b24a64cc-c82b-4843-b963-839982836c7b"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1c66b64-53c7-4b2e-9759-d40dfb022b7b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TwoHook"",
            ""id"": ""f8d4b155-5da0-4a35-b5d5-9c74936bcb08"",
            ""actions"": [
                {
                    ""name"": ""HoriztonalAxis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0a21f89c-5de9-4a4b-ae02-c10a6e2cc7f9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalAxis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d059ce98-2764-4319-9cbb-44b937174f87"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""ae99bba1-9b15-4f85-921d-006e47695a64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightFire"",
                    ""type"": ""Button"",
                    ""id"": ""37423a95-3773-4328-9586-4f6f1ab75d75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightReel"",
                    ""type"": ""Button"",
                    ""id"": ""3c6424c1-cc94-4b63-9d1c-e283ca331875"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftFire"",
                    ""type"": ""Button"",
                    ""id"": ""b01af4eb-f43b-4602-a56b-7e2fc4e1ae01"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftReel"",
                    ""type"": ""Button"",
                    ""id"": ""caeec3c4-148d-4c6f-a637-bff0e656083b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard X Axis"",
                    ""id"": ""1c223008-3633-4ae6-b60e-175d91ea6f15"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""66e655f9-3524-48ab-a95a-3544886881f1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d6167233-7e88-437f-a8b4-7d47ff9bf902"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller X Axis"",
                    ""id"": ""1b34e785-121c-4a17-8e80-ea0ebba200df"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a9b1dbe7-2dbd-4c5f-b0e7-5dd0495523d3"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""73d7ed8c-a203-454f-8f52-496f31ade747"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoriztonalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Y Axis"",
                    ""id"": ""387708d4-04c0-4d1a-8ccf-aeac02866f1b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9a22d39a-045b-4c52-84c1-1b05dd59f67c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a0d3e3f2-e72d-4af1-b0bd-8722c6b5faa7"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller Y Axis"",
                    ""id"": ""138969fa-4b75-4699-b249-848e0b98df90"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3d95a643-c1e4-41e4-9ca0-9125ca8c6ccd"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""223d7e48-10c2-4794-bd1a-2920a31a305e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fa863669-462a-4a0c-aceb-29800689385b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7d6820d-0835-40c7-8e17-a9f368f31839"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82ee809d-1881-42d0-95ba-c100dde57cf9"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""125c3004-2c77-456f-9f34-edbd4116e288"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f858e3a7-336a-49ce-92e6-40ced3efe668"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""327aaf1e-cdb7-4d4b-904f-4e42fec78988"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b796a289-39bb-42a0-bb23-932448561258"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftReel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18491475-f966-48c1-a66f-eb6fcae39edd"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftReel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a402bd21-f35a-4f0d-bbc5-dedbcd17d3a1"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightReel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54af9172-e515-479a-9799-6ab6750d7209"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightReel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""67b24dce-e5ea-449a-aa01-7b0ada42e7bf"",
            ""actions"": [
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""1fd391dd-afd7-44d3-b6ad-8f60edf5684f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""50ef50ea-67b6-4382-b47c-d608e9237d7d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98783a3b-8835-4c28-980a-8a4f2b5e5bea"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // OneHook
        m_OneHook = asset.FindActionMap("OneHook", throwIfNotFound: true);
        m_OneHook_HoriztonalAxis = m_OneHook.FindAction("HoriztonalAxis", throwIfNotFound: true);
        m_OneHook_VerticalAxis = m_OneHook.FindAction("VerticalAxis", throwIfNotFound: true);
        m_OneHook_Jump = m_OneHook.FindAction("Jump", throwIfNotFound: true);
        m_OneHook_Fire = m_OneHook.FindAction("Fire", throwIfNotFound: true);
        m_OneHook_Reel = m_OneHook.FindAction("Reel", throwIfNotFound: true);
        // TwoHook
        m_TwoHook = asset.FindActionMap("TwoHook", throwIfNotFound: true);
        m_TwoHook_HoriztonalAxis = m_TwoHook.FindAction("HoriztonalAxis", throwIfNotFound: true);
        m_TwoHook_VerticalAxis = m_TwoHook.FindAction("VerticalAxis", throwIfNotFound: true);
        m_TwoHook_Jump = m_TwoHook.FindAction("Jump", throwIfNotFound: true);
        m_TwoHook_RightFire = m_TwoHook.FindAction("RightFire", throwIfNotFound: true);
        m_TwoHook_RightReel = m_TwoHook.FindAction("RightReel", throwIfNotFound: true);
        m_TwoHook_LeftFire = m_TwoHook.FindAction("LeftFire", throwIfNotFound: true);
        m_TwoHook_LeftReel = m_TwoHook.FindAction("LeftReel", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Menu = m_UI.FindAction("Menu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // OneHook
    private readonly InputActionMap m_OneHook;
    private IOneHookActions m_OneHookActionsCallbackInterface;
    private readonly InputAction m_OneHook_HoriztonalAxis;
    private readonly InputAction m_OneHook_VerticalAxis;
    private readonly InputAction m_OneHook_Jump;
    private readonly InputAction m_OneHook_Fire;
    private readonly InputAction m_OneHook_Reel;
    public struct OneHookActions
    {
        private @PlayerControls m_Wrapper;
        public OneHookActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HoriztonalAxis => m_Wrapper.m_OneHook_HoriztonalAxis;
        public InputAction @VerticalAxis => m_Wrapper.m_OneHook_VerticalAxis;
        public InputAction @Jump => m_Wrapper.m_OneHook_Jump;
        public InputAction @Fire => m_Wrapper.m_OneHook_Fire;
        public InputAction @Reel => m_Wrapper.m_OneHook_Reel;
        public InputActionMap Get() { return m_Wrapper.m_OneHook; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OneHookActions set) { return set.Get(); }
        public void SetCallbacks(IOneHookActions instance)
        {
            if (m_Wrapper.m_OneHookActionsCallbackInterface != null)
            {
                @HoriztonalAxis.started -= m_Wrapper.m_OneHookActionsCallbackInterface.OnHoriztonalAxis;
                @HoriztonalAxis.performed -= m_Wrapper.m_OneHookActionsCallbackInterface.OnHoriztonalAxis;
                @HoriztonalAxis.canceled -= m_Wrapper.m_OneHookActionsCallbackInterface.OnHoriztonalAxis;
                @VerticalAxis.started -= m_Wrapper.m_OneHookActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.performed -= m_Wrapper.m_OneHookActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.canceled -= m_Wrapper.m_OneHookActionsCallbackInterface.OnVerticalAxis;
                @Jump.started -= m_Wrapper.m_OneHookActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_OneHookActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_OneHookActionsCallbackInterface.OnJump;
                @Fire.started -= m_Wrapper.m_OneHookActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_OneHookActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_OneHookActionsCallbackInterface.OnFire;
                @Reel.started -= m_Wrapper.m_OneHookActionsCallbackInterface.OnReel;
                @Reel.performed -= m_Wrapper.m_OneHookActionsCallbackInterface.OnReel;
                @Reel.canceled -= m_Wrapper.m_OneHookActionsCallbackInterface.OnReel;
            }
            m_Wrapper.m_OneHookActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HoriztonalAxis.started += instance.OnHoriztonalAxis;
                @HoriztonalAxis.performed += instance.OnHoriztonalAxis;
                @HoriztonalAxis.canceled += instance.OnHoriztonalAxis;
                @VerticalAxis.started += instance.OnVerticalAxis;
                @VerticalAxis.performed += instance.OnVerticalAxis;
                @VerticalAxis.canceled += instance.OnVerticalAxis;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Reel.started += instance.OnReel;
                @Reel.performed += instance.OnReel;
                @Reel.canceled += instance.OnReel;
            }
        }
    }
    public OneHookActions @OneHook => new OneHookActions(this);

    // TwoHook
    private readonly InputActionMap m_TwoHook;
    private ITwoHookActions m_TwoHookActionsCallbackInterface;
    private readonly InputAction m_TwoHook_HoriztonalAxis;
    private readonly InputAction m_TwoHook_VerticalAxis;
    private readonly InputAction m_TwoHook_Jump;
    private readonly InputAction m_TwoHook_RightFire;
    private readonly InputAction m_TwoHook_RightReel;
    private readonly InputAction m_TwoHook_LeftFire;
    private readonly InputAction m_TwoHook_LeftReel;
    public struct TwoHookActions
    {
        private @PlayerControls m_Wrapper;
        public TwoHookActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HoriztonalAxis => m_Wrapper.m_TwoHook_HoriztonalAxis;
        public InputAction @VerticalAxis => m_Wrapper.m_TwoHook_VerticalAxis;
        public InputAction @Jump => m_Wrapper.m_TwoHook_Jump;
        public InputAction @RightFire => m_Wrapper.m_TwoHook_RightFire;
        public InputAction @RightReel => m_Wrapper.m_TwoHook_RightReel;
        public InputAction @LeftFire => m_Wrapper.m_TwoHook_LeftFire;
        public InputAction @LeftReel => m_Wrapper.m_TwoHook_LeftReel;
        public InputActionMap Get() { return m_Wrapper.m_TwoHook; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TwoHookActions set) { return set.Get(); }
        public void SetCallbacks(ITwoHookActions instance)
        {
            if (m_Wrapper.m_TwoHookActionsCallbackInterface != null)
            {
                @HoriztonalAxis.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnHoriztonalAxis;
                @HoriztonalAxis.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnHoriztonalAxis;
                @HoriztonalAxis.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnHoriztonalAxis;
                @VerticalAxis.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnVerticalAxis;
                @Jump.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnJump;
                @RightFire.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightFire;
                @RightFire.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightFire;
                @RightFire.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightFire;
                @RightReel.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightReel;
                @RightReel.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightReel;
                @RightReel.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnRightReel;
                @LeftFire.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftFire;
                @LeftFire.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftFire;
                @LeftFire.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftFire;
                @LeftReel.started -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftReel;
                @LeftReel.performed -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftReel;
                @LeftReel.canceled -= m_Wrapper.m_TwoHookActionsCallbackInterface.OnLeftReel;
            }
            m_Wrapper.m_TwoHookActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HoriztonalAxis.started += instance.OnHoriztonalAxis;
                @HoriztonalAxis.performed += instance.OnHoriztonalAxis;
                @HoriztonalAxis.canceled += instance.OnHoriztonalAxis;
                @VerticalAxis.started += instance.OnVerticalAxis;
                @VerticalAxis.performed += instance.OnVerticalAxis;
                @VerticalAxis.canceled += instance.OnVerticalAxis;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @RightFire.started += instance.OnRightFire;
                @RightFire.performed += instance.OnRightFire;
                @RightFire.canceled += instance.OnRightFire;
                @RightReel.started += instance.OnRightReel;
                @RightReel.performed += instance.OnRightReel;
                @RightReel.canceled += instance.OnRightReel;
                @LeftFire.started += instance.OnLeftFire;
                @LeftFire.performed += instance.OnLeftFire;
                @LeftFire.canceled += instance.OnLeftFire;
                @LeftReel.started += instance.OnLeftReel;
                @LeftReel.performed += instance.OnLeftReel;
                @LeftReel.canceled += instance.OnLeftReel;
            }
        }
    }
    public TwoHookActions @TwoHook => new TwoHookActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Menu;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Menu => m_Wrapper.m_UI_Menu;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Menu.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IOneHookActions
    {
        void OnHoriztonalAxis(InputAction.CallbackContext context);
        void OnVerticalAxis(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnReel(InputAction.CallbackContext context);
    }
    public interface ITwoHookActions
    {
        void OnHoriztonalAxis(InputAction.CallbackContext context);
        void OnVerticalAxis(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRightFire(InputAction.CallbackContext context);
        void OnRightReel(InputAction.CallbackContext context);
        void OnLeftFire(InputAction.CallbackContext context);
        void OnLeftReel(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMenu(InputAction.CallbackContext context);
    }
}
