Dim AFOAM_COUNTER As Integer
Dim SHOT_VOLUME As Double = 0.5
Dim MAX_SHOTS As Integer = 5
Dim AFOAM_SHOT_FED As Double
Dim AFOAM_THRESHOLD As Double
Dim NUMBER_SHOTS As Integer = 0
Dim CURRENT_CASE As Integer = 0

Select Case CURRENT_CASE

    Case 0 ' Foam under control
        If p.LvlPv >= 500 Then
            CURRENT_CASE = 1
        End If

    Case 1 ' Dispense anti-foam
        If NUMBER_SHOTS < MAX_SHOTS Then
            ' Disepense logic:
            PumpOn()
            AFOAM_SHOT_FED = SHOT_VOLUME
            LogMessage("Shot Dispensed: " & SHOT_VOLUME & " mL")
            CURRENT_CASE = 2
        Else
            NUMBER_SHOTS = MAX_SHOTS
            CURRENT_CASE = 3 ' Max shots reached
        End If
    
    Case 2 ' Finish shot, update counter
        NUMBER_SHOTS += 1
        PumpOff()
        CURRENT_CASE = 0
        LogMessage("Shot #" & NUMBER_SHOTS & " completed")

    Case 3 ' Max shots reached
        LogMessage("Max Shots Reached")
End Select

Sub PumpOn()
    PumpAActive = 1
End Sub

Sub PumpOff()
    PumpAActive = 0
End Sub
