#if UNITY_EDITOR || UNITY_ANDROID || PACKAGE_DOCS_GENERATION
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using System;
using UnityEngine.InputSystem.Android;
using UnityEngine.InputSystem.Android.LowLevel;

namespace PlaySolanaSdk {
    [StructLayout(LayoutKind.Sequential)]

    public unsafe struct PSG1StateController : IInputStateTypeInfo
    {
        public const int MaxAxes = 48;
        public const int MaxButtons = 220;

        public class Variants
        {
            public const string Gamepad = "Gamepad";
            public const string Joystick = "Joystick";
            public const string DPadAxes = "DpadAxes";
            public const string DPadButtons = "DpadButtons";
        }

        internal const uint kAxisOffset = sizeof(uint) * (uint)((MaxButtons + 31) / 32);

        public static FourCC kFormat = new FourCC('A', 'G', 'C', ' ');

        [InputControl(name = "dpad", layout = "Dpad", bit = (uint)AndroidKeyCode.DpadUp, sizeInBits = 4, variants = Variants.DPadButtons)]
        [InputControl(name = "dpad/up", bit = (uint)AndroidKeyCode.DpadUp, variants = Variants.DPadButtons)]
        [InputControl(name = "dpad/down", bit = (uint)AndroidKeyCode.DpadDown, variants = Variants.DPadButtons)]
        [InputControl(name = "dpad/left", bit = (uint)AndroidKeyCode.DpadLeft, variants = Variants.DPadButtons)]
        [InputControl(name = "dpad/right", bit = (uint)AndroidKeyCode.DpadRight, variants = Variants.DPadButtons)]
        [InputControl(name = "buttonSouth", bit = (uint)AndroidKeyCode.ButtonA, variants = Variants.Gamepad, displayName = "B")]
        [InputControl(name = "buttonWest", bit = (uint)AndroidKeyCode.ButtonY, variants = Variants.Gamepad, displayName = "Y")]
        [InputControl(name = "buttonNorth", bit = (uint)AndroidKeyCode.ButtonX, variants = Variants.Gamepad, displayName = "X")]
        [InputControl(name = "buttonEast", bit = (uint)AndroidKeyCode.ButtonB, variants = Variants.Gamepad, displayName = "A")]
        [InputControl(name = "leftStickPress", bit = (uint)AndroidKeyCode.ButtonThumbl, variants = Variants.Gamepad, displayName = "L3")]
        [InputControl(name = "rightStickPress", bit = (uint)AndroidKeyCode.ButtonThumbr, variants = Variants.Gamepad, displayName = "R3")]
        [InputControl(name = "leftShoulder", bit = (uint)AndroidKeyCode.ButtonL1, variants = Variants.Gamepad, displayName = "L1")]
        [InputControl(name = "rightShoulder", bit = (uint)AndroidKeyCode.ButtonR1, variants = Variants.Gamepad, displayName = "R1")]
        [InputControl(name = "start", bit = (uint)AndroidKeyCode.ButtonStart, variants = Variants.Gamepad)]
        [InputControl(name = "select", bit = (uint)AndroidKeyCode.ButtonSelect, variants = Variants.Gamepad)]
        public fixed uint buttons[(MaxButtons + 31) / 32];

        [InputControl(name = "dpad", layout = "Dpad", offset = (uint)AndroidAxis.HatX * sizeof(float) + kAxisOffset, format = "VEC2", sizeInBits = 64, variants = Variants.DPadAxes)]
        [InputControl(name = "dpad/right", offset = 0, bit = 0, sizeInBits = 32, format = "FLT", parameters = "clamp=3,clampConstant=0,clampMin=0,clampMax=1", variants = Variants.DPadAxes)]
        [InputControl(name = "dpad/left", offset = 0, bit = 0, sizeInBits = 32, format = "FLT", parameters = "clamp=3,clampConstant=0,clampMin=-1,clampMax=0,invert", variants = Variants.DPadAxes)]
        [InputControl(name = "dpad/down", offset = ((uint)AndroidAxis.HatY - (uint)AndroidAxis.HatX) * sizeof(float), bit = 0, sizeInBits = 32, format = "FLT", parameters = "clamp=3,clampConstant=0,clampMin=0,clampMax=1", variants = Variants.DPadAxes)]
        [InputControl(name = "dpad/up", offset = ((uint)AndroidAxis.HatY - (uint)AndroidAxis.HatX) * sizeof(float), bit = 0, sizeInBits = 32, format = "FLT", parameters = "clamp=3,clampConstant=0,clampMin=-1,clampMax=0,invert", variants = Variants.DPadAxes)]
        [InputControl(name = "leftTrigger", offset = (uint)AndroidAxis.Brake * sizeof(float) + kAxisOffset, parameters = "clamp=1,clampMin=0,clampMax=1.0", variants = Variants.Gamepad,
        displayName = " ")]
        [InputControl(name = "rightTrigger", offset = (uint)AndroidAxis.Gas * sizeof(float) + kAxisOffset, parameters = "clamp=1,clampMin=0,clampMax=1.0", variants = Variants.Gamepad, displayName = " ")]
        [InputControl(name = "leftStick", variants = Variants.Gamepad)]
        [InputControl(name = "leftStick/y", variants = Variants.Gamepad, parameters = "invert")]
        [InputControl(name = "leftStick/up", variants = Variants.Gamepad, parameters = "invert,clamp=1,clampMin=-1.0,clampMax=0.0")]
        [InputControl(name = "leftStick/down", variants = Variants.Gamepad, parameters = "invert=false,clamp=1,clampMin=0,clampMax=1.0")]
        [InputControl(name = "rightStick", offset = (uint)AndroidAxis.Z * sizeof(float) + kAxisOffset, sizeInBits = ((uint)AndroidAxis.Rz - (uint)AndroidAxis.Z + 1) * sizeof(float) * 8, variants = Variants.Gamepad)]
        [InputControl(name = "rightStick/x", variants = Variants.Gamepad)]
        [InputControl(name = "rightStick/y", offset = ((uint)AndroidAxis.Rz - (uint)AndroidAxis.Z) * sizeof(float), variants = Variants.Gamepad, parameters = "invert")]
        [InputControl(name = "rightStick/up", offset = ((uint)AndroidAxis.Rz - (uint)AndroidAxis.Z) * sizeof(float), variants = Variants.Gamepad, parameters = "invert,clamp=1,clampMin=-1.0,clampMax=0.0")]
        [InputControl(name = "rightStick/down", offset = ((uint)AndroidAxis.Rz - (uint)AndroidAxis.Z) * sizeof(float), variants = Variants.Gamepad, parameters = "invert=false,clamp=1,clampMin=0,clampMax=1.0")]
        public fixed float axis[MaxAxes];


        public FourCC format
        {
            get { return kFormat; }
        }

        public PSG1StateController WithButton(AndroidKeyCode code, bool value = true)
        {
            fixed(uint* buttonsPtr = buttons)
            {
                if (value)
                    buttonsPtr[(int)code / 32] |= 1U << ((int)code % 32);
                else
                    buttonsPtr[(int)code / 32] &= ~(1U << ((int)code % 32));
            }
            return this;
        }

        public PSG1StateController WithAxis(AndroidAxis axis, float value)
        {
            fixed(float* axisPtr = this.axis)
            {
                axisPtr[(int)axis] = value;
            }
            return this;
        }
    }

    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif
    [InputControlLayout(stateType = typeof(PSG1StateController), variants = PSG1StateController.Variants.Gamepad + InputControlLayout.VariantSeparator + PSG1StateController.Variants.DPadButtons)]
    public class PSG1 : Gamepad {

        #if UNITY_EDITOR
        static PSG1() {
            Initialize();
        }
        #endif


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() {
            InputSystem.RegisterLayout<PSG1>(matches: new InputDeviceMatcher()
                    .WithInterface("Android")
                    .WithProduct("virtual-gamepad")
                    .WithCapability("vendorId", 0x1234)
                    .WithCapability("productId", 0x5678)
                    );
        }

        protected override void FinishSetup() {
            base.FinishSetup();
        }

        public static new PSG1 current { get; private set; }
        public override void MakeCurrent() {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (current == this)
                current = null;
        }

    }
}

#endif