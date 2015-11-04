﻿using System.Runtime.InteropServices;

    public int scale = 1000;

    void OnEnable() {
        HandInputManager.handMotionDetected += SelectMenu;

    void OnDisable()
    {
        HandInputManager.handMotionDetected -= SelectMenu;
    }
        int dx = (int)((this.rightHandPos.x - this.preRightHandPos.x)*scale);
        int dy = (int)((this.rightHandPos.z - this.preRightHandPos.z)*scale);
        mousePos.X += dx;
        mousePos.Y += dy;
        SetCursorPos(mousePos.X, mousePos.Y);
        this.preRightHandPos = this.rightHandPos;


    void SelectMenu(object sender, HandMotionDetectedEventArgs args) {
        if (args.motion == HandMotion.LEFT_HAND_WAVE_OUT) {
            //TODO: Change the code here, implement choosing mene function